using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MapTileMenu : MonoBehaviour, IPointerDownHandler
{
    public TMP_Text tileNumberText, tileLevelText, levelPercentText, minedTimeText;
    public SpriteRenderer tileSprite, tileAttSprite, monsterSprite;
    public GameObject monsterInfoMenu;
    public MonsterInfoMenus infoMenu;

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
        tileSprite.GetComponent<SpriteRenderer>();
        tileAttSprite.GetComponent<SpriteRenderer>();
        infoMenu.GetComponent<MonsterInfoMenus>();
    }

    //a tile is imported to this method from the World Map script
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
        
        ////if there is a monster on the tile, display it on the menu
        //if (monsterOnTile)
        //{
        //    monsterSprite.GetComponent<Image>().color = Color.white;
        //    monsterSprite.GetComponent<Image>().sprite = monsterOnTile.frontModel.GetComponent<SpriteRenderer>().sprite;
        //    monsterSprite.tag = "Monster";
        //}
        //else
        //{
        //    monsterSprite.GetComponent<Image>().color = Color.clear;
        //    monsterSprite.GetComponent<Image>().sprite = null;
        //    monsterSprite.tag = "Untagged";
        //}

        float totalNextLevelNeeded = (float)GameManager.Instance.tileLevelUp[tile.info.level + 1];
        float thisLevelNeeded = (float)GameManager.Instance.tileLevelUp[tile.info.level];

        float nextLevelNeeded = totalNextLevelNeeded - thisLevelNeeded;

        tileExpBar.BarProgress = (nextLevelNeeded  - tile.info.expToLevel) / nextLevelNeeded;
        levelPercentText.text = "Level Percentage: " + System.Math.Round((tileExpBar.BarProgress * 100f), 2) + "%";
        //Debug.Log(totalNextLevelNeeded - (float)tile.info.totalExp / nextLevelNeeded);

    }

    //snap the camera to the active tile
    public void FindTileBtn()
    {
        mainCamera.transform.position = new Vector3(activeTile.transform.position.x, activeTile.transform.position.y, -10f);
        mainCamera.orthographicSize = 100;
    }

    // Update is called once per frame
    void Update()
    {

        

        if (activeTile)
        {
            
            LoadTile(activeTile);
            

            if (activeTile.isMining)
            {
                miningButton.GetComponentInChildren<TMP_Text>().text = "Stop Mining";
            }
            else
            {
                miningButton.GetComponentInChildren<TMP_Text>().text = "Start Mining";
            }

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
                infoMenu.activeMonster = monsterOnTile;
            }
        }
    }




    //click this to open the monster menu to place a monster on the given tile
    public void PlaceMonsterBtn()
    {
        //if there is a monster on the tile, then just go to the monster instead of letting one be placed
        if (activeTile.hasMonster)
        {

            infoMenu.gameObject.SetActive(true);
            infoMenu.activeMonster = activeTile.monsterOn;
            mainCamera.transform.position = new Vector3(activeTile.transform.position.x, activeTile.transform.position.y, -10f);
            mainCamera.orthographicSize = 35;
        }
        else
        {
            if (infoMenu.towerMenu.activeSelf == false)
            {
                infoMenu.showTowersBtn.gameObject.SetActive(false);
                infoMenu.towerMenu.SetActive(true);
            }
            
            infoMenu.tileToBePlaced = activeTile;
        }
    }

    //click this to start mining the tile with your companion
    public void StartMiningBtn()
    {
        if (!activeTile.isMining)
        {
            activeTile.StartMining();
        }
        else
        {
            activeTile.StopMining();
        }
        
    }

    public void CloseWindow()
    {

        activeTile.ActiveTile();
        activeTile = null;
        gameObject.SetActive(false);
    }

}
