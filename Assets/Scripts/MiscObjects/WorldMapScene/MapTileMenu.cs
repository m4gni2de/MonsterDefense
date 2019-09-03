using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MapTileMenu : MonoBehaviour, IPointerDownHandler
{
    public TMP_Text tileNumberText, tileAttText;
    public SpriteRenderer tileSprite;
    public Image monsterSprite;
    public GameObject monsterInfoMenu;
    public MonsterInfoMenus infoMenu;

    public MapTile activeTile;
    private Monster monsterOnTile;

    //the main Camera on the map
    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        tileSprite.GetComponent<SpriteRenderer>();
        infoMenu.GetComponent<MonsterInfoMenus>();
    }

    //a tile is imported to this method from the World Map script
    public void LoadTile(MapTile tile)
    {
        activeTile = tile;
        

        tileNumberText.text = "Tile Number: " + tile.tileNumber.ToString();
        tileAttText.text = "Tile Attribute: " + tile.tileAtt.ToString();
        tileSprite.sprite = tile.GetComponent<SpriteRenderer>().sprite;

        monsterOnTile = tile.monsterOn;
        
        //if there is a monster on the tile, display it on the menu
        if (monsterOnTile)
        {
            monsterSprite.GetComponent<Image>().color = Color.white;
            monsterSprite.GetComponent<Image>().sprite = monsterOnTile.frontModel.GetComponent<SpriteRenderer>().sprite;
            monsterSprite.tag = "Monster";
        }
        else
        {
            monsterSprite.GetComponent<Image>().color = Color.clear;
            monsterSprite.GetComponent<Image>().sprite = null;
            monsterSprite.tag = "Untagged";
        }
        

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
}
