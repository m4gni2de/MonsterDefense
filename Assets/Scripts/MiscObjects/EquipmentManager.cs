using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentManager : MonoBehaviour, IPointerDownHandler
{
    public GameObject equipPlacement;
    public GameObject infoMenu;

    private Monster monster;
    private int slot;

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
       
    }

    //load the equipment menu with a specific monster and equipment slot 
    public void ChangeEquipment(Monster mon, int equipSlot)
    {
        monster = mon;
        slot = equipSlot;

        var items = GameManager.Instance.GetComponent<YourItems>().yourItemsDict;
        var equipIds = GameManager.Instance.GetComponent<YourItems>().equipIds;
        var allEquips = GameManager.Instance.GetComponent<Items>().allEquipmentDict;
        var equipByPrefab = GameManager.Instance.GetComponent<Items>().equipmentByPrefab;


        for (int i = 1; i <= equipIds.Count; i++)
        {

            if (equipIds.ContainsKey(i))
            {
                string name = equipIds[i];

                if (PlayerPrefs.HasKey(name))
                {
                    Equipment item = allEquips[name];
                    int itemCount = PlayerPrefs.GetInt(item.name);
                    var x = Instantiate(equipByPrefab[item.name], new Vector2(equipPlacement.transform.position.x + (50 * (i - 1)), equipPlacement.transform.position.y), Quaternion.identity);
                    x.transform.SetParent(transform, true);
                    x.GetComponent<EquipmentItem>().EquipItemInfo(item);
                    x.transform.localScale = Vector3.one;
                    x.GetComponent<SpriteRenderer>().sortingLayerName = "GameUI";
                    x.GetComponent<SpriteRenderer>().sortingOrder = 80;

                    if (item.typeMonsterReq ==  monster.info.type1 || item.typeMonsterReq == monster.info.type2)
                    {
                        
                    }
                    else
                    {
                        x.name = "Ineligible";
                    }

                }
            }
        }

       

        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.pointerEnter)
        {
            var tag = eventData.pointerEnter.gameObject.tag;
            var hit = eventData.pointerEnter.gameObject;

            if (tag == "Equipment")
            {
                var equipment = hit.gameObject.GetComponent<EquipmentItem>();
                monster.EquipItem(equipment.equip, slot);
                infoMenu.GetComponent<MonsterInfoPanel>().LoadInfo(monster);
                gameObject.SetActive(false);
            }
        }
    }

   
}
