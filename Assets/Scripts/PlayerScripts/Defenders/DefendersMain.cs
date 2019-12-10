using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
//each tile has specific information so it can be saved as a json tree
public class TileData
{
    public int index;
    public TileAttribute attribute;
    public int level;
    public int monsterOnIndex;
    public MapTile mapTile;
    public bool isRoad;
    public string monsterOn;

    public TileData(int i, TileAttribute a, int l, int m)
    {
        index = i;
        attribute = a;
        level = l;
        monsterOnIndex = m;
        isRoad = false;
        monsterOn = "";
    }
}

[System.Serializable]
//all of the tiles are saved as a single DefenderInfo object
public class DefenderInfo
{
    public List<TileData> tileList = new List<TileData>();
    public List<string> pathCodes = new List<string>();

    public int maxHP;
    public int currentHP;
    public float maxEnergy;
    public float currentEnergy;


    public void DefenderMapInfo(AccountInfo account, MapInformation map)
    {
        maxHP = account.playerLevel * 20;
        currentHP = maxHP;
        maxEnergy = 100 + (account.playerLevel * 10f);
        currentEnergy = maxEnergy;

        map.mapHealthMax = maxHP;
        map.mapHealthCurrent = maxHP;
        map.playerEnergyMax = (int)maxEnergy;
        map.playerEnergy = currentEnergy;
    }
    
}



public class DefendersMain : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public DefenderInfo defenderInfo = new DefenderInfo();
    public MapDetails mapDetails;

    public GameObject mapTile, mapCanvas, tileInfoMenu, towersMenu, menuContentView, towerBase, mapObject;
    public MonsterInfoMenus monsterInfoMenu;

    public Button showTowersBtn;


    public int rows, columns, tileNumber, totalTiles;
    public float width, height;

    public List<MapTile> path = new List<MapTile>();
    public MapTile activeTile;
    public bool isTapping, towersFilled;
    public float acumTime;

    public Dictionary<int, string> yourDefenders = new Dictionary<int, string>();

    public List<GameObject> towers = new List<GameObject>();
    public Monster activeMonster;

    // Start is called before the first frame update
    void Start()
    {
        //clear the active tiles and active towers list
        GameManager.Instance.activeTiles.Clear();
        GameManager.Instance.activeTowers.Clear();

        GameManager.Instance.gameMode = GameMode.DefenderMode;
        yourDefenders = GameManager.Instance.GetComponent<YourMonsters>().yourDefendersDict;

        towers.Clear();

        //set the info of your Defender Map based on your player information, and set it to the map informaiton of Map Details
        defenderInfo.DefenderMapInfo(GameManager.Instance.GetComponent<YourAccount>().account, mapDetails.mapInformation);

        rows = 3;
        columns = 7;
        width = 350;
        height = 175;
        tileNumber = 0;
        totalTiles = (((rows * 2) - 1) * columns) *2;

        defenderInfo.tileList.Clear();
        defenderInfo.pathCodes.Clear();

        GameManager.Instance.activeMap = mapDetails;
        //LoadYourTowers();

        if (GameManager.Instance.GetComponent<YourAccount>().account.defenderDataString != "none")
        {
            string json = PlayerPrefs.GetString("DefenderInfo");
            var info = JsonUtility.FromJson<DefenderInfo>(json);
            GameManager.Instance.GetComponent<YourAccount>().account.defenderDataString = PlayerPrefs.GetString("DefenderInfo");
            defenderInfo = info;
            StartCoroutine(LoadTiles());
        }
        else
        {

            defenderInfo.pathCodes.Add("040041052039036037048049046033030031042029"); 

            for (int i = 0; i < totalTiles; i++)
            {
                //Debug.Log(i);
                TileData tile = new TileData(i, TileAttribute.None, 1, 0);
                defenderInfo.tileList.Add(tile);

                if (i >= totalTiles - 1)
                {

                    PlayerPrefs.SetString("DefenderInfo", JsonUtility.ToJson(defenderInfo));
                    GameManager.Instance.GetComponent<YourAccount>().account.defenderDataString = PlayerPrefs.GetString("DefenderInfo");
                    StartCoroutine(LoadTiles());

                    return;
                }
            }
           
        }

        

        
    }

    // Update is called once per frame
    void Update()
    {
        if (isTapping == true)
        {
            acumTime += Time.deltaTime;
        }
        else
        {
            acumTime = 0;
        }

        if (Input.GetMouseButton(0))
        {
            var clickPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "MapTile" && !isTapping)
                {
                    isTapping = true;
                    activeTile = hit.collider.gameObject.GetComponent<MapTile>();

                }

                //Debug.Log(hit.collider);

            }


        }
    }


    public IEnumerator LoadTiles()
    {
        tileNumber = 0;

        //create the baseline for all of the tiles
        for (int i = 1; i < rows * 2; i++)
        {
            //Debug.Log(i);

            for (int c = 1; c <= columns; c++)
            {
                var tile = Instantiate(mapTile, transform.position, Quaternion.identity);
                var tile2 = Instantiate(mapTile, transform.position, Quaternion.identity);

                tile.GetComponent<MapTile>().mapDetails = GetComponent<MapDetails>();
                tile.GetComponent<MapTile>().roadSprite.sortingLayerName = "Pathways";
                tile.GetComponent<MapTile>().roadSprite.sortingOrder = 2 + (int)-tile.transform.position.y;
                tile.GetComponent<MapTile>().info.row = c * 2;
                tile.GetComponent<MapTile>().info.column = i * 2 - 1;


                tile2.GetComponent<MapTile>().mapDetails = GetComponent<MapDetails>();
                tile2.GetComponent<MapTile>().roadSprite.sortingLayerName = "Pathways";
                tile2.GetComponent<MapTile>().roadSprite.sortingOrder = 2 + (int)-tile.transform.position.y;
                tile2.GetComponent<MapTile>().info.row = c * 2 - 1;
                tile2.GetComponent<MapTile>().info.column = i * 2;

                tile.GetComponent<MapTile>().tileNumber = tileNumber;
                tile.name = tileNumber.ToString();


                defenderInfo.tileList[tileNumber].mapTile = tile.GetComponent<MapTile>();

                tileNumber += 1;



                //*******************************************************************TILE 1 AND 2 SEPARATOR**********************8




                tile2.GetComponent<MapTile>().tileNumber = tileNumber;
                tile2.name = tileNumber.ToString();

                defenderInfo.tileList[tileNumber].mapTile = tile2.GetComponent<MapTile>();
                tileNumber += 1;

                tile.transform.position = new Vector2((-width / 2) + (i * 50), (height / 2) - (c * 25));
                tile.GetComponent<SpriteRenderer>().sortingOrder = (int)-tile.transform.position.y;
                tile2.transform.position = new Vector2((-width / 2) + (i * 50) + 25, (height / 2) - (c * 25) + 12.5f);
                tile2.GetComponent<SpriteRenderer>().sortingOrder = (int)-tile2.transform.position.y;

                tile.transform.SetParent(mapCanvas.transform, false);
                tile2.transform.SetParent(mapCanvas.transform, false);
            }
        }

        yield return new WaitUntil(() => tileNumber >= 70);

        StartCoroutine(UpdateTiles());
    }

    //use this to update all of the tiles
    public IEnumerator UpdateTiles()
    {
        int x = 0;
        //after the tiles are spawned, fill them with data from the DefenderInfo json string
        for (int i = 0; i < totalTiles; i++)
        {
            MapTile tile = defenderInfo.tileList[i].mapTile;
            //var tileAtt = (TileAttribute)Enum.ToObject(typeof(TileAttribute), tileInt);
            int tileAtt = (int)defenderInfo.tileList[i].attribute;

           
            tile.GetAttribute(tileAtt);

            tile.info.level = defenderInfo.tileList[i].level;
            tile.info.totalExp = GameManager.Instance.tileLevelUp[defenderInfo.tileList[i].level];
            tile.info.expToLevel = GameManager.Instance.tileLevelUp[defenderInfo.tileList[i].level + 1] - (int)tile.info.totalExp;

            tile.transform.SetParent(mapObject.transform, true);

            defenderInfo.tileList[i].mapTile = tile;

            //add the tile to the list of active tiles
            GameManager.Instance.activeTiles.Add(defenderInfo.tileList[i].mapTile.tileNumber, tile.GetComponent<MapTile>());

            x += 1;
        }

        yield return new WaitUntil(() => x >= tileNumber);

        
        GeneratePath();
        
        showTowersBtn.interactable = true;
    }

    public void GeneratePath()
    {
        var pathCodes = defenderInfo.pathCodes;

        //make a path code for each possible path, and add them to a Dictionary of PathCodes
        for (int p = 0; p < pathCodes.Count; p++)
        {
            
            string[] pathChars = new string[pathCodes[p].Length];
            string code = pathCodes[p];
            List<string> pathCode = new List<string>();
            List<MapTile> pathTiles = new List<MapTile>();

            path.Clear();

            int h = 2;
            for (int i = 0; i < code.Length / 3; i++)
            {
                pathChars[i] = code[h - 2].ToString() + code[h - 1].ToString() + code[h].ToString();
                h += 3;
                int tileCheck = int.Parse(pathChars[i]);


                //Debug.Log(defenderInfo.tileList[tileCheck].mapTile.tileNumber);
                
                path.Add(defenderInfo.tileList[tileCheck].mapTile);
                path[i].Road();
                pathTiles.Add(path[i]);

                if (i > 0)
                {
                    TileSprite tileSprite = new TileSprite(pathTiles[i], pathTiles[i - 1], "road");
                    //TileSprite tileSprite = new TileSprite(path[i], path[i - 1], "road");

                }


            }


        }

        
        StartCoroutine(LoadYourTowers());
        mapDetails.weatherSystem.intensity = UnityEngine.Random.Range(0, 3);
        mapDetails.weatherSystem.StartWeather(mapDetails);
    }


    //use this to change a potential list of tiles without changing all of them
    public void ChangeTile()
    {

    }

   

    public void OnPointerDown(PointerEventData eventData)
    {
        var worldMousePosition =
                    Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var x = worldMousePosition.x;
        var y = worldMousePosition.y;


        if (Input.GetMouseButton(0))
        {
            var clickPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "MapTile" && !isTapping)
                {
                    isTapping = true;
                    activeTile = hit.collider.gameObject.GetComponent<MapTile>();
                    monsterInfoMenu.tileToBePlaced = activeTile;
                }

            }

            //if (hit.collider != null)
            //{
            //    if (hit.collider.gameObject.tag == "Tower")
            //    {
            //        isTapping = true;
            //        activeTile = hit.collider.gameObject.GetComponent<MapTile>();
            //        monsterInfoMenu.tileToBePlaced = activeTile;
            //    }

            //}


        }


        if (eventData.pointerEnter)
        {
            var tag = eventData.pointerEnter.gameObject.tag;
            var name = eventData.pointerEnter.gameObject.name;


            //if a UI object is tapped, turn the camera off
            if (name == "Handle")
            {

                GameManager.Instance.FreezeCameraMotion();
                Debug.Log(name);
            }


        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //if a player just taps a tile, open the tile. if they hold, do nothing
        if (isTapping)
        {
            if (acumTime <= .15f)
            {
                if (tileInfoMenu.GetComponent<TileInfoObject>().activeTile != null)
                {
                    tileInfoMenu.SetActive(true);
                    tileInfoMenu.GetComponent<TileInfoObject>().activeTile.ActiveTile();
                    tileInfoMenu.GetComponent<TileInfoObject>().LoadTile(activeTile);
                    tileInfoMenu.GetComponent<TileInfoObject>().activeTile.ActiveTile();
                    isTapping = false;

                }
                else
                {
                    tileInfoMenu.SetActive(true);
                    tileInfoMenu.GetComponent<TileInfoObject>().LoadTile(activeTile);
                    tileInfoMenu.GetComponent<TileInfoObject>().activeTile.ActiveTile();
                    isTapping = false;
                }

            }
            else
            {
                //
                isTapping = false;
                //activeTile = null;

            }
        }



    }




    public IEnumerator LoadYourTowers()
    {
        var defenders = yourDefenders;

        if (!towersFilled)
        {
            towersFilled = true;
            var monsters = GameManager.Instance.GetComponent<YourMonsters>().yourMonstersDict;
            var byId = GameManager.Instance.monstersData.monstersByIdDict;
            var monstersDict = GameManager.Instance.monstersData.monstersAllDict;
            var active = GameManager.Instance.activeTowers;
            //int index = new int();

            //List<GameObject> indexes = new List<GameObject>();

            int x = 1;
            for (int i = 1; i <= monsters.Count; i++)
            {

                if (monsters.ContainsKey(i))
                {
                    string monsterJson = monsters[i];
                    var info = JsonUtility.FromJson<MonsterInfo>(monsterJson);


                    string species = info.species;

                    
                        if (monstersDict.ContainsKey(species))
                        {
                            var tower = Instantiate(monstersDict[species].monsterPrefab, menuContentView.transform.position, Quaternion.identity);
                            tower.transform.SetParent(menuContentView.transform, false);
                            tower.transform.position = new Vector3(towerBase.transform.position.x, towerBase.transform.position.y - ((i - 1) * 60), -2f);
                            tower.transform.localScale = new Vector3(tower.transform.localScale.x * 3.5f, tower.transform.localScale.y * 3.5f, tower.transform.localScale.z);
                            tower.GetComponent<Monster>().isTower = true;
                            tower.GetComponent<Monster>().GetComponent<Enemy>().enemyCanvas.SetActive(false);
                            //tower.GetComponent<Monster>().info = JsonUtility.FromJson<MonsterInfo>(monsters[i]);
                            tower.GetComponent<Monster>().saveToken = JsonUtility.FromJson<MonsterSaveToken>(monsters[i]);
                            tower.GetComponent<Monster>().LoadMonsterToken(tower.GetComponent<Monster>().saveToken);
                            tower.GetComponent<Monster>().MonsterEquipment();
                            tower.GetComponent<Monster>().monsterIcon.GetComponentInChildren<MonsterIcon>().DisplayMonster(tower.GetComponent<Monster>());
                            tower.GetComponent<Monster>().monsterIcon.GetComponentInChildren<MonsterIcon>().IconVisibility("GameUI");

                            tower.gameObject.tag = "Tower";
                            tower.gameObject.name = tower.GetComponent<Monster>().info.species + " " + tower.GetComponent<Monster>().info.index;
                        tower.GetComponent<Tower>().monster = tower.GetComponent<Monster>();

                        if (info.defenderIndex != 0)
                        {
                            towers.Add(tower);
                        }


                    }
                           
                   
                }

                x += 1;
            }

            towersMenu.SetActive(true);
            showTowersBtn.gameObject.SetActive(false);
            yield return new WaitUntil(() => x >= monsters.Count - 1);
            
            StartCoroutine(SummonTowers());

            //Debug.Log(JsonUtility.FromJson<DefenderInfo>());
        }
    }

   public IEnumerator SummonTowers()
    {
        var defenders = yourDefenders;

        for (int i = 0; i < towers.Count; i++)
        {
          towers[i].GetComponent<Tower>().mapTileOn = defenderInfo.tileList[towers[i].GetComponent<Monster>().saveToken.defenderIndex - 1].mapTile;
          towers[i].GetComponent<Tower>().StartCoroutine("PlaceTower");

            yield return new WaitUntil(() => towers[i].GetComponent<Tower>().summonAnimationComplete == true);
        }
 
    }


    public void MoveMonster()
    {

    }























    private void OnDestroy()
    {
        string json = PlayerPrefs.GetString("DefenderInfo");

        GameManager.Instance.GetComponent<YourAccount>().account.defenderDataString = JsonUtility.ToJson(defenderInfo);
        PlayerPrefs.SetString("DefenderInfo", JsonUtility.ToJson(defenderInfo));
        
    }

}
