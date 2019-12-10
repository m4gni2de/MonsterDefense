using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject equipPlacement;
    public GameObject infoMenu, popMenu;
    public GameObject equipmentObject;
    public GameObject equipmentUpgradeObject;

    private Monster monster;
    private int slot;

    public bool isEquipping, isTapping;

    public float acumTime;

    public Equipment equipment;

    // Start is called before the first frame update
    void Start()
    {
        //var items = GameManager.Instance.GetComponent<YourItems>().yourItemsDict;
        //var equipIds = GameManager.Instance.GetComponent<YourItems>().equipIds;
        //var allEquips = GameManager.Instance.GetComponent<Items>().allEquipmentDict;
        //var equipByPrefab = GameManager.Instance.GetComponent<Items>().equipmentByPrefab;


        //for (int i = 1; i <= equipIds.Count; i++)
        //{

        //    if (equipIds.ContainsKey(i))
        //    {
        //        string name = equipIds[i];

        //        if (PlayerPrefs.HasKey(name))
        //        {
        //            Equipment item = allEquips[name];
        //            int itemCount = PlayerPrefs.GetInt(item.name);
        //            var x = Instantiate(equipByPrefab[item.name], new Vector2(equipPlacement.transform.position.x + (50 * (i - 1)), equipPlacement.transform.position.y), Quaternion.identity);
        //            x.transform.SetParent(transform, true);
        //            x.GetComponent<EquipmentItem>().EquipItemInfo(item);
        //            x.transform.localScale = Vector3.one;
        //            x.GetComponent<SpriteRenderer>().sortingLayerName = "GameUI";
        //            x.GetComponent<SpriteRenderer>().sortingOrder = 80;

        //        }
        //    }
        //}
    }

    // Update is called once per frame
    void Update()
    {
       if (isTapping == true)
        {
            acumTime += Time.deltaTime;
        }
        else
        {
            acumTime = 0;
        }
    }

    //load the equipment menu with a specific monster and equipment slot 
    public void ChangeEquipment(Monster mon, int equipSlot)
    {
        monster = mon;
        slot = equipSlot;

        isEquipping = true;

        //var allItems = GameManager.Instance.items.allItemsDict;
        //var allItems = GameManager.Instance.items.fullItemList;
        var allEquips = GameManager.Instance.GetComponent<Items>().allEquipsDict;
        var yourEquips = GameManager.Instance.Inventory.EquipmentPocket;
        //var allEquips = GameManager.Instance.Inventory.EquipmentPocket.items;


        int q = 1;
        //loops through all the items that you have, and if the selected monster meets the equipment requirements, then you can equip this item to the monster
        for (int i = 0; i < yourEquips.items.Count; i++)
        {
            string name = yourEquips.items[i].itemName;

                PocketItem p = yourEquips.items[i];
                //EquipmentScript eq = Instantiate(allEquips[name]);
                Equipment item = new Equipment(allEquips[name]);
                item.inventorySlot = p;


                if (item.itemName == monster.info.equip1Name || item.itemName == monster.info.equip2Name)
                {
                    //

                }
                else
                {
                    //Debug.Log(item.itemName + "    " + monster.info.equip1Name);
                    if (item.equipment.typeMonsterReq == monster.info.type1 || item.equipment.typeMonsterReq == monster.info.type2 || item.equipment.typeMonsterReq == "none")
                    {
                        var x = Instantiate(equipmentObject, new Vector2(equipPlacement.transform.position.x + (50 * (q - 1)), equipPlacement.transform.position.y), Quaternion.identity);
                        x.transform.SetParent(transform, true);
                        x.GetComponent<EquipmentObject>().LoadItem(item);
                        x.GetComponent<EquipmentObject>().valueText.text = "Lv. " + item.inventorySlot.itemLevel;
                        item.equipment.ActivateItem(item.equipment, x);

                       
                        x.transform.localScale = new Vector3(1f, 1f, 1f);

                        q += 1;
                    }
                }

        }




    }

    public void LoadEquipment()
    {
        isEquipping = false;
        var allEquips = GameManager.Instance.GetComponent<Items>().allEquipsDict;
        var yourEquips = GameManager.Instance.Inventory.EquipmentPocket;


       

        int q = 1;
        //loops through all the items that you have, and if the selected monster meets the equipment requirements, then you can equip this item to the monster
        for (int i = 0; i < yourEquips.items.Count; i++)
        {
            string name = yourEquips.items[i].itemName;

            PocketItem p = yourEquips.items[i];
            //EquipmentScript eq = Instantiate(allEquips[name]);
            Equipment item = new Equipment(allEquips[name]);
            item.SetInventorySlot(p);
            var x = Instantiate(equipmentObject, new Vector2(equipPlacement.transform.position.x + (50 * (q - 1)), equipPlacement.transform.position.y), Quaternion.identity);
            x.transform.SetParent(transform, true);
            x.GetComponent<EquipmentObject>().LoadItem(item);
            x.GetComponent<EquipmentObject>().valueText.text = "Lv. " + item.inventorySlot.itemLevel;
            item.equipment.ActivateItem(item.equipment, x);
            x.transform.localScale = new Vector3(1f, 1f, 1f);

            q += 1;
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
       
            if (eventData.pointerEnter)
            {
                var tag = eventData.pointerEnter.gameObject.tag;
                var hit = eventData.pointerEnter.gameObject;

            
                //if the menu is opened with the purpose of Equipping a monster with an item, then allow it to be equipped. Otherwise, show the item's details
                if (tag == "Equipment" && !isTapping)
                {
                //equipment = hit.gameObject.GetComponent<EquipmentItem>();
                //EquipmentScript eq = Instantiate(hit.gameObject.GetComponent<EquipmentObject>().equipment.equipment);
                equipment = hit.gameObject.GetComponent<EquipmentObject>().equipment;
                   

                    if (isEquipping)
                    {
                        isTapping = true;
                    }
                    else
                    {
                    //popMenu.SetActive(true);
                    //popMenu.GetComponent<PopMenuObject>().Item(equipment.itemName);
                    GameManager.Instance.DisplayPopMenu(equipment);
                }
                }

            }
       


    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //if a player holds their finger on an item, display the item details. If they just tap the item, equip it
        if (isTapping)
        {
            if (acumTime >= 1)
            {
                isTapping = false;
                //popMenu.SetActive(true);
                //popMenu.GetComponent<PopMenuObject>().Item(equipment.itemName);
                GameManager.Instance.DisplayPopMenu(equipment);
            }
            else
            {
                monster.EquipItem(equipment, slot);
                //equipment.RemoveFromInventory();
                
                infoMenu.GetComponent<MonsterInfoPanel>().LoadInfo(monster);
                //infoMenu.GetComponent<MonsterInfoPanel>().RefreshEquipment();
                isTapping = false;
                int amount = PlayerPrefs.GetInt(equipment.itemName);
                CloseEquipment();

            }
        }

        
        

    }


    public void CloseEquipment()
    {
        GameObject[] equips = GameObject.FindGameObjectsWithTag("Equipment");

        if (equips.Length == 0)
        {
            if (!infoMenu.activeSelf)
            {
                GetComponentInParent<YourHome>().monsterScrollList.SetActive(true);
            }
            gameObject.SetActive(false);

        }
        else
        {
            for (int i = 0; i < equips.Length; i++)
            {
                Destroy(equips[i]);

                if (i >= equips.Length - 1)
                {
                    if (!infoMenu.activeSelf)
                    {
                        GetComponentInParent<YourHome>().monsterScrollList.SetActive(true);
                    }
                    gameObject.SetActive(false);
                }
            }
        }

        
       
    }

    


}
