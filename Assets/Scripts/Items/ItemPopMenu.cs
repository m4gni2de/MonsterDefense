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

    public void DisplayEquipment(EquipmentItem equip, GameObject obj)
    {
        GameObject[] r = GameObject.FindGameObjectsWithTag("Respawn");
        
        for (int i = 0; i < r.Length; i++)
        {
            Destroy(r[i]);
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
        nameText.text = equip.equip.name;
        typeText.text = equip.equip.equipType.ToString();

        //if the item doesn't have a required time, or doesn't even list one, check for that
        if (equip.equip.typeMonsterReq != "")
        {
            typeReqText.text = "Required Type: " + equip.equip.typeMonsterReq;
        }
        else
        {
            typeReqText.text = "Required Type: none";
        }

        boostsText.text = equip.equip.description;
        yourQuantityText.text = "On hand: " + PlayerPrefs.GetInt(equip.equip.name).ToString();
        buyCostText.text = "Buy For: " + equip.equip.cost.ToString();

        float sellValue = equip.equip.cost * .8f;
        sellCostText.text = "Sell For: " + sellValue;

        activeItemName = equip.equip.name;
        activeItem = obj;

        itemBuyValue = (int)equip.equip.cost;
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
            GameObject item = Instantiate(Item, transform.position, Quaternion.identity);
            item.transform.SetParent(transform);
            item.transform.position = new Vector3(itemSprite.transform.position.x, itemSprite.transform.position.y, -2f);
            item.GetComponentInChildren<TMP_Text>().text = "";
            item.tag = "Respawn";

            nameText.text = name;
            typeText.text = "Consumable Item";
            typeReqText.text = "";
            boostsText.text = item.GetComponent<ConsumableObject>().consumableItem.description;
            yourQuantityText.text = "On hand: " + PlayerPrefs.GetInt(item.name).ToString();
            buyCostText.text = "Buy For: " + item.GetComponent<ConsumableObject>().consumableItem.cost.ToString();


            float sellValue = item.GetComponent<ConsumableObject>().consumableItem.cost * .8f;
            sellCostText.text = "Sell For: " + sellValue;

            activeItemName = Item.name;
            activeItem = Item;

            itemBuyValue = (int)item.GetComponent<ConsumableObject>().consumableItem.cost;
            itemSellValue = (int)sellValue;

            PriceCheck();
        }
    }

    public void BuyItemBtn()
    {

        if (account.coins >= itemBuyValue)
        {
            int itemCount = PlayerPrefs.GetInt(activeItemName, 0);
            PlayerPrefs.SetInt(activeItemName, itemCount + 1);

            GameManager.Instance.GetComponent<YourItems>().GetYourItems();


            account.coins -= itemBuyValue;
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
        int itemCount = PlayerPrefs.GetInt(activeItemName, 0);
        PlayerPrefs.SetInt(activeItemName, itemCount - 1);

        GameManager.Instance.GetComponent<YourItems>().GetYourItems();

        account.coins += itemSellValue;
        GetComponentInParent<ItemShop>().UpdateItem();
        gameObject.SetActive(false);
    }


    //use this to check your coins against the value of the item being inspected
    public void PriceCheck()
    {
        if (yourCoins < itemBuyValue)
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
