using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class WorldMap : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject mapObject, worldMapObject, mapDetailsObject;
    
    public TMP_Dropdown mapSelector;
    public List<string> mapNames = new List<string>();
    public string mapName;
    private int mapCount;
    private MapDetails mapDetails;
    public Button loadTowerMenuBtn;

    //gameobject that is on every map that gives info about any given tile, as well as allowing the player to build a tower on that tile[if possible]
    public GameObject tileInfoMenu;
    private MapTile activeTile, tempTile;

    public bool isTapping;
    public float acumTime;


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
            mapDetails.mapCode = allMaps[mapName].mapCode;
            mapDetails.enemyMax = allMaps[mapName].enemyMax;
            mapDetails.levelMin = allMaps[mapName].levelMin;
            mapDetails.levelMax = allMaps[mapName].levelMax;
            mapDetails.spawnInterval = allMaps[mapName].spawnInterval;
            mapDetails.width = allMaps[mapName].width;
            mapDetails.height = allMaps[mapName].height;

            //clear the active tiles and active towers list
            GameManager.Instance.activeTiles.Clear();
            GameManager.Instance.activeTowers.Clear();


            mapDetails.LoadMap(mapName);
            mapObject.transform.position = new Vector2(0f, 0f);
            loadTowerMenuBtn.interactable = true;
            GameManager.Instance.inGame = true;
            GameManager.Instance.activeMap = mapDetails;
            mapDetailsObject.SetActive(true);
            worldMapObject.SetActive(false);
        }
        

            

       


        

        //mapObject.GetComponentInChildren<MapDetails>().DisplayMap(mapName);
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        var worldMousePosition =
                    Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var x = worldMousePosition.x;
        var y = worldMousePosition.y;

        if (Input.GetMouseButton(0))
        {
                var clickPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
                RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "MapTile" && !isTapping)
                {
                    isTapping = true;
                    activeTile = hit.collider.gameObject.GetComponent<MapTile>();

                }

            }

            
        }


        if (eventData.pointerEnter)
        {
            var tag = eventData.pointerEnter.gameObject.tag;
            var name = eventData.pointerEnter.gameObject.name;


            //if a UI object is tapped, turn the camera off
            if (name == "Handle")
            {

                GameManager.Instance.FreezeCameraMotion();
                Debug.Log(name);
            }


        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //if a player just taps a tile, open the tile. if they hold, do nothing
        if (isTapping)
        {
            if (acumTime <= .15f)
            {
                if (tileInfoMenu.GetComponent<MapTileMenu>().activeTile != null)
                {
                    tileInfoMenu.SetActive(true);
                    tileInfoMenu.GetComponent<MapTileMenu>().activeTile.ActiveTile();
                    tileInfoMenu.GetComponent<MapTileMenu>().LoadTile(activeTile);
                    tileInfoMenu.GetComponent<MapTileMenu>().activeTile.ActiveTile();
                    isTapping = false;

                }
                else
                {
                    tileInfoMenu.SetActive(true);
                    tileInfoMenu.GetComponent<MapTileMenu>().LoadTile(activeTile);
                    tileInfoMenu.GetComponent<MapTileMenu>().activeTile.ActiveTile();
                    isTapping = false;
                }
              
            }
            else
            {
                //
                isTapping = false;
                //activeTile = null;
                
            }
        }

        

    }

    // Update is called once per frame
    void Update()
    {
        if (isTapping == true)
        {
            acumTime += Time.deltaTime;
        }
        else
        {
            acumTime = 0;
        }

    }
}
