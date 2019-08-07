using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MapDetails : MonoBehaviour
{
    public string mapName;
    public string levelCode;
    public string pathCode;
    public MapTile[] path;
    public List<string> pathOrder = new List<string>();

    
    public List<int> enemies = new List<int>();
    public int enemyMax;
    public float spawnX;
    public float spawnY;
    public int levelMin;
    public int levelMax;
    public float spawnInterval;
    public int enemyCount;

    private Map Map;

    public GameObject spawnPoint;
    public GameObject enemy;

    public Dictionary<int, float> spawnRates = new Dictionary<int, float>();

    //bool used to determine if the map is ending it's current run
    public bool isOver;


    // Start is called before the first frame update
    void Start()
    {
        Map = GetComponent<Map>();
        path = Map.path;
   
    }

    public void LoadMap(string name)
    {
        var allMaps = GameManager.Instance.GetComponent<Maps>().allMapsDict;

        mapName = name;


        if (allMaps.ContainsKey(mapName))
        {
            levelCode = allMaps[mapName].mapCode;
            pathCode = allMaps[mapName].pathCode;
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

            

            
            spawnPoint.transform.position = new Vector2(spawnX, spawnY);
            InvokeRepeating("SpawnEnemy", 4f, spawnInterval);



            var width = Map.width;
        var height = Map.height;
        var columns = Map.columns;
        var rows = Map.rows;
        var tileNumber = Map.tileNumber;
        var mapTile = Map.mapTile;
        var mapCanvas = Map.mapCanvas;

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

            Map.path[i] = GameObject.Find(tileCheck.ToString()).GetComponent<MapTile>();
        }



        }
    }

   

    public void SpawnEnemy()
    {
        
        //var random = Random.Range(1, GameManager.Instance.monstersData.monstersByIdDict.Count + 1);
        //var random = Random.Range(enemies[0], enemies[enemies.Count - 1]);
        var rand = Random.Range(0, 1001);
        int random = new int();
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
                        enemyMonster.GetComponent<Enemy>().SetEnemyStats(random);
                        enemyMonster.gameObject.tag = "Enemy";
                        enemyMonster.gameObject.name = "Enemy " + enemyMonster.GetComponent<Monster>().info.species;
                        enemyMonster.transform.localScale = new Vector3(1.8f, 1.8f, 1.0f);

                        if (enemyCount >= enemyMax)
                        {
                            CancelInvoke("SpawnEnemy");
                            isOver = true;
                        }
                    }
                }
                break;
            }
        }
        

        

       


        //var enemyMonster = Instantiate(enemy, transform.position, Quaternion.identity);
        ////x.transform.SetParent(GetComponentInParent<MapTemplate>().gameObject.transform, false);
        //enemyMonster.transform.position = spawnPoint.transform.position;
        //enemyMonster.GetComponent<Monster>().isEnemy = true;
        //enemyMonster.gameObject.tag = "Enemy";
        //enemyMonster.gameObject.name = "Enemy " + enemyMonster.GetComponent<Monster>().info.species;
        ////x.transform.localScale = new Vector2(x.transform.localScale.x * .85f, x.transform.localScale.y * .85f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisplayMap()
    {
        //Map = GetComponent<Map>();
        //path = Map.path;


        




        var width = Map.width;
        var height = Map.height;
        var columns = Map.columns;
        var rows = Map.rows;
        var tileNumber = Map.tileNumber;
        var mapTile = Map.mapTile;
        var mapCanvas = Map.mapCanvas;

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

            Map.path[i] = GameObject.Find(tileCheck.ToString()).GetComponent<MapTile>();
        }



        //}


    }
}
