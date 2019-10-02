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
        enemies = new int[3] { 1, 2, 3 },
        enemyChance = new float[3] { 333, 666, 1000 },
        enemyMax = 50,
        spawnX = 0f,
        spawnY = -194f,
        levelMin = 1,
        levelMax = 91,
        spawnInterval = 3.8f,
        mapId = 0,
        width = 700,
        height = 350,

    };

    public MapInfo DualPath = new MapInfo
    {
        mapName = "Dual Path Map",
        mapCode = "01000105010304050500040603000604040406060500050306040300040601030205050403010300020300000501060006050202000003020300010106050301050600040603040601010404030606030505030303000503030605020403030306040601020306010300030200010601030202010300050000040303020605040301060605030403060202040003020603030601020500000302060304000003040504060506030300010102000505010605010001010305060406000600060404060604030604040205040605020606040604050204040604000003000301050104060003020605060200030306040103020505010103020506020104050501010204010201010406010104040100030200000606020004020105010501030201030100060304040306010602050501030204030100020602020300060200030406050204020201060403020203000506000205020102020104040604020103000103020402030203010306",
        pathCode = new string[2] { "022023048049076079104105130131156157184187214217244247274277304307334", "138139164165162135132105102075100101126127152153178181208211238241268271298301300275274249248223250" },
        enemies = new int[3] { 1, 2, 3 },
        enemyChance = new float[3] { 333, 666, 1000 },
        enemyMax = 50,
        spawnX = -241f,
        spawnY = -205f,
        levelMin = 1,
        levelMax = 99,
        spawnInterval = 3.8f,
        mapId = 1,
        width = 700,
        height = 350,

    };

    public MapInfo SmallMap = new MapInfo
    {
        mapName = "Small Map",
        mapCode = "07070504040102020501000504000301000404000006060004040601010400020400050006060005030601020006040603040006000103020100040000000106000605040104",
        pathCode = new string[1] { "040041052039036037048049046033030031042029" },
        enemies = new int[1] { 4 },
        //enemies = new int[3] { 1, 2, 3 },
        //enemyChance = new float[3] { 333, 666, 1000 },
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

    public MapInfo BaseMap = new MapInfo
    {
        mapName = "Base Map",
        mapCode = "04060500020006040002020403000000030206030601020300010303000203010306040506030004040002000303040005050202050205020100050405030504020004050302",
        pathCode = new string[1] { "040027024025036037048035032019018007020023034035048049060061" },
        enemies = new int[3] { 1, 2, 3 },
        enemyChance = new float[3] { 333, 666, 1000 },
        enemyMax = 50,
        spawnX = -10.4f,
        spawnY = -100.7f,
        levelMin = 1,
        levelMax = 4,
        spawnInterval = 3.8f,
        mapId = 3,
        width = 350,
        height = 175,

    };

}




public class Maps : MonoBehaviour
{
    public AllMaps allMaps = new AllMaps();
    public Dictionary<string, MapInfo> allMapsDict = new Dictionary<string, MapInfo>();
    public Dictionary<int, Sprite> allTileSpritesDict = new Dictionary<int, Sprite>();
    //public Dictionary<int, Sprite> tileTypeSpritesDict = new Dictionary<int, Sprite>();


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
