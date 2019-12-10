using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class TileInfoObject : MonoBehaviour, IPointerDownHandler
{
    public TMP_Text tileNumberText, tileLevelText, levelPercentText, minedTimeText;
    public SpriteRenderer tileSprite, tileAttSprite, monsterSprite;
    public GameObject monsterInfoMenu;
    public DefendersMain defendersMain;

    public Button miningButton, placeMonsterBtn;

    public MapTile activeTile;
    private Monster monsterOnTile;

    //the main Camera on the map
    public Camera mainCamera;

    //a list of the monsters who are able to be placed on to the given Map Tile
    public List<Monster> placeableMonsters = new List<Monster>();

    //used to show the amount of exp a tile has to the next level
    public EnergyBar tileExpBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (activeTile)
        {
            if (activeTile.hasMonster)
            {
                placeMonsterBtn.GetComponentInChildren<TMP_Text>().text = "Find Tower";
                monsterSprite.sprite = activeTile.monsterOn.frontModel.GetComponent<SpriteRenderer>().sprite;
            }
            else
            {
                placeMonsterBtn.GetComponentInChildren<TMP_Text>().text = "Place Tower";
                monsterSprite.sprite = null;
            }
        }
    }

    public void LoadTile(MapTile tile)
    {
        var types = GameManager.Instance.monstersData.typeChartDict;
        activeTile = tile;


        tileNumberText.text = tile.tileNumber.ToString();
        minedTimeText.text = "Mined for: " + System.Math.Round(tile.mineTotalTime, 0).ToString() + " s";
        tileLevelText.text = tile.info.level.ToString();
        tileSprite.sprite = tile.GetComponent<SpriteRenderer>().sprite;

        if (types.ContainsKey(tile.tileAtt.ToString()))
        {
            tileAttSprite.sprite = types[tile.tileAtt.ToString()].typeSprite;
        }
        else
        {
            tileAttSprite.sprite = null;
        }


        monsterOnTile = tile.monsterOn;


        float totalNextLevelNeeded = (float)GameManager.Instance.tileLevelUp[tile.info.level + 1];
        float thisLevelNeeded = (float)GameManager.Instance.tileLevelUp[tile.info.level];

        float nextLevelNeeded = totalNextLevelNeeded - thisLevelNeeded;

        tileExpBar.BarProgress = (nextLevelNeeded - tile.info.expToLevel) / nextLevelNeeded;
        levelPercentText.text = "Level Percentage: " + System.Math.Round((tileExpBar.BarProgress * 100f), 2) + "%";


        

    }

    //click this to open the monster menu to place a monster on the given tile
    public void PlaceMonsterBtn()
    {
        //if there is a monster on the tile, then just go to the monster instead of letting one be placed
        if (activeTile.hasMonster)
        {

            monsterInfoMenu.SetActive(true);
            defendersMain.GetComponent<MonsterInfoMenus>().activeMonster = activeTile.monsterOn;
            mainCamera.transform.position = new Vector3(activeTile.transform.position.x, activeTile.transform.position.y, -10f);
            mainCamera.orthographicSize = 35;

            
        }
        else
        {
            if (defendersMain.towersMenu.activeSelf == false)
            {
                defendersMain.showTowersBtn.gameObject.SetActive(false);
                defendersMain.towersMenu.SetActive(true);
            }

            defendersMain.activeTile = activeTile;
            
        }

        
    }


    public void OnPointerDown(PointerEventData eventData)
    {

        if (eventData.pointerEnter)
        {
            var tag = eventData.pointerEnter.gameObject.tag;
            var hit = eventData.pointerEnter.gameObject;

            //if the monster icon that is on the tile is touched by the player, set that monster as the active monster on the map
            if (tag == "Monster")
            {
                monsterInfoMenu.SetActive(true);
                monsterInfoMenu.GetComponent<MonsterInfoMenus>().activeMonster = monsterOnTile;
            }
        }
    }

    public void CloseMenu()
    {
        activeTile.ActiveTile();
        activeTile = null;
        gameObject.SetActive(false);
    }



}
