using System.Collections;
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
    public GameObject activeItem;
    

    private AccountInfo account;
    public int itemBuyValue, itemSellValue, yourCoins;
    
    

    // Start is called before the first frame update
    void Start()
    {
        account = GameManager.Instance.GetComponent<YourAccount>().account;
        
    }

    // Update is called once per frame
    void Update()
    {
        yourCoins = (int)Mathf.Round((float)account.coins);
    }

    public void DisplayEquipment(Equipment equip, GameObject obj)
    {
        GameObject[] r = GameObject.FindGameObjectsWithTag("Respawn");
        
        for (int i = 0; i < r.Length; i++)
        {
            Destroy(r[i]);
        }


        int itemQuantity = 0;

        for (int i = 0; i < GameManager.Instance.Inventory.EquipmentPocket.items.Count; i++)
        {
            PocketItem p = GameManager.Instance.Inventory.EquipmentPocket.items[i];
            if (p.itemName == equip.itemName)
            {
                itemQuantity += 1;
            }
        }
        //GameObject item = Instantiate(equip.equip.equipPrefab, transform.position, Quaternion.identity);
        GameObject item = Instantiate(obj, transform.position, Quaternion.identity);
        item.transform.SetParent(transform);
        item.GetComponentInChildren<TMP_Text>().text = "";
        item.transform.position = new Vector3(itemSprite.transform.position.x, itemSprite.transform.position.y, -2f);
        //item.transform.localScale = new Vector3(item.transform.localScale.x * 2f, item.transform.localScale.y * 2f, 1f);
        //item.GetComponent<SpriteRenderer>().sortingLayerName = "PopMenu";
        item.tag = "Respawn";

        //itemSprite.GetComponent<SpriteRenderer>().sprite = equip.GetComponent<Image>().sprite;
        nameText.text = equip.itemName;
        typeText.text = equip.equipClass.ToString();

        //if the item doesn't have a required time, or doesn't even list one, check for that
        if (equip.typeMonsterReq != "")
        {
            typeReqText.text = "Required Type: " + equip.typeMonsterReq;
        }
        else
        {
            typeReqText.text = "Required Type: none";
        }

        boostsText.text = equip.equipment.description;
        //yourQuantityText.text = "On hand: " + PlayerPrefs.GetInt(equip.itemName).ToString();
        yourQuantityText.text = "On hand: " + itemQuantity;
        buyCostText.text = "Buy For: " + equip.cost.ToString();

        float sellValue = equip.cost * .8f;
        sellCostText.text = "Sell For: " + sellValue;

        activeItemName = equip.itemName;
        activeItem = obj;

        itemBuyValue = (int)equip.cost;
        itemSellValue = (int)sellValue;

        PriceCheck();
        
    }

    //use this to display any item that isn't an Equipment
    public void DisplayItem(string name, DisplayMode displayMode, GameObject Item)
    {
        GameObject[] r = GameObject.FindGameObjectsWithTag("Respawn");

        for (int i = 0; i < r.Length; i++)
        {
            Destroy(r[i]);
        }

        if (displayMode == DisplayMode.Consumable)
        {
            int itemQuantity = 0;

            for (int i = 0; i < GameManager.Instance.Inventory.ConsumablePocket.items.Count; i++)
            {
                PocketItem p = GameManager.Instance.Inventory.ConsumablePocket.items[i];
                if (p.itemName == name)
                {
                    itemQuantity += 1;
                }
            }


            GameObject item = Instantiate(Item, transform.position, Quaternion.identity);
            item.transform.SetParent(transform);
            item.transform.position = new Vector3(itemSprite.transform.position.x, itemSprite.transform.position.y, -2f);
            item.GetComponentInChildren<TMP_Text>().text = "";
            item.tag = "Respawn";

            nameText.text = name;
            typeText.text = "Consumable Item";
            typeReqText.text = "";
            boostsText.text = item.GetComponent<ConsumableObject>().consumableItem.description;
            //yourQuantityText.text = "On hand: " + PlayerPrefs.GetInt(name).ToString();
            yourQuantityText.text = "On hand: " + itemQuantity;
            buyCostText.text = "Buy For: " + item.GetComponent<ConsumableObject>().consumableItem.cost.ToString();


            float sellValue = item.GetComponent<ConsumableObject>().consumableItem.cost * .8f;
            sellCostText.text = "Sell For: " + sellValue;

            activeItemName = item.GetComponent<ConsumableObject>().consumableItem.itemName;
            activeItem = Item;

            itemBuyValue = (int)item.GetComponent<ConsumableObject>().consumableItem.cost;
            itemSellValue = (int)sellValue;

            PriceCheck();
        }
    }

    public void BuyItemBtn()
    {

        if (GameManager.Instance.GetComponent<YourAccount>().account.coins >= itemBuyValue)
        {
            //int itemCount = PlayerPrefs.GetInt(activeItemName, 0);
            //PlayerPrefs.SetInt(activeItemName, itemCount + 1);

            GameManager.Instance.GetComponent<YourItems>().GetYourItems();


            if (GameManager.Instance.items.allEquipsDict.ContainsKey(activeItemName))
            {
                
                Equipment e = new Equipment(GameManager.Instance.items.allEquipsDict[activeItemName]);
                e.AddToInventory(1);
            }

            if (GameManager.Instance.items.allConsumablesDict.ContainsKey(activeItemName))
            {
                GameManager.Instance.items.allConsumablesDict[activeItemName].AddToInventory(1);
            }

            GameManager.Instance.GetComponent<YourAccount>().account.coins -= itemBuyValue;
            GetComponentInParent<ItemShop>().UpdateItem();
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("You cannot afford this");
        }
        

    }

    public void SellItemBtn()
    {
        //int itemCount = PlayerPrefs.GetInt(activeItemName, 0);
        //PlayerPrefs.SetInt(activeItemName, itemCount - 1);

        if (GameManager.Instance.items.allEquipsDict.ContainsKey(activeItemName))
        {
            activeItem.GetComponent<EquipmentObject>().equipment.RemoveFromInventory();
        }
        if (GameManager.Instance.items.allConsumablesDict.ContainsKey(activeItemName))
        {
            activeItem.GetComponent<ConsumableObject>().consumableItem.RemoveFromInventory();
        }

        GameManager.Instance.GetComponent<YourItems>().GetYourItems();

        GameManager.Instance.GetComponent<YourAccount>().account.coins += itemSellValue;
        GetComponentInParent<ItemShop>().UpdateItem();
        gameObject.SetActive(false);
    }


    //use this to check your coins against the value of the item being inspected
    public void PriceCheck()
    {
        if (GameManager.Instance.GetComponent<YourAccount>().account.coins < itemBuyValue)
        {
            buyButton.interactable = false;
        }
        else
        {
            buyButton.interactable = true;
        }
    }

    public void OnDisable()
    {
        GameObject[] r = GameObject.FindGameObjectsWithTag("Respawn");

        for (int i = 0; i < r.Length; i++)
        {
            Destroy(r[i]);
        }
    }
}
