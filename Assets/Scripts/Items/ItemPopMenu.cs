﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPopMenu : MonoBehaviour
{
    public GameObject itemSprite;

    public TMP_Text nameText, typeText, typeReqText, boostsText, yourQuantityText, buyCostText, sellCostText;

    public Button buyButton, sellButton;

    public string activeItemName;

    //public Items item;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayEquipment(EquipmentItem equip)
    {
        itemSprite.GetComponent<SpriteRenderer>().sprite = equip.GetComponent<Image>().sprite;
        nameText.text = equip.equip.name;
        typeText.text = equip.equip.equipType.ToString();
        typeReqText.text = "Required Type: " + equip.equip.typeMonsterReq;
        boostsText.text = equip.equip.description;
        yourQuantityText.text = "On hand: " + PlayerPrefs.GetInt(equip.equip.name).ToString();
        buyCostText.text = "Buy For: " + equip.equip.cost.ToString();

        float sellValue = equip.equip.cost * .8f;
        sellCostText.text = "Sell For: " + sellValue;

        activeItemName = equip.equip.name;
        
    }

    public void BuyItemBtn()
    {

        int itemCount =  PlayerPrefs.GetInt(activeItemName, 0);
        PlayerPrefs.SetInt(activeItemName, itemCount + 1);

        GameManager.Instance.GetComponent<YourItems>().GetYourItems();
        

        GetComponentInParent<ItemShop>().DisplayYourItems();
        gameObject.SetActive(false);

    }

    public void SellItemBtn()
    {
        int itemCount = PlayerPrefs.GetInt(activeItemName, 0);
        PlayerPrefs.SetInt(activeItemName, itemCount - 1);

        GameManager.Instance.GetComponent<YourItems>().GetYourItems();
      
        GetComponentInParent<ItemShop>().DisplayYourItems();
        gameObject.SetActive(false);
    }
}
