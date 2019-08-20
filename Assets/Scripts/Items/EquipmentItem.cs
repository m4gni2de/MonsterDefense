using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public struct EquipmentDetails
{
    public string name;
    public string description;
    public float cost;
    public List<string> boosts;
    public string moveTypeReq;
    public string monsterTypeReq;
    public Monster equippedMonster;
    public int equipSlot;
    public int quantity;
};
public class EquipmentItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public EquipmentDetails equipDetails = new EquipmentDetails();
    public Equipment equip;
    private Monster monster;
    private int slot;

    public SpriteRenderer sp;
    public Sprite sprite;
    public Image image;

    //used to determine if the equipment item is in your inventory or equipped to a monster;
    public bool isEquipped;

    public TMP_Text valueText;

    // Start is called before the first frame update
    void Start()
    {
        sp.GetComponent<SpriteRenderer>();
        sprite = sp.sprite;
        image.GetComponent<Image>();

        
    }

    // Update is called once per frame
    void Update()
    {
        //these are the same thing, but I am using GetMouse for Unity/WebGL and the Touch for mobile
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);



            //if (hit.collider != null)
            //{
            //    if (hit.collider.gameObject.name == gameObject.name)
            //    {
            //        if (isEquipped)
            //        {
            //            GetComponentInParent<YourHome>().activeMonster.GetComponent<Monster>().UnEquipItem(equip, slot);
            //        }
            //        else
            //        {
            //            GetComponentInParent<YourHome>().activeMonster.GetComponent<Monster>().EquipItem(equip);

            //        }
            //    }
            //}
        }
    }

    //use this when the equipment is equipped to a monster
    public void GetEquipInfo(Equipment Equip, Monster Monster, int equipSlot)
    {
        equip = Equip;
        monster = Monster;
        slot = equipSlot;
        equipDetails.equippedMonster = monster;

        isEquipped = true;

        equipDetails.equipSlot = slot;
        equipDetails.name = equip.name;
        equipDetails.description = equip.description;
        equipDetails.cost = equip.cost;
        equipDetails.moveTypeReq = equip.typeMoveReq;
        equipDetails.monsterTypeReq = equip.typeMonsterReq;
        
        
        for (int i = 0; i < equip.boosts.Length; i++)
        {
            equipDetails.boosts.Add(equip.boosts[i]);
            
        }
    }

    //use this when the equipment is just in your inventory
    public void EquipItemInfo(Equipment Equip)
    {
        equip = Equip;

        isEquipped = false;
        equipDetails.name = equip.name;
        equipDetails.description = equip.description;
        equipDetails.cost = equip.cost;
        equipDetails.moveTypeReq = equip.typeMoveReq;
        equipDetails.monsterTypeReq = equip.typeMonsterReq;
        equipDetails.quantity = PlayerPrefs.GetInt(equipDetails.name);


        for (int i = 0; i < equip.boosts.Length; i++)
        {
            equipDetails.boosts.Add(equip.boosts[i]);

        }
    }


    public void UnEquip()
    {
        //EquippableItem equippedItem = new EquippableItem();

        //equippedItem.Unequip(monster, slot);
        //int itemCount = PlayerPrefs.GetInt(equipDetails.name);
        //PlayerPrefs.SetInt(equipDetails.name, itemCount + 1);

        //Debug.Log(itemCount);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("okay");
    }

    

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    
}
