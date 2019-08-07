using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Tower : MonoBehaviour, IPointerDownHandler
{
    private int columns, rows;
    private float height, width;

    private Map Map;
    private GameObject levelTile;

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

    //private gameObject for the induvidual tiles on the map
    private GameObject[] maps;

    //private GameObject for the tower overlay UI menu
    private GameObject towerMenu;
    private GameObject infoMenu;
    private GameObject mainCamera;

    //starting position for the tower on the menu, so if it's placed on an incorrect tile, it'll snap back to this position
    public Vector2 menuPosition;


    //the monster's info and body specs
    private Monster monster;
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



    // Start is called before the first frame update
    void Start()
    {
        monster = GetComponent<Monster>();
        specs = monster.specs;

        maps = GameObject.FindGameObjectsWithTag("MapTile");
        mainCamera = GameObject.Find("Main Camera");
        Map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();
        infoMenu = Map.GetComponent<MonsterInfoMenus>().infoMenu;
        towerMenu = Map.GetComponent<MonsterInfoMenus>().towerMenu;
        levelTile = Map.mapTile;

        //load the monster's info as a json string
        string json = JsonUtility.ToJson(GetComponent<Monster>().info);
        monsterMotion = GetComponent<Animator>();

        var animationsDict = GameManager.Instance.baseAttacks.attackAnimationsDict;

        //set the animations for each attack based on the attack's name
        if (animationsDict.ContainsKey(monster.info.attack1.name))
        {
            attack1Animation = animationsDict[monster.info.attack1.name];
        }



    }

    // Update is called once per frame
    void Update()
    {
       
            //if the tower has already been placed, then there is no need to run the methods that have to do with the placement of the tower
            if (isPlaced == false)
            {
                
                TowerPlacement();

            }




            if (isIdle)
            {
                monsterMotion.SetBool("isIdle", true);
                
            }
            else
            {
                monsterMotion.SetBool("isIdle", false);
            }

            //use this to scan for attack ranges for incoming enemies
            if (isScanning)
            {
                AttackTimer();
            }
        

        if (isPlaced == true)
        {
            TowerInfo();

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


        for (var i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                touchTime = Time.time;
                
            }
            float acumTime = Time.time - touchTime;

            if (Input.GetTouch(i).phase == TouchPhase.Ended)
            {
                releaseTime = Time.time;
                acumTime = releaseTime - touchTime;

                //shoot a raycast that does not hit the map tile layer, which is layer 9
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position), Vector2.zero, 0f, 1 <<8);
                //What to do on a tap
                if (acumTime <= .6f)
                {
                    isTapped = true;


                    // RaycastHit2D can be either true or null, but has an implicit conversion to bool, so we can use it like this
                    if (hit)
                    {
                        string itemHit = (hit.transform.gameObject.tag);

                        //if you tap a monster on the menu that has all of your towers, it becomes the "active monster" on the screen and it's stats are displayed
                        if (hit.collider.gameObject.name == gameObject.name)
                        {
                            infoMenu.SetActive(true);
                            Map.GetComponent<MonsterInfoMenus>().activeMonster = hit.collider.gameObject.GetComponent<Monster>();
                            //GameManager.Instance.overworldMenu.SetActive(true);
                            //GameManager.Instance.overworldMenu.GetComponentInChildren<OverworldInfoMenu>().activeMonster = hit.collider.gameObject.GetComponent<Monster>();
                        }
                    }
                }
                //What do to on a hold
                //else
                //{
                //    isTapped = false;
                //    if (isBeingPlaced == false)
                //    {
                //        if (hit)
                //        {
                //            if (hit.collider != null)
                //            {
                //                if (hit.collider.gameObject.name == gameObject.name)
                //                {
                //                    isBeingPlaced = true;
                //                    var position = Input.GetTouch(i).position;
                //                    Debug.Log(position);
                //                    var x = position.x;
                //                    var y = position.y;
                //                    transform.position = new Vector3(x, y, transform.position.z);

                //                }
                //            }
                //        }
                //    }

                //}
            }
            else
            {
                //shoot a raycast that does not hit the map tile layer, which is layer 9
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position), Vector2.zero, 0f, 1 <<8);


                isTapped = false;
                if (isBeingPlaced == false)
                {
                    if (hit)
                    {
                        if (hit.collider != null)
                        {
                            if (hit.collider.gameObject.name == gameObject.name)
                            {
                                isBeingPlaced = true;
                                Map.GetComponent<MonsterInfoMenus>().TowerMenuBtn();
                                infoMenu.SetActive(true);
                                Map.GetComponent<MonsterInfoMenus>().activeMonster = hit.collider.gameObject.GetComponent<Monster>();
                            }
                        }
                    }
                }

            }

        }

        //if the tower is being placed, it's movement is that of the mouse
        if (isBeingPlaced == true)
        {
            CheckForPlacement();
            mainCamera.GetComponent<CameraMotion>().isFree = false;
            //creates a copy of the tower in the menu, so when the tower is placed, the player knows that it has been placed. 
            if (!isCopy)
            {

                isCopy = true;
                var copy = Instantiate(this.gameObject, transform.position, Quaternion.identity);
                var menu = GameObject.Find("Content");
                copy.transform.SetParent(menu.transform, true);
                copy.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
                copy.name = gameObject.name + " Placeholder" + monster.info.index;
                copy.tag = "TowerAvatar";
                copy.layer = 12;
                copy.GetComponent<Tower>().isPlaced = true;
                SpriteRenderer[] bodyparts = copy.GetComponentsInChildren<SpriteRenderer>();

               
                for (int i = 0; i < bodyparts.Length; i++)
                {
                    bodyparts[i].color = Color.black;
                }
                //copy.GetComponent<SpriteRenderer>().color = Color.black;
            }


            //GameManager.Instance.GetComponentInChildren<CameraMotion>().isFree = false;
            
            var position = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            //Debug.Log(position);
            var x = position.x;
            var y = position.y;
            transform.position = new Vector3(x, y, transform.position.z);

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                isTileMap = false;
                if (isCorrectTile)
                {
                    //gameObject.transform.localScale = new Vector3(transform.localScale.x * .66f, transform.localScale.y * .66f, transform.localScale.z);

                    //creates local variables for the height of the monster's legs and the relative position to the monster's body the legs are
                    float legHeight = new float();
                    Vector2 legPos = new Vector2();


                    //loops through all of the monsters legs to get their average height
                    for (int i = 0; i < specs.legs.Length; i++)
                    {
                        specs.legPos[i] = specs.legs[i].transform.position;
                        legHeight += specs.legs[i].GetComponent<RectTransform>().rect.height;
                        legPos = specs.legs[0].transform.localPosition;
                        
                    }
                    //gets the average height of the monsters legs
                    legHeight = legHeight / specs.legs.Length;

                    //places the monster on the tile that is selected, at a position off-set relative to the height of the monster's legs
                    //transform.position = new Vector2(tilePlacementPosition.x, tilePlacementPosition.y - ((legHeight / specs.legs[0].transform.localScale.y) * legPos.y));
                    transform.position = new Vector2(tilePlacementPosition.x, tilePlacementPosition.y - ((legHeight / specs.legs[0].transform.localScale.y) * legPos.y));
                    
                   
                    //make the feet of the monster unable to move so they act as an anchor for the monster
                    for (int m = 0; m < specs.legs.Length; m++)
                    {
                        specs.legs[m].GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                        specs.legs[m].GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                    }




                    //changes the tils back to the correct color and then the tower is ready for attack
                    for (int m = 0; m < maps.Length; m++)
                    {
                        maps[m].GetComponent<MapTile>().sp.color = maps[m].GetComponent<MapTile>().tileColor;


                    }

                    //GameManager.Instance.GetComponentInChildren<CameraMotion>().isFree = true;
                    mainCamera.GetComponent<CameraMotion>().isFree = true;
                    gameObject.transform.SetParent(GameObject.FindGameObjectWithTag("Map").transform);
                    

                    int towerCount = GameManager.Instance.activeTowers.Count;
                    GameManager.Instance.activeTowers.Add(towerCount, gameObject.GetComponent<Monster>());
                    gameObject.GetComponent<Monster>().activeIndex = towerCount;

                    isBeingPlaced = false;
                    isPlaced = true;
                    isIdle = true;
                    AttackScan();

                    gameObject.transform.localScale = new Vector3(2f, 2f, transform.localScale.z);
                    
                    towerMenu.SetActive(false);

                }
                //if the tower is placed at an ineligible space, revert it back to the manu and destroy the avatar/placeholder
                else
                {
                    for (int m = 0; m < maps.Length; m++)
                    {
                        maps[m].GetComponent<MapTile>().sp.color = maps[m].GetComponent<MapTile>().tileColor;
                    }

                    var copy = GameObject.Find(gameObject.name + " Placeholder" + monster.info.index);
                    transform.position = copy.transform.position;
                    isBeingPlaced = false;
                    mainCamera.GetComponent<CameraMotion>().isFree = true;
                    Destroy(copy);
                    isCopy = false;
                    towerMenu.SetActive(false);

                }
            }
            //    GameManager.Instance.GetComponentInChildren<CameraMotion>().isFree = false;
            //var worldMousePosition =
            //Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
            //var x = worldMousePosition.x;
            //var y = worldMousePosition.y;
            //transform.position = new Vector3(x, y, transform.position.z);




            //if (Input.GetMouseButtonDown(0))
            //{
            //    if (isBeingPlaced == false)
            //    {
            //        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            //        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            //       //if this tower is clicked on, it can be placed
            //        if (hit.collider != null)
            //        {
            //            if (hit.collider.gameObject.name == gameObject.name)
            //            {
            //                isBeingPlaced = true;

            //            }
            //        }
            //        //
            //    }

            //    if (isBeingPlaced == true)
            //    {
            //        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            //        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            //        CheckForPlacement();
            //        //if (hit.collider != null)
            //        //{
            //        //    if (hit.collider.gameObject.name == gameObject.name)
            //        //    {

            //        //    }
            //        //}
            //    }
            //}
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
            for (int m = 0; m < maps.Length; m++)
            {
                if (maps[m].GetComponent<MapTile>().isBuildable == true)
                {
                    maps[m].GetComponent<MapTile>().sp.color = colorYes;
                }
                else
                {
                    maps[m].GetComponent<MapTile>().sp.color = colorNo;
                }


            }
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

        

        if (!isPlaced)
        {
            var tag = other.gameObject.tag;


           
            if (isCorrectTile == false)
            {

                if (tag == "MapTile")
                {
                    Color colorYes = new Color(0.3f, 1.0f, 0.38f, 1.0f);

                    if (other.gameObject.GetComponent<MapTile>().isBuildable == true)
                    {
                        other.gameObject.GetComponent<MapTile>().sp.color = colorYes;
                        tilePlacementPosition = other.gameObject.transform.position;
                        tileRect = other.gameObject.GetComponent<RectTransform>();
                        tileOn = other.gameObject.GetComponent<MapTile>().tileNumber;
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

        if (!isPlaced)
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

    


    //once the tower is placed, allow it to function like one.
    public void TowerInfo()
    {

        if (!infoMenu.activeSelf)
        {

            //these are the same thing, but I am using GetMouse for Unity/WebGL and the Touch for mobile
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.name == gameObject.name)
                    {
                        infoMenu.SetActive(true);
                        Map.GetComponent<MonsterInfoMenus>().activeMonster = hit.collider.gameObject.GetComponent<Monster>();

                        //if (hit.collider.gameObject.GetComponent<Monster>().isNewMonster)
                        //{
                        //GameManager.Instance.overworldMenu.GetComponent<OverworldInfoMenu>().DisplayManager();
                        //}
                    }
                }
            }
        }



        //if (Input.touchCount == 1)
        //{
        //    Debug.Log("okay");

        //}




    }

    //this method is invoked by a tile when an enemy is in range from the MapTile script
    public void Attack(Monster target)
    {
        if (isScanning && !isAttacking)
        {
            Enemy enemy = target.GetComponent<Enemy>();
            float def = enemy.stats.def;
            float hp = enemy.stats.currentHp;
            string type1 = enemy.stats.type1;
            string type2 = enemy.stats.type2;

            if (attackNumber == 1)
            {
                if (atkRange1List.Contains(enemy.currentTile))
                {
                    isAttacking = true;
                    BaseAttack attack = monster.info.attack1;
                    var attackSprite = Instantiate(attack1Animation, transform.position, Quaternion.identity);
                    attackSprite.GetComponent<AttackEffects>().AttackMotion(enemy.transform.position - gameObject.transform.position);
                    attackSprite.gameObject.name = attack.name;
                    attackSprite.GetComponent<AttackEffects>().FromAttacker(attack.name, attack.type, monster.info.Attack.Value, (int)attack.Power.Value, monster.info.level, attack.CritChance.Value, attack.CritMod.Value, gameObject.GetComponent<Monster>());
                    Debug.Log(monster.info.Attack.Value);
                }
            }
            else
            {
                if (atkRange2List.Contains(enemy.currentTile))
                {
                    isAttacking = true;
                    BaseAttack attack = monster.info.attack2;
                    var attackSprite = Instantiate(attack1Animation, transform.position, Quaternion.identity);
                    attackSprite.GetComponent<AttackEffects>().AttackMotion(enemy.transform.position - gameObject.transform.position);
                    attackSprite.gameObject.name = attack.name;
                    attackSprite.GetComponent<AttackEffects>().FromAttacker(attack.name, attack.type, monster.info.Attack.Value, (int)attack.Power.Value, monster.info.level, attack.CritChance.Value, attack.CritMod.Value, gameObject.GetComponent<Monster>());
                    Debug.Log(monster.info.Attack.Value);
                }
            }

            
        }
    }

    //method used to keep track of time between attacks
    public void AttackTimer()
    {
        float attackTime = new float();

        if (isAttacking)
        {
            acumTime += Time.deltaTime;

            if (attackNumber == 1)
            {
                attackTime = monster.info.attack1.attackTime;
            }
            else
            {
                attackTime = monster.info.attack2.attackTime; 
            }

            if (acumTime >= attackTime)
            {
                isAttacking = false;
                acumTime = 0;
            }

        }
    }

    //this method creates the Lists of tile numbers that are included in the tower's attack ranges. Only runs once so that rays are not continuously being fired
    public void AttackScan()
    {

        atkRange1List.Clear();
        atkRange2List.Clear();


        var aimAngle = Mathf.Atan2(transform.position.x, transform.position.y);
        aimAngle = Mathf.PI * 2 + (Time.time * 50);
        var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.up;


        int range = monster.info.attack1.range;
        int range2 = monster.info.attack2.range;

        //int range = 2;


        for (int r = 0; r <= range; r++)
        {
            MapTile down = maps[tileOn].GetComponent<MapTile>();
            MapTile up = maps[tileOn].GetComponent<MapTile>();
            MapTile left = maps[tileOn].GetComponent<MapTile>();
            MapTile right = maps[tileOn].GetComponent<MapTile>();


            //MapTile down = maps[tileOn + 26 * r].GetComponent<MapTile>();
            //atkRange1List.Add(down.tileNumber);
            //down.AttackRange(monster);
            //MapTile up = maps[tileOn - 26 * r].GetComponent<MapTile>();
            //atkRange1List.Add(up.tileNumber);
            //up.AttackRange(monster);
            //MapTile left = maps[tileOn - 2 * r].GetComponent<MapTile>();
            //atkRange1List.Add(left.tileNumber);
            //left.AttackRange(monster);
            //MapTile right = maps[tileOn + (2 * r)].GetComponent<MapTile>();
            //atkRange1List.Add(right.tileNumber);
            //right.AttackRange(monster);


            if (tileOn + 2 * r < maps.Length)
            {
                down = maps[tileOn + 2 * r].GetComponent<MapTile>();

                if (down.transform.position.y > transform.position.y)
                {
                    down = maps[tileOn + 2].GetComponent<MapTile>();
                }

                if (down.transform.position.y > transform.position.y)
                {
                    down = maps[tileOn].GetComponent<MapTile>();
                }
            }
            atkRange1List.Add(down.tileNumber);
            down.AttackRange(monster);

            if (tileOn - 2 * r >= 0)
            {
                up = maps[tileOn - 2 * r].GetComponent<MapTile>();

                if (up.transform.position.y < transform.position.y)
                {
                    up = maps[tileOn - 2].GetComponent<MapTile>();
                }

                if (up.transform.position.y < transform.position.y)
                {
                    up = maps[tileOn].GetComponent<MapTile>();
                }
            }
            atkRange1List.Add(up.tileNumber);
            up.AttackRange(monster);

            if (tileOn - 28 * r >= 0)
            {
                left = maps[tileOn - 28 * r].GetComponent<MapTile>();

              
                    if (left.transform.position.x > transform.position.x)
                    {
                        left = maps[tileOn - 28].GetComponent<MapTile>();

                        if (left.transform.position.x > transform.position.x)
                        {
                            left = maps[tileOn].GetComponent<MapTile>();

                        }
                    }
            }

            atkRange1List.Add(left.tileNumber);
            left.AttackRange(monster);


            if (tileOn + (28 * r) < maps.Length)
            {
                right = maps[tileOn + (28 * r)].GetComponent<MapTile>();


                if (right.transform.position.x < transform.position.x)
                {
                    right = maps[tileOn + 28].GetComponent<MapTile>();


                    if (right.transform.position.x < transform.position.x)
                    {
                        right = maps[tileOn].GetComponent<MapTile>();

                    }

                }

            }

            atkRange1List.Add(right.tileNumber);
            right.AttackRange(monster);




            RaycastHit2D[] hitVert = Physics2D.RaycastAll(down.transform.position, Vector2.up, up.transform.position.y - down.transform.position.y);
            RaycastHit2D[] hitHorz = Physics2D.RaycastAll(right.transform.position, Vector2.left, right.transform.position.x - left.transform.position.x);


            //RaycastHit2D[] hitLeftUp = Physics2D.RaycastAll(up.transform.position, new Vector2(-1f, -.58f), (tileRect.localScale.x * r));
            RaycastHit2D[] hitLeftUp = Physics2D.RaycastAll(left.transform.position, new Vector2(up.transform.position.x - left.transform.position.x, up.transform.position.y - left.transform.position.y), (tileRect.localScale.x * r));


            //RaycastHit2D[] hitRightUp = Physics2D.RaycastAll(up.transform.position, new Vector2(1f, -.58f), (tileRect.localScale.x * r));
            RaycastHit2D[] hitRightUp = Physics2D.RaycastAll(up.transform.position, new Vector2(right.transform.position.x - up.transform.position.x, right.transform.position.y - up.transform.position.y), (tileRect.localScale.x * r));


            //RaycastHit2D[] hitLeftDown = Physics2D.RaycastAll(down.transform.position, new Vector2(-1f, .58f), (tileRect.localScale.x * r));
            RaycastHit2D[] hitLeftDown = Physics2D.RaycastAll(down.transform.position, new Vector2(left.transform.position.x - down.transform.position.x, left.transform.position.y - down.transform.position.y), (tileRect.localScale.x * r));


            //RaycastHit2D[] hitRightDown = Physics2D.RaycastAll(down.transform.position, new Vector2(1f, .58f), (tileRect.localScale.x * r));
            RaycastHit2D[] hitRightDown = Physics2D.RaycastAll(right.transform.position, new Vector2(down.transform.position.x - right.transform.position.x, down.transform.position.y - right.transform.position.y), (tileRect.localScale.x * r));



            if (maps[tileOn].GetComponent<MapTile>() == down)
            {
                hitLeftUp = Physics2D.RaycastAll(up.transform.position, new Vector2(-1f, -.58f), (tileRect.localScale.x * r - 1));
                hitRightUp = Physics2D.RaycastAll(up.transform.position, new Vector2(1f, -.58f), (tileRect.localScale.x * r - 1));
                hitLeftDown = Physics2D.RaycastAll(left.transform.position, new Vector2(1f, -.58f), (tileRect.localScale.x * r));
                hitRightDown = Physics2D.RaycastAll(right.transform.position, new Vector2(-1f, -.58f), (tileRect.localScale.x * r));
            }

            if (maps[tileOn].GetComponent<MapTile>() == up)
            {
                hitLeftUp = Physics2D.RaycastAll(left.transform.position, new Vector2(1f, .58f), tileRect.localScale.x * r);
                hitRightUp = Physics2D.RaycastAll(right.transform.position, new Vector2(-1f, .58f), tileRect.localScale.x * r);
                hitLeftDown = Physics2D.RaycastAll(down.transform.position, new Vector2(-1f, .58f), (tileRect.localScale.x * r - 1));
                hitRightDown = Physics2D.RaycastAll(down.transform.position, new Vector2(1f, .58f), (tileRect.localScale.x * r - 1));
            }

            if (right.transform.position.x - maps[tileOn].transform.position.x > maps[tileOn].transform.position.x - left.transform.position.x)
            {
                hitLeftUp = Physics2D.RaycastAll(up.transform.position, new Vector2(-1f, -.58f), tileRect.localScale.x * r);
                hitLeftDown = Physics2D.RaycastAll(down.transform.position, new Vector2(-1f, .58f), (tileRect.localScale.x * r));
            }

            if (right.transform.position.x - maps[tileOn].transform.position.x < maps[tileOn].transform.position.x - left.transform.position.x)
            {
                hitRightUp = Physics2D.RaycastAll(up.transform.position, new Vector2(1f, -.58f), tileRect.localScale.x * r);
                hitRightDown = Physics2D.RaycastAll(down.transform.position, new Vector2(1f, .58f), (tileRect.localScale.x * r));
            }


            if (down.transform.position.y - maps[tileOn].transform.position.y < maps[tileOn].transform.position.y - up.transform.position.y)
            {
                hitLeftUp = Physics2D.RaycastAll(left.transform.position, new Vector2(1f, .58f), tileRect.localScale.x * r);
                hitRightUp = Physics2D.RaycastAll(right.transform.position, new Vector2(-1f, .58f), (tileRect.localScale.x * r));
            }

            if (down.transform.position.y - maps[tileOn].transform.position.y > maps[tileOn].transform.position.y - up.transform.position.y)
            {
                hitLeftDown = Physics2D.RaycastAll(left.transform.position, new Vector2(1f, -.58f), tileRect.localScale.x * r);
                hitRightDown = Physics2D.RaycastAll(right.transform.position, new Vector2(-1f, -.58f), (tileRect.localScale.x * r));

               
            }
           

     


            for (int i = 0; i < hitVert.Length; i++)
            {
                if (hitVert[i].collider.gameObject.tag == "MapTile")
                {
                    //hitVert[i].collider.gameObject.GetComponent<MapTile>().sp.color = Color.blue;
                    atkRange1List.Add(hitVert[i].collider.gameObject.GetComponent<MapTile>().tileNumber);
                    hitVert[i].collider.gameObject.GetComponent<MapTile>().AttackRange(monster);
                }
            }

            for (int i = 0; i < hitHorz.Length; i++)
            {
                if (hitHorz[i].collider.gameObject.tag == "MapTile")
                {

                    //hitHorz[i].collider.gameObject.GetComponent<MapTile>().sp.color = Color.blue;
                    atkRange1List.Add(hitHorz[i].collider.gameObject.GetComponent<MapTile>().tileNumber);
                    hitHorz[i].collider.gameObject.GetComponent<MapTile>().AttackRange(monster);
                }
            }


            for (int i = 0; i < hitLeftUp.Length; i++)
            {
                if (hitLeftUp[i].collider.gameObject.tag == "MapTile")
                {
                    //hitLeftUp[i].collider.gameObject.GetComponent<MapTile>().sp.color = Color.blue;
                    atkRange1List.Add(hitLeftUp[i].collider.gameObject.GetComponent<MapTile>().tileNumber);
                    hitLeftUp[i].collider.gameObject.GetComponent<MapTile>().AttackRange(monster);
                }
            }

            for (int i = 0; i < hitRightUp.Length; i++)
            {
                if (hitRightUp[i].collider.gameObject.tag == "MapTile")
                {
                    //hitRightUp[i].collider.gameObject.GetComponent<MapTile>().sp.color = Color.blue;
                    atkRange1List.Add(hitRightUp[i].collider.gameObject.GetComponent<MapTile>().tileNumber);
                    hitRightUp[i].collider.gameObject.GetComponent<MapTile>().AttackRange(monster);
                }
            }

            for (int i = 0; i < hitLeftDown.Length; i++)
            {
                if (hitLeftDown[i].collider.gameObject.tag == "MapTile")
                {
                    //hitLeftDown[i].collider.gameObject.GetComponent<MapTile>().sp.color = Color.blue;
                    atkRange1List.Add(hitLeftDown[i].collider.gameObject.GetComponent<MapTile>().tileNumber);
                    hitLeftDown[i].collider.gameObject.GetComponent<MapTile>().AttackRange(monster);
                }
            }

            for (int i = 0; i < hitRightDown.Length; i++)
            {
                if (hitRightDown[i].collider.gameObject.tag == "MapTile")
                {
                    //hitRightDown[i].collider.gameObject.GetComponent<MapTile>().sp.color = Color.blue;
                    atkRange1List.Add(hitRightDown[i].collider.gameObject.GetComponent<MapTile>().tileNumber);
                    hitRightDown[i].collider.gameObject.GetComponent<MapTile>().AttackRange(monster);
                }
            }


            //if (r == range)
            //{
            //    isScanning = true;
            //}
        }

        for (int r = 0; r <= range2; r++)
        {

            MapTile down = maps[tileOn].GetComponent<MapTile>();
            MapTile up = maps[tileOn].GetComponent<MapTile>();
            MapTile left = maps[tileOn].GetComponent<MapTile>();
            MapTile right = maps[tileOn].GetComponent<MapTile>();

            if (tileOn + 2 * r < maps.Length)
            {
                down = maps[tileOn + 2 * r].GetComponent<MapTile>();

                if (down.transform.position.y > transform.position.y)
                {
                    down = maps[tileOn + 2].GetComponent<MapTile>();
                }

                if (down.transform.position.y > transform.position.y)
                {
                    down = maps[tileOn].GetComponent<MapTile>();
                }
            }
            atkRange2List.Add(down.tileNumber);
            down.AttackRange(monster);

            if (tileOn - 2 * r >= 0)
            {
                up = maps[tileOn - 2 * r].GetComponent<MapTile>();

                if (up.transform.position.y < transform.position.y)
                {
                    up = maps[tileOn - 2].GetComponent<MapTile>();
                }

                if (up.transform.position.y < transform.position.y)
                {
                    up = maps[tileOn].GetComponent<MapTile>();
                }
            }

            atkRange2List.Add(up.tileNumber);
            up.AttackRange(monster);

            if (tileOn - 28 * r >= 0)
            {
                left = maps[tileOn - 28 * r].GetComponent<MapTile>();

                    if (left.transform.position.x > transform.position.x)
                    {
                        left = maps[tileOn - 28].GetComponent<MapTile>();

                        if (left.transform.position.x > transform.position.x)
                        {
                            left = maps[tileOn].GetComponent<MapTile>();

                        }
                    }
            }
            atkRange2List.Add(left.tileNumber);
            left.AttackRange(monster);



            if (tileOn + (28 * r) < maps.Length)
            {
                right = maps[tileOn + (28 * r)].GetComponent<MapTile>();


                if (right.transform.position.x < transform.position.x)
                {
                    right = maps[tileOn + 28].GetComponent<MapTile>();


                    if (right.transform.position.x < transform.position.x)
                    {
                        right = maps[tileOn].GetComponent<MapTile>();

                    }

                }

            }
            atkRange2List.Add(right.tileNumber);
            right.AttackRange(monster);


            //MapTile down = maps[tileOn + 26 * r].GetComponent<MapTile>();
            //atkRange2List.Add(down.tileNumber);
            //down.AttackRange(monster);

            //MapTile up = maps[tileOn - 26 * r].GetComponent<MapTile>();
            //atkRange2List.Add(up.tileNumber);
            //up.AttackRange(monster);

            //MapTile left = maps[tileOn - 2 * r].GetComponent<MapTile>();
            //atkRange2List.Add(left.tileNumber);
            //left.AttackRange(monster);


            //MapTile right = maps[tileOn + (2 * r)].GetComponent<MapTile>();
            //atkRange2List.Add(right.tileNumber);
            //right.AttackRange(monster);





            RaycastHit2D[] hitVert = Physics2D.RaycastAll(down.transform.position, Vector2.up, up.transform.position.y - down.transform.position.y);
            RaycastHit2D[] hitHorz = Physics2D.RaycastAll(right.transform.position, Vector2.left, right.transform.position.x - left.transform.position.x);


            

            //RaycastHit2D[] hitLeftUp = Physics2D.RaycastAll(up.transform.position, new Vector2(-1f, -.58f), (tileRect.localScale.x * r));
            RaycastHit2D[] hitLeftUp = Physics2D.RaycastAll(left.transform.position, new Vector2(up.transform.position.x - left.transform.position.x, up.transform.position.y - left.transform.position.y), (tileRect.localScale.x * r));


            //RaycastHit2D[] hitRightUp = Physics2D.RaycastAll(up.transform.position, new Vector2(1f, -.58f), (tileRect.localScale.x * r));
            RaycastHit2D[] hitRightUp = Physics2D.RaycastAll(up.transform.position, new Vector2(right.transform.position.x - up.transform.position.x, right.transform.position.y - up.transform.position.y), (tileRect.localScale.x * r));


            //RaycastHit2D[] hitLeftDown = Physics2D.RaycastAll(down.transform.position, new Vector2(-1f, .58f), (tileRect.localScale.x * r));
            RaycastHit2D[] hitLeftDown = Physics2D.RaycastAll(down.transform.position, new Vector2(left.transform.position.x - down.transform.position.x, left.transform.position.y - down.transform.position.y), (tileRect.localScale.x * r));


            //RaycastHit2D[] hitRightDown = Physics2D.RaycastAll(down.transform.position, new Vector2(1f, .58f), (tileRect.localScale.x * r));
            RaycastHit2D[] hitRightDown = Physics2D.RaycastAll(right.transform.position, new Vector2(down.transform.position.x - right.transform.position.x, down.transform.position.y - right.transform.position.y), (tileRect.localScale.x * r));



            if (maps[tileOn].GetComponent<MapTile>() == down)
            {
                hitLeftUp = Physics2D.RaycastAll(up.transform.position, new Vector2(-1f, -.58f), (tileRect.localScale.x * r - 1));
                hitRightUp = Physics2D.RaycastAll(up.transform.position, new Vector2(1f, -.58f), (tileRect.localScale.x * r - 1));
                hitLeftDown = Physics2D.RaycastAll(left.transform.position, new Vector2(1f, -.58f), (tileRect.localScale.x * r));
                hitRightDown = Physics2D.RaycastAll(right.transform.position, new Vector2(-1f, -.58f), (tileRect.localScale.x * r));
            }

            if (maps[tileOn].GetComponent<MapTile>() == up)
            {
                hitLeftUp = Physics2D.RaycastAll(left.transform.position, new Vector2(-1f, .58f), tileRect.localScale.x * r - 1);
                hitRightUp = Physics2D.RaycastAll(right.transform.position, new Vector2(1f, .58f), tileRect.localScale.x * r - 1);
                hitLeftDown = Physics2D.RaycastAll(down.transform.position, new Vector2(-1f, .58f), (tileRect.localScale.x * r - 1));
                hitRightDown = Physics2D.RaycastAll(down.transform.position, new Vector2(1f, .58f), (tileRect.localScale.x * r - 1));
            }

            if (right.transform.position.x - maps[tileOn].transform.position.x > maps[tileOn].transform.position.x - left.transform.position.x)
            {
                hitLeftUp = Physics2D.RaycastAll(up.transform.position, new Vector2(-1f, -.58f), tileRect.localScale.x * r);
                hitLeftDown = Physics2D.RaycastAll(down.transform.position, new Vector2(-1f, .58f), (tileRect.localScale.x * r));
            }

            if (right.transform.position.x - maps[tileOn].transform.position.x < maps[tileOn].transform.position.x - left.transform.position.x)
            {
                hitRightUp = Physics2D.RaycastAll(up.transform.position, new Vector2(1f, -.58f), tileRect.localScale.x * r);
                hitRightDown = Physics2D.RaycastAll(down.transform.position, new Vector2(1f, .58f), (tileRect.localScale.x * r));
            }



            if (down.transform.position.y - maps[tileOn].transform.position.y < maps[tileOn].transform.position.y - up.transform.position.y)
            {
                hitLeftUp = Physics2D.RaycastAll(left.transform.position, new Vector2(1f, .58f), tileRect.localScale.x * r);
                hitRightUp = Physics2D.RaycastAll(right.transform.position, new Vector2(-1f, .58f), (tileRect.localScale.x * r));
                
            }

            if (down.transform.position.y - maps[tileOn].transform.position.y > maps[tileOn].transform.position.y - up.transform.position.y)
            {
                hitLeftDown = Physics2D.RaycastAll(left.transform.position, new Vector2(1f, -.58f), tileRect.localScale.x * r);
                hitRightDown = Physics2D.RaycastAll(right.transform.position, new Vector2(-1f, -.58f), (tileRect.localScale.x * r));
            }


            for (int i = 0; i < hitVert.Length; i++)
            {
                if (hitVert[i].collider.gameObject.tag == "MapTile")
                {
                    //hitVert[i].collider.gameObject.GetComponent<MapTile>().sp.color = Color.blue;
                    atkRange2List.Add(hitVert[i].collider.gameObject.GetComponent<MapTile>().tileNumber);
                    hitVert[i].collider.gameObject.GetComponent<MapTile>().AttackRange(monster);
                }
            }

            for (int i = 0; i < hitHorz.Length; i++)
            {
                if (hitHorz[i].collider.gameObject.tag == "MapTile")
                {
                    if (hitHorz[i].collider.gameObject.transform.position.x >= transform.position.x + (28 * range2) && (hitHorz[i].collider.gameObject.transform.position.x <= transform.position.x - (28 * range2)))
                    {
                        //hitHorz[i].collider.gameObject.GetComponent<MapTile>().sp.color = Color.blue;
                        atkRange2List.Add(hitHorz[i].collider.gameObject.GetComponent<MapTile>().tileNumber);
                        hitHorz[i].collider.gameObject.GetComponent<MapTile>().AttackRange(monster);
                    }
                }
            }


            for (int i = 0; i < hitLeftUp.Length; i++)
            {
                if (hitLeftUp[i].collider.gameObject.tag == "MapTile")
                {
                    //hitLeftUp[i].collider.gameObject.GetComponent<MapTile>().sp.color = Color.blue;
                    atkRange2List.Add(hitLeftUp[i].collider.gameObject.GetComponent<MapTile>().tileNumber);
                    hitLeftUp[i].collider.gameObject.GetComponent<MapTile>().AttackRange(monster);
                }
            }

            for (int i = 0; i < hitRightUp.Length; i++)
            {
                if (hitRightUp[i].collider.gameObject.tag == "MapTile")
                {

                    //hitRightUp[i].collider.gameObject.GetComponent<MapTile>().sp.color = Color.blue;
                    atkRange2List.Add(hitRightUp[i].collider.gameObject.GetComponent<MapTile>().tileNumber);
                    hitRightUp[i].collider.gameObject.GetComponent<MapTile>().AttackRange(monster);
                }
            }

            for (int i = 0; i < hitLeftDown.Length; i++)
            {
                if (hitLeftDown[i].collider.gameObject.tag == "MapTile")
                {
                    //hitLeftDown[i].collider.gameObject.GetComponent<MapTile>().sp.color = Color.blue;
                    atkRange2List.Add(hitLeftDown[i].collider.gameObject.GetComponent<MapTile>().tileNumber);
                    hitLeftDown[i].collider.gameObject.GetComponent<MapTile>().AttackRange(monster);
                }
            }

            for (int i = 0; i < hitRightDown.Length; i++)
            {
                if (hitRightDown[i].collider.gameObject.tag == "MapTile")
                {
                    //hitRightDown[i].collider.gameObject.GetComponent<MapTile>().sp.color = Color.blue;
                    atkRange2List.Add(hitRightDown[i].collider.gameObject.GetComponent<MapTile>().tileNumber);
                    hitRightDown[i].collider.gameObject.GetComponent<MapTile>().AttackRange(monster);
                }
            }

            //once all of the attack ranges cycle through, make this tower the active monster on the map
            if (r == range2)
            {

                isScanning = true;
                attackNumber = 1;
                //GameManager.Instance.overworldMenu.GetComponent<OverworldInfoMenu>().activeMonster = monster;
            }
        }

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
            type = monster.info.attack1.type;
            name = monster.info.attack1.name;
        }
        if (attackNumber == 2)
        {
            attackList = atkRange2List;
            type = monster.info.attack2.type;
            name = monster.info.attack2.name;
        }




        if (GameManager.Instance.typeColorDictionary.ContainsKey(type))
        {
            Color color = GameManager.Instance.typeColorDictionary[type];

            foreach (int target in attackList)
            {
                var range = Instantiate(levelTile, maps[target].transform.position, Quaternion.identity);
                range.GetComponent<SpriteRenderer>().sortingOrder = 1000;
                range.gameObject.tag = "RangeTile";
                range.gameObject.name = name + "'s Range";
                range.transform.SetParent(Map.gameObject.transform, false);
                range.GetComponent<MapTile>().ShowRange(color);
            }
        }
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
       
    }
}
