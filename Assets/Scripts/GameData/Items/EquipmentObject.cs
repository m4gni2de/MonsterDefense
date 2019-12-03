using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

//[System.Serializable]
//public struct EquipmentInformation
//{
//    public int equipSlot;
//    public int quantity;

//    public int equipLevel;
//    public int equipExp;

//    //used to determine if the equipment item is in your inventory or equipped to a monster;
//    public bool isEquipped;
//    public Monster equippedMonster;
//    public List<string> boosts;
//}


public class EquipmentObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Equipment equipment;
    public Image image;
    

    //public EquipmentInformation info = new EquipmentInformation();
    //public EquipManager equip = new EquipManager();
    public TMP_Text valueText;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //load this item's equipment script
    public void LoadItem(Equipment equip)
    {
        equipment = equip;
        image.sprite = equip.equipment.sprite;
        gameObject.name = equip.equipment.itemName;
        
        ////if the equipment has a type to be added, add it on the sprite
        //if (equip.spriteEffect != EquipmentSpriteEffect.None)
        //{

        //    gameObject.AddComponent(Type.GetType(equip.spriteEffect.ToString()));
        //}
        //image.material = sp.material;
        //sp.enabled = false;
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("okay");
    }



    public void OnPointerExit(PointerEventData eventData)
    {

    }
}



