using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.ObjectModel;

//************** following classes are used to calcuate a monster's stats**********************

public class StatsCalc
{

    public Monster Monster;
    
    

    public StatsCalc(Monster monster)
    {
        float level = (float)monster.info.level;

        
        
        

        //monster.info.hpMax = (int)((2 * monster.info.hpBase + monster.info.hpPot) * (level / 100) * Mathf.Sqrt(1 + (level / 100)) + 4);
        monster.info.HP.BaseValue = (int)((2 * monster.info.hpBase + monster.info.HPPotential.BaseValue) * (level / 100) * Mathf.Sqrt(1 + (level / 100)) + 4);
        //monster.info.defStat = (int)((2 * monster.info.defBase + monster.info.defPot) * (level / 100) * Mathf.Sqrt(1 + (level / 100)) + 4);
        monster.info.Defense.BaseValue = (int)((2 * monster.info.defBase + monster.info.DefensePotential.BaseValue) * (level / 100) * Mathf.Sqrt(1 + (level / 100)) + 4);
        //monster.info.atkStat = (int)((2 * monster.info.atkBase + monster.info.atkPot) * (level / 100) * Mathf.Sqrt(1 + (level / 100)) + 4);
        monster.info.Attack.BaseValue = (int)((2 * monster.info.atkBase + monster.info.AttackPotential.BaseValue) * (level / 100) * Mathf.Sqrt(1 + (level / 100)) + 4);
        //monster.info.speStat = (int)((2 * monster.info.speBase + monster.info.spePot) * (level / 100) * Mathf.Sqrt(1 + (level / 100)) + 4);
        monster.info.Speed.BaseValue = (int)((2 * monster.info.speBase + monster.info.SpeedPotential.BaseValue) * (level / 100) * Mathf.Sqrt(1 + (level / 100)) + 4);
        //monster.info.precStat = (int)((2 * monster.info.precBase + monster.info.precPot) * (level / 100) * Mathf.Sqrt(1 + (level / 100)) + 4);
        monster.info.Precision.BaseValue = (int)((2 * monster.info.precBase + monster.info.PrecisionPotential.BaseValue) * (level / 100) * Mathf.Sqrt(1 + (level / 100)) + 4);
        monster.info.critBase = (monster.info.precBase / 33) + Mathf.Sqrt(monster.info.Precision.BaseValue);


        monster.info.evasionBase = Mathf.Round(((monster.info.Speed.BaseValue / monster.info.HP.BaseValue) / 5) * 100);
        monster.info.Stamina.BaseValue = (int)monster.info.staminaBase;
        monster.info.EnergyCost.BaseValue = (int)monster.info.energyCost;
        monster.info.EnergyGeneration.BaseValue = (monster.info.energyGenBase);

        //sets the monster's temporary stats, using it's permanent stats as a base
        monster.tempStats.HP.BaseValue = monster.info.HP.Value;
        monster.tempStats.Defense.BaseValue = monster.info.Defense.Value;
        monster.tempStats.Attack.BaseValue = monster.info.Attack.Value;
        monster.tempStats.Speed.BaseValue = monster.info.Speed.Value;
        monster.tempStats.Precision.BaseValue = monster.info.Precision.Value;
        monster.tempStats.Stamina.BaseValue = monster.info.Stamina.Value;
        monster.tempStats.EnergyCost.BaseValue = monster.info.EnergyCost.Value;
        monster.tempStats.EnergyGeneration.BaseValue = monster.info.EnergyGeneration.Value;
        monster.tempStats.evasionBase = monster.info.evasionBase;
        monster.tempStats.critBase = monster.info.critBase;

        Monster = monster;
    }



    

}

[Serializable]
public class Stat
{

    public float BaseValue;

    private readonly List<StatModifier> statModifiers;

    // Add these variables
    protected bool isDirty = true;
    protected float _value;
    public readonly ReadOnlyCollection<StatModifier> StatModifiers; // Add this variable
    protected float lastBaseValue = float.MinValue;

    public Stat()
    {
        statModifiers = new List<StatModifier>();
        StatModifiers = statModifiers.AsReadOnly();
    }

    public Stat(float baseValue) : this()
    {
        BaseValue = baseValue;
    }

    public virtual float Value
    {
        get
        {
            if (isDirty || lastBaseValue != BaseValue)
            {
                lastBaseValue = BaseValue;
                _value = CalculateFinalValue();
                isDirty = false;
            }
            return _value;
        }
    }

    public virtual void AddModifier(StatModifier mod)
    {
        isDirty = true;
        statModifiers.Add(mod);
        statModifiers.Sort(CompareModifierOrder);


    }

    protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
    {
        if (a.Order < b.Order)
            return -1;
        else if (a.Order > b.Order)
            return 1;
        return 0; // if (a.Order == b.Order)
    }

    public virtual bool RemoveModifier(StatModifier mod)
    {
        if (statModifiers.Remove(mod))
        {
            Debug.Log(mod.Value);
            isDirty = true;
            return true;
        }
        return false;
    }

    public virtual bool RemoveAllModifiersFromSource(object source)
    {
        bool didRemove = false;

        for (int i = statModifiers.Count - 1; i >= 0; i--)
        {
            if (statModifiers[i].Source == source)
            {
                isDirty = true;
                didRemove = true;
                statModifiers.RemoveAt(i);

                
            }
        }
        return didRemove;
    }

    protected virtual float CalculateFinalValue()
    {
        float finalValue = BaseValue;
        float sumPercentAdd = 0; // This will hold the sum of our "PercentAdd" modifiers

        for (int i = 0; i < statModifiers.Count; i++)
        {
            StatModifier mod = statModifiers[i];

            if (mod.Type == StatModType.Flat)
            {
                finalValue += mod.Value;
            }
            else if (mod.Type == StatModType.PercentAdd) // When we encounter a "PercentAdd" modifier
            {
                sumPercentAdd += mod.Value; // Start adding together all modifiers of this type

                // If we're at the end of the list OR the next modifer isn't of this type
                if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdd)
                {
                    finalValue *= 1 + sumPercentAdd; // Multiply the sum with the "finalValue", like we do for "PercentMult" modifiers
                    sumPercentAdd = 0; // Reset the sum back to 0
                }
            }
            else if (mod.Type == StatModType.PercentMult) // Percent renamed to PercentMult
            {
                finalValue *= 1 + mod.Value;
            }
        }

        return (float)Math.Round(finalValue, 0);
    }


}

//The reason for doing this is simple - if someone wants to add a custom Order value for some modifiers to sit in the middle of the default ones, this allows a lot more flexibility.
//If we want to add a Flat modifier that applies between PercentAdd and PercentMult, we can just assign an Order anywhere between 201 and 299. Before we made this change,
//we'd have to assign custom Order values to all PercentAdd and PercentMult modifiers too.
public enum StatModType
{
    Flat = 100,
    PercentAdd = 200,
    PercentMult = 300,
}

public class StatModifier
{
    public readonly float Value;
    public readonly StatModType Type;
    public readonly int Order;
    public readonly object Source; // Added this variable

    // "Main" constructor. Requires all variables.
    public StatModifier(float value, StatModType type, int order, object source) // Added "source" input parameter
    {
        Value = value;
        Type = type;
        Order = order;
        Source = source; // Assign Source to our new input parameter

        Debug.Log(source);

    }

    // Requires Value and Type. Calls the "Main" constructor and sets Order and Source to their default values: (int)type and null, respectively.
    public StatModifier(float value, StatModType type) : this(value, type, (int)type, null) { }

    // Requires Value, Type and Order. Sets Source to its default value: null
    public StatModifier(float value, StatModType type, int order) : this(value, type, order, null) { }

    // Requires Value, Type and Source. Sets Order to its default value: (int)Type
    public StatModifier(float value, StatModType type, object source) : this(value, type, (int)type, source) { }
}


//*********************End Calcs***********************//



public class MapTileStatChange
{
    public void ApplyTileChanges(Monster monster, MapTile mapTile)
    {
        TileInfo tile = mapTile.info;

        if (tile.hpBonus != 0)
            //monster.hp += tile.hpBonus;
            monster.tempStats.HP.AddModifier(new StatModifier(tile.hpBonus, StatModType.Flat, this));
        if (tile.atkBonus != 0)
            //monster.attack += tile.atkBonus;
            monster.tempStats.Attack.AddModifier(new StatModifier(tile.atkBonus, StatModType.Flat, this));
        if (tile.defBonus != 0)
            //monster.defense += tile.defBonus;
            monster.tempStats.Defense.AddModifier(new StatModifier(tile.defBonus, StatModType.Flat, this));
        if (tile.speedBonus != 0)
            //monster.speed += tile.speedBonus;
            monster.tempStats.Speed.AddModifier(new StatModifier(tile.speedBonus, StatModType.Flat, this));

        if (tile.hpPercentBonus != 0)
            //monster.hp *= 1 + tile.hpPercentBonus;
            monster.tempStats.HP.AddModifier(new StatModifier(tile.hpPercentBonus, StatModType.PercentMult, this));
        if (tile.atkPercentBonus != 0)
            //monster.attack *= 1 + tile.atkPercentBonus;
            monster.tempStats.Attack.AddModifier(new StatModifier(tile.atkPercentBonus, StatModType.PercentMult, this));
        if (tile.defPercentBonus != 0)
            //if (tile.defPercentBonus != 0)
            monster.tempStats.Defense.AddModifier(new StatModifier(tile.defPercentBonus, StatModType.PercentMult, this));
        if (tile.precPercentBonus != 0)
            //monster.defense *= 1 + tile.defPercentBonus;
            monster.tempStats.Precision.AddModifier(new StatModifier(tile.precPercentBonus, StatModType.PercentMult, this));
        if (tile.spePercentBonus != 0)
            //monster.speed *= 1 + tile.spePercentBonus;
            monster.tempStats.Speed.AddModifier(new StatModifier(tile.speedBonus, StatModType.PercentMult, this));

        ////if the equipment item is Type protected, check and make sure the types match. If they do, apply bonuses
        //if (tile.typeMoveReq == monster.info.attack1.type)
        //{

        //    if (tile.atkPowerBonus != 0)
        //        monster.info.attack1.Power.AddModifier(new StatModifier(tile.atkPowerBonus, StatModType.Flat, this));
        //    if (tile.atkRangeBonus != 0)
        //        monster.info.attack1.Range.AddModifier(new StatModifier(tile.atkRangeBonus, StatModType.Flat, this));
        //    if (tile.atkTimeBonus != 0)
        //        monster.info.attack1.AttackTime.AddModifier(new StatModifier(tile.atkTimeBonus, StatModType.Flat, this));
        //    if (tile.critChanceBonus != 0)
        //        monster.info.attack1.CritChance.AddModifier(new StatModifier(tile.critChanceBonus, StatModType.Flat, this));
        //    if (tile.critModBonus != 0)
        //        monster.info.attack1.CritMod.AddModifier(new StatModifier(tile.critModBonus, StatModType.Flat, this));

        //    if (tile.atkPowerPercentBonus != 0)
        //        monster.info.attack1.Power.AddModifier(new StatModifier(tile.atkPowerPercentBonus, StatModType.PercentMult, this));
        //    if (tile.atkTimePercentBonus != 0)
        //        monster.info.attack1.AttackTime.AddModifier(new StatModifier(tile.atkTimePercentBonus, StatModType.PercentMult, this));


        //}

        //if (tile.typeMoveReq == monster.info.attack2.type)
        //{
        //    if (tile.atkPowerBonus != 0)
        //        monster.info.attack2.Power.AddModifier(new StatModifier(tile.atkPowerBonus, StatModType.Flat, this));
        //    if (tile.atkRangeBonus != 0)
        //        monster.info.attack2.Range.AddModifier(new StatModifier(tile.atkRangeBonus, StatModType.Flat, this));
        //    if (tile.atkTimeBonus != 0)
        //        monster.info.attack2.AttackTime.AddModifier(new StatModifier(tile.atkTimeBonus, StatModType.Flat, this));
        //    if (tile.critChanceBonus != 0)
        //        monster.info.attack2.CritChance.AddModifier(new StatModifier(tile.critChanceBonus, StatModType.Flat, this));
        //    if (tile.critModBonus != 0)
        //        monster.info.attack2.CritMod.AddModifier(new StatModifier(tile.critModBonus, StatModType.Flat, this));

        //    if (tile.atkPowerPercentBonus != 0)
        //        monster.info.attack2.Power.AddModifier(new StatModifier(tile.atkPowerPercentBonus, StatModType.PercentMult, this));
        //    if (tile.atkTimePercentBonus != 0)
        //        monster.info.attack2.AttackTime.AddModifier(new StatModifier(tile.atkTimePercentBonus, StatModType.PercentMult, this));
        //}
    }
}