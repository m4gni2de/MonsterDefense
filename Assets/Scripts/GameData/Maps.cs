using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MapInfo
{
    public string mapName;
    public string mapCode;
    public int mapId;
    public float width;
    public float height;
    public string[] pathCode;
    public string tileTypeCode;
    public int[] enemies;
    public float[] enemyChance;
    public int enemyMax;
    public float spawnX;
    public float spawnY;
    public int levelMin;
    public int levelMax;
    public float spawnInterval;
   
    
};


[System.Serializable]
public class MapsInfoRoot
{
    public MapInfo MapInfo;
    
}

public class AllMaps
{
    public MapInfo TestMap = new MapInfo
    {
        mapName = "Test Map",
        mapCode = "04050104020402000304040405060504000506030403010006010601050400000001020404030604000400060504040404030105060106030401020005010001040402020102040504040006030604000205050300040305040303050202020303030306030000060002040402000206040101050101050302020002020502020001020105060006050305060105060603030100060402030604020101010401040305060103060206020402030405030204000403030300000005000304000503000405020302050502000505000102010000010605010202040101020605020104050105020505050002000405040205030306000101060201030601060602000006050301010001020504010604030106060001050001060601040105000405040001020601050200020004050205030002010105030603060106030501040104010502040604010203000106020602010304020502050006000002020200030502050106030506030305",
        pathCode = new string[1] { "222195192165162135132105102077074047072073070043040013010" },
        enemies = new int[1] { 2, },
        enemyChance = new float[1] { 1000 },
        enemyMax = 50,
        spawnX = 0f,
        spawnY = -194f,
        levelMin = 88,
        levelMax = 91,
        spawnInterval = 40.8f,
        mapId = 0,
        width = 700,
        height = 350,

    };

    public MapInfo DualPath = new MapInfo
    {
        mapName = "Dual Path Map",
        mapCode = "01000105010304050500040603000604040406060500050306040300040601030205050403010300020300000501060006050202000003020300010106050301050600040603040601010404030606030505030303000503030605020403030306040601020306010300030200010601030202010300050000040303020605040301060605030403060202040003020603030601020500000302060304000003040504060506030300010102000505010605010001010305060406000600060404060604030604040205040605020606040604050204040604000003000301050104060003020605060200030306040103020505010103020506020104050501010204010201010406010104040100030200000606020004020105010501030201030100060304040306010602050501030204030100020602020300060200030406050204020201060403020203000506000205020102020104040604020103000103020402030203010306",
        pathCode = new string[2] { "022023048049076079104105130131156157184187214217244247274277304307334", "138139164165162135132105102075101126127152153178181208211238241268271298301300275274249248223250" },
        enemies = new int[1] { 1, },
        enemyChance = new float[1] { 1000 },
        enemyMax = 50,
        spawnX = -241f,
        spawnY = -205f,
        levelMin = 1,
        levelMax = 4,
        spawnInterval = 3.8f,
        mapId = 1,
        width = 700,
        height = 350,

    };

    public MapInfo SmallMap = new MapInfo
    {
        mapName = "Small Map",
        mapCode = "04060504040102020501000504000301000404000006060004040601010400020400050006060005030601020006040603040006000103020100040000000106000605040104",
        pathCode = new string[1] { "040041052039036037048049046033030031042029" },
        enemies = new int[1] { 1, },
        enemyChance = new float[1] { 1000 },
        enemyMax = 50,
        spawnX = -50f,
        spawnY = -118f,
        levelMin = 1,
        levelMax = 4,
        spawnInterval = 3.8f,
        mapId = 2,
        width = 350,
        height = 175,

    };

}




public class Maps : MonoBehaviour
{
    public Sprite[] tileSprites;
    public Sprite[] tileTypeSprites;

    public AllMaps allMaps = new AllMaps();
    public Dictionary<string, MapInfo> allMapsDict = new Dictionary<string, MapInfo>();
    public Dictionary<int, Sprite> allTileSpritesDict = new Dictionary<int, Sprite>();
    //public Dictionary<int, Sprite> tileTypeSpritesDict = new Dictionary<int, Sprite>();


    private void Awake()
    {
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
    }

}
