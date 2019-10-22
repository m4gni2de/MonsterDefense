﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EquipEffect
{
    
    public Monster monster;

    public EquipmentScript equipment;
    public int Slot;

    //this variable is used to delegate which item method to use, given the name of the item
    public delegate void EquipmentDelegate();
    public EquipmentDelegate equipMethod;

    public EquipEffect(Monster Monster, EquipmentScript Equipment, int slot)
    {
        monster = Monster;
        equipment = Equipment;
        Slot = slot;

        //get string of the name of the item
        string name = string.Concat(equipment.itemName.Where(c => !char.IsWhiteSpace(c)));

        //convert string to a delegate to call the method of the name of the ability
        equipMethod = DelegateCreation(this, name);
        //call the method that lines up with the name of the item
        equipMethod.Invoke();
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
}