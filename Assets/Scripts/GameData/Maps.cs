using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MapInfo
{
    public string mapName;
    public string mapCode;
    public int mapId;
    public string pathCode;
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
        mapCode = "2020011221301310111011011022211210222010320302130212110201011011201011303330032201221010210111221002013123112012202101200011200221103213100222012121012021020212013123102201012020122012022210003213101011012022122211000000003121210010122100220022111112012010221012100120020120022022111022200021121210211102002111122101211110221011000101220020111020001000211201212221",
        pathCode = "222195192165162135132105102077074047072073070043040013010",
        enemies = new int [3] { 1, 2, 3 },
        enemyChance = new float[3] {333, 666, 1000},
        enemyMax = 50,
        spawnX = 0f,
        spawnY = -194f,
        levelMin = 1,
        levelMax = 4,
        spawnInterval = 1.8f,

    };

}

public class Maps : MonoBehaviour
{
    public AllMaps allMaps = new AllMaps();
    public Dictionary<string, MapInfo> allMapsDict = new Dictionary<string, MapInfo>();


    private void Awake()
    {
        AddAllMaps();
    }

    public void AddAllMaps()
    {
        allMapsDict.Add(allMaps.TestMap.mapName, allMaps.TestMap);
    }

}
