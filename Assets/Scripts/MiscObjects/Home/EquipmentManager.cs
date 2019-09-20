﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public GameObject equipPlacement;
    public GameObject infoMenu, popMenu;

    private Monster monster;
    private int slot;

    public bool isEquipping, isTapping;

    public float acumTime;

    public EquipmentItem equipment;

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

        var allItems = GameManager.Instance.items.allItemsDict;
        var allEquips = GameManager.Instance.GetComponent<Items>().allEquipmentDict;
        

        int i = 1;

        //loops through all the items that you have, and if the selected monster meets the equipment requirements, then you can equip this item to the monster
        foreach (KeyValuePair<string, Equipment> items in allEquips)
        {
            string name = items.Key;
            if (PlayerPrefs.HasKey(name))
            {
                Equipment item = allEquips[name];
                int itemCount = PlayerPrefs.GetInt(item.name);

                if (item.name != monster.info.equip1Name || item.name != monster.info.equip2Name)
                {
                    if (item.typeMonsterReq == monster.info.type1 || item.typeMonsterReq == monster.info.type2 || item.typeMonsterReq == "none")
                    {
                        var x = Instantiate(allEquips[item.name].equipPrefab, new Vector2(equipPlacement.transform.position.x + (50 * (i - 1)), equipPlacement.transform.position.y), Quaternion.identity);
                        x.transform.SetParent(transform, true);
                        x.GetComponent<EquipmentItem>().EquipItemInfo(item);
                        x.transform.localScale = new Vector3(2f, 2f, 2f);
                        x.GetComponent<SpriteRenderer>().sortingLayerName = "Equipment";
                        x.GetComponent<SpriteRenderer>().sortingOrder = 1;
                        x.GetComponent<Image>().color = Color.clear;
                        i += 1;
                    }
                    else
                    {
                        //x.name = "Ineligible";
                    }
                }

                
                //var x = Instantiate(allEquips[item.name].equipPrefab, new Vector2(equipPlacement.transform.position.x + (50 * (i - 1)), equipPlacement.transform.position.y), Quaternion.identity);
                //x.transform.SetParent(transform, true);
                //x.GetComponent<EquipmentItem>().EquipItemInfo(item);
                //x.transform.localScale = Vector3.one;
                //x.GetComponent<SpriteRenderer>().sortingLayerName = "Equipment";
                //x.GetComponent<SpriteRenderer>().sortingOrder = 1;

                //if (item.typeMonsterReq == monster.info.type1 || item.typeMonsterReq == monster.info.type2)
                //{

                //}
                //else
                //{
                //    x.name = "Ineligible";
                //}
                //i += 1;
            }
        }


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
        //            x.GetComponent<SpriteRenderer>().sortingLayerName = "Equipment";
        //            x.GetComponent<SpriteRenderer>().sortingOrder = 1;

        //            if (item.typeMonsterReq ==  monster.info.type1 || item.typeMonsterReq == monster.info.type2)
        //            {
                        
        //            }
        //            else
        //            {
        //                x.name = "Ineligible";
        //            }

        //        }
        //    }
        //}

       

        
    }

    public void LoadEquipment()
    {
        isEquipping = false;
        
        var allItems = GameManager.Instance.items.allItemsDict;
        var allEquips = GameManager.Instance.GetComponent<Items>().allEquipmentDict;

        int i = 1;

        //loops through all the items in the game, checks them against a playerpref of the same name. if the playerpref exists, then the player has at least 1 of that item. Add those items to a Dictionary of your items
        foreach (KeyValuePair<string, Equipment> items in allEquips)
        {
            string name = items.Key;
            if (PlayerPrefs.HasKey(name))
            {
                Equipment item = allEquips[name];
                int itemCount = PlayerPrefs.GetInt(item.name);
                var x = Instantiate(allEquips[item.name].equipPrefab, new Vector2(equipPlacement.transform.position.x + (50 * (i - 1)), equipPlacement.transform.position.y), Quaternion.identity);
                x.transform.SetParent(transform, true);
                x.GetComponent<EquipmentItem>().EquipItemInfo(item);
                x.transform.localScale = new Vector3(1f, 1f, 1f);
                x.GetComponent<SpriteRenderer>().sortingLayerName = "Equipment";
                x.GetComponent<SpriteRenderer>().sortingOrder = 1;
                
                i += 1;
            }
        }


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
        //            x.GetComponent<SpriteRenderer>().sortingLayerName = "Equipment";
        //            x.GetComponent<SpriteRenderer>().sortingOrder = 1;

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

            
                //if the menu is opened with the purpose of Equipping a monster with an item, then allow it to be equipped. Otherwise, show the item's details
                if (tag == "Equipment" && !isTapping)
                {
                    equipment = hit.gameObject.GetComponent<EquipmentItem>();

                    if (isEquipping)
                    {
                        isTapping = true;
                    }
                    else
                    {
                    popMenu.SetActive(true);
                    popMenu.GetComponent<PopMenuObject>().Item(equipment.equipDetails.name);
                }
                }

            }
       


    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //if a player holds their finger on an item, equip the item. If they just tap the item, display the item details
        if (isTapping)
        {
            if (acumTime >= 1)
            {
                monster.EquipItem(equipment.equip, slot);
                infoMenu.GetComponent<MonsterInfoPanel>().LoadInfo(monster);
                isTapping = false;
                gameObject.SetActive(false);
                int amount = PlayerPrefs.GetInt(equipment.equip.name);
                PlayerPrefs.SetInt(equipment.equip.name, amount - 1);
            }
            else
            {
                isTapping = false;
                popMenu.SetActive(true);
                popMenu.GetComponent<PopMenuObject>().Item(equipment.equipDetails.name);
            }
        }

        


    }


    public void CloseEquipment()
    {
        GameObject[] equips = GameObject.FindGameObjectsWithTag("Equipment");

        if (equips.Length == 0)
        {
            gameObject.SetActive(false);

        }
        else
        {
            for (int i = 0; i < equips.Length; i++)
            {
                Destroy(equips[i]);

                if (i >= equips.Length - 1)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }


}
