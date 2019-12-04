using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public enum DisplayMode
{
    Equipment = 1,
    Consumable = 2,
    Cells = 3
}
public class ItemShop : MonoBehaviour, IPointerDownHandler
{
    public GameObject itemSprite, shopItemSprite;
    public TMP_Text itemQuantity, shopItemCost;
    public GameObject[] itemSprites, shopItemSprites, itemList, shopItemList;
    public TMP_Text[] itemQuantities, shopItemCosts;
    public GameObject itemScrollContent, shopScrollContent, itemPopMenu;

    //private int shopSpriteTotal;

    public GameObject shopCanvas, itemCanvas;
    public GameObject itemPanel, shopPanel;

    public GameObject consumableObject, equipmentObject;
    
    //variable used to determine which items are being displayed in the shop
    public DisplayMode displayMode = new DisplayMode();

    //the amount of coins your account has
    public TMP_Text yourCoinsText;
    public int yourCoins;

    private void Awake()
    {
        
        //shopSpriteTotal = 0;
        itemQuantity.GetComponent<TMP_Text>();
        displayMode = DisplayMode.Equipment;


        for (int c = 0; c < 20; c++)
        {
            for (int r = 0; r < 4; r++)
            {
                //itemSprites[itemSpriteTotal] = Instantiate(itemSprite, itemScrollContent.transform.position, Quaternion.identity);
                //itemQuantities[itemSpriteTotal] = itemSprites[itemSpriteTotal].GetComponentInChildren<TMP_Text>();
                //itemSprites[itemSpriteTotal].transform.SetParent(itemScrollContent.transform, true);
                //itemSprites[itemSpriteTotal].transform.position = new Vector3(itemSprite.transform.position.x + (r * itemScrollContent.GetComponent<RectTransform>().rect.width /7), itemSprite.transform.position.y - (c * 30), itemSprite.transform.position.z);
                //itemSpriteTotal += 1;

                //shopItemSprites[shopSpriteTotal] = Instantiate(shopItemSprite, shopScrollContent.transform.position, Quaternion.identity);
                //shopItemCosts[shopSpriteTotal] = shopItemSprites[shopSpriteTotal].GetComponentInChildren<TMP_Text>();
                //shopItemSprites[shopSpriteTotal].transform.SetParent(shopScrollContent.transform, true);
                //shopItemSprites[shopSpriteTotal].transform.position = new Vector3(shopItemSprite.transform.position.x + (r * shopScrollContent.GetComponent<RectTransform>().rect.width / 6), shopItemSprite.transform.position.y - (c * 35), shopItemSprite.transform.position.z);
                //shopSpriteTotal += 1;


            }

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        //shopSpriteTotal = 0;


        DisplayShop();
        DisplayYourItems();
    }

    public void DisplayYourItems()
    {
       
        
        
        //shopSpriteTotal = 0;

        if (displayMode == DisplayMode.Equipment)
        {
            DisplayYourEquipment();
        }

        if (displayMode == DisplayMode.Consumable)
        {
            DisplayYourConsumables();
        }
    }


    //call this from other objects to reload the item quantities and values after buying/selling an items
    public void UpdateItem()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("Item");


        for (int i = 0; i < obj.Length; i++)
        {
            Destroy(obj[i]);

            if (i >= obj.Length - 1)
            {
                if (displayMode == DisplayMode.Equipment)
                {
                    DisplayYourEquipment();
                    DisplayShop();
                }

                if (displayMode == DisplayMode.Consumable)
                {
                    DisplayYourConsumables();
                    DisplayShop();
                }

                return;
            }
        }

        if (obj.Length == 0)
        {
            if (displayMode == DisplayMode.Equipment)
            {
                DisplayYourEquipment();
                DisplayShop();
            }

            if (displayMode == DisplayMode.Consumable)
            {
                DisplayYourConsumables();
                DisplayShop();
            }
        }
        

       

    }


    

    

    //use this to display the correct items in the shop
    public void DisplayShop()
    {
        if (displayMode == DisplayMode.Equipment)
        {
            DisplayShopEquipment();
        }

        if (displayMode == DisplayMode.Consumable)
        {
            DisplayShopConsumables();
        }

    }

    //use this to show the equipment items for sale in the shop
    public void DisplayShopEquipment()
    {
        var allEquips = GameManager.Instance.items.allEquipsDict;
        var allCells = GameManager.Instance.items.allMonsterCellsDict;
        //var allConsumables = GameManager.Instance.items.allConsumablesDict;

        int row = 0;
        float rowCheck = 0f;


        foreach (KeyValuePair<string, EquipmentScript> equip in allEquips)
        {
            string name = equip.Key;

            //Debug.Log(name);

            //if the player has more than 0 of the item, display it

                //EquipmentScript item = allEquips[name];
                Equipment item = new Equipment(allEquips[name]);

                GameObject a = Instantiate(equipmentObject, shopScrollContent.transform.position, Quaternion.identity);



                a.transform.SetParent(shopScrollContent.transform, true);

                a.GetComponent<EquipmentObject>().equipment = item;
                a.GetComponent<EquipmentObject>().LoadItem(item);
            //a.GetComponentInChildren<TMP_Text>().text = PlayerPrefs.GetInt(item.name).ToString();
            a.GetComponentInChildren<TMP_Text>().text = item.equipment.cost.ToString();

                item.equipment.ActivateItem(item.equipment, a);
                a.transform.position = new Vector3(shopItemSprite.transform.position.x + ((rowCheck * 4) * shopScrollContent.GetComponent<RectTransform>().rect.width / 6), shopItemSprite.transform.position.y - (row * 35), shopItemSprite.transform.position.z);
                a.transform.localScale = new Vector3(a.transform.localScale.x * 1.5f, a.transform.localScale.y * 1.5f, 1f);
                a.tag = "Item";
                a.name = item.itemName;


                rowCheck += .25f;

                if (rowCheck > .8f)
                {
                    rowCheck = 0f;
                    row += 1;
                }
            

        }
    }

    //use this to show the equipment items you have in your inventory
    void DisplayYourEquipment()
    {

        //var equips = GameManager.Instance.items.equipment;
        var allEquips = GameManager.Instance.items.allEquipsDict;
        var yourEquips = GameManager.Instance.GetComponent<YourItems>().yourEquipment;
        var allCells = GameManager.Instance.items.allMonsterCellsDict;
        //var allConsumables = GameManager.Instance.items.allConsumablesDict;
        int row = 0;
        float rowCheck = 0f;

        for (int i = 0; i < GameManager.Instance.Inventory.EquipmentPocket.items.Count; i++)
        {
            PocketItem p = GameManager.Instance.Inventory.EquipmentPocket.items[i];
            //EquipmentScript item = Instantiate(allEquips[p.itemName]);
            Equipment item = new Equipment(allEquips[p.itemName]);
            //set the item to match the slot in the inventory
            item.SetInventorySlot(p);
            //change the item's stats to match its level
            item.GetStats();
            int itemCount = PlayerPrefs.GetInt(item.itemName);

            GameObject a = Instantiate(equipmentObject, itemSprite.transform.position, Quaternion.identity);


            //a.GetComponent<EquipmentObject>().equipment.info.
            a.transform.SetParent(itemScrollContent.transform, true);

            a.GetComponent<EquipmentObject>().equipment = item;
            a.GetComponent<EquipmentObject>().LoadItem(item);
            //a.GetComponentInChildren<TMP_Text>().text = PlayerPrefs.GetInt(item.name).ToString();
            a.GetComponentInChildren<TMP_Text>().text = "Lv: " + p.itemLevel;
            a.transform.position = new Vector3(itemSprite.transform.position.x + ((rowCheck * 4) * itemScrollContent.GetComponent<RectTransform>().rect.width / 6), itemSprite.transform.position.y - (row * 35), itemSprite.transform.position.z);
            a.transform.localScale = new Vector3(a.transform.localScale.x * 1.5f, a.transform.localScale.y * 1.5f, 1f);
            a.tag = "Item";
            a.name = p.itemName;
            item.equipment.ActivateItem(item.equipment, a);

            rowCheck += .25f;

            if (rowCheck > .8f)
            {
                rowCheck = 0f;
                row += 1;
            }
        }

        //foreach (KeyValuePair<string, int> equip in yourEquips)
        //{
        //    string name = equip.Key;

        //    //Debug.Log(name);

        //    //if the player has more than 0 of the item, display it
        //    if (equip.Value > 0)
        //    {

        //        EquipmentScript item = allEquips[name];
        //        int itemCount = PlayerPrefs.GetInt(item.name);

        //        GameObject a = Instantiate(equipmentObject, itemSprite.transform.position, Quaternion.identity);



        //        a.transform.SetParent(itemScrollContent.transform, true);

        //        a.GetComponent<EquipmentObject>().equipment = item;
        //        a.GetComponent<EquipmentObject>().LoadItem(item);
        //        //a.GetComponentInChildren<TMP_Text>().text = PlayerPrefs.GetInt(item.name).ToString();
        //        a.GetComponentInChildren<TMP_Text>().text = equip.Value.ToString();
        //        a.transform.position = new Vector3(itemSprite.transform.position.x + ((rowCheck * 4) * itemScrollContent.GetComponent<RectTransform>().rect.width / 6), itemSprite.transform.position.y - (row * 35), itemSprite.transform.position.z);
        //        a.transform.localScale = new Vector3(a.transform.localScale.x * 1.5f, a.transform.localScale.y * 1.5f, 1f);
        //        a.tag = "Item";
        //        a.name = item.name;
        //        item.ActivateItem(item, a);

        //        rowCheck += .25f;

        //        if (rowCheck > .8f)
        //        {
        //            rowCheck = 0f;
        //            row += 1;
        //        }
        //    }

        //}

        
    }



    public void DisplayShopConsumables()
    {
        var consumables = GameManager.Instance.GetComponent<Items>().consumables;
        var allCells = GameManager.Instance.items.allMonsterCellsDict;
        //var allConsumables = GameManager.Instance.items.allConsumablesDict;

        int row = 0;
        float rowCheck = 0f;

        //Debug.Log("ok");

        foreach (ConsumableItem c in consumables)
        {
            
            ConsumableItem item = c;

            int itemCount = PlayerPrefs.GetInt(item.name);

            GameObject a = Instantiate(consumableObject, shopScrollContent.transform.position, Quaternion.identity);


            //itemQuantities[shopSpriteTotal] = itemSprites[shopSpriteTotal].GetComponentInChildren<TMP_Text>();
            a.transform.SetParent(shopScrollContent.transform, true);


            a.GetComponent<ConsumableObject>().consumableItem = item;
            a.GetComponent<ConsumableObject>().LoadItem();
            a.GetComponentInChildren<TMP_Text>().text = item.cost.ToString();
            a.transform.position = new Vector3(shopItemSprite.transform.position.x + ((rowCheck * 4) * shopScrollContent.GetComponent<RectTransform>().rect.width / 6), shopItemSprite.transform.position.y - (row * 35), itemSprite.transform.position.z);
            a.transform.localScale = new Vector3(a.transform.localScale.x * 1.5f, a.transform.localScale.y * 1.5f, 1f);
            a.tag = "Item";
            a.name = item.name;
            


            rowCheck += .25f;

            if (rowCheck > .8f)
            {
                rowCheck = 0f;
                row += 1;
            }


        }
    }

    //use this to show the consumable items you have in your inventory
    void DisplayYourConsumables()
    {
        var consumables = GameManager.Instance.GetComponent<YourAccount>().yourItems.yourConsumables;
        var allCons = GameManager.Instance.items.allConsumablesDict;
        int row = 0;
        float rowCheck = 0f;

        for (int i = 0; i < GameManager.Instance.Inventory.ConsumablePocket.items.Count; i++)
        {
            PocketItem item = GameManager.Instance.Inventory.ConsumablePocket.items[i];
            ConsumableItem cItem = Instantiate(allCons[item.itemName]);
            int itemCount = PlayerPrefs.GetInt(cItem.name);
            cItem.inventorySlot = item;

            GameObject a = Instantiate(consumableObject, itemScrollContent.transform.position, Quaternion.identity);


            //itemQuantities[shopSpriteTotal] = itemSprites[shopSpriteTotal].GetComponentInChildren<TMP_Text>();
            a.transform.SetParent(itemScrollContent.transform, true);


            a.GetComponent<ConsumableObject>().consumableItem = cItem;
            a.GetComponent<ConsumableObject>().LoadItem();
            a.GetComponentInChildren<TMP_Text>().text = "";
            a.transform.position = new Vector3(itemSprite.transform.position.x + ((rowCheck * 4) * itemScrollContent.GetComponent<RectTransform>().rect.width / 6), itemSprite.transform.position.y - (row * 35), itemSprite.transform.position.z);
            a.transform.localScale = new Vector3(a.transform.localScale.x * 1.5f, a.transform.localScale.y * 1.5f, 1f);
            a.tag = "Item";
            a.name = cItem.name;


            rowCheck += .25f;

            if (rowCheck > .8f)
            {
                rowCheck = 0f;
                row += 1;
            }
        }


        //foreach (KeyValuePair<string, int> item in consumables)
        //{
        //    if (consumables.ContainsKey(item.Key))
        //    {

        //        string name = item.Key;

        //        //if the player has more than 0 of the item, display it
        //        if (item.Value > 0)
        //        {

        //            ConsumableItem cItem = allCons[item.Key];
        //            int itemCount = PlayerPrefs.GetInt(cItem.name);

        //            GameObject a = Instantiate(consumableObject, itemScrollContent.transform.position, Quaternion.identity);


        //            //itemQuantities[shopSpriteTotal] = itemSprites[shopSpriteTotal].GetComponentInChildren<TMP_Text>();
        //            a.transform.SetParent(itemScrollContent.transform, true);


        //            a.GetComponent<ConsumableObject>().consumableItem = cItem;
        //            a.GetComponent<ConsumableObject>().LoadItem();
        //            a.GetComponentInChildren<TMP_Text>().text = PlayerPrefs.GetInt(item.Key).ToString();
        //            a.transform.position = new Vector3(itemSprite.transform.position.x + ((rowCheck * 4) * itemScrollContent.GetComponent<RectTransform>().rect.width / 6), itemSprite.transform.position.y - (row * 35), itemSprite.transform.position.z);
        //            a.transform.localScale = new Vector3(a.transform.localScale.x * 1.5f, a.transform.localScale.y * 1.5f, 1f);
        //            a.tag = "Item";
        //            a.name = cItem.name;


        //            rowCheck += .25f;

        //            if (rowCheck > .8f)
        //            {
        //                rowCheck = 0f;
        //                row += 1;
        //            }
        //        }
        //    }
        //}
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if (eventData.pointerEnter)
        {
            
            var tag = eventData.pointerEnter.gameObject.tag;
            var hit = eventData.pointerEnter.gameObject;

            //Debug.Log(tag);
            //if the menu is opened with the purpose of Equipping a monster with an item, then allow it to be equipped. Otherwise, show the item's details
            if (tag == "Item")
            {
                if (displayMode == DisplayMode.Equipment)
                {
                    //EquipmentScript item = hit.GetComponent<EquipmentObject>().equipment;
                    Equipment item = hit.GetComponent<EquipmentObject>().equipment;


                    itemPopMenu.SetActive(true);
                    itemPopMenu.GetComponent<ItemPopMenu>().DisplayEquipment(item, hit.gameObject);
                }

                if (displayMode == DisplayMode.Consumable)
                {
                    itemPopMenu.SetActive(true);
                    itemPopMenu.GetComponent<ItemPopMenu>().DisplayItem(hit.GetComponent<ConsumableObject>().consumableItem.itemName, DisplayMode.Consumable, hit.gameObject);
                }
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }



    void LateUpdate()
    {
        yourCoins = (int)Mathf.Round((float)GameManager.Instance.GetComponent<YourAccount>().account.coins);
        yourCoinsText.text = yourCoins.ToString();
    }



    public void RightButton()
    {
        int i = (int)displayMode + 1;

        if (i > 2)
        {
            i = 1;
        }

        displayMode = (DisplayMode)Enum.ToObject(typeof(DisplayMode), i);
        UpdateItem();

        //Debug.Log(displayMode);
    }

    public void LeftButton()
    {
        int i = (int)displayMode - 1;

        if (i < 1)
        {
            i = 2;
        }

        displayMode = (DisplayMode)Enum.ToObject(typeof(DisplayMode), i);
        UpdateItem();

        //Debug.Log(displayMode);
    }
}



