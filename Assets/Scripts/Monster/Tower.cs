using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;


//how the tower determines its targets
public enum TargetMode
{
    
    Newest,
    Oldest,
    Weakest,
    Strongest,
    MostHP,
    LeastHP,
    Closest,
    Furthest,
    Random
}

public class Tower : MonoBehaviour, IPointerDownHandler
{
    public TargetMode targetMode = new TargetMode();

    private int columns, rows;
    private float height, width;

    
    public GameObject Map;
    private GameObject levelTile;
    private MapInformation mapInformation;

    //bool to determine if the tower is looking for a tile or not, and bool to see if the tower has been placed already. 
    public bool isBeingPlaced, isPlaced;

    //floats related to the length of a finger touch by the user
    public float touchTime, releaseTime;

    //used to determine if a touch was a hold or just a tap
    public bool isTapped;

    //when this is true, the tower is scanning for enemies that go in to his range
    public bool isScanning;

    //bool to determine of the tower being placed is placed over a tile that it can be placed on. if it is, don't let it detect another correct tile, until it is off the current one. used to prevent detection of multiple tiles
    public bool isCorrectTile;

    //bool used to determine if the map of eligible tiles has been displayed or not
    public bool isTileMap;

    //variable for the position of the transform of the tile that the tower will be placed on
    private Vector2 tilePlacementPosition;
    private RectTransform tileRect;

    //variable to keep what tile the tower is on
    public int tileOn;
    public MapTile mapTileOn;

    //private gameObject for the induvidual tiles on the map
    private GameObject[] tiles;
    

    //private GameObject for the tower overlay UI menu
    private GameObject towerMenu;
    private GameObject infoMenu;
    private GameObject mainCamera;

    //starting position for the tower on the menu, so if it's placed on an incorrect tile, it'll snap back to this position
    public Vector2 menuPosition;


    //the monster's info and body specs
    public Monster monster;
    private MonsterSpecs specs;


    //a list of the integers of the tiles in range of the tower's attack
    public List<int> atkRange1List = new List<int>();
    public List<int> atkRange2List = new List<int>();

    //used to determine which attack of the tower you're using/looking up
    public int attackNumber;

    //float used to keep track of the current time between attacks
    public float acumTime;
    //bool to determine if the tower is currently attacking
    public bool isAttacking;
    //bool to determine if the avatar/copy/placeholder for the tower in the tower menu exists or not
    public bool isCopy;
    //bool to determine if the tower is in it's "idle" state or not, trigger it's idle animation
    public bool isIdle = false;

    private Animator monsterMotion;

    //use this to pre-load the attack animation that the monster has
    public GameObject attack1Animation, attack2Animation;

    //objects for the monster's bones and non boned character model
    public GameObject boneStructure, frontModel;
    private GameObject monsterIcon;

    //bool to determine is the player has enough energy to summon this monster or not
    public bool isSummonable;

    //the spawn point for this monster's attacks
    public GameObject attackPoint;

    //a list of enemies that are in range of this tower's attacks
    public List<Enemy> enemiesInRange = new List<Enemy>();
    //order in which this monster prioritizes enemys
    public List<Enemy> targetOrder = new List<Enemy>();


    //canvas used for in game sliders for a Tower monster
    public GameObject towerCanvas;
    public EnergyBar staminaBar;

    //bool that is changed when the monster's summon animation is complete. changed by the Summon Animation object
    public bool summonAnimationComplete;
    //when a monster's stamina is full, it can use it's ability. this bool checks that
    public bool abilityReady, abilityAuraActive;
    public GameObject abilityAura;

    public MapDetails mapDetails;
    // Start is called before the first frame update
    void Start()
    {
        monster = GetComponent<Monster>();
        specs = monster.specs;

        tiles = GameObject.FindGameObjectsWithTag("MapTile");
        mainCamera = GameObject.Find("Main Camera");
        //Map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();
        Map = GameObject.FindGameObjectWithTag("Map");
        //mapDetails = Map.GetComponent<WorldMap>().mapDetails;
        mapDetails = GameManager.Instance.activeMap;
        //mapInformation = Map.GetComponent<MapDetails>().mapInformation;
        mapInformation = mapDetails.mapInformation;
        infoMenu = Map.GetComponent<MonsterInfoMenus>().infoMenu;
        towerMenu = Map.GetComponent<MonsterInfoMenus>().towerMenu;
        levelTile = mapDetails.mapTile;
        //levelTile = Map.GetComponent<MapDetails>().mapTile;

        //load the monster's info as a json string
        string json = JsonUtility.ToJson(GetComponent<Monster>().info);
        monsterMotion = GetComponent<Animator>();
        monsterIcon = GetComponent<Monster>().monsterIcon;

        attack1Animation = monster.tempStats.attack1.attackAnimation;
        attack2Animation = monster.tempStats.attack2.attackAnimation;

        //towers default to targetting the newest monster that enters their range
        targetMode = TargetMode.Newest;

        

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(mapInformation.playerEnergy);
        //if (mapInformation.playerEnergy >= monster.energyCost)
        if (mapInformation.playerEnergy >= monster.info.EnergyCost.Value)
        {
            isSummonable = true;
        }
        else
        {
            isSummonable = false;
        }


        //if the tower has already been placed, then there is no need to run the methods that have to do with the placement of the tower
        if (isPlaced == false)
        {
            //frontModel.SetActive(true);
            monsterIcon.SetActive(true);
            monsterIcon.GetComponentInChildren<Canvas>().overrideSorting = true;
            monsterIcon.GetComponentInChildren<Canvas>().sortingLayerName = "GameUI";
            boneStructure.SetActive(false);

            if (isSummonable)
            {
                TowerPlacement();
            }

        }




        //if (isIdle)
        //{
        //    monsterMotion.SetBool("isIdle", true);

        //}
        //else
        //{
        //    monsterMotion.SetBool("isIdle", false);
        //}

        //use this to scan for attack ranges for incoming enemies
       


        

        //foreach (Enemy e in enemiesInRange)
        //{
        //    if (!atkRange1List.Contains(e.currentTile) && !atkRange2List.Contains(e.currentTile))
        //    {
        //        enemiesInRange.Remove(e);
        //    }
        //}

        

    }

    private void LateUpdate()
    {
        if (isScanning)
        {
            AttackTimer();
        }
    }



    //method used to start the check for the tiles that this tower can be placed on
    private void TowerPlacement()
    {
        if (isBeingPlaced == false)
        {

            menuPosition = transform.position;
            if (towerMenu)
            {
                towerMenu.GetComponent<ScrollRect>().vertical = true;
            }
        }
        else
        {
            if (towerMenu)
            {
                towerMenu.GetComponent<ScrollRect>().vertical = false;
            }
        }


        //for (var i = 0; i < Input.touchCount; ++i)
        //{
        //    if (Input.GetTouch(i).phase == TouchPhase.Began)
        //    {
        //        touchTime = Time.time;

        //    }
        //    float acumTime = Time.time - touchTime;

        //    if (Input.GetTouch(i).phase == TouchPhase.Ended)
        //    {
        //        releaseTime = Time.time;
        //        acumTime = releaseTime - touchTime;

        //        //shoot a raycast that does not hit the map tile layer, which is layer 9
        //        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position), Vector2.zero, 0f, 1 << 8);
        //        //What to do on a tap
        //        if (acumTime <= 1f)
        //        {
        //            isTapped = true;


        //            // RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
        //            if (hit)
        //            {
        //                string itemHit = (hit.transform.gameObject.tag);

        //                //if you tap a monster on the menu that has all of your towers, it becomes the "active monster" on the screen and it's stats are displayed
        //                if (hit.collider.gameObject.name == gameObject.name)
        //                {
        //                    infoMenu.SetActive(true);
        //                    Map.GetComponent<MonsterInfoMenus>().activeMonster = hit.collider.gameObject.GetComponent<Monster>();
        //                    //GameManager.Instance.overworldMenu.SetActive(true);
        //                    //GameManager.Instance.overworldMenu.GetComponentInChildren<OverworldInfoMenu>().activeMonster = hit.collider.gameObject.GetComponent<Monster>();
        //                }
        //            }
        //        }
        //        //What do to on a long hold
        //        //else
        //        //{
        //        //    isTapped = false;
        //        //    if (isBeingPlaced == false)
        //        //    {
        //        //        if (hit)
        //        //        {
        //        //            if (hit.collider != null)
        //        //            {
        //        //                if (hit.collider.gameObject.name == gameObject.name)
        //        //                {
        //        //                    isBeingPlaced = true;
        //        //                    var position = Input.GetTouch(i).position;
        //        //                    Debug.Log(position);
        //        //                    var x = position.x;
        //        //                    var y = position.y;
        //        //                    transform.position = new Vector3(x, y, transform.position.z);

        //        //                }
        //        //            }
        //        //        }
        //        //    }

        //        //}
        //    }
        //    //what to do on a long tap
        //    else
        //    {
        //        if (acumTime >= .7f)
        //        {
        //            //shoot a raycast that does not hit the map tile layer, which is layer 9
        //            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position), Vector2.zero, 0f, 1 << 8);


        //            isTapped = false;
        //            if (isBeingPlaced == false)
        //            {
        //                if (hit)
        //                {
        //                    if (hit.collider != null)
        //                    {
        //                        if (hit.collider.gameObject.name == gameObject.name)
        //                        {
        //                            isBeingPlaced = true;
        //                            Map.GetComponent<MonsterInfoMenus>().TowerMenuBtn();
        //                            infoMenu.SetActive(true);
        //                            Map.GetComponent<MonsterInfoMenus>().activeMonster = hit.collider.gameObject.GetComponent<Monster>();
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //    }

        //}

        //if the tower is being placed, it's movement is that of the mouse
        if (isBeingPlaced == true)
        {
            
            CheckForPlacement();
            //mainCamera.GetComponent<CameraMotion>().isFree = false;
            //creates a copy of the tower in the menu, so when the tower is placed, the player knows that it has been placed. 
            if (!isCopy)
            {

                isCopy = true;
                var copy = Instantiate(this.gameObject, transform.position, Quaternion.identity);
                //copy.GetComponent<Monster>().frontModel.SetActive(true);
                var menu = GameObject.Find("Content");
                copy.transform.SetParent(menu.transform, true);
                copy.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
                copy.name = gameObject.name + " Placeholder" + monster.info.index;
                copy.tag = "TowerAvatar";
                copy.layer = 12;
                //copy.GetComponent<Tower>().isPlaced = true;
                SpriteRenderer[] bodyparts = copy.GetComponentsInChildren<SpriteRenderer>();


                for (int i = 0; i < bodyparts.Length; i++)
                {
                    bodyparts[i].color = new Color(bodyparts[i].color.r, bodyparts[i].color.g, bodyparts[i].color.b, .5f);
                }
                //copy.GetComponent<SpriteRenderer>().color = Color.black;
            }


            //make the tower icon smaller so you can have a better idea of the placement of the tower
            transform.localScale = new Vector3(1f, 1f, monster.monsterIcon.transform.localScale.z);

            //GameManager.Instance.GetComponentInChildren<CameraMotion>().isFree = false;

            var position = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            //Debug.Log(position);
            var x = position.x;
            var y = position.y;
            transform.position = new Vector3(x, y, -2f);

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                isTileMap = false;
                if (isCorrectTile)
                {
                    ////add this monster's energy rate to the energy rate per second being generated by your active monsters
                    ////every second, add the monster's energy to the player's available energy for the map
                    //InvokeRepeating("TowerEnergy", 0, 1);


                    //Map.GetComponent<MapDetails>().MapEnergyRate(monster.tempStats.EnergyGeneration.Value / 60);
                    //Map.GetComponent<MapDetails>().UseMapEnergy(monster.tempStats.EnergyCost.Value);
                    //mapInformation.playerEnergy -= monster.tempStats.EnergyCost.Value;



                    ////set the current tile to hold this monster's data as the monster on that tile
                    //mapTileOn.MonsterOnTile(gameObject.GetComponent<Monster>());
                    ////creates local variables for the height of the monster's legs and the relative position to the monster's body the legs are

                    ////makes any sprites of the tower set to the monster sorting layer
                    //SpriteRenderer[] sprites = gameObject.GetComponentsInChildren<SpriteRenderer>();

                    //for (int s = 0; s < sprites.Length; s++)
                    //{
                    //    sprites[s].sortingLayerName = "Monster";
                    //}

                    ////changes the tiles back to the correct color and then the tower is ready for attack
                    //for (int m = 0; m < tiles.Length; m++)
                    //{
                    //    tiles[m].GetComponent<MapTile>().sp.color = tiles[m].GetComponent<MapTile>().tileColor;


                    //}

                    ////GameManager.Instance.GetComponentInChildren<CameraMotion>().isFree = true;
                    //mainCamera.GetComponent<CameraMotion>().isFree = true;
                    //gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("Map").transform);

                    ////adds the monster to the active towers dictionary
                    //int towerCount = GameManager.Instance.activeTowers.Count;
                    //GameManager.Instance.activeTowers.Add(towerCount, gameObject.GetComponent<Monster>());
                    //gameObject.GetComponent<Monster>().activeIndex = towerCount;

                    //isBeingPlaced = false;
                    //isPlaced = true;
                    //isIdle = true;
                    //AttackScan();

                    ////add this tower to the map's list of your active towers
                    //Map.GetComponent<MapDetails>().liveTowers.Add(monster);
                    ////every second, update this monster's entry in the active towers list
                    //StartCoroutine(ActiveTower(1f));

                    //transform.position = new Vector3(tilePlacementPosition.x, tilePlacementPosition.y + gameObject.GetComponent<RectTransform>().rect.height, -2f);
                    //gameObject.transform.localScale = new Vector3(1.7f, 1.7f, transform.localScale.z);
                    //transform.position = new Vector3(tilePlacementPosition.x, tilePlacementPosition.y + gameObject.GetComponent<RectTransform>().rect.height, -2f);

                    ////make the menu of your towers vanish
                    //towerMenu.SetActive(false);

                    ////set active this tower's canvas
                    //towerCanvas.SetActive(true);
                    PlaceTower();
                }
                //if the tower is placed at an ineligible space, revert it back to the manu and destroy the avatar/placeholder
                else
                {
                    for (int m = 0; m < tiles.Length; m++)
                    {
                        tiles[m].GetComponent<MapTile>().sp.color = tiles[m].GetComponent<MapTile>().tileColor;
                    }

                    var copy = GameObject.Find(gameObject.name + " Placeholder" + monster.info.index);
                    transform.position = copy.transform.position;
                    isBeingPlaced = false;
                    //mainCamera.GetComponent<CameraMotion>().isFree = true;
                    Destroy(copy);
                    isCopy = false;
                    towerMenu.SetActive(false);
                    //make the tower icon back to its original size
                    transform.localScale = new Vector3(transform.localScale.x * 3.5f, transform.localScale.y * 3.5f, transform.localScale.z);

                }
            }
            //    GameManager.Instance.GetComponentInChildren<CameraMotion>().isFree = false;
            //var worldMousePosition =
            //Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
            //var x = worldMousePosition.x;
            //var y = worldMousePosition.y;
            //transform.position = new Vector3(x, y, transform.position.z);





        }
    }

    //grid of the map shows of the eligbile tiles that a tower can be placed on
    public void CheckForPlacement()
    {

        if (!isTileMap)
        {
            isTileMap = true;
            Color colorYes = new Color(0.3f, 1.0f, 0.38f, 0.76f);
            Color colorNo = new Color(1.0f, 0.32f, 0.40f, 0.76f);

            //the tiles that it can be placed on glow green, while the tiles it can't be placed on glow red
            for (int m = 0; m < tiles.Length; m++)
            {
                if (tiles[m].GetComponent<MapTile>().isBuildable == true && !tiles[m].GetComponent<MapTile>().hasMonster)
                {
                    tiles[m].GetComponent<MapTile>().sp.color = colorYes;
                }
                else
                {
                    tiles[m].GetComponent<MapTile>().sp.color = colorNo;
                }


            }
        }
    }


    //use this to actually place the tower on the field
    public IEnumerator PlaceTower()
    {

        //Map = GameObject.FindGameObjectWithTag("Map");
        //mapDetails = Map.GetComponent<WorldMap>().mapDetails;
        ////mapInformation = Map.GetComponent<MapDetails>().mapInformation;
        //mapInformation = mapDetails.mapInformation;
        //infoMenu = Map.GetComponent<MonsterInfoMenus>().infoMenu;
        //towerMenu = Map.GetComponent<MonsterInfoMenus>().towerMenu;
        //levelTile = mapDetails.mapTile;



        //set the current tile to hold this monster's data as the monster on that tile
        mapTileOn.MonsterOnTile(gameObject.GetComponent<Monster>());
        tileOn = mapTileOn.tileNumber;

        tilePlacementPosition = new Vector2(mapTileOn.transform.position.x, mapTileOn.transform.position.y);
        //creates local variables for the height of the monster's legs and the relative position to the monster's body the legs are

        



        if (!isCopy)
        {

            isCopy = true;
            var copy = Instantiate(this.gameObject, transform.position, Quaternion.identity);
            //copy.GetComponent<Monster>().frontModel.SetActive(true);
            var menu = GameObject.Find("Content");
            copy.transform.SetParent(menu.transform, true);
            copy.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
            copy.name = gameObject.name + " Placeholder" + monster.info.index;
            copy.tag = "TowerAvatar";
            copy.layer = 12;
            //copy.GetComponent<Tower>().isPlaced = true;
            SpriteRenderer[] bodyparts = copy.GetComponentsInChildren<SpriteRenderer>();


            for (int i = 0; i < bodyparts.Length; i++)
            {
                bodyparts[i].color = new Color(bodyparts[i].color.r, bodyparts[i].color.g, bodyparts[i].color.b, .5f);
            }
            //copy.GetComponent<SpriteRenderer>().color = Color.black;
        }

        var summon = Instantiate(GameManager.Instance.summonAnimation, tilePlacementPosition, Quaternion.identity);
        summon.transform.localScale = new Vector2(summon.transform.localScale.x * 1.2f, summon.transform.localScale.y * 1.2f);
        summon.transform.position = new Vector3(tilePlacementPosition.x, tilePlacementPosition.y + gameObject.GetComponent<RectTransform>().rect.height, -2f);
        summon.GetComponent<SummonAnimation>().StartSummon(monster);

        //waits for the animation to be completed by the animator object before continuting
        yield return new WaitUntil(() => summonAnimationComplete == true);




        isBeingPlaced = false;
        isPlaced = true;
        monsterIcon.SetActive(false);
        boneStructure.SetActive(true);
        isIdle = true;
        AttackScan();
        


        isTileMap = false;
        //add this monster's energy rate to the energy rate per second being generated by your active monsters
      

        //if the game mode is a standard game, when a monster is summoned, start regenerating energy for other monsters
        if (GameManager.Instance.gameMode == GameMode.NormalMode)
        {
            mapDetails.MapEnergyRate(monster.info.EnergyGeneration.Value / 60);
            mapDetails.UseMapEnergy(monster.info.EnergyCost.Value);

            //every second, add the monster's energy to the player's available energy for the map
            InvokeRepeating("TowerEnergy", 0, 1);

            //add this tower to the map's list of your active towers
            mapDetails.liveTowers.Add(monster);

            //adds the monster to the active towers dictionary
            int towerCount = GameManager.Instance.activeTowers.Count;
            GameManager.Instance.activeTowers.Add(towerCount, gameObject.GetComponent<Monster>());
            gameObject.GetComponent<Monster>().activeIndex = towerCount;

            Map.GetComponent<MonsterInfoMenus>().towerMenu.SetActive(false);
            Map.GetComponent<MonsterInfoMenus>().showTowersBtn.gameObject.SetActive(true);
            Map.GetComponent<MonsterInfoMenus>().tileToBePlaced = null;

        }

        //what to do if the game is in Defender Mode
        if (GameManager.Instance.gameMode == GameMode.DefenderMode)
        {
                monster.info.defenderIndex = tileOn + 1;
                monster.SaveMonsterToken();

            //if the dictionary of Defenders doesn't have this monster already, add it. Also used to protect against dupliate entries
            if (!GameManager.Instance.GetComponent<YourMonsters>().yourDefendersDict.ContainsKey(monster.info.defenderIndex))
            {
                //add their index + 1 so that 0 can be a null check, as opposed to the 0th item in a list
                GameManager.Instance.GetComponent<YourMonsters>().yourDefendersDict.Add(monster.info.defenderIndex, JsonUtility.ToJson(monster.saveToken));
            }

            mapDetails.GetComponent<DefendersMain>().defenderInfo.tileList[tileOn].monsterOn = JsonUtility.ToJson(monster.saveToken);
            GameManager.Instance.GetComponent<YourAccount>().account.defenderDataString = JsonUtility.ToJson(mapDetails.GetComponent<DefendersMain>().defenderInfo);
        }




        mapInformation.playerEnergy -= monster.info.EnergyCost.Value;


        //makes any sprites of the tower set to the monster sorting layer
        SpriteRenderer[] sprites = gameObject.GetComponentsInChildren<SpriteRenderer>();

        for (int s = 0; s < sprites.Length; s++)
        {
            sprites[s].sortingLayerName = "Monster";

        }

        //changes the tiles back to the correct color and then the tower is ready for attack
        for (int m = 0; m < tiles.Length; m++)
        {
            tiles[m].GetComponent<MapTile>().sp.color = tiles[m].GetComponent<MapTile>().tileColor;


        }

        //GameManager.Instance.GetComponentInChildren<CameraMotion>().isFree = true;
        //mainCamera.GetComponent<CameraMotion>().isFree = true;
        gameObject.transform.SetParent(mapDetails.mapCanvas.transform);
        //gameObject.transform.SetParent(mapDetails.transform);

        


        transform.position = new Vector3(tilePlacementPosition.x, tilePlacementPosition.y + gameObject.GetComponent<RectTransform>().rect.height, -2f);
        gameObject.transform.localScale = new Vector3(1.2f, 1.2f, transform.localScale.z);
        transform.position = new Vector3(tilePlacementPosition.x, tilePlacementPosition.y + gameObject.GetComponent<RectTransform>().rect.height, -2f);


        GameManager.Instance.SendNotificationToPlayer(monster.info.name, 1, NotificationType.TowerSummon, tileOn.ToString());


       
        //set active this tower's canvas
        towerCanvas.SetActive(true);

        //active the monster's passive skill, unless the skill is activated upon a Tower Summon, because it will have activated already
        if (monster.info.passiveSkill.skill.triggerType != TriggerType.TowerSummon)
        {
            monster.PassiveSkill();
        }
        StartCoroutine(monster.WeatherCheck());
       

    }

    //this is used to add energy to the player's total for the map, as well as check to make sure this monster has enough energy to be summoned
    public void TowerEnergy()
    {
        if (isPlaced)
        {
            //Map.GetComponent<MapDetails>().AddMapEnergy(monster.energyGeneration / 60);
            mapDetails.AddMapEnergy(monster.tempStats.EnergyGeneration.Value / 60);
        }

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!this.enabled) return;

    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (!this.enabled) return;

        //var tag = other.gameObject.tag;

        //if (!isPlaced)
        //{
        //    gameObject.layer = 8;
        //}
    }




    //used to detect a new tile that this tower can be placed on. Stay and Exit are used to make sure only 1 tile can be marked as "correct" at once.
    private void OnTriggerStay2D(Collider2D other)
    {
        if (!this.enabled) return;



        if (isBeingPlaced)
        {
            var tag = other.gameObject.tag;



            if (isCorrectTile == false)
            {

                if (tag == "MapTile")
                {
                    Color colorYes = new Color(0.3f, 1.0f, 0.38f, 1.0f);

                    if (other.gameObject.GetComponent<MapTile>().isBuildable == true && !other.gameObject.GetComponent<MapTile>().hasMonster)
                    {
                        other.gameObject.GetComponent<MapTile>().sp.color = colorYes;
                        tilePlacementPosition = other.gameObject.transform.position;
                        tileRect = other.gameObject.GetComponent<RectTransform>();
                        tileOn = other.gameObject.GetComponent<MapTile>().tileNumber;
                        mapTileOn = other.gameObject.GetComponent<MapTile>();
                        isCorrectTile = true;
                    }
                }
            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (!this.enabled) return;

        //used to detect when this tower is moved off of an eliglbe tower

        var tag = other.gameObject.tag;

        if (isBeingPlaced)
        {

            if (tag == "MapTile")
            {
                Color colorYes = new Color(0.3f, 1.0f, 0.38f, 0.76f);

                if (other.gameObject.GetComponent<MapTile>().isBuildable == true)
                {

                    other.gameObject.GetComponent<MapTile>().sp.color = colorYes;
                    isCorrectTile = false;
                }
            }
        }


    }




   

    //this method is invoked by a tile when an enemy is in range from the MapTile script
    public void Attack(Monster Enemy, int tileTarget)
    {

        if (isScanning && !isAttacking)
        {


            Enemy enemy = Enemy.GetComponent<Enemy>();
            //change the direction of the tower if the enemy is on the opposite direction of this tower
           


            //start the monster's attack animation
            if (attackNumber == 1)
            {
                if (atkRange1List.Contains(enemy.currentTile))
                {
                    isAttacking = true;
                    monster.monsterMotion.SetBool("isAttacking", true);
                    

                    boneStructure.GetComponent<MotionControl>().AttackModeCheck(monster.info.baseAttack1.attackMode);
                    boneStructure.GetComponent<MotionControl>().AttackDirection(tileTarget, enemy);

                  

                    if (enemy.transform.position.x <= attackPoint.transform.position.x)
                    {
                        monster.puppet.flip = true;
                        

                    }
                    else
                    {
                        monster.puppet.flip = false;
                        
                    }


                }
            }
            else
            {
                if (atkRange2List.Contains(enemy.currentTile))
                {
                    isAttacking = true;
                    monster.monsterMotion.SetBool("isAttacking", true);
                    

                    boneStructure.GetComponent<MotionControl>().AttackModeCheck(monster.info.baseAttack2.attackMode);
                    boneStructure.GetComponent<MotionControl>().AttackDirection(tileTarget, enemy);

                    if (enemy.transform.position.x <= attackPoint.transform.position.x)
                    {
                        monster.puppet.flip = true;
                        
                    }
                    else
                    {
                        monster.puppet.flip = false;
                       
                    }

                }
            }


            

        }
    }

    //use this to launch the readied attack by the tower. called by the Montion Control for this monster
    public void LaunchAttack(int tileNumber, Enemy Enemy)
    {
        
        Enemy enemy = Enemy;

        if (enemy)
        {
            Vector3 position = enemy.transform.position;

            //change the direction of the tower if the enemy is on the opposite direction of this tower
            if (enemy.transform.position.x <= attackPoint.transform.position.x)
            {
                monster.puppet.flip = true;
               
            }
            else
            {
                monster.puppet.flip = false;
                
            }




            if (attackNumber == 1)
            {
                if (atkRange1List.Contains(tileNumber))
                {
                    MonsterAttack attack = monster.tempStats.attack1;
                    var attackSprite = Instantiate(attack1Animation, attackPoint.transform.position, Quaternion.identity);
                    attackSprite.gameObject.name = attack.name;
                    //attackSprite.GetComponent<AttackEffects>().FromAttacker(attack, attack.name, attack.type, monster.attack, (int)attack.Power.Value, monster.info.level, attack.CritChance.Value, attack.CritMod.Value, gameObject.GetComponent<Monster>());
                    attackSprite.GetComponent<AttackEffects>().FromAttacker(attack, attack.name, attack.type, monster.info.Attack.Value, (int)attack.Power.Value, monster.info.level, attack.CritChance.Value, attack.CritMod.Value, gameObject.GetComponent<Monster>());
                    attackSprite.GetComponent<AttackEffects>().AttackMotion(position - attackPoint.transform.position);
                    staminaBar.BarProgress += attack.staminaGained + (monster.info.Stamina.Value / 1000);

                    
                }


            }
            else
            {
                if (atkRange2List.Contains(tileNumber))
                {
                    MonsterAttack attack = monster.tempStats.attack2;
                    var attackSprite = Instantiate(attack2Animation, attackPoint.transform.position, Quaternion.identity);
                    attackSprite.gameObject.name = attack.name;
                    //attackSprite.GetComponent<AttackEffects>().FromAttacker(attack, attack.name, attack.type, monster.attack, (int)attack.Power.Value, monster.info.level, attack.CritChance.Value, attack.CritMod.Value, gameObject.GetComponent<Monster>());
                    attackSprite.GetComponent<AttackEffects>().FromAttacker(attack, attack.name, attack.type, monster.info.Attack.Value, (int)attack.Power.Value, monster.info.level, attack.CritChance.Value, attack.CritMod.Value, gameObject.GetComponent<Monster>());
                    attackSprite.GetComponent<AttackEffects>().AttackMotion(position - attackPoint.transform.position);
                    staminaBar.BarProgress += attack.staminaGained + (monster.info.Stamina.Value / 1000);
                }
            }

            //Debug.Log(monster.info.Attack.Value);
        }


        //if the ability is ready to be used, set it ready for activation
        if (staminaBar.BarProgress >= 1 && (monster.info.specialAbility.castingAmmo - monster.info.specialAbility.castingCount) > 0)
        {
            abilityReady = true;
            monster.abilityAura.SetActive(true);
        }
        
    }

    


    //DO SOMETHING HERE WHEN A MONSTER'S STAMINA GETS TO FULL
    public void SpecialAbility()
    {
        monster.abilityAura.SetActive(false);
        abilityReady = false;
        
        //if the monster's ability can still be used, use it
        if (monster.info.specialAbility.castingCount < monster.info.specialAbility.castingAmmo)
        {

            
            //add another casting count to the ability's total uses
            monster.info.specialAbility.castingCount += 1;

            //if the monster has any ammo left for the ability, reset your stanima gauge so the monster can charge up another
            if (monster.info.specialAbility.castingCount < monster.info.specialAbility.castingAmmo)
            {
                staminaBar.BarProgress = 0;
                
            }
            else
            {
                //
            }
        }
        
    }

    //method used to keep track of time between attacks
    public void AttackTimer()
    {
        if (!isAttacking && acumTime == 0)
        {
            AttackCheck();
        }

       
    }

    //this method creates the Lists of tile numbers that are included in the tower's attack ranges. Only runs once so that rays are not continuously being fired
    public void AttackScan()
    {

        atkRange1List.Clear();
        atkRange2List.Clear();

        var mapTiles = mapDetails.allTiles;


        var aimAngle = Mathf.Atan2(transform.position.x, transform.position.y); 
        aimAngle = Mathf.PI * 2 + (Time.time * 50);
        var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.up;


        int range = monster.info.baseAttack1.attack.range;
        int range2 = monster.info.baseAttack2.attack.range;

        //int range = 2;

        //for (int i = 0; i <= range; i++)
        //{

        //    //get the total and difference of your tile's row and column coordinate. filter through them to determine the range of an attack given its range3
        //    int total = tiles[tileOn].GetComponent<MapTile>().info.row + tiles[tileOn].GetComponent<MapTile>().info.column;
        //    int difference = tiles[tileOn].GetComponent<MapTile>().info.row - tiles[tileOn].GetComponent<MapTile>().info.column;

        //    for (int a = 0; a < mapTiles.Count; a++)
        //    {
        //        int check = tiles[a].GetComponent<MapTile>().info.row + tiles[a].GetComponent<MapTile>().info.column;
        //        int check2 = tiles[a].GetComponent<MapTile>().info.row - tiles[a].GetComponent<MapTile>().info.column;

        //        if (total <= check + (2 * range) && total >= check - (2 * range) && difference <= check2 + (2 * range) && difference >= check2 - (2 * range))
        //        {


        //            atkRange1List.Add(tiles[a].GetComponent<MapTile>().tileNumber);
        //            tiles[a].GetComponent<MapTile>().AttackRange(monster);
        //        }

        //        if (total <= check + (2 * range2) && total >= check - (2 * range2) && difference <= check2 + (2 * range2) && difference >= check2 - (2 * range2))
        //        {


        //            atkRange2List.Add(tiles[a].GetComponent<MapTile>().tileNumber);
        //            tiles[a].GetComponent<MapTile>().AttackRange(monster);
        //        }

        //        if (a >= tiles.Length - 1)
        //        {

        //            isScanning = true;
        //            attackNumber = 1;

        //            //GameManager.Instance.overworldMenu.GetComponent<OverworldInfoMenu>().activeMonster = monster;
        //        }
        //    }


        //}

        

            //get the total and difference of your tile's row and column coordinate. filter through them to determine the range of an attack given its range3
            int total = mapTiles[tileOn].info.row + mapTiles[tileOn].info.column;
            int difference = mapTiles[tileOn].info.row - mapTiles[tileOn].info.column;

            for (int a = 0; a < mapTiles.Count; a++)
            {
                int check = mapTiles[a].info.row + mapTiles[a].info.column;
                int check2 = mapTiles[a].info.row - mapTiles[a].info.column;

                if (total <= check + (2 * range) && total >= check - (2 * range) && difference <= check2 + (2 * range) && difference >= check2 - (2 * range))
                {

                    
                    atkRange1List.Add(mapTiles[a].tileNumber);
                    mapTiles[a].AttackRange(monster);
                }

                if (total <= check + (2 * range2) && total >= check - (2 * range2) && difference <= check2 + (2 * range2) && difference >= check2 - (2 * range2))
                {


                    atkRange2List.Add(mapTiles[a].tileNumber);
                    mapTiles[a].AttackRange(monster);
                }

                if (a >= tiles.Length - 1)
                {

                    isScanning = true;
                    attackNumber = 1;

                    //GameManager.Instance.overworldMenu.GetComponent<OverworldInfoMenu>().activeMonster = monster;
                }
            }


        

        //for (int i = 0; i <= range; i++)
        //{

        //    //get the total and difference of your tile's row and column coordinate. filter through them to determine the range of an attack given its range3
        //    int total = mapTiles[tileOn].info.row + mapTiles[tileOn].info.column;
        //    int difference = mapTiles[tileOn].info.row - mapTiles[tileOn].info.column;

        //    int a = 0;

        //    foreach(MapTile tile in mapTiles)
        //    {
        //        int number = tile.tileNumber;

        //        int check = mapTiles[number].info.row + mapTiles[number].info.column;
        //        int check2 = mapTiles[number].info.row - mapTiles[number].info.column;

        //        if (total <= check + (2 * range) && total >= check - (2 * range) && difference <= check2 + (2 * range) && difference >= check2 - (2 * range))
        //        {


        //            atkRange1List.Add(mapTiles[number].tileNumber);
        //            mapTiles[number].AttackRange(monster);
        //        }

        //        if (total <= check + (2 * range2) && total >= check - (2 * range2) && difference <= check2 + (2 * range2) && difference >= check2 - (2 * range2))
        //        {


        //            atkRange2List.Add(mapTiles[number].tileNumber);
        //            mapTiles[number].AttackRange(monster);
        //        }

        //        if (a >= mapTiles.Count - 1)
        //        {

        //            isScanning = true;
        //            attackNumber = 1;

        //            //GameManager.Instance.overworldMenu.GetComponent<OverworldInfoMenu>().activeMonster = monster;
        //        }
        //    }



        //}
    }
    //creates temporary tiles to act as a visible range for a tower's attack
    public void AttackRangeUI()
    {
        List<int> attackList = new List<int>();
        string type = "";
        string name = "";

        //GameObject[] rangeTiles = GameObject.FindGameObjectsWithTag("RangeTile");
        GameObject[] rangeTiles = GameObject.FindGameObjectsWithTag("RangeTile");

        if (rangeTiles.Length > 0)
        {
            for (int i = 0; i < rangeTiles.Length; i++)
            {
                Destroy(rangeTiles[i]);
            }
        }


        if (attackNumber == 1)
        {
            attackList = atkRange1List;
            type = monster.info.baseAttack1.type;
            name = monster.info.baseAttack1.attack.name;
        }
        if (attackNumber == 2)
        {
            attackList = atkRange2List;
            type = monster.info.baseAttack2.type;
            name = monster.info.baseAttack2.attack.name;
        }




        if (GameManager.Instance.typeColorDictionary.ContainsKey(type))
        {
            Color color = GameManager.Instance.typeColorDictionary[type];

            foreach (int target in attackList)
            {
                //var range = Instantiate(levelTile, transform.position, Quaternion.identity);
                //range.GetComponent<SpriteRenderer>().sortingOrder = 1000;
                //range.gameObject.tag = "RangeTile";
                //range.gameObject.name = name + "'s Range";
                ////range.transform.SetParent(Map.gameObject.transform, false);
                //range.transform.SetParent(mapDetails.transform, false);
                //range.transform.position = mapDetails.allTiles[target].transform.position;
                //range.GetComponent<MapTile>().ShowRange(color);

                mapDetails.allTiles[target].ShowRange(color);
            }
        }

        isAttacking = false;
        AttackCheck();
    }

    //stop showing the monster's attack ranges
    public void RangeUIOff()
    {
        GameObject[] rangeTiles = GameObject.FindGameObjectsWithTag("RangeTile");

        if (rangeTiles.Length > 0)
        {
            for (int i = 0; i < rangeTiles.Length; i++)
            {
                Destroy(rangeTiles[i]);
            }
        }
    }






    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerEnter)
        {
            var tag = eventData.pointerEnter.gameObject.tag;
            var hit = eventData.pointerEnter.gameObject;
            var name = eventData.pointerEnter.gameObject.name;



            if (tag == "Tower")
            {
                infoMenu.SetActive(true);

                int index = hit.gameObject.GetComponent<Monster>().info.index;
                Map.GetComponent<MonsterInfoMenus>().activeMonster = hit.gameObject.GetComponent<Monster>();
                //Map.GetComponent<MonsterInfoMenus>().activeMonster = GameManager.Instance.GetComponent<YourMonsters>().yourMonstersComplete[index];
            }

            if (tag == "RangeTile")
            {
                RangeUIOff();
            }

        }
    }



        public void AttackCheck()
    {

        List<TargetSort> inRange = new List<TargetSort>();
        int n = 0;
        //check the enemies this monster can hit and determine which enemy to attack
        foreach (Enemy liveEnemy in mapDetails.liveEnemies)
        {
            //if the enemy still exists, check to see if it's on a tile that this monster can hit
            if (liveEnemy)
            {
                if (attackNumber == 1)
                {
                    if (atkRange1List.Contains(liveEnemy.currentTile))
                    {
                        TargetSort sort = new TargetSort();
                        sort.enemy = liveEnemy;
                        inRange.Add(sort);
                        //n += 1;
                        //Debug.Log("In Range: " + inRange.Count + " N: " + n);
                    }
                }
                else if (attackNumber == 2)
                {
                    if (atkRange2List.Contains(liveEnemy.currentTile))
                    {
                        TargetSort sort = new TargetSort();
                        sort.enemy = liveEnemy;
                        inRange.Add(sort);
                        //n += 1;
                        //Debug.Log("In Range: " + inRange.Count + " N: " + n);
                    }
                }
                //if (atkRange1List.Contains(liveEnemy.currentTile) || atkRange2List.Contains(liveEnemy.currentTile))
                //{
                //    TargetSort sort = new TargetSort();
                //    sort.enemy = liveEnemy;
                //    inRange.Add(sort);
                //    //n += 1;
                //    //Debug.Log("In Range: " + inRange.Count + " N: " + n);
                //}

                //else
                //{
                //    //enemiesInRange.Remove(liveEnemy);
                //}
            }
            else
            {
                //acumTime = 0;
                //isAttacking = false;
            }
            
        }

        //for every monster that this monster can hit, sort them based on the monster's target type
        foreach (TargetSort sort in inRange)
        {
            if (targetMode == TargetMode.MostHP)
            {

                inRange.Sort(delegate (TargetSort x, TargetSort y)
                {
                    if (x.enemy.monster.info.currentHP == y.enemy.monster.info.currentHP) return 0;
                    else if (x.enemy.monster.info.currentHP < y.enemy.monster.info.currentHP) return 1;
                    else if (x.enemy.monster.info.currentHP > y.enemy.monster.info.currentHP) return -1;
                    else return x.enemy.monster.info.currentHP.CompareTo(y.enemy.monster.info.currentHP);
                });
            }
            else if (targetMode == TargetMode.LeastHP)
            {
                inRange.Sort(delegate (TargetSort x, TargetSort y)
                {
                    if (x.enemy.monster.info.currentHP == y.enemy.monster.info.currentHP) return 0;
                    else if (x.enemy.monster.info.currentHP > y.enemy.monster.info.currentHP) return 1;
                    else if (x.enemy.monster.info.currentHP < y.enemy.monster.info.currentHP) return -1;
                    else return x.enemy.monster.info.currentHP.CompareTo(y.enemy.monster.info.currentHP);
                });
            }

            else if (targetMode == TargetMode.Strongest)
            {
                inRange.Sort(delegate (TargetSort x, TargetSort y)
                {
                    if (x.enemy.monster.info.level == y.enemy.monster.info.level) return 0;
                    else if (x.enemy.monster.info.level < y.enemy.monster.info.level) return 1;
                    else if (x.enemy.monster.info.level > y.enemy.monster.info.level) return -1;
                    else return x.enemy.monster.info.level.CompareTo(y.enemy.monster.info.level);
                });
            }

            else if (targetMode == TargetMode.Weakest)
            {
                inRange.Sort(delegate (TargetSort x, TargetSort y)
                {
                    if (x.enemy.monster.info.level == y.enemy.monster.info.level) return 0;
                    else if (x.enemy.monster.info.level > y.enemy.monster.info.level) return 1;
                    else if (x.enemy.monster.info.level < y.enemy.monster.info.level) return -1;
                    else return x.enemy.monster.info.level.CompareTo(y.enemy.monster.info.level);
                });
            }

            else if (targetMode == TargetMode.Closest)
            {
                inRange.Sort(delegate (TargetSort x, TargetSort y)
                {
                    double xDiff = Math.Sqrt(Math.Pow((x.enemy.transform.position.x - attackPoint.transform.position.x), 2) + Math.Pow((x.enemy.transform.position.y - attackPoint.transform.position.y), 2));
                    double yDiff = Math.Sqrt(Math.Pow((y.enemy.transform.position.x - attackPoint.transform.position.x), 2) + Math.Pow((y.enemy.transform.position.y - attackPoint.transform.position.y), 2));

                    if (xDiff == yDiff) return 0;
                    else if (xDiff > yDiff) return 1;
                    else if (xDiff < yDiff) return -1;
                    else return xDiff.CompareTo(yDiff);
                });
            }
            else if (targetMode == TargetMode.Furthest)
            {
                inRange.Sort(delegate (TargetSort x, TargetSort y)
                {
                    double xDiff = Math.Sqrt(Math.Pow((x.enemy.transform.position.x - attackPoint.transform.position.x), 2) + Math.Pow((x.enemy.transform.position.y - attackPoint.transform.position.y), 2));
                    double yDiff = Math.Sqrt(Math.Pow((y.enemy.transform.position.x - attackPoint.transform.position.x), 2) + Math.Pow((y.enemy.transform.position.y - attackPoint.transform.position.y), 2));

                    if (xDiff == yDiff) return 0;
                    else if (xDiff < yDiff) return 1;
                    else if (xDiff > yDiff) return -1;
                    else return xDiff.CompareTo(yDiff);
                });
            }
            else if (targetMode == TargetMode.Newest)
            {
                inRange.Sort(delegate (TargetSort x, TargetSort y)
                {
                    if (x.enemy.currentIndex == y.enemy.currentIndex) return 0;
                    else if (x.enemy.currentIndex > y.enemy.currentIndex) return 1;
                    else if (x.enemy.currentIndex < y.enemy.currentIndex) return -1;
                    else return x.enemy.monster.info.level.CompareTo(y.enemy.monster.info.level);
                });
            }

            else if (targetMode == TargetMode.Oldest)
            {
                inRange.Sort(delegate (TargetSort x, TargetSort y)
                {
                    if (x.enemy.currentIndex == y.enemy.currentIndex) return 0;
                    else if (x.enemy.currentIndex < y.enemy.currentIndex) return 1;
                    else if (x.enemy.currentIndex > y.enemy.currentIndex) return -1;
                    else return x.enemy.monster.info.level.CompareTo(y.enemy.monster.info.level);
                });
            }


            
            if (n == inRange.Count - 1)
            
                {


                for (int r = 0; r < inRange.Count; r++)
                {
                    if (inRange[r].enemy.GetComponent<Monster>())
                    {
                        Attack(inRange[r].enemy.GetComponent<Monster>(), inRange[0].enemy.currentTile);

                        break;
                    }
                }

                
                //Attack(inRange[0].enemy.GetComponent<Monster>(), inRange[0].enemy.currentTile);

                //targetOrder.Clear();
                //n = 0;
                //int a = 0;

                ////for each of the sorted enemies, the first monster in the sorted order becomes the target of the tower
                //foreach (TargetSort e in inRange)
                //{

                //    targetOrder.Add(e.enemy);
                //    a += 1;

                //    if (a >= inRange.Count - 1)
                //    {
                //        if (targetOrder[0] != null)
                //        {
                //            Attack(targetOrder[0].GetComponent<Monster>(), targetOrder[0].currentTile);

                //        }
                //        a = 0;
                //    }
                //}
            }
            


            n += 1;
        }
        

        //isAttacking = true;


        ////change the direction of the tower if the enemy is on the opposite direction of this tower
        //if (enemy.transform.position.x <= attackPoint.transform.position.x)
        //{
        //    monster.puppet.flip = true;

        //}
        //else
        //{
        //    monster.puppet.flip = false;
        //}



        ////start the monster's attack animation
        //if (attackNumber == 1)
        //{
        //    if (atkRange1List.Contains(enemy.currentTile))
        //    {
        //        monster.monsterMotion.SetBool("isAttacking", true);
        //        boneStructure.GetComponent<MotionControl>().AttackDirection(target.tileNumber, enemy);
        //    }
        //}
        //else
        //{
        //    if (atkRange2List.Contains(enemy.currentTile))
        //    {

        //        monster.monsterMotion.SetBool("isAttacking", true);
        //        boneStructure.GetComponent<MotionControl>().AttackDirection(target.tileNumber, enemy);

        //    }
        //}


    }

    //when the monster becomes an active tower, update the active towers dictionary in GameManager every second with this monster's information
    //public IEnumerator ActiveTower(float time)
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(time);
    //        GameManager.Instance.activeTowers.Remove(monster.activeIndex);
    //        GameManager.Instance.activeTowers.Add(monster.activeIndex, monster);
    //    }
    //}

}

public class TargetSort : IEquatable<TargetSort>, IComparable<TargetSort>
{
    public Enemy enemy;

    public override string ToString()
    {
        return "ID: " + enemy.GetComponent<Monster>().info.dexId + "   Name: " + enemy.GetComponent<Monster>().info.name;
    }
    public override bool Equals(object obj)
    {
        if (obj == null) return false;
        TargetSort objAsPart = obj as TargetSort;
        if (objAsPart == null) return false;
        else return Equals(objAsPart);
    }
    public int SortByNameAscending(string name1, string name2)
    {

        return name1.CompareTo(name2);
    }

    // Default comparer for Part type.
    public int CompareTo(TargetSort comparePart)
    {
        // A null value means that this object is greater.
        if (comparePart == null)
            return 1;

        else
            //return this.enemy.monster.info.currentHP.CompareTo(comparePart.enemy.monster.info.currentHP);
            //return CompareTo(comparePart);
            return 0;
    }
    public override int GetHashCode()
    {
        //return enemy.GetComponent<Monster>().info.level;
        return enemy.GetHashCode();
    }
    public bool Equals(TargetSort other)
    {
        if (other == null) return false;
        //return (this.enemy.GetComponent<Monster>().info.level.Equals(other.enemy.GetComponent<Monster>().info.level));
        return true;
    }
    // Should also override == and != operators.
}



