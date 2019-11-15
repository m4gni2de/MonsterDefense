using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public string mapName;
    public int mapId;
    public GameObject[] spawnPoint;
    public GameObject[] endPoint;
    public List<string> pathCodes;
    
    

    //dictionary to hold the enemies that spawn and their precent chance to spawn
    public Dictionary<string, float> enemyList = new Dictionary<string, float>();


    //variables to be entered in editor, not at runtime
    public List<string> mapEnemies = new List<string>();
    public List<float> enemySpawnChance = new List<float>();
    public List<MapTile> mapTiles = new List<MapTile>();
    public int levelMin;
    public int levelMax;
    public int maxHP;
    public int enemyMax;
    public float spawnInterval;
    public int mapLevel;

    public MapWeather weather;

    public MapDetails mapDetails;
    public void Awake()
    {
        //create a dictionary out of the enemies in the lists, with their spawn chances
        for(int i = 0; i < mapEnemies.Count; i++)
        {
            enemyList.Add(mapEnemies[i], enemySpawnChance[i]);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //get the map details script from the World Map, and then set the MapDetails values of the WorldMap according to this map's variables
    public void LoadMap(MapDetails MapDetails)
    {
        mapDetails = MapDetails;

        mapDetails.mapName = mapName;
        //mapDetails.mapCode = allMaps[mapName].mapCode;
        mapDetails.enemyCount = 0;
        mapDetails.enemyMax = enemyMax;
        mapDetails.levelMin = levelMin;
        mapDetails.levelMax = levelMax;
        mapDetails.spawnInterval = maxHP;
        mapDetails.mapLevel = mapLevel;
        mapDetails.mapWeather = weather;

        mapDetails.mapName = mapName;
        mapDetails.mapCanvas = gameObject;

        mapDetails.mapInformation.playerEnergy = 10;
        mapDetails.mapInformation.playerEnergyMax = 100;
        mapDetails.mapInformation.mapHealthMax = maxHP;
        mapDetails.mapInformation.mapHealthCurrent = maxHP;

        mapTiles = mapDetails.allTiles;

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

                mapDetails.path[i] = GameObject.Find(tileCheck.ToString()).GetComponent<MapTile>();
                mapDetails.path[i].Road();
                pathTiles.Add(mapDetails.path[i]);
            }


        }

        //clear the active tiles and active towers list
        GameManager.Instance.activeTiles.Clear();
        GameManager.Instance.activeTowers.Clear();

        mapDetails.GetComponent<MonsterInfoMenus>().LoadYourTowers();
        mapDetails.weatherSystem.intensity = Random.Range(0, 3);
        mapDetails.weatherSystem.StartWeather(mapDetails);

        

        InvokeRepeating("SpawnEnemy", 2, spawnInterval);

    }





    public void SpawnEnemy()
    {
        var rand = Random.Range(0f, 1f);
        float random = new float();
        int randomLevel = Random.Range(levelMin, levelMax + 1);
        var monstersDict = GameManager.Instance.monstersData.monstersAllDict;

        foreach (KeyValuePair<string, float> enemy in enemyList)
        {
            if (rand >= enemy.Value)
            {
                //
            }
            else
            {
              int r = Random.Range(0, pathCodes.Count);


                //break up each path code in to sections of 3, since each tile is a 3 digit number, and store them in a dictionary of path codes that an enemy will choose at random upon their spawn
                string[] pathChars = new string[pathCodes[r].Length];
                string code = pathCodes[r];
                //Debug.Log(spawnPoints[r]);


                mapDetails.enemyCount += 1;

                var enemyMonster = Instantiate(monstersDict[enemy.Key].monsterPrefab, transform.position, Quaternion.identity);
                enemyMonster.transform.SetParent(transform);
                enemyMonster.GetComponent<Monster>().isEnemy = true;
                enemyMonster.GetComponent<Enemy>().SetEnemyStats(randomLevel);
                enemyMonster.gameObject.tag = "Enemy";
                enemyMonster.gameObject.name = "Enemy " + enemyMonster.GetComponent<Monster>().info.species;
                enemyMonster.transform.localScale = new Vector3(1.2f, 1.2f, 1.0f);
                mapDetails.LiveEnemyList();


                int h = 2;
                for (int i = 0; i < code.Length / 3; i++)
                {
                    pathChars[i] = code[h - 2].ToString() + code[h - 1].ToString() + code[h].ToString();
                    h += 3;
                    int tileCheck = int.Parse(pathChars[i]);
                    enemyMonster.GetComponent<Enemy>().pathList.Add(GameObject.Find(tileCheck.ToString()).GetComponent<MapTile>());

                    if (enemyMonster.GetComponent<Enemy>().pathList.Count == 1)
                    {

                        enemyMonster.GetComponent<Enemy>().currentPath = enemyMonster.GetComponent<Enemy>().pathList[0];

                    }
                }
                enemyMonster.transform.position = enemyMonster.GetComponent<Enemy>().pathList[0].transform.position;



            }


            if (mapDetails.enemyCount >= enemyMax)
            {
                CancelInvoke("SpawnEnemy");
                mapDetails.isOver = true;
             
            break;
            }
        }
       
    }
}
