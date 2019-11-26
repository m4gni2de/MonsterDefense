using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THINGS TO MANUALLY ADD TO MAP AFTER IT HAS BEEN GENERATED:
//SPAWN POINTS - MAKE SURE SPAWN POINT 1 LINES UP WITH PATH CODE 1, AND SO ON FOR EACH PATH AND SPAWN POINT
//END POINTS
//PATH BEGINNINGS
//CAMERA MIN AND MAX SIZES


[System.Serializable]
public struct MapInformation
{
    public float playerEnergy;
    public int playerEnergyMax;
    public float energyRate;
    public int mapHealthMax;
    public float mapHealthCurrent;

    //toggle auto play on and off, which means monsters choose their own attacks and activate their own abilities
    public bool autoPlay;

};

public class MapDetails : MonoBehaviour
{
    public float width, height;
    public int columns, rows;
    

    public string mapName;
    public string mapCode;
    //public string tileTypeCode;
    public string pathCode;

    public int tileNumber;
    public GameObject mapTile, mapCanvas;


    //list to keep all of the global stats, which can be added by monster effects or items
    public List<GlobalStat> activeGlobalStats = new List<GlobalStat>();

    //public MapTile[] path;
    public List<MapTile> path = new List<MapTile>();
    public List<string> pathOrder = new List<string>();

    public List<string> pathCodes = new List<string>();

    //keep a list of all of the enemies that are alive on the map
    public List<Enemy> liveEnemies = new List<Enemy>();

    //keep a list of all of the monsters that are acting as your towers
    public List<Monster> liveTowers = new List<Monster>();

    public List<MapTile> allTiles = new List<MapTile>();

    //variables related to the enemies that the map can spawn, as well as the map itself
    public List<int> enemies = new List<int>();
    public int enemyMax;
    public int levelMin;
    public int levelMax;
    public float spawnInterval;
    public int enemyCount;
    public int mapLevel;


    //use this to weight the chances of higher level tiles appearing
    public float mapConst;

    //the weather system object attached to the map
    public WeatherSystem weatherSystem;
    //the weather of the map
    public MapWeather mapWeather;

    public GameObject pathEnd;
    //public GameObject enemy;

    public Dictionary<int, float> spawnRates = new Dictionary<int, float>();

    //bool used to determine if the map is ending it's current run
    public bool isOver;

    public MapInformation mapInformation = new MapInformation();
    public GameObject mapInfoMenu;

    //public Dictionary<int, Vector3> spawnPoints = new Dictionary<int, Vector3>();

    //since the maps will have different sizes, these numbers represent the minimum and maximum scale of the camera
    public int cameraMin, cameraMax;

    public GameObject[] spawnPoints, pathEndPoints;
    
    // Start is called before the first frame update
    void Start()
    {
        //Map = GetComponent<Map>();
        //path = Map.path;

        width = gameObject.GetComponent<RectTransform>().rect.width;
        height = gameObject.GetComponent<RectTransform>().rect.height;



        //Debug.Log(height);

        columns = int.Parse(width.ToString()) / 50;
        rows = int.Parse(height.ToString()) / 50;

        

    }

    //use this to Load a map to be made in to a prefab
    public void LoadMap(string name)
    {
        var allMaps = GameManager.Instance.GetComponent<Maps>().allMapsDict;
        

        

        mapName = name;

        mapInformation.playerEnergy = 10;
        mapInformation.playerEnergyMax = 100;
        mapInformation.mapHealthMax = allMaps[mapName].maxHP;
        mapInformation.mapHealthCurrent = allMaps[mapName].maxHP;



        if (allMaps.ContainsKey(mapName))
        {
            mapCode = allMaps[mapName].mapCode;
            
           

            //get a list of all possible path codes from the map data and creates a list of them
            foreach (string pathCode in allMaps[mapName].pathCode)
            {
                pathCodes.Add(pathCode);
                
                
            }
            //pathCode = allMaps[mapName].pathCode;
            enemyMax = allMaps[mapName].enemyMax;
            levelMin = allMaps[mapName].levelMin;
            levelMax = allMaps[mapName].levelMax;
            spawnInterval = allMaps[mapName].spawnInterval;
            width = allMaps[mapName].width;
            height = allMaps[mapName].height;
            mapLevel = allMaps[mapName].mapLevel;
            mapWeather = allMaps[mapName].weather;

            
            

            columns = int.Parse(width.ToString()) / 50;
            rows = int.Parse(height.ToString()) / 50;

            //add the IDs for the possible enemies this map can have
            foreach (int enemy in allMaps[mapName].enemies)
            {
                
                enemies.Add(enemy);
            }

            //add the IDs for the possible enemies and their spawn rates this map can have
            for (int e = 0; e < allMaps[mapName].enemies.Length; e++)
            {
                spawnRates.Add(allMaps[mapName].enemies[e], allMaps[mapName].enemyChance[e]);
            }


            int charCount = 0;

        for (int i = 1; i < rows * 2; i++)
        {
                //Debug.Log(i);

                for (int c = 1; c <= columns; c++)
                {



                    var tile = Instantiate(mapTile, transform.position, Quaternion.identity);
                    var tile2 = Instantiate(mapTile, transform.position, Quaternion.identity);



                    //tile.GetComponent<MapTile>().GetType(int.Parse(chars[charCount]));

                    tile.GetComponent<MapTile>().tileNumber = tileNumber;
                    tile.name = tileNumber.ToString();
                    tile.GetComponent<MapTile>().info.row = c *2;
                    tile.GetComponent<MapTile>().info.column = i *2 - 1;

                    tile.GetComponent<MapTile>().mapDetails = this;
                    tile2.GetComponent<MapTile>().mapDetails = this;


                    tileNumber += 1;


                charCount += 1;


                //tile2.GetComponent<MapTile>().GetType(int.Parse(chars[charCount]));

                tile2.GetComponent<MapTile>().tileNumber = tileNumber;
                tile2.name = tileNumber.ToString();
                    tile2.GetComponent<MapTile>().info.row = c * 2 - 1;
                    tile2.GetComponent<MapTile>().info.column = i *2;
                    
                    tileNumber += 1;




                    tile.transform.position = new Vector2((-width / 2) + (i * 50), (height / 2) - (c * 25));
                    tile2.transform.position = new Vector2((-width / 2) + (i * 50) + 25, (height / 2) - (c * 25) + 12.50f);


                    //tile.transform.position = new Vector2((-width / 2) + (i * 75) - 25, (height / 2) - (c * 25));
                    //tile2.transform.position = new Vector2((-width / 2) + (i * 75) + 25, (height / 2) - (c * 25) + 12.50f);


                    tile.GetComponent<SpriteRenderer>().sortingOrder = (int)-tile.transform.position.y;
                    tile.GetComponent<MapTile>().roadSprite.sortingLayerName = "Pathways";
                    tile.GetComponent<MapTile>().roadSprite.sortingOrder = 2 + (int)-tile.transform.position.y;
               
                
              
                

                
                tile2.GetComponent<SpriteRenderer>().sortingOrder = (int)-tile2.transform.position.y;
                    tile2.GetComponent<MapTile>().roadSprite.sortingLayerName = "Pathways";
                    tile2.GetComponent<MapTile>().roadSprite.sortingOrder = 2 + (int)-tile2.transform.position.y;

                    tile.transform.SetParent(mapCanvas.transform, false);
                    
                    tile2.transform.SetParent(mapCanvas.transform, false);
                    

                    charCount += 1;


            }

        }
            //set the tile attributes based on their attribute code
            string[] tileChars = new string[mapCode.Length];
            int g = 1;

           
            for (int t = 0; t < mapCode.Length / 2; t++)
            {
                tileChars[t] = mapCode[g - 1].ToString() + mapCode[g].ToString();
                g += 2;
                var tile = GameObject.Find(t.ToString()).GetComponent<MapTile>();
                tile.GetAttribute(int.Parse(tileChars[t]));
                tile.Build();

                //add the tile to the list of active tiles
                GameManager.Instance.activeTiles.Add(t, tile.GetComponent<MapTile>());
                //set the level and EXP of the tile
                tile.SetLevel(mapLevel);
               
                

            }

            //make a path code for each possible path, and add them to a Dictionary of PathCodes
            for (int p = 0; p < pathCodes.Count; p++)
            {
                //break up each path code in to sections of 3, since each tile is a 3 digit number, and store them in a dictionary of path codes that an enemy will choose at random upon their spawn
                //string[] pathChars = new string[pathCode.Length];
                string[] pathChars = new string[pathCodes[p].Length];
                string code = pathCodes[p];
                List<string> pathCode = new List<string>();
                List<MapTile> pathTiles = new List<MapTile>();

                //make sure to clear this if there is more than 1 path so that the game knows to generate more than just one path
                path.Clear();
                
                int h = 2;
                for (int i = 0; i < code.Length / 3; i++)
                {
                    pathChars[i] = code[h - 2].ToString() + code[h - 1].ToString() + code[h].ToString();
                    h += 3;
                    int tileCheck = int.Parse(pathChars[i]);


                    //path[i] = GameObject.Find(tileCheck.ToString()).GetComponent<MapTile>();

                    
                    //path[i] = allTiles[tileCheck];
                    path.Add(allTiles[tileCheck]);
                    path[i].Road();
                    pathTiles.Add(path[i]);

                    ////add the first tile in each path to a dictionary, so the spawning monster knows where to spawn
                    //if (i == 0)
                    //{
                    //    spawnPoints.Add(p, path[i].transform.position);
                    //}
        
                    if (i > 0)
                    {
                        //get the direction of the road sprite from this class
                        //TileSprite tileSprite = new TileSprite(path[i], path[i - 1], "road");
                        TileSprite tileSprite = new TileSprite(pathTiles[i], pathTiles[i - 1], "road");

                    }

   
                }

               
            }




            //pathEnd.transform.position = new Vector2(pathEndX, pathEndY);
            InvokeRepeating("SpawnEnemy", 4f, spawnInterval);

        }


        GetComponentInParent<MonsterInfoMenus>().LoadYourTowers();
        weatherSystem.intensity = UnityEngine.Random.Range(0, 3);
        weatherSystem.StartWeather(this);




    }


    //use this to summon a map for play
    public void SummonMap()
    {
        var allMaps = GameManager.Instance.GetComponent<Maps>().allMapsDict;
        GameManager.Instance.activeTiles.Clear();

        

        //add the IDs for the possible enemies this map can have
        foreach (int enemy in allMaps[mapName].enemies)
        {

            enemies.Add(enemy);
        }

        //add the IDs for the possible enemies and their spawn rates this map can have
        for (int e = 0; e < allMaps[mapName].enemies.Length; e++)
        {
            spawnRates.Add(allMaps[mapName].enemies[e], allMaps[mapName].enemyChance[e]);
        }


        int i = 0;
        foreach(MapTile tile in allTiles)
        {
            
            if (tile.tileAtt == TileAttribute.Water || tile.tileAtt == TileAttribute.Fire || tile.tileAtt == TileAttribute.Magic)
            {
                int tileAtt = tile.tileAttInt;
                tile.ClearAttribute();
                tile.GetAttribute(tileAtt);
            }
            else
            {
                
            }
            GameManager.Instance.activeTiles.Add(tile.tileNumber, tile);

            i += 1;
            if (i >= allTiles.Count)
            {
                GetComponentInParent<MonsterInfoMenus>().LoadYourTowers();
                break;
            }
        }

       
        weatherSystem.intensity = UnityEngine.Random.Range(0, 3);
        weatherSystem.StartWeather(this);

        
        InvokeRepeating("SpawnEnemy", 4f, spawnInterval);


        GameManager.Instance.activeMap = this;
        Camera.main.GetComponent<CameraMotion>().cameraMinSize = cameraMin;
        Camera.main.GetComponent<CameraMotion>().cameraMaxSize = cameraMax;
        Camera.main.orthographicSize = cameraMax;

        StartCoroutine(GlobalStatsCheck());
    }
    

    public void SpawnEnemy()
    {
        
        //var random = Random.Range(1, GameManager.Instance.monstersData.monstersByIdDict.Count + 1);
        //var random = Random.Range(enemies[0], enemies[enemies.Count - 1]);
        var rand = UnityEngine.Random.Range(0, 1001);
        int random = new int();
        int randomLevel = UnityEngine.Random.Range(levelMin, levelMax + 1);
        var byId = GameManager.Instance.monstersData.monstersByIdDict;
        var monstersDict = GameManager.Instance.monstersData.monstersAllDict;



        //checks the random number against all of the spawn rates for the map. once the number is less than a spawn rate for one of the enemies, then it's in that enemy's range, and that enemy will spawn. the enemy's and their rates will be 
        //in ascending order in the map object so this method will works
        foreach (KeyValuePair<int, float> enemy in spawnRates)
        {

            if (rand > enemy.Value)
            {
                
            }
            else
            {
                //Debug.Log("Number Chosen: " + rand);
                random = enemy.Key;
                
                //picks a random number. then translates that number to the Monsters by Id Dictionary. Then takes that number, and summons a prefab based on the name of the matching key
                if (byId.ContainsKey(random))
                {
                    string species = byId[random];

                    if (monstersDict.ContainsKey(species))
                    {
                        int r = UnityEngine.Random.Range(0, pathCodes.Count);


                        //break up each path code in to sections of 3, since each tile is a 3 digit number, and store them in a dictionary of path codes that an enemy will choose at random upon their spawn
                        string[] pathChars = new string[pathCodes[r].Length];
                        string code = pathCodes[r];
                        //Debug.Log(spawnPoints[r]);
                        

                        enemyCount += 1;
                        
                        var enemyMonster = Instantiate(monstersDict[species].monsterPrefab, transform.position, Quaternion.identity);
                        //enemyMonster.transform.position = spawnPoints[r];
                        enemyMonster.GetComponent<Monster>().isEnemy = true;
                        enemyMonster.GetComponent<Enemy>().SetEnemyStats(randomLevel);
                        enemyMonster.gameObject.tag = "Enemy";
                        enemyMonster.gameObject.name = "Enemy " + enemyMonster.GetComponent<Monster>().info.species;
                        enemyMonster.transform.localScale = new Vector3(1.2f, 1.2f, 1.0f);
                        //liveEnemies.Add(enemyMonster.GetComponent<Enemy>());
                        LiveEnemyList();
                        
                            
                            int h = 2;
                            for (int i = 0; i < code.Length / 3; i++)
                            {
                                pathChars[i] = code[h - 2].ToString() + code[h - 1].ToString() + code[h].ToString();
                                h += 3;
                                int tileCheck = int.Parse(pathChars[i]);
                                //enemyMonster.GetComponent<Enemy>().path[i] = GameObject.Find(tileCheck.ToString()).GetComponent<MapTile>();
                                //enemyMonster.GetComponent<Enemy>().pathTileCount += 1;
                                enemyMonster.GetComponent<Enemy>().pathList.Add(allTiles[tileCheck]);

                            if (enemyMonster.GetComponent<Enemy>().pathList.Count == 1)
                            {
                                
                                enemyMonster.GetComponent<Enemy>().currentPath = enemyMonster.GetComponent<Enemy>().pathList[0];
                                
                            }
                            
                            //Map.path[i] = GameObject.Find(tileCheck.ToString()).GetComponent<MapTile>();
                            }
                        enemyMonster.transform.position = enemyMonster.GetComponent<Enemy>().pathList[0].transform.position;
                        enemyMonster.GetComponent<Monster>().PassiveSkill();
                        GameManager.Instance.TriggerEvent(TriggerType.EnemySpawned);


                    }


                        if (enemyCount >= enemyMax)
                        {
                            CancelInvoke("SpawnEnemy");
                            isOver = true;
                        }

                       
                    }
                break;
            }
                
            }
        }
        
    // Update is called once per frame
    void Update()
    {
       
    }

    //keeps a running check on the current global stats
    public IEnumerator GlobalStatsCheck()
    {
        do
        {
            for (int i = 0; i < activeGlobalStats.Count; i++)
            {
                if (activeGlobalStats[i].Owner != null)
                {
                    AddGlobalStat(activeGlobalStats[i]);
                }
                else
                {
                    StopCoroutine(GlobalStatsCheck());
                    RemoveGlobalStat(activeGlobalStats[i]);
                }
            }   
            yield return new WaitForSeconds(.05f);
        } while (true);
    }

    //this is invokved by the PathEnd object, when it is hit by an enemy
    public void MapHealth(Enemy enemy)
    {
        mapInformation.mapHealthCurrent -= 1;

        
        Destroy(enemy.gameObject);
    }


    //this is invoked by a monster's tower script, and sometimes an enemy script. used to control the energy growth rate
    public void MapEnergyRate(float Rate)
    {
        float rate = (float)System.Math.Round(Rate, 2);

        mapInformation.energyRate += rate;

        
    }

    //this is invoked by a monster's tower script, and sometimes an enemy script
    public void AddMapEnergy(float Added)
    {
        float added = (float)System.Math.Round(Added, 2);

        mapInformation.playerEnergy += added;

        if (mapInformation.playerEnergy > mapInformation.playerEnergyMax)
        {
            mapInformation.playerEnergy = mapInformation.playerEnergyMax;
        }

        mapInformation.playerEnergy = (float)System.Math.Round(mapInformation.playerEnergy, 2);
    }

    //this is invoked by a monster's tower script, and sometimes an enemy script
    public void UseMapEnergy(float Removed)
    {
        float removed = (float)System.Math.Round(Removed, 2);

        mapInformation.playerEnergy -= removed;

        if (mapInformation.playerEnergy < 0)
        {
            mapInformation.playerEnergy = 0;
        }
    }

    
    public void MapMenuControl()
    {
        mapInfoMenu.SetActive(!mapInfoMenu.activeSelf);
    }

    //used to update the enemy list when an enemy has been destroyed
    public void LiveEnemyList()
    {
        liveEnemies.Clear();

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < enemies.Length; i++)
        {
            liveEnemies.Add(enemies[i].GetComponent<Enemy>());

            
            
        }
    }




    public void AddGlobalStat(GlobalStat globalStat)
    {
        if (!activeGlobalStats.Contains(globalStat))
        {
            activeGlobalStats.Add(globalStat);
        }

        if (globalStat.type == GlobalStatModType.Enemies)
        {
            foreach (Enemy enemy in liveEnemies)
            {

                enemy.GetComponent<Monster>().GlobalStatMod(this);
            }

        }
        else
        {
            foreach (Monster monster in liveTowers)
            {
                monster.GlobalStatMod(this);

            }
        }
    }

    
    public void RemoveGlobalStat(GlobalStat globalStat)
    {
        if (globalStat.type == GlobalStatModType.Enemies)
        {
            foreach (Enemy enemy in liveEnemies)
            {

                globalStat.RemoveStat(enemy.GetComponent<Monster>());
                

            }

        }
        else
        {
            foreach (Monster monster in liveTowers)
            {
                globalStat.RemoveStat(monster);
               

            }
        }

        activeGlobalStats.Remove(globalStat);
        StartCoroutine(GlobalStatsCheck());
    }



}

public enum GlobalStatModType
{
    AllMonsters,
    Towers,
    Enemies,

}

[System.Serializable]
public class GlobalStat
{
    
    public StatModifier mod;
    public string stat;
    public Monster Owner;
    public bool isAdding;
    public GlobalStatModType type;
    public string origin;

    

    public GlobalStat(StatModifier Mod, string Stat, Monster Monster, string Origin, GlobalStatModType Type)
    {
        mod = Mod;
        stat = Stat;
        Owner = Monster;
        type = Type;
        origin = Origin;


        


    }

    public void AddStat(Monster monster)
    {
        if (stat == "Speed")
        {
            monster.info.Speed.AddModifier(mod);
        }

        
        
    }

    public void RemoveStat(Monster monster)
    {
        if (stat == "Speed")
        {
            //monster.info.Speed.RemoveAllModifiersFromSource(mod.Source);
            monster.info.Speed.RemoveModifier(mod);
        }

        monster.activeGlobalStats.Remove(this);
        
    }

   
}
