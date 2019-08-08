using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorldMap : MonoBehaviour
{
    public GameObject mapObject, worldMapObject;
    
    public TMP_Dropdown mapSelector;
    public List<string> mapNames = new List<string>();
    public string mapName;
    private int mapCount;
    private MapDetails mapDetails;
    public Button loadTowerMenuBtn;

    // Start is called before the first frame update
    void Start()
    {
        mapDetails = GetComponent<MapDetails>();
        var allMaps = GameManager.Instance.GetComponent<Maps>().allMapsDict;
        mapSelector.GetComponent<TMP_Dropdown>();
        

        foreach (KeyValuePair<string, MapInfo> map in allMaps)
        {
            mapNames.Add(map.Key);
            
        }

        mapSelector.AddOptions(mapNames);
        mapSelector.value = 0;
    }

    public void ChooseMap()
    {
        var allMaps = GameManager.Instance.GetComponent<Maps>().allMapsDict;
        int menuIndex = mapSelector.value;
        List<TMP_Dropdown.OptionData> menuOptions = mapSelector.options;
        mapName = menuOptions[menuIndex].text;

        if (allMaps.ContainsKey(mapName))
        {
            mapDetails.mapName = mapName;
            mapDetails.levelCode = allMaps[mapName].mapCode;
            mapDetails.enemyMax = allMaps[mapName].enemyMax;
            mapDetails.levelMin = allMaps[mapName].levelMin;
            mapDetails.levelMax = allMaps[mapName].levelMax;
            mapDetails.spawnInterval = allMaps[mapName].spawnInterval;

            mapDetails.LoadMap(mapName);
            mapObject.transform.position = new Vector2(0f, 0f);
            loadTowerMenuBtn.interactable = true;
            worldMapObject.SetActive(false);
        }
        

            

       


        

        //mapObject.GetComponentInChildren<MapDetails>().DisplayMap(mapName);
    }

    // Update is called once per frame
    void Update()
    {
        

    }
}
