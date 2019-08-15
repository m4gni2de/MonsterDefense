using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[System.Serializable]
public struct MapInformation
{
    public float playerEnergy;
    public int playerEnergyMax;
    public float energyRate;
    public int mapHealthMax;
    public float mapHealthCurrent;

};

public class MapDetails : MonoBehaviour
{
    public float width, height;
    public int columns, rows;

    public string mapName;
    public string levelCode;
    public string tileTypeCode;
    public string pathCode;

    public int tileNumber;
    public GameObject mapTile, mapCanvas;
    

    
    public MapTile[] path;
    public List<string> pathOrder = new List<string>();

    public List<string> pathCodes = new List<string>();
    
    

    public List<int> enemies = new List<int>();
    public int enemyMax;
    public float spawnX;
    public float spawnY;
    public int levelMin;
    public int levelMax;
    public float spawnInterval;
    public int enemyCount;

    

    public GameObject spawnPoint;
    public GameObject enemy;

    public Dictionary<int, float> spawnRates = new Dictionary<int, float>();

    //bool used to determine if the map is ending it's current run
    public bool isOver;

    public MapInformation mapInformation = new MapInformation();
    public GameObject mapInfoMenu;

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

    public void LoadMap(string name)
    {
        var allMaps = GameManager.Instance.GetComponent<Maps>().allMapsDict;

        mapName = name;

        mapInformation.playerEnergy = 10;
        mapInformation.playerEnergyMax = 100;


        if (allMaps.ContainsKey(mapName))
        {
            levelCode = allMaps[mapName].mapCode;
            tileTypeCode = allMaps[mapName].tileTypeCode;
           

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
            spawnX = allMaps[mapName].spawnX;
            spawnY = allMaps[mapName].spawnY;

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

            

            
            



        

            string[] chars = new string[levelCode.Length];

            for (int i = 0; i < levelCode.Length; i++)
            {
                chars[i] = levelCode[i].ToString();
            }





            

            int charCount = 0;

        for (int i = 1; i < rows * 2; i++)
        {
            //Debug.Log(i);

            for (int c = 1; c <= columns; c++)
            {



                var tile = Instantiate(mapTile, transform.position, Quaternion.identity);
                var tile2 = Instantiate(mapTile, transform.position, Quaternion.identity);



                tile.GetComponent<MapTile>().GetType(int.Parse(chars[charCount]));

                tile.GetComponent<MapTile>().tileNumber = tileNumber;
                tile.name = tileNumber.ToString();
                tileNumber += 1;

               

                charCount += 1;


                tile2.GetComponent<MapTile>().GetType(int.Parse(chars[charCount]));

                tile2.GetComponent<MapTile>().tileNumber = tileNumber;
                tile2.name = tileNumber.ToString();
                tileNumber += 1;

              

                //tile.transform.position = new Vector2(((-height / 4) + (c * 50)), (width / 4) - 6.5f - (i * 27));
                tile.transform.position = new Vector2((-width / 2) + (i * 50), (height / 2) - (c * 25));
                tile.GetComponent<SpriteRenderer>().sortingOrder = (int)-tile.transform.position.y;
                //tile2.transform.position = new Vector2(((-height / 4) + 25 + (c * 50)), (width / 4) + 7f - (i * 27));
                tile2.transform.position = new Vector2((-width / 2) + (i * 50) + 25, (height / 2) - (c * 25) + 12.5f);
                tile2.GetComponent<SpriteRenderer>().sortingOrder = (int)-tile2.transform.position.y;

                tile.transform.SetParent(mapCanvas.transform, false);
                tile2.transform.SetParent(mapCanvas.transform, false);

                charCount += 1;


            }





        }


        //GameObject[] paths = GameObject.FindGameObjectsWithTag("MapTile");

            //make a path code for each possible path, and add them to a Dictionary of PathCodes
            for (int p = 0; p < pathCodes.Count; p++)
            {
                //break up each path code in to sections of 3, since each tile is a 3 digit number, and store them in a dictionary of path codes that an enemy will choose at random upon their spawn
                //string[] pathChars = new string[pathCode.Length];
                string[] pathChars = new string[pathCodes[p].Length];
                string code = pathCodes[p];
                List<string> pathCode = new List<string>();
                List<MapTile> pathTiles = new List<MapTile>();
                
                
                int h = 2;
                for (int i = 0; i < code.Length / 3; i++)
                {
                    pathChars[i] = code[h - 2].ToString() + code[h - 1].ToString() + code[h].ToString();
                    h += 3;
                    int tileCheck = int.Parse(pathChars[i]);
                    
                    path[i] = GameObject.Find(tileCheck.ToString()).GetComponent<MapTile>();
                    pathTiles.Add(path[i]);
                    //Map.path[i] = GameObject.Find(tileCheck.ToString()).GetComponent<MapTile>();
                }

               
            }

        //set the tile attributes based on their attribute code
        string[] tileChars = new string[tileTypeCode.Length];
        int g = 1;
        for (int t = 0; t < tileTypeCode.Length / 2; t++)
        {
            tileChars[t] = tileTypeCode[g - 1].ToString() + tileTypeCode[g].ToString();
            g += 2;
            GameObject.Find(t.ToString()).GetComponent<MapTile>().GetAttribute(int.Parse(tileChars[t]));
        }

        spawnPoint.transform.position = new Vector2(spawnX, spawnY);
        InvokeRepeating("SpawnEnemy", 4f, spawnInterval);

        }

       
    }

   

    public void SpawnEnemy()
    {
        
        //var random = Random.Range(1, GameManager.Instance.monstersData.monstersByIdDict.Count + 1);
        //var random = Random.Range(enemies[0], enemies[enemies.Count - 1]);
        var rand = Random.Range(0, 1001);
        int random = new int();
        int randomLevel = Random.Range(levelMin, levelMax + 1);
        var byId = GameManager.Instance.monstersData.monstersByIdDict;
        var byPrefab = GameManager.Instance.monstersData.monsterPrefabsDict;

        //checks the random number against all of the spawn rates for the map. once the number is less than a spawn rate for one of the enemies, then it's in that enemy's range, and that enemy will spawn. the enemy's and their rates will be 
        //in ascending order in the map object so this method will works
        foreach(KeyValuePair<int, float> enemy in spawnRates)
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

                    if (byPrefab.ContainsKey(species))
                    {
                        enemyCount += 1;
                        var enemyMonster = Instantiate(byPrefab[species], transform.position, Quaternion.identity);
                        enemyMonster.transform.position = spawnPoint.transform.position;
                        enemyMonster.GetComponent<Monster>().isEnemy = true;
                        enemyMonster.GetComponent<Enemy>().SetEnemyStats(randomLevel);
                        enemyMonster.gameObject.tag = "Enemy";
                        enemyMonster.gameObject.name = "Enemy " + enemyMonster.GetComponent<Monster>().info.species;
                        enemyMonster.transform.localScale = new Vector3(1.8f, 1.8f, 1.0f);

                        int r = Random.Range(0, pathCodes.Count);


                            //break up each path code in to sections of 3, since each tile is a 3 digit number, and store them in a dictionary of path codes that an enemy will choose at random upon their spawn
                            //string[] pathChars = new string[pathCode.Length];
                            string[] pathChars = new string[pathCodes[r].Length];
                            string code = pathCodes[r];
                            
                            int h = 2;
                            for (int i = 0; i < code.Length / 3; i++)
                            {
                                pathChars[i] = code[h - 2].ToString() + code[h - 1].ToString() + code[h].ToString();
                                h += 3;
                                int tileCheck = int.Parse(pathChars[i]);
                                enemyMonster.GetComponent<Enemy>().path[i] = GameObject.Find(tileCheck.ToString()).GetComponent<MapTile>();

                            if (enemyMonster.GetComponent<Enemy>().path.Length > 0)
                            {
                                enemyMonster.GetComponent<Enemy>().currentPath = enemyMonster.GetComponent<Enemy>().path[0];
                            }
                            //Map.path[i] = GameObject.Find(tileCheck.ToString()).GetComponent<MapTile>();
                        }


                        

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

    public void DisplayMap()
    {
        //Map = GetComponent<Map>();
        //path = Map.path;


        




        //var width = Map.width;
        //var height = Map.height;
        //var columns = Map.columns;
        //var rows = Map.rows;
        //var tileNumber = Map.tileNumber;
        //var mapTile = Map.mapTile;
        //var mapCanvas = Map.mapCanvas;

        string[] chars = new string[levelCode.Length];

        for (int i = 0; i < levelCode.Length; i++)
        {
            chars[i] = levelCode[i].ToString();
        }

        int charCount = 0;

        for (int i = 1; i < rows * 2; i++)
        {
            //Debug.Log(i);

            for (int c = 1; c <= columns; c++)
            {



                var tile = Instantiate(mapTile, transform.position, Quaternion.identity);
                var tile2 = Instantiate(mapTile, transform.position, Quaternion.identity);
                if (chars[charCount] == "0")
                {
                    tile.GetComponent<MapTile>().Build();
                }
                if (chars[charCount] == "1")
                {
                    tile.GetComponent<MapTile>().Dirt();
                }
                if (chars[charCount] == "2")
                {
                    tile.GetComponent<MapTile>().Water();
                }
                if (chars[charCount] == "3")
                {
                    tile.GetComponent<MapTile>().Road();
                }

                tile.GetComponent<MapTile>().tileNumber = tileNumber;
                tile.name = tileNumber.ToString();
                tileNumber += 1;

                charCount += 1;

                if (chars[charCount] == "0")
                {
                    tile2.GetComponent<MapTile>().Build();
                }
                if (chars[charCount] == "1")
                {
                    tile2.GetComponent<MapTile>().Dirt();
                }
                if (chars[charCount] == "2")
                {
                    tile2.GetComponent<MapTile>().Water();
                }
                if (chars[charCount] == "3")
                {
                    tile2.GetComponent<MapTile>().Road();
                }


                tile2.GetComponent<MapTile>().tileNumber = tileNumber;
                tile2.name = tileNumber.ToString();
                tileNumber += 1;

                //tile.transform.position = new Vector2(((-height / 4) + (c * 50)), (width / 4) - 6.5f - (i * 27));
                tile.transform.position = new Vector2((-width / 2) + (i * 50), (height / 2) - (c * 25));
                tile.GetComponent<SpriteRenderer>().sortingOrder = (int)-tile.transform.position.y;
                //tile2.transform.position = new Vector2(((-height / 4) + 25 + (c * 50)), (width / 4) + 7f - (i * 27));
                tile2.transform.position = new Vector2((-width / 2) + (i * 50) + 25, (height / 2) - (c * 25) + 12.5f);
                tile2.GetComponent<SpriteRenderer>().sortingOrder = (int)-tile2.transform.position.y;

                tile.transform.SetParent(mapCanvas.transform, false);
                tile2.transform.SetParent(mapCanvas.transform, false);

                charCount += 1;


            }





        }


        //GameObject[] paths = GameObject.FindGameObjectsWithTag("MapTile");

        //break up the path code in to sections of 3, since each tile is a 3 digit number
        string[] pathChars = new string[pathCode.Length];
        int h = 2;
        for (int i = 0; i < pathCode.Length / 3; i++)
        {
            chars[i] = pathCode[h - 2].ToString() + pathCode[h - 1].ToString() + pathCode[h].ToString();
            h += 3;
            int tileCheck = int.Parse(chars[i]);
            path[i] = GameObject.Find(tileCheck.ToString()).GetComponent<MapTile>();
            //path[i] = paths[tileCheck].GetComponent<MapTile>();
            pathOrder.Add(chars[i]);

            path[i] = GameObject.Find(tileCheck.ToString()).GetComponent<MapTile>();
        }



        //}


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
}
