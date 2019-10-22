using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum EquipmentClass
{
    Rune, 
    Axe, 
    Glove, 
    Ring,
}

[System.Serializable]
public struct EquipmentInformation
{
    public int equipSlot;
    public int quantity;

    public int equipLevel;
    public int equipExp;

    //used to determine if the equipment item is in your inventory or equipped to a monster;
    public bool isEquipped;
    public Monster equippedMonster;
    public List<string> boosts;
}

[CreateAssetMenu]
public class EquipmentScript : ScriptableObject
{
    public string itemName;
    public int id;
    public string description;
    public Sprite sprite;
    public EquipmentClass equipClass;
    public string typeMonsterReq;
    public string typeMoveReq;
    public DamageForce forceReq;
    public AttackMode attackModeReq;
    public string[] boosts;
    public float cost;

    public ItemRarity rarity;
    public int equipLevelMax;


    public int hpBonus;
    public int atkBonus;
    public int defBonus;
    public int speedBonus;
    public int precBonus;
    public int atkPowerBonus;
    public int atkTimeBonus;
    public int atkRangeBonus;
    public int critModBonus;
    public int critChanceBonus;
    public int staminaBonus;

    public float hpPercentBonus;
    public float atkPercentBonus;
    public float defPercentBonus;
    public float spePercentBonus;
    public float precPercentBonus;
    public float atkPowerPercentBonus;
    public float atkTimePercentBonus;
    public float evasionPercentBonus;
    public int staminaPercentBonus;

    public EquipManager equip = new EquipManager();
    public EquipmentInformation info = new EquipmentInformation();

    //use this when the equipment is equipped to a monster
    public void GetEquipInfo(Monster Monster, int EquipSlot)
    {
        

        info.equipSlot = EquipSlot;
        info.equippedMonster = Monster;

        info.isEquipped = true;



        //unequip the item first to avoid stacking of the same item's equipment
        UnEquip();
        EquipItem();
    }

    

    //used to equip the attached monster with this item
    public void EquipItem()
    {
        
        equip.Equip(info.equippedMonster, info.equipSlot, this);

    }


    public void UnEquip()
    {
        
        equip.Unequip(info.equippedMonster);

    }
}


public class EquipManager
{
    public MonsterAttack attack1;
    public MonsterAttack attack2;

    public Monster monster;

    public EquipmentScript equipment;
    public int Slot;


    //this variable is used to delegate which item method to use, given the name of the item
    public delegate void EquipmentDelegate();
    public EquipmentDelegate equipMethod;


    public void Equip(Monster Monster, int slot, EquipmentScript equip)
    {
        monster = Monster;
        Slot = slot;
        equipment = equip;

        //get string of the name of the item
        string name = string.Concat(equip.itemName.Where(c => !char.IsWhiteSpace(c)));

        //convert string to a delegate to call the method of the name of the ability
        equipMethod = DelegateCreation(this, name);
        equipMethod.Invoke();

        //EquipmentStatChanges();
        
    }


    //create the method delegate for each item
    EquipmentDelegate DelegateCreation(object target, string functionName)
    {
        EquipmentDelegate eq = (EquipmentDelegate)Delegate.CreateDelegate(typeof(EquipmentDelegate), target, functionName);
        return eq;
    }


    
    public void NatureRune()
    {
        if (monster.info.type1 == "Nature" || monster.info.type2 == "Nature")
        {
            monster.info.Attack.AddModifier(new StatModifier(equipment.atkPercentBonus, StatModType.PercentMult, this, equipment.name));
        }
    }

    public void WaterRune()
    {
        if (monster.info.type1 == "Water" || monster.info.type2 == "Water")
        {
            monster.info.Attack.AddModifier(new StatModifier(equipment.atkPercentBonus, StatModType.PercentMult, this, equipment.name));
        }
    }

    public void ShadowRune()
    {
        if (monster.info.type1 == "Shadow" || monster.info.type2 == "Shadow")
        {
            monster.info.Attack.AddModifier(new StatModifier(equipment.atkPercentBonus, StatModType.PercentMult, this, equipment.name));
        }
    }

    public void FireRune()
    {
        if (monster.info.type1 == "Fire" || monster.info.type2 == "Fire")
        {
            monster.info.Attack.AddModifier(new StatModifier(equipment.atkPercentBonus, StatModType.PercentMult, this, equipment.name));
        }
    }

    public void MechanicalRune()
    {
        if (monster.info.type1 == "Mechanical" || monster.info.type2 == "Mechanical")
        {
            monster.info.Attack.AddModifier(new StatModifier(equipment.atkPercentBonus, StatModType.PercentMult, this, equipment.name));
        }
    }

    public void MagicRune()
    {
        if (monster.info.type1 == "Magic" || monster.info.type2 == "Magic")
        {
            monster.info.Attack.AddModifier(new StatModifier(equipment.atkPercentBonus, StatModType.PercentMult, this, equipment.name));
        }
    }

    public void IceRune()
    {
        if (monster.info.type1 == "Ice" || monster.info.type2 == "Ice")
        {
            monster.info.Attack.AddModifier(new StatModifier(equipment.atkPercentBonus, StatModType.PercentMult, this, equipment.name));
        }
    }

    public void ThunderRune()
    {
        if (monster.info.type1 == "Electric" || monster.info.type2 == "Electric")
        {
            monster.info.Attack.AddModifier(new StatModifier(equipment.atkPercentBonus, StatModType.PercentMult, this, equipment.name));
        }
    }

    public void NormalRune()
    {
        if (monster.info.type1 == "Normal" || monster.info.type2 == "Normal")
        {
            monster.info.Attack.AddModifier(new StatModifier(equipment.atkPercentBonus, StatModType.PercentMult, this, equipment.name));
        }
    }

    public void WoodAxe()
    {
        monster.info.attack1.Power.AddModifier(new StatModifier(equipment.atkPowerBonus, StatModType.Flat, this, equipment.name));
        monster.info.attack2.Power.AddModifier(new StatModifier(equipment.atkPowerBonus, StatModType.Flat, this, equipment.name));
    }

    public void SpikedKnuckles()
    {
        if (monster.info.attack1.attackMode == AttackMode.Punch)
        {
            monster.info.attack1.Power.AddModifier(new StatModifier(equipment.atkPowerPercentBonus, StatModType.PercentMult, this, equipment.name));
        }

        if (monster.info.attack2.attackMode == AttackMode.Punch)
        {
            monster.info.attack2.Power.AddModifier(new StatModifier(equipment.atkPowerPercentBonus, StatModType.PercentMult, this, equipment.name));
        }
    }

    public void RingOfFluctuation()
    {
        monster.info.Speed.AddModifier(new StatModifier(equipment.spePercentBonus, StatModType.PercentMult, this, equipment.name));
    }




    public void EquipmentStatChanges()
    {
        if (equipment.hpBonus != 0)
            monster.info.HP.AddModifier(new StatModifier(equipment.hpBonus, StatModType.Flat, this, equipment.name));
        if (equipment.atkBonus != 0)
            monster.info.Attack.AddModifier(new StatModifier(equipment.atkBonus, StatModType.Flat, this, equipment.name));
        if (equipment.defBonus != 0)
            monster.info.Defense.AddModifier(new StatModifier(equipment.defBonus, StatModType.Flat, this, equipment.name));
        if (equipment.speedBonus != 0)
            monster.info.Speed.AddModifier(new StatModifier(equipment.speedBonus, StatModType.Flat, this, equipment.name));

        if (equipment.hpPercentBonus != 0)
            monster.info.HP.AddModifier(new StatModifier(equipment.hpPercentBonus, StatModType.PercentMult, this, equipment.name));
        if (equipment.atkPercentBonus != 0)
            monster.info.Attack.AddModifier(new StatModifier(equipment.atkPercentBonus, StatModType.PercentMult, this, equipment.name));
        if (equipment.defPercentBonus != 0)
            monster.info.Defense.AddModifier(new StatModifier(equipment.defPercentBonus, StatModType.PercentMult, this, equipment.name));
        if (equipment.spePercentBonus != 0)
            monster.info.Speed.AddModifier(new StatModifier(equipment.spePercentBonus, StatModType.PercentMult, this, equipment.name));


    }


    //invoke this to remove all of the stat boosts when unequipping an equipment item
    public void Unequip(Monster monster)
    {
        monster.info.HP.RemoveAllModifiersFromSource(this);
        monster.info.Attack.RemoveAllModifiersFromSource(this);
        monster.info.Defense.RemoveAllModifiersFromSource(this);
        monster.info.Speed.RemoveAllModifiersFromSource(this);
        monster.info.Precision.RemoveAllModifiersFromSource(this);


        monster.info.attack1.Power.RemoveAllModifiersFromSource(this);
        monster.info.attack1.Range.RemoveAllModifiersFromSource(this);
        monster.info.attack1.AttackSpeed.RemoveAllModifiersFromSource(this);
        monster.info.attack1.AttackTime.RemoveAllModifiersFromSource(this);
        monster.info.attack1.CritChance.RemoveAllModifiersFromSource(this);
        monster.info.attack1.CritMod.RemoveAllModifiersFromSource(this);
        monster.info.attack1.EffectChance.RemoveAllModifiersFromSource(this);

        monster.info.attack2.Power.RemoveAllModifiersFromSource(this);
        monster.info.attack2.Range.RemoveAllModifiersFromSource(this);
        monster.info.attack2.AttackSpeed.RemoveAllModifiersFromSource(this);
        monster.info.attack2.AttackTime.RemoveAllModifiersFromSource(this);
        monster.info.attack2.CritChance.RemoveAllModifiersFromSource(this);
        monster.info.attack2.CritMod.RemoveAllModifiersFromSource(this);
        monster.info.attack2.EffectChance.RemoveAllModifiersFromSource(this);
    }
}
