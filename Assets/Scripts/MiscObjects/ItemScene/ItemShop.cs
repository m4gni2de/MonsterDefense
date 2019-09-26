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

    private int itemSpriteTotal, shopSpriteTotal;

    public GameObject shopCanvas, itemCanvas;
    public GameObject itemPanel, shopPanel;

    public GameObject consumableObject;
    
    //variable used to determine which items are being displayed in the shop
    public DisplayMode displayMode = new DisplayMode();


    private void Awake()
    {
        itemSpriteTotal = 0;
        shopSpriteTotal = 0;
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
        itemSpriteTotal = 0;
        shopSpriteTotal = 0;


        DisplayShop();
        DisplayYourItems();
    }

    public void DisplayYourItems()
    {
       
        
        itemSpriteTotal = 0;
        shopSpriteTotal = 0;

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

                
            }
        }

        //if (displayMode == DisplayMode.Equipment)
        //{
        //    GameObject[] obj = GameObject.FindGameObjectsWithTag("Equipment");


        //        for (int i = 0; i < obj.Length; i++)
        //        {
        //            Destroy(obj[i]);

        //            if (i >= obj.Length - 1)
        //            {
        //                DisplayYourEquipment();
        //                DisplayShop();
        //            }
        //        }

        //}

        //if (displayMode == DisplayMode.Consumable)
        //{
        //    GameObject[] obj = GameObject.FindGameObjectsWithTag("ConsumableItem");

        //    for (int i = 0; i < obj.Length; i++)
        //    {
        //        Destroy(obj[i]);

        //        if (i >= obj.Length - 1)
        //        {
        //            DisplayYourConsumables();
        //            DisplayShop();
        //        }
        //    }
        //}

    }


    //use this to show the equipment items you have in your inventory
    void DisplayYourEquipment()
    {
        

        var allEquips = GameManager.Instance.items.allEquipmentDict;
        var yourEquips = GameManager.Instance.GetComponent<YourItems>().yourEquipment;
        var allCells = GameManager.Instance.items.allMonsterCellsDict;
        //var allConsumables = GameManager.Instance.items.allConsumablesDict;
        int row = 0;
        float rowCheck = 0f;



        foreach (KeyValuePair<string, int> equip in yourEquips)
        {
            string name = equip.Key;

            //if the player has more than 0 of the item, display it
            if (equip.Value > 0)
            {

                Equipment item = allEquips[name];
                int itemCount = PlayerPrefs.GetInt(item.name);

                GameObject a = Instantiate(allEquips[item.name].equipPrefab, itemScrollContent.transform.position, Quaternion.identity);


                //itemQuantities[shopSpriteTotal] = itemSprites[shopSpriteTotal].GetComponentInChildren<TMP_Text>();
                a.transform.SetParent(itemScrollContent.transform, true);



                a.GetComponent<EquipmentItem>().EquipItemInfo(item);
                a.GetComponent<EquipmentItem>().valueText.gameObject.SetActive(true);
                a.GetComponent<EquipmentItem>().valueText.text = PlayerPrefs.GetInt(item.name).ToString();


                //a.GetComponent<SpriteRenderer>().sortingLayerName = "Equipment";
                //a.GetComponent<SpriteRenderer>().sortingOrder = 1;


                ////a.GetComponent<Image>().enabled = false;
                ////a.GetComponent<Image>().color = Color.clear;

                //a.GetComponent<Image>().material = a.GetComponent<SpriteRenderer>().material;
                //a.GetComponent<SpriteRenderer>().enabled = false;


                a.transform.position = new Vector3(itemSprite.transform.position.x + ((rowCheck * 4) * itemScrollContent.GetComponent<RectTransform>().rect.width / 6), itemSprite.transform.position.y - (row * 35), itemSprite.transform.position.z);
                a.transform.localScale = new Vector3(a.transform.localScale.x, a.transform.localScale.y, 1f);
                a.tag = "Item";

                //itemSprites[itemSpriteTotal] = Instantiate(allEquips[item.name].equipPrefab, itemScrollContent.transform.position, Quaternion.identity);
                ////itemQuantities[itemSpriteTotal] = itemSprites[itemSpriteTotal].GetComponentInChildren<TMP_Text>();
                //itemSprites[itemSpriteTotal].transform.SetParent(itemScrollContent.transform, true);



                //itemSprites[itemSpriteTotal].GetComponent<EquipmentItem>().EquipItemInfo(item);
                //itemSprites[itemSpriteTotal].GetComponent<EquipmentItem>().valueText.gameObject.SetActive(true);
                //itemSprites[itemSpriteTotal].GetComponent<EquipmentItem>().valueText.text = PlayerPrefs.GetInt(item.name).ToString();


                //itemSprites[itemSpriteTotal].GetComponent<SpriteRenderer>().sortingLayerName = "Equipment";
                //itemSprites[itemSpriteTotal].GetComponent<SpriteRenderer>().sortingOrder = 1;
                //itemSprites[itemSpriteTotal].GetComponent<SpriteRenderer>().sprite = null;
                //itemSprites[itemSpriteTotal].transform.position = new Vector3(itemSprite.transform.position.x + ((rowCheck * 4) * itemScrollContent.GetComponent<RectTransform>().rect.width / 6), itemSprite.transform.position.y - (row * 35), itemSprite.transform.position.z);

                //itemQuantities[itemSpriteTotal] = Instantiate(itemQuantity, itemQuantity.transform.position, Quaternion.identity);
                //itemQuantities[itemSpriteTotal].transform.position = itemQuantity.transform.position;
                //itemQuantities[itemSpriteTotal].transform.SetParent(a.transform, true);

                //itemQuantities[itemSpriteTotal].text = PlayerPrefs.GetInt(item.name).ToString();


                itemSpriteTotal += 1;
                rowCheck += .25f;

                if (rowCheck > .8f)
                {
                    rowCheck = 0f;
                    row += 1;
                }
            }

        }




        //foreach (KeyValuePair<string, Equipment> equipment in allEquips)
        //{
            //string name = equipment.Key;
            

        //        Equipment item = allEquips[name];
        //        int itemCount = PlayerPrefs.GetInt(item.name);
        //    //var x = Instantiate(equipByPrefab[item.name], new Vector2(itemSprite.transform.position.x + (50 * (i - 1)), equipPlacement.transform.position.y), Quaternion.identity);
        //    //itemSprites[itemSpriteTotal] = Instantiate(equipByPrefab[item.name], itemSprites[itemSpriteTotal].transform.position, Quaternion.identity);
        //        itemSprites[itemSpriteTotal] = Instantiate(equipByPrefab[item.name], itemSprites[itemSpriteTotal].transform.position, Quaternion.identity);
        //        itemSprites[itemSpriteTotal].transform.SetParent(itemScrollContent.transform, true);
        //        itemSprites[itemSpriteTotal].GetComponent<EquipmentItem>().EquipItemInfo(item);
        //        //itemSprites[itemSpriteTotal].transform.localScale = new Vector3(10, 10, 10);
        //        itemSprites[itemSpriteTotal].GetComponent<SpriteRenderer>().sortingLayerName = "Equipment";
        //        itemSprites[itemSpriteTotal].GetComponent<SpriteRenderer>().sortingOrder = 1;
        //        itemSprites[itemSpriteTotal].GetComponent<SpriteRenderer>().sprite = null;
        //        itemQuantities[itemSpriteTotal] = Instantiate(itemQuantities[itemSpriteTotal], itemSprites[itemSpriteTotal].transform.position, Quaternion.identity);
        //        itemQuantities[itemSpriteTotal].text = PlayerPrefs.GetInt(item.name).ToString();

        //        Vector3 position = itemQuantities[itemSpriteTotal].transform.position;     
        //        itemQuantities[itemSpriteTotal].transform.SetParent(itemSprites[itemSpriteTotal].transform, true);
        //        itemQuantities[itemSpriteTotal].transform.position = position;

        //        //itemSprites[itemSpriteTotal].transform.position = new Vector3(itemSprite.transform.position.x + (itemSpriteTotal * itemScrollContent.GetComponent<RectTransform>().rect.width / 6), itemSprite.transform.position.y - (row * 30), itemSprite.transform.position.z);
        //        itemSprites[itemSpriteTotal].transform.position = new Vector3(itemSprite.transform.position.x + ((rowCheck *4) * itemScrollContent.GetComponent<RectTransform>().rect.width / 6), itemSprite.transform.position.y - (row * 35), itemSprite.transform.position.z);
            
        //itemQuantities[itemSpriteTotal].name = itemCount.ToString();


            


            //itemSpriteTotal += 1;
            //rowCheck += .25f;

            //if (rowCheck > .8f)
            //{
            //    rowCheck = 0f;
            //    row += 1;
            //}

               
        //}

        //if (allEquips.Count < itemSprites.Length)
        //{
        //    int difference = itemSprites.Length - allEquips.Count;

        //    for (int x = itemSprites.Length; x > difference; x--)
        //    {
        //        Destroy(itemSprites[x]);
        //    }
        //}


    }

    //use this to show the consumable items you have in your inventory
    void DisplayYourConsumables()
    {

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
        var allEquips = GameManager.Instance.items.allEquipmentDict;
        var allCells = GameManager.Instance.items.allMonsterCellsDict;
        //var allConsumables = GameManager.Instance.items.allConsumablesDict;

        int row = 0;
        float rowCheck = 0f;
        

        foreach (KeyValuePair<string, Equipment> equipment in allEquips)
        {
            string name = equipment.Key;

            Equipment item = allEquips[name];
            int itemCount = PlayerPrefs.GetInt(item.name);




            //shopItemSprites[shopSpriteTotal].GetComponent<Image>().sprite = item.equipPrefab.GetComponent<SpriteRenderer>().sprite;
            //shopItemSprites[shopSpriteTotal].GetComponent<SpriteRenderer>().sprite = null;
            //shopItemSprites[shopSpriteTotal].transform.localScale = new Vector3(175f, 175f, 1f);
            //shopItemSprites[shopSpriteTotal].tag = "Equipment";
            //itemQuantities[shopSpriteTotal] = itemSprites[shopSpriteTotal].GetComponentInChildren<TMP_Text>();
            //shopItemSprites[shopSpriteTotal].transform.SetParent(shopScrollContent.transform, true);

            GameObject a = Instantiate(allEquips[item.name].equipPrefab, shopScrollContent.transform.position, Quaternion.identity);

            
            //itemQuantities[shopSpriteTotal] = itemSprites[shopSpriteTotal].GetComponentInChildren<TMP_Text>();
            a.transform.SetParent(shopScrollContent.transform, true);



            a.GetComponent<EquipmentItem>().EquipItemInfo(item);
            a.GetComponent<EquipmentItem>().valueText.gameObject.SetActive(true);
            a.GetComponent<EquipmentItem>().valueText.text = a.GetComponent<EquipmentItem>().equipDetails.cost.ToString();


            //a.GetComponent<SpriteRenderer>().sortingLayerName = "Equipment";
            //a.GetComponent<SpriteRenderer>().sortingOrder = 1;
            //a.GetComponent<Image>().enabled = false;
            //a.GetComponent<Image>().color = Color.clear;

            //a.GetComponent<Image>().sprite = a.GetComponent<SpriteRenderer>().sprite;
            //a.GetComponent<SpriteRenderer>().sprite = null;
            //a.GetComponent<Image>().material = a.GetComponent<SpriteRenderer>().material;
            //a.GetComponent<SpriteRenderer>().enabled = false;

            a.transform.position = new Vector3(shopItemSprite.transform.position.x + ((rowCheck * 4) * shopScrollContent.GetComponent<RectTransform>().rect.width / 6), shopItemSprite.transform.position.y - (row * 35), itemSprite.transform.position.z);
            a.transform.localScale = new Vector3(a.transform.localScale.x, a.transform.localScale.y, 1f);
            a.tag = "Item";

            //shopItemSprites[shopSpriteTotal] = Instantiate(allEquips[item.name].equipPrefab, shopScrollContent.transform.position, Quaternion.identity);
            ////itemQuantities[shopSpriteTotal] = itemSprites[shopSpriteTotal].GetComponentInChildren<TMP_Text>();
            //shopItemSprites[shopSpriteTotal].transform.SetParent(shopScrollContent.transform, true);



            //shopItemSprites[shopSpriteTotal].GetComponent<EquipmentItem>().EquipItemInfo(item);
            //shopItemSprites[shopSpriteTotal].GetComponent<EquipmentItem>().valueText.gameObject.SetActive(true);
            //shopItemSprites[shopSpriteTotal].GetComponent<EquipmentItem>().valueText.text = shopItemSprites[shopSpriteTotal].GetComponent<EquipmentItem>().equipDetails.cost.ToString();


            //shopItemSprites[shopSpriteTotal].GetComponent<SpriteRenderer>().sortingLayerName = "Equipment";
            //shopItemSprites[shopSpriteTotal].GetComponent<SpriteRenderer>().sortingOrder = 1;
            //shopItemSprites[shopSpriteTotal].GetComponent<SpriteRenderer>().sprite = null;
            //shopItemSprites[shopSpriteTotal].transform.position = new Vector3(shopItemSprite.transform.position.x + ((rowCheck * 4) * shopScrollContent.GetComponent<RectTransform>().rect.width / 6), shopItemSprite.transform.position.y - (row * 35), itemSprite.transform.position.z);

            //itemQuantities[shopSpriteTotal] = Instantiate(itemQuantity, itemQuantity.transform.position, Quaternion.identity);
            //itemQuantities[shopSpriteTotal].transform.position = itemQuantity.transform.position;
            //itemQuantities[shopSpriteTotal].transform.SetParent(itemSprites[shopSpriteTotal].transform, true);

            //itemQuantities[shopSpriteTotal].text = PlayerPrefs.GetInt(item.name).ToString();


            //shopSpriteTotal += 1;
            rowCheck += .25f;

            if (rowCheck > .8f)
            {
                rowCheck = 0f;
                row += 1;
            }


        }
    }



    public void DisplayShopConsumables()
    {
        var consumables = GameManager.Instance.GetComponent<Items>().consumables;
        var allCells = GameManager.Instance.items.allMonsterCellsDict;
        //var allConsumables = GameManager.Instance.items.allConsumablesDict;

        int row = 0;
        float rowCheck = 0f;


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
            //x.transform.localScale = new Vector3(x.transform.localScale.x * 100, x.transform.localScale.y * 100, x.transform.localScale.z);
            //a.GetComponent<EquipmentItem>().EquipItemInfo(item);
            //a.GetComponent<EquipmentItem>().valueText.gameObject.SetActive(true);
            //a.GetComponent<EquipmentItem>().valueText.text = a.GetComponent<EquipmentItem>().equipDetails.cost.ToString();

            //a.GetComponent<SpriteRenderer>().sprite = null;
            //a.GetComponent<SpriteRenderer>().sortingLayerName = "Equipment";
            //a.GetComponent<SpriteRenderer>().sortingOrder = 1;
            //a.GetComponent<Image>().color = Color.clear;
            a.transform.position = new Vector3(shopItemSprite.transform.position.x + ((rowCheck * 4) * shopScrollContent.GetComponent<RectTransform>().rect.width / 6), shopItemSprite.transform.position.y - (row * 35), itemSprite.transform.position.z);
            a.transform.localScale = new Vector3(a.transform.localScale.x * 1.5f, a.transform.localScale.y * 1.5f, 1f);
            a.tag = "Item";



            rowCheck += .25f;

            if (rowCheck > .8f)
            {
                rowCheck = 0f;
                row += 1;
            }


        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if (eventData.pointerEnter)
        {
            ;
            var tag = eventData.pointerEnter.gameObject.tag;
            var hit = eventData.pointerEnter.gameObject;

            //Debug.Log(tag);
            //if the menu is opened with the purpose of Equipping a monster with an item, then allow it to be equipped. Otherwise, show the item's details
            if (tag == "Item")
            {
                if (displayMode == DisplayMode.Equipment)
                {
                    EquipmentItem item = hit.gameObject.GetComponent<EquipmentItem>();

                    itemPopMenu.SetActive(true);
                    itemPopMenu.GetComponent<ItemPopMenu>().DisplayEquipment(item, hit.gameObject);
                }
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
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

        Debug.Log(displayMode);
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

        Debug.Log(displayMode);
    }
}
