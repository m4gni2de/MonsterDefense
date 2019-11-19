using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

[System.Serializable]
public struct MapInfo
{
    public string mapName;
    public string mapCode;
    public int mapId;
    public int maxHP;
    public float width;
    public float height;
    public string[] pathCode;
    public string tileTypeCode;
    public int[] enemies;
    public float[] enemyChance;
    public int enemyMax;
    public int levelMin;
    public int levelMax;
    public float spawnInterval;
    public int mapLevel;
    public string[] itemDrops;
    public MapWeather weather;
    public GameObject mapPrefab;
    
};


[System.Serializable]
public class MapsInfoRoot
{
    public MapInfo MapInfo;
    
}

[System.Serializable]
public class AllMaps
{
    [Header("Separate Maps")]

   

    public MapInfo SmallMap = new MapInfo
    {
        mapName = "Small Map",
        mapCode = "07080904040102020501000504000301000404000006060004040601010400020400050006060005030601020006040603040006000103020100040000000106000605041004",
        pathCode = new string[1] { "040041052039036037048049046033030031042029" },
        enemies = new int[1] { 5 },
        enemyChance = new float[1] { 1000 },
        enemyMax = 50,
        levelMin = 1,
        levelMax = 4,
        spawnInterval = 3.8f,
        mapId = 1,
        width = 350,
        height = 175,
        mapLevel = 1,
        maxHP = 25,
        itemDrops = new string[2] { "Ice Shard", "Nature Rune" },
        weather = MapWeather.Snow,

    };

    public MapInfo BaseMap = new MapInfo
    {
        mapName = "Base Map",
        mapCode = "04060500020006040002020403000000030206030601020300010303000203010306040506030004040002000303040005050202050205020100050405030504020004050302",
        pathCode = new string[1] { "040027024025036037048035032019018007020023034035048049060061" },
        enemies = new int[3] { 1, 2, 3 },
        enemyChance = new float[3] { 333, 666, 1000 },
        enemyMax = 50,
        levelMin = 1,
        levelMax = 4,
        spawnInterval = 3.8f,
        mapId = 2,
        width = 350,
        height = 175,
        mapLevel = 9,
        maxHP = 25,
        weather = MapWeather.Sunny,

    };

    public MapInfo Water1 = new MapInfo
    {
        mapName = "Water Map 1",
        mapCode = "00010001000100000101000100000101000001010101000101000101010101000101010000000001010100010101010100010101000001000101010001010101000101000101",
        pathCode = new string[1] { "006007018021034037036025038041054055066067064051048049060047044031028017014001" },
        enemies = new int[3] { 1, 2, 3 },
        enemyChance = new float[3] { 333, 666, 1000 },
        enemyMax = 50,
        levelMin = 1,
        levelMax = 4,
        spawnInterval = 3.8f,
        mapId = 3,
        width = 350,
        height = 175,
        mapLevel = 9,
        maxHP = 25,
        weather = MapWeather.Sunny,

    };

    public MapInfo DualPaths = new MapInfo
    {
        mapName = "Dual Paths Map",
        mapCode = "00020109010200010102010901000105010101020106000700070103010400060108000801040100000601030002010400030100010301090005000800080000000401060009000101050009000501040101010801070005010600050003000400010007000601080001000800090101",
        pathCode = new string[2] { "006007022025040041054055068053050035048049", "046031028029042043056059074077090093106107" },
        enemies = new int[5] { 1, 2, 3, 4, 5 },
        enemyChance = new float[5] { 200, 400, 600, 800, 1000 },
        enemyMax = 50,
        levelMin = 1,
        levelMax = 10,
        spawnInterval = 2.8f,
        mapId = 4,
        width = 400,
        height = 200,
        mapLevel = 9,
        maxHP = 40,
        weather = MapWeather.Sunny,

    };


}




public class Maps : MonoBehaviour
{
    

    public AllMaps allMaps = new AllMaps();
    public Dictionary<string, MapInfo> allMapsDict = new Dictionary<string, MapInfo>();
    public Dictionary<int, Sprite> allTileSpritesDict = new Dictionary<int, Sprite>();
    //public Dictionary<int, float> mapLevelConstants = new Dictionary<int, float>();
    //public Dictionary<int, Sprite> tileTypeSpritesDict = new Dictionary<int, Sprite>();

    [Header("Tile Sprite Arrays")]

    public Sprite[] tileSprites;
    public Sprite[] tileTypeSprites;

    public Sprite[] roadTiles;

    

    //variables and objects related to the water tiles
    public Sprite[] waterTileSprites;
    //variables and objects related to the fire tiles
    public Sprite[] fireTileSprites;
    //lava sprites that will glow
    public Sprite[] fireTileTopSprites;
    public GameObject fireDebris;

    public GameObject[] natureTileBrush;

    public Sprite[] magicTileTopSprites;
    public GameObject magicTileBurst;
    public Sprite electricTileTopSprite;






    


    private void Awake()
    {
        //for (int a = 1; a < 20; a++)
        //{
        //    mapLevelConstants.Add(a, 1 + (a / 10f));
        //    //Debug.Log(mapLevelConstants[a]);
        //}

        for (int i = 0; i < tileSprites.Length; i++)
        {
            allTileSpritesDict.Add(i, tileSprites[i]);
        }


       
        //for (int i = 0; i < tileTypeSprites.Length; i++)
        //{
        //    tileTypeSpritesDict.Add(i + 1, tileTypeSprites[i]);
        //}


        AddAllMaps();
        
    }

    public void AddAllMaps()
    {

        
        allMapsDict.Add(allMaps.SmallMap.mapName, allMaps.SmallMap);
        allMapsDict.Add(allMaps.BaseMap.mapName, allMaps.BaseMap);
        allMapsDict.Add(allMaps.Water1.mapName, allMaps.Water1);
        allMapsDict.Add(allMaps.DualPaths.mapName, allMaps.DualPaths);
    }

}







public class TileSprite
{
    public MapTile currentTile;
    public MapTile previousTile;

    public string tileType;



    public TileSprite(MapTile CurrentTile, MapTile PreviousTile, string TileType)
    {
        
        currentTile = CurrentTile;
        previousTile = PreviousTile;
        tileType = TileType;

        Maps maps = GameManager.Instance.GetComponent<Maps>();

        if (tileType == "road")
        {

            if (previousTile.transform.position.x < currentTile.transform.position.x && previousTile.transform.position.y < currentTile.transform.position.y)
            {
                    
                currentTile.roadSprite.sprite = maps.roadTiles[0];
                currentTile.pathDirection = "NE";

                if (!currentTile.pathDirections.Contains(3))
                {
                    currentTile.pathDirections.Add(3);
                }

                if (!previousTile.pathDirections.Contains(2))
                {
                    previousTile.pathDirections.Add(2);
                }
            }
            if (previousTile.transform.position.x > currentTile.transform.position.x && previousTile.transform.position.y > currentTile.transform.position.y)
            {
                currentTile.roadSprite.sprite = maps.roadTiles[0];
                currentTile.pathDirection = "SW";
                

                if (!currentTile.pathDirections.Contains(2))
                {
                    currentTile.pathDirections.Add(2);
                }

                if (!previousTile.pathDirections.Contains(3))
                {
                    previousTile.pathDirections.Add(3);
                }
            }

            if (previousTile.transform.position.x < currentTile.transform.position.x && previousTile.transform.position.y > currentTile.transform.position.y)
            {
                currentTile.roadSprite.sprite = maps.roadTiles[0];
                currentTile.pathDirection = "SE";
                currentTile.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
                

                if (!currentTile.pathDirections.Contains(1))
                {
                    currentTile.pathDirections.Add(1);
                }

                if (!previousTile.pathDirections.Contains(4))
                {
                    previousTile.pathDirections.Add(4);
                }
            }
            if (previousTile.transform.position.x > currentTile.transform.position.x && previousTile.transform.position.y < currentTile.transform.position.y)
            {
                currentTile.roadSprite.sprite = maps.roadTiles[0];
                currentTile.pathDirection = "NW";
                currentTile.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
                

                if (!currentTile.pathDirections.Contains(4))
                {
                    currentTile.pathDirections.Add(4);
                }

                if (!previousTile.pathDirections.Contains(1))
                {
                    previousTile.pathDirections.Add(1);
                }
            }

            if (previousTile.pathDirection == "NE" && currentTile.pathDirection == "SE" || previousTile.pathDirection == "NW" && currentTile.pathDirection == "SW")
            {
                previousTile.roadSprite.sprite = maps.roadTiles[1];
                //previousTile.transform.position = new Vector3(previousTile.transform.position.x, previousTile.transform.position.y + 1.5f, previousTile.transform.position.z);
            }

            if (previousTile.pathDirection == "SE" && currentTile.pathDirection == "NE" || previousTile.pathDirection == "SW" && currentTile.pathDirection == "NW")
            {
                previousTile.roadSprite.sprite = maps.roadTiles[2];
                previousTile.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                //previousTile.transform.position = new Vector3(previousTile.transform.position.x + .5f, previousTile.transform.position.y, previousTile.transform.position.z);
            }

            if (previousTile.pathDirection == "NW" && currentTile.pathDirection == "NE" || previousTile.pathDirection == "SW" && currentTile.pathDirection == "SE")
            {
                previousTile.roadSprite.sprite = maps.roadTiles[3];
                previousTile.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
                //previousTile.transform.position = new Vector3(previousTile.transform.position.x, previousTile.transform.position.y + .5f, previousTile.transform.position.z);

            }

            if (previousTile.pathDirection == "SE" && currentTile.pathDirection == "SW" || previousTile.pathDirection == "NE" && currentTile.pathDirection == "NW")
            {
                previousTile.roadSprite.sprite = maps.roadTiles[3];
                previousTile.transform.rotation = new Quaternion(0f, 180f, 0f, 0f);
                //previousTile.roadSprite.transform.position = new Vector3(previousTile.transform.position.x, previousTile.transform.position.y +.5f, previousTile.transform.position.z);

            }

            //if (currentTile.pathCount > 1)
            //{
                if (previousTile.pathDirections.Contains(1) && previousTile.pathDirections.Contains(2) && previousTile.pathDirections.Contains(3))
                {
                    previousTile.roadSprite.sprite = maps.roadTiles[7];
                previousTile.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            }

                if (previousTile.pathDirections.Contains(1) && previousTile.pathDirections.Contains(2) && previousTile.pathDirections.Contains(4))
                {
                    previousTile.roadSprite.sprite = maps.roadTiles[6];
                previousTile.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            }

                if (previousTile.pathDirections.Contains(1) && previousTile.pathDirections.Contains(3) && previousTile.pathDirections.Contains(4))
                {
                    previousTile.roadSprite.sprite = maps.roadTiles[8];
                previousTile.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            }

                if (previousTile.pathDirections.Contains(2) && previousTile.pathDirections.Contains(3) && previousTile.pathDirections.Contains(4))
                {
                    previousTile.roadSprite.sprite = maps.roadTiles[5];
                previousTile.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            }

                if (previousTile.pathDirections.Contains(1) && previousTile.pathDirections.Contains(2) && previousTile.pathDirections.Contains(3) && previousTile.pathDirections.Contains(4))
                {
                    previousTile.roadSprite.sprite = maps.roadTiles[4];
                previousTile.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            }

                //Debug.Log(previousTile.tileNumber + "   " + previousTile.pathDirections.Count);
                //if (currentTile.pathDirections.Count == 4)
                //{
                //    currentTile.roadSprite.sprite = maps.roadTiles[4];
                //}

                //if (currentTile.pathDirections.Count == 3)
                //{



                //}


            //}
        }

    }
}


//import a map's level and map level constant to get the formula that weights the chances of a map tile's level
[System.Serializable]
public class MapTileLevelCalc
{

    public float value;
    public AnimationCurve Curve;

    //public static AnimationCurve CurveField(AnimationCurve value, params GUILayoutOption[] options);

    //the max level that a tile can be, gien the level
    public int maxLevel;

    public MapTileLevelCalc(int mapLevel, AnimationCurve curve)
    {
        float rand = UnityEngine.Random.Range(0f, 100f);

        //get the maximum level a tile can be based on the level of the map
        Keyframe key2 = new Keyframe();
        float val = (float)(3.639 + 0.783 * (mapLevel));
        key2.time = 1f;
        key2.value = Mathf.RoundToInt(val) / 10f;
        curve.MoveKey(2, key2);

        Keyframe key = new Keyframe();
        key.time = .9f;
        key.value = key2.value * ((float)mapLevel / 10f);

        //Debug.Log(key.inTangent);
        //key.outTangent = 1;
       key.inTangent = (key.value - (curve.keys[0].value)) / (key.time - curve.keys[0].time);
        curve.MoveKey(1, key);
        

        curve.SmoothTangents(2, 2);
        

        value = curve.Evaluate(UnityEngine.Random.value);

        //Debug.Log(x);

        
        
        Curve = curve;

    }

    
}

//import a map and a tile to get a weighted list of possible mining outcomes
[System.Serializable]
public class MapTileMining
{
    public MapTile MapTile;
    public MapDetails Map;

    //chance out of 100 that an item will be found on the mining check
    public float mineChance;

    //array to hold item rarities and their chances of being mined
    public float[] itemChance = new float[3];

    public MapTileMining(MapDetails map, MapTile tile)
    {
        MapTile = tile;
        Map = map;

        TileAttribute att = tile.tileAtt;
        int level = tile.info.level;

        itemChance[0] = .95f;
        itemChance[1] = .04f;
        itemChance[2] = .01f;

        mineChance = .90f;

    }

    //use this to check if an item is mined
    public void MineCheck()
    {
        float rand = UnityEngine.Random.Range(0f, 1f);

        
        
        if (rand <= mineChance)
        {
            MineItem();
            mineChance = .1f;
        }
        else
        {
            mineChance += .1f;
        }
    }


    //use this to actually mine the item
    public void MineItem()
    {
        
        var allMaps = GameManager.Instance.GetComponent<Maps>().allMapsDict;
        //get a list of all of the items the map can produce
        List<string> itemList = new List<string>();

        if (allMaps.ContainsKey(Map.mapName))
        {
            
            Debug.Log(allMaps[Map.mapName].itemDrops.Length);
            //get all of the items in the map's item drop and add them to a local list

            for (int i = 0; i < allMaps[Map.mapName].itemDrops.Length; i++)
            {
                itemList.Add(allMaps[Map.mapName].itemDrops[i]);
                

                //once the list is full, choose an item to add
                if (i >= allMaps[Map.mapName].itemDrops.Length - 1)
                {
                    int rand = UnityEngine.Random.Range(0, i + 1);

                    bool hasKey = PlayerPrefs.HasKey(itemList[rand]);

                    if (hasKey || !hasKey)
                    {
                        int itemAmount = PlayerPrefs.GetInt(itemList[rand], 0);

                        PlayerPrefs.SetInt(itemList[rand], itemAmount + 1);
                        //Debug.Log("Mined a " + itemList[rand] + "! You now have " + (itemAmount + 1) + " of these!");
                        GameManager.Instance.SendNotificationToPlayer(itemList[rand], 1, NotificationType.TileMine, "Tile " + MapTile.tileNumber);
                        GameManager.Instance.GetComponent<YourItems>().GetYourItems();
                    }
                }
            }
            
        }
    }
    

   


}


public enum MapWeather
{
    Sunny,
    Fog,
    Cloudy,
    Rain,
    Snow,
    Windy
}

