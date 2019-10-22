using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

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
    public EquipmentScript equipment;
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

    //load this item's consumable item
    public void LoadItem(EquipmentScript equip)
    {
        equipment = equip;
        image.sprite = equipment.sprite;
        gameObject.name = equipment.itemName;
        //image.material = sp.material;
        //sp.enabled = false;
    }

    ////use this when the equipment is equipped to a monster
    //public void GetEquipInfo(EquipmentScript Equip, Monster Monster, int equipSlot)
    //{
    //    equipment = Equip;
    //    info.equipSlot = equipSlot;
    //    info.equippedMonster = Monster;

    //    info.isEquipped = true;



    //    for (int i = 0; i < equipment.boosts.Length; i++)
    //    {
    //        info.boosts.Add(equipment.boosts[i]);

    //    }

    //    //unequip the item first to avoid stacking of the same item's equipment
    //    UnEquip();
    //    EquipItem();
    //}



    //use this when the equipment is just in your inventory
    public void EquipItemInfo(EquipmentScript Equip)
    {
        equipment = Equip;


        equipment.info.isEquipped = false;
        equipment.info.quantity = PlayerPrefs.GetInt(Equip.name);


       
    }



    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("okay");
    }



    public void OnPointerExit(PointerEventData eventData)
    {

    }
}



//public class EquipManager
//{
//    public MonsterAttack attack1;
//    public MonsterAttack attack2;

//    public Monster monster;

//    public EquipmentScript equipment;
//    public int Slot;


   


//    public void Equip(Monster Monster, int slot, EquipmentScript equip)
//    {
//        monster = Monster;
//        Slot = slot;
//        equipment = equip;

//        EquipmentStatChanges();
//        EquipmentOtherEffects();
//    }


//    public void EquipmentOtherEffects()
//    {

//    }

//    public void EquipmentStatChanges()
//    {
//        if (equipment.hpBonus != 0)
//            monster.info.HP.AddModifier(new StatModifier(equipment.hpBonus, StatModType.Flat, this, equipment.name));
//        if (equipment.atkBonus != 0)
//            monster.info.Attack.AddModifier(new StatModifier(equipment.atkBonus, StatModType.Flat, this, equipment.name));
//        if (equipment.defBonus != 0)
//            monster.info.Defense.AddModifier(new StatModifier(equipment.defBonus, StatModType.Flat, this, equipment.name));
//        if (equipment.speedBonus != 0)
//            monster.info.Speed.AddModifier(new StatModifier(equipment.speedBonus, StatModType.Flat, this, equipment.name));

//        if (equipment.hpPercentBonus != 0)
//            monster.info.HP.AddModifier(new StatModifier(equipment.hpPercentBonus, StatModType.PercentMult, this, equipment.name));
//        if (equipment.atkPercentBonus != 0)
//            monster.info.Attack.AddModifier(new StatModifier(equipment.atkPercentBonus, StatModType.PercentMult, this, equipment.name));
//        if (equipment.defPercentBonus != 0)
//            monster.info.Defense.AddModifier(new StatModifier(equipment.defPercentBonus, StatModType.PercentMult, this, equipment.name));
//        if (equipment.spePercentBonus != 0)
//            monster.info.Speed.AddModifier(new StatModifier(equipment.spePercentBonus, StatModType.PercentMult, this, equipment.name));
//    }


//    //invoke this to remove all of the stat boosts when unequipping an equipment item
//    public void Unequip(Monster monster)
//    {
//        monster.info.HP.RemoveAllModifiersFromSource(this);
//        monster.info.Attack.RemoveAllModifiersFromSource(this);
//        monster.info.Defense.RemoveAllModifiersFromSource(this);
//        monster.info.Speed.RemoveAllModifiersFromSource(this);
//        monster.info.Precision.RemoveAllModifiersFromSource(this);


//        monster.info.attack1.Power.RemoveAllModifiersFromSource(this);
//        monster.info.attack1.Range.RemoveAllModifiersFromSource(this);
//        monster.info.attack1.AttackSpeed.RemoveAllModifiersFromSource(this);
//        monster.info.attack1.AttackTime.RemoveAllModifiersFromSource(this);
//        monster.info.attack1.CritChance.RemoveAllModifiersFromSource(this);
//        monster.info.attack1.CritMod.RemoveAllModifiersFromSource(this);
//        monster.info.attack1.EffectChance.RemoveAllModifiersFromSource(this);

//        monster.info.attack2.Power.RemoveAllModifiersFromSource(this);
//        monster.info.attack2.Range.RemoveAllModifiersFromSource(this);
//        monster.info.attack2.AttackSpeed.RemoveAllModifiersFromSource(this);
//        monster.info.attack2.AttackTime.RemoveAllModifiersFromSource(this);
//        monster.info.attack2.CritChance.RemoveAllModifiersFromSource(this);
//        monster.info.attack2.CritMod.RemoveAllModifiersFromSource(this);
//        monster.info.attack2.EffectChance.RemoveAllModifiersFromSource(this);
//    }
//}
