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

    private Monster monster;
    private int slot;

    public bool isEquipping, isTapping;

    public float acumTime;

    public EquipmentScript equipment;

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
        //var allEquips = GameManager.Instance.GetComponent<Items>().allEquipmentDict;
        var allEquips = GameManager.Instance.GetComponent<Items>().allEquipsDict;
        

        int i = 1;

        //loops through all the items that you have, and if the selected monster meets the equipment requirements, then you can equip this item to the monster
        foreach (KeyValuePair<string, EquipmentScript> items in allEquips)
        {
            string name = items.Key;
            if (PlayerPrefs.HasKey(name))
            {
                EquipmentScript item = allEquips[name];
                int itemCount = PlayerPrefs.GetInt(item.name);

                
                if (item.itemName == monster.info.equip1Name || item.itemName == monster.info.equip2Name)
                {
                    //
                    
                }
                    else
                    {
                    //Debug.Log(item.itemName + "    " + monster.info.equip1Name);
                    if (item.typeMonsterReq == monster.info.type1 || item.typeMonsterReq == monster.info.type2 || item.typeMonsterReq == "none")
                    {
                        var x = Instantiate(equipmentObject, new Vector2(equipPlacement.transform.position.x + (50 * (i - 1)), equipPlacement.transform.position.y), Quaternion.identity);
                        x.transform.SetParent(transform, true);
                        x.GetComponent<EquipmentObject>().LoadItem(item);
                        x.GetComponent<EquipmentObject>().valueText.text = "";
                        item.ActivateItem(item, x);

                        ////if the equipment has a type to be added, add it on the sprite
                        //if (item.spriteEffect != EquipmentSpriteEffect.None)
                        //{

                        //    x.AddComponent(Type.GetType(item.spriteEffect.ToString()));
                        //}

                        //x.GetComponent<EquipmentItem>().EquipItemInfo(item);
                        x.transform.localScale = new Vector3(1f, 1f, 1f);
                        //x.GetComponent<Image>().color = Color.white;
                        i += 1;
                    }
                }

                
               
            }
        }


        
        
    }

    public void LoadEquipment()
    {
        isEquipping = false;

        //var allItems = GameManager.Instance.items.allItemsDict;
        //var allItems = GameManager.Instance.items.fullItemList;
        //var allEquips = GameManager.Instance.GetComponent<Items>().allEquipmentDict;
        var allEquips = GameManager.Instance.GetComponent<Items>().allEquipsDict;

        int i = 1;

        //loops through all the items in the game, checks them against a playerpref of the same name. if the playerpref exists, then the player has at least 1 of that item. Add those items to a Dictionary of your items
        foreach (KeyValuePair<string, EquipmentScript> items in allEquips)
        {
            string name = items.Key;
            if (PlayerPrefs.HasKey(name))
            {
                //Equipment item = allEquips[name];
                EquipmentScript item = allEquips[name];
                int itemCount = PlayerPrefs.GetInt(item.name);
                //var x = Instantiate(allEquips[item.name].equipPrefab, new Vector2(equipPlacement.transform.position.x + (50 * (i - 1)), equipPlacement.transform.position.y), Quaternion.identity);
                var x = Instantiate(equipmentObject, new Vector2(equipPlacement.transform.position.x + (50 * (i - 1)), equipPlacement.transform.position.y), Quaternion.identity);
                x.transform.SetParent(transform, true);
                x.GetComponent<EquipmentObject>().LoadItem(item);
                x.GetComponent<EquipmentObject>().valueText.text = "";

                item.ActivateItem(item, x);
                //x.transform.SetParent(transform, true);
                //x.GetComponent<EquipmentItem>().EquipItemInfo(item);
                x.transform.localScale = new Vector3(1f, 1f, 1f);

                ////if the equipment has a type to be added, add it on the sprite
                //if (item.spriteEffect != EquipmentSpriteEffect.None)
                //{

                //    x.AddComponent(Type.GetType(item.spriteEffect.ToString()));
                //}

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
                    //equipment = hit.gameObject.GetComponent<EquipmentItem>();
                    equipment = hit.gameObject.GetComponent<EquipmentObject>().equipment;

                    if (isEquipping)
                    {
                        isTapping = true;
                    }
                    else
                    {
                    popMenu.SetActive(true);
                    popMenu.GetComponent<PopMenuObject>().Item(equipment.itemName);
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
                popMenu.SetActive(true);
                popMenu.GetComponent<PopMenuObject>().Item(equipment.itemName);
            }
            else
            {
                monster.EquipItem(equipment, slot);
                infoMenu.GetComponent<MonsterInfoPanel>().LoadInfo(monster);
                isTapping = false;
                gameObject.SetActive(false);
                int amount = PlayerPrefs.GetInt(equipment.name);
                PlayerPrefs.SetInt(equipment.name, amount - 1);


                
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
