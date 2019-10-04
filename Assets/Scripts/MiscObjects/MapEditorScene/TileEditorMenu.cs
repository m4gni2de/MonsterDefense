using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TileEditorMenu : MonoBehaviour
{
    public TMP_Text tileNumberText, tileTypeText, tileAttText;
    public Button typeBtn, attBtn;

    public TMP_Dropdown typeChangeDrop, attChangeDrop;

    public MapTile activeTile;

    
    // Start is called before the first frame update
    void Start()
    {
        typeChangeDrop.GetComponent<TMP_Dropdown>();
        attChangeDrop.GetComponent<TMP_Dropdown>();
        typeBtn.GetComponent<Button>();
        attBtn.GetComponent<Button>();

        List<string> types = new List<string>();
        List<string> atts = new List<string>();

        foreach (KeyValuePair<string, string> tileTypes in GetComponentInParent<MapEditor>().TileValues)
        {
            types.Add(tileTypes.Value);

        }

        foreach (KeyValuePair<string, string> attTypes in GetComponentInParent<MapEditor>().TileAttributes)
        {
            atts.Add(attTypes.Value);

        }

        typeChangeDrop.AddOptions(types);
        attChangeDrop.AddOptions(atts);
        //typeChangeDrop.value = 0;
        //attChangeDrop.value = 0;
    }

    //get the tile information from the MapEditor script
    public void SetActiveTile(MapTile tile)
    {
        activeTile = tile;
        tileNumberText.text = "Tile Number: " + activeTile.tileNumber;
        tileTypeText.text = activeTile.tileType.ToString();
        tileAttText.text = "Tile Attribute: " + activeTile.tileAtt.ToString();

        //typeChangeDrop.value = activeTile.type
        attChangeDrop.value = activeTile.tileAttInt;

    }

    //change the tile's type; from buildable, to road, etc
    public void ChangeTileType()
    {
        typeChangeDrop.interactable = !typeChangeDrop.interactable;
        attBtn.interactable = false;
    }

    public void TypeValueChange()
    {

    }

    public void AttributeValueChange()
    {
        activeTile.ClearAttribute();
        activeTile.GetAttribute(attChangeDrop.value);
        SetActiveTile(activeTile);
        string mapCode = GetComponentInParent<MapDetails>().mapCode;
        string attValue = "";

        if (attChangeDrop.value < 10)
        {
            attValue = "0" + attChangeDrop.value;
        }
        else
        {
            attValue = attChangeDrop.value.ToString();
        }

        mapCode = mapCode.Remove(activeTile.tileNumber * 2, 2);
        mapCode = mapCode.Insert(activeTile.tileNumber * 2, attValue);

        GetComponentInParent<MapDetails>().mapCode = mapCode;
        typeBtn.interactable = true;
    }

    //change the tile's attribute
    public void ChangeTileAttribute()
    {
        attChangeDrop.interactable = !attChangeDrop.interactable;
        typeBtn.interactable = false;
    }

   

    // Update is called once per frame
    void Update()
    {
        
    }
}
