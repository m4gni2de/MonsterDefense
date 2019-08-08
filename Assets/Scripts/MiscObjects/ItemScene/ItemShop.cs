using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemShop : MonoBehaviour, IPointerDownHandler
{
    public GameObject itemSprite, shopItemSprite;
    public TMP_Text itemQuantity, shopItemCost;
    public GameObject[] itemSprites, shopItemSprites;
    public TMP_Text[] itemQuantities, shopItemCosts;
    public GameObject itemScrollContent, shopScrollContent, itemPopMenu;

    private int itemSpriteTotal, shopSpriteTotal;

    

    private void Awake()
    {
        itemSpriteTotal = 0;
        shopSpriteTotal = 0;
        itemQuantity.GetComponent<TMP_Text>();
        

        for (int c = 0; c < 20; c++)
        {
            for (int r = 0; r < 4; r++)
            {
                itemSprites[itemSpriteTotal] = Instantiate(itemSprite, itemScrollContent.transform.position, Quaternion.identity);
                itemQuantities[itemSpriteTotal] = itemSprites[itemSpriteTotal].GetComponentInChildren<TMP_Text>();
                itemSprites[itemSpriteTotal].transform.SetParent(itemScrollContent.transform, true);
                itemSprites[itemSpriteTotal].transform.position = new Vector3(itemSprite.transform.position.x + (r * 33), itemSprite.transform.position.y - (c * 30), itemSprite.transform.position.z);
                itemSpriteTotal += 1;

                shopItemSprites[shopSpriteTotal] = Instantiate(shopItemSprite, shopScrollContent.transform.position, Quaternion.identity);
                shopItemCosts[shopSpriteTotal] = shopItemSprites[shopSpriteTotal].GetComponentInChildren<TMP_Text>();
                shopItemSprites[shopSpriteTotal].transform.SetParent(shopScrollContent.transform, true);
                shopItemSprites[shopSpriteTotal].transform.position = new Vector3(shopItemSprite.transform.position.x + (r * 33), shopItemSprite.transform.position.y - (c * 30), shopItemSprite.transform.position.z);
                shopSpriteTotal += 1;


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


        DisplayYourEquipment();
    }


    void DisplayYourEquipment()
    {
        var allEquips = GameManager.Instance.items.allEquipmentDict;
        var allCells = GameManager.Instance.items.allMonsterCellsDict;
        var allConsumables = GameManager.Instance.items.allConsumablesDict;
        var equipByPrefab = GameManager.Instance.GetComponent<Items>().equipmentByPrefab;

        foreach (KeyValuePair<string, Equipment> equipment in allEquips)
        {
            string name = equipment.Key;
            
                Equipment item = allEquips[name];
                int itemCount = PlayerPrefs.GetInt(item.name);
                //var x = Instantiate(equipByPrefab[item.name], new Vector2(itemSprite.transform.position.x + (50 * (i - 1)), equipPlacement.transform.position.y), Quaternion.identity);
                itemSprites[itemSpriteTotal] = Instantiate(equipByPrefab[item.name], itemSprites[itemSpriteTotal].transform.position, Quaternion.identity);
                itemSprites[itemSpriteTotal].transform.SetParent(itemScrollContent.transform, true);
                itemSprites[itemSpriteTotal].GetComponent<EquipmentItem>().EquipItemInfo(item);
                //itemSprites[itemSpriteTotal].transform.localScale = new Vector3(10, 10, 10);
                itemSprites[itemSpriteTotal].GetComponent<SpriteRenderer>().sortingLayerName = "Equipment";
                itemSprites[itemSpriteTotal].GetComponent<SpriteRenderer>().sortingOrder = 1;
                itemSprites[itemSpriteTotal].GetComponent<SpriteRenderer>().sprite = null;
                itemQuantities[itemSpriteTotal].text = PlayerPrefs.GetInt(item.name).ToString();
                itemQuantities[itemSpriteTotal].transform.SetParent(itemSprites[itemSpriteTotal].transform);
                itemQuantities[itemSpriteTotal].name = itemCount.ToString();
                itemSpriteTotal += 1;
        }

        //if (allEquips.Count < itemSprites.Length)
        //{
        //    int difference = itemSprites.Length - allEquips.Count;

        //    for (int x = itemSprites.Length; x > difference; x--)
        //    {
        //        Destroy(itemSprites[x]);
        //    }
        //}

       
    }

    public void DisplayShop()
    {
        DisplayShopEquipment();

        
    }
    
    public void DisplayShopEquipment()
    {
        var allEquips = GameManager.Instance.items.allEquipmentDict;
        var allCells = GameManager.Instance.items.allMonsterCellsDict;
        var allConsumables = GameManager.Instance.items.allConsumablesDict;
        var equipByPrefab = GameManager.Instance.GetComponent<Items>().equipmentByPrefab;

        foreach (KeyValuePair<string, Equipment> equipment in allEquips)
        {
            string name = equipment.Key;

            Equipment item = allEquips[name];

            //var x = Instantiate(equipByPrefab[item.name], new Vector2(itemSprite.transform.position.x + (50 * (i - 1)), equipPlacement.transform.position.y), Quaternion.identity);
            shopItemSprites[shopSpriteTotal] = Instantiate(equipByPrefab[item.name], shopItemSprites[shopSpriteTotal].transform.position, Quaternion.identity);
            shopItemSprites[shopSpriteTotal].transform.SetParent(shopScrollContent.transform, true);
            shopItemSprites[shopSpriteTotal].GetComponent<EquipmentItem>().EquipItemInfo(item);
            //itemSprites[itemSpriteTotal].transform.localScale = new Vector3(10, 10, 10);
            shopItemSprites[shopSpriteTotal].GetComponent<SpriteRenderer>().sortingLayerName = "Equipment";
            shopItemSprites[shopSpriteTotal].GetComponent<SpriteRenderer>().sortingOrder = 1;
            shopItemSprites[shopSpriteTotal].GetComponent<SpriteRenderer>().sprite = null;
            shopItemCosts[shopSpriteTotal].text = item.cost.ToString();
            shopItemCosts[shopSpriteTotal].transform.SetParent(shopItemSprites[shopSpriteTotal].transform);
            //itemQuantities[itemSpriteTotal].name = itemCount.ToString();
            shopSpriteTotal += 1;


        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if (eventData.pointerEnter)
        {
            var tag = eventData.pointerEnter.gameObject.tag;
            var hit = eventData.pointerEnter.gameObject;
            //if the menu is opened with the purpose of Equipping a monster with an item, then allow it to be equipped. Otherwise, show the item's details
            if (tag == "Equipment" || tag == "Consumable" || tag == "MonsterCell")
            {
                EquipmentItem item = hit.gameObject.GetComponent<EquipmentItem>();

                itemPopMenu.SetActive(true);
                itemPopMenu.GetComponent<ItemPopMenu>().DisplayEquipment(item);
            }
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
