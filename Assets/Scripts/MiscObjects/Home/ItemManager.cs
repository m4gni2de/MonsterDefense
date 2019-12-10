using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ItemManager : MonoBehaviour, IPointerDownHandler
{
    public GameObject itemObject, itemPlacement, infoMenu, itemPopMenu;
    public ConsumableItem activeItem;
    public Monster activeMonster;
    // Start is called before the first frame update
    void Start()
    {
        //itemPopMenu = GameManager.Instance.popMenu;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadItems()
    {
        var allItems = GameManager.Instance.GetComponent<Items>().allConsumablesDict;
        var yourItems = GameManager.Instance.Inventory.ConsumablePocket;

        int q = 1;
        //loops through all the items that you have, and if the selected monster meets the equipment requirements, then you can equip this item to the monster
        for (int i = 0; i < yourItems.items.Count; i++)
        {
            string name = yourItems.items[i].itemName;

            PocketItem p = yourItems.items[i];
            //EquipmentScript eq = Instantiate(allEquips[name]);
            ConsumableItem cItem = Instantiate(allItems[name]);
            cItem.inventorySlot = p;
            var x = Instantiate(itemObject, new Vector2(itemPlacement.transform.position.x + (50 * (q - 1)), itemPlacement.transform.position.y), Quaternion.identity);
            x.transform.SetParent(transform, true);
            x.GetComponent<ConsumableObject>().consumableItem = cItem;
            x.GetComponent<ConsumableObject>().LoadItem();
            x.GetComponentInChildren<TMP_Text>().text = "";
            x.transform.localScale = new Vector3(1f, 1f, 1f);
            x.tag = "ConsumableItem";
            x.name = cItem.itemName;

            q += 1;
        }
    }

    //call this to load items only useable on a monster, since it's called from the Monster Info Panel
    public void LoadMonsterItems()
    {
        var allItems = GameManager.Instance.GetComponent<Items>().allConsumablesDict;
        var yourItems = GameManager.Instance.Inventory.ConsumablePocket;

        int q = 1;
        //loops through all the items that you have, and if the selected monster meets the equipment requirements, then you can equip this item to the monster
        for (int i = 0; i < yourItems.items.Count; i++)
        {
            string name = yourItems.items[i].itemName;

            PocketItem p = yourItems.items[i];
            //EquipmentScript eq = Instantiate(allEquips[name]);
            ConsumableItem cItem = Instantiate(allItems[name]);
            cItem.inventorySlot = p;

            if (cItem.itemType == ConsumableType.MonsterUse)
            {
                var x = Instantiate(itemObject, new Vector2(itemPlacement.transform.position.x + (50 * (q - 1)), itemPlacement.transform.position.y), Quaternion.identity);
                x.transform.SetParent(transform, true);
                x.GetComponent<ConsumableObject>().consumableItem = cItem;
                x.GetComponent<ConsumableObject>().LoadItem();
                x.GetComponentInChildren<TMP_Text>().text = "";
                x.transform.localScale = new Vector3(1f, 1f, 1f);
                x.tag = "ConsumableItem";
                x.name = cItem.itemName;

                q += 1;
            }
            else
            {
                Destroy(cItem);
            }
        }
    }

    public void UseItem()
    {
        activeItem.UseItem();
        activeItem.RemoveFromInventory();
        CloseItems();
    }


    public void OnPointerDown(PointerEventData eventData)
    {

        if (eventData.pointerEnter)
        {
            var tag = eventData.pointerEnter.gameObject.tag;
            var hit = eventData.pointerEnter.gameObject;


            //set the active monster for the item to be the active monster of this window, in case an item needs a monster as a target to use
            if (tag == "ConsumableItem")
            {
                
                itemPopMenu.SetActive(true);
                itemPopMenu.GetComponent<PopMenuObject>().AcceptItem(hit.GetComponent<ConsumableObject>().consumableItem.inventorySlot);
                activeItem = hit.GetComponent<ConsumableObject>().consumableItem;
                activeItem.SetTarget(GetComponentInParent<YourHome>().activeMonster.GetComponent<Monster>());
                //RemoveFromInventory();
            }

        }



    }

    
    public void CloseItems()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("ConsumableItem");

        if (items.Length == 0)
        {
            if (!infoMenu.activeSelf)
            {
                GetComponentInParent<YourHome>().monsterScrollList.SetActive(true);
                GetComponentInParent<YourHome>().LoadMonsters();
            }
            else
            {
                //infoMenu.GetComponent<MonsterInfoPanel>().LoadInfo(activeMonster);
                infoMenu.GetComponent<MonsterInfoPanel>().LoadInfo(GetComponentInParent<YourHome>().activeMonster.GetComponent<Monster>());
            }
            activeMonster = null;
            gameObject.SetActive(false);

        }
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                Destroy(items[i]);

                if (i >= items.Length - 1)
                {
                    if (!infoMenu.activeSelf)
                    {
                        GetComponentInParent<YourHome>().monsterScrollList.SetActive(true);
                        GetComponentInParent<YourHome>().LoadMonsters();
                    }
                    else
                    {
                        infoMenu.GetComponent<MonsterInfoPanel>().LoadInfo(GetComponentInParent<YourHome>().activeMonster.GetComponent<Monster>());
                    }
                    activeMonster = null;
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
