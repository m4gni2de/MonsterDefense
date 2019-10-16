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
    public float pathEndX;
    public float pathEndY;
    public int levelMin;
    public int levelMax;
    public float spawnInterval;
    public int mapLevel;
    public string[] itemDrops;
   
    
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

    public MapInfo TestMap = new MapInfo
    {
        mapName = "Test Map",
        mapCode = "04050104020402000304040405060504000506030403010006010601050400000001020404030604000400060504040404030105060106030401020005010001040402020102040504040006030604000205050300040305040303050202020303030306030000060002040402000206040101050101050302020002020502020001020105060006050305060105060603030100060402030604020101010401040305060103060206020402030405030204000403030300000005000304000503000405020302050502000505000102010000010605010202040101020605020104050105020505050002000405040205030306000101060201030601060602000006050301010001020504010604030106060001050001060601040105000405040001020601050200020004050205030002010105030603060106030501040104010502040604010203000106020602010304020502050006000002020200030502050106030506030305",
        pathCode = new string[1] { "222195192165162135132105102077074047072073070043040013010" },
        enemies = new int[3] { 1, 2, 3 },
        enemyChance = new float[3] { 333, 666, 1000 },
        enemyMax = 50,
        pathEndX = -331.7f,
        pathEndY = 43.9f,
        levelMin = 1,
        levelMax = 91,
        spawnInterval = 3.8f,
        mapId = 0,
        width = 700,
        height = 350,
        maxHP = 25,
        
        
        
        

    };

    public MapInfo DualPath = new MapInfo
    {
        mapName = "Dual Path Map",
        mapCode = "01000105010304050500040603000604040406060500050306040300040601030205050403010300020300000501060006050202000003020300010106050301050600040603040601010404030606030505030303000503030605020403030306040601020306010300030200010601030202010300050000040303020605040301060605030403060202040003020603030601020500000302060304000003040504060506030300010102000505010605010001010305060406000600060404060604030604040205040605020606040604050204040604000003000301050104060003020605060200030306040103020505010103020506020104050501010204010201010406010104040100030200000606020004020105010501030201030100060304040306010602050501030204030100020602020300060200030406050204020201060403020203000506000205020102020104040604020103000103020402030203010306",
        pathCode = new string[2] { "022023048049076079104105130131156157184187214217244247274277304307334", "138139164165162135132105102075100101126127152153178181208211238241268271298301300275274249248223250" },
        enemies = new int[3] { 1, 2, 3 },
        enemyChance = new float[3] { 333, 666, 1000 },
        enemyMax = 50,
        pathEndX = 185.6f,
        pathEndY = -213.3f,
        levelMin = 1,
        levelMax = 99,
        spawnInterval = 3.8f,
        mapId = 1,
        width = 700,
        height = 350,
        mapLevel = 5,
        maxHP = 25,

    };

    public MapInfo SmallMap = new MapInfo
    {
        mapName = "Small Map",
        mapCode = "07080904040102020501000504000301000404000006060004040601010400020400050006060005030601020006040603040006000103020100040000000106000605041004",
        pathCode = new string[1] { "040041052039036037048049046033030031042029" },
        enemies = new int[1] { 4 },
        //enemies = new int[3] { 1, 2, 3 },
        //enemyChance = new float[3] { 333, 666, 1000 },
        enemyChance = new float[1] { 1000 },
        enemyMax = 50,
        pathEndX = -33.8f,
        pathEndY = 103f,
        levelMin = 1,
        levelMax = 4,
        spawnInterval = 3.8f,
        mapId = 2,
        width = 350,
        height = 175,
        mapLevel = 1,
        maxHP = 25,
        itemDrops = new string[2] { "Ice Shard", "Nature Rune" },

    };

    public MapInfo BaseMap = new MapInfo
    {
        mapName = "Base Map",
        mapCode = "04060500020006040002020403000000030206030601020300010303000203010306040506030004040002000303040005050202050205020100050405030504020004050302",
        pathCode = new string[1] { "040027024025036037048035032019018007020023034035048049060061" },
        enemies = new int[3] { 1, 2, 3 },
        enemyChance = new float[3] { 333, 666, 1000 },
        enemyMax = 50,
        pathEndX = 131f,
        pathEndY = 40.7f,
        levelMin = 1,
        levelMax = 4,
        spawnInterval = 3.8f,
        mapId = 3,
        width = 350,
        height = 175,
        mapLevel = 9,
        maxHP = 25,

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
        allMapsDict.Add(allMaps.TestMap.mapName, allMaps.TestMap);
        allMapsDict.Add(allMaps.DualPath.mapName, allMaps.DualPath);
        allMapsDict.Add(allMaps.SmallMap.mapName, allMaps.SmallMap);
        allMapsDict.Add(allMaps.BaseMap.mapName, allMaps.BaseMap);
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
