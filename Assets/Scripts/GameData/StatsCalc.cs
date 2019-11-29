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

        var allAttacks = GameManager.Instance.baseAttacks.attackDict;
        var allMonstersDict = GameManager.Instance.monstersData.monstersAllDict;


        //MonsterAttack attack1 = allAttacks[monster.info.attack1Name];
        //MonsterAttack attack2 = allAttacks[monster.info.attack2Name];

        MonsterAttack attack1 = allAttacks[monster.info.attack1Name];
        MonsterAttack attack2 = allAttacks[monster.info.attack2Name];





        //monster.info.hpMax = (int)((2 * monster.info.hpBase + monster.info.hpPot) * (level / 100) * Mathf.Sqrt(1 + (level / 100)) + 100);
        monster.info.HP.BaseValue = (int)((2 * monster.info.hpBase + monster.info.HPPotential.BaseValue) * (level / 25) * Mathf.Sqrt(1 + (level / 25)) + 100);
        //monster.info.defStat = (int)((2 * monster.info.defBase + monster.info.defPot) * (level / 100) * Mathf.Sqrt(1 + (level / 100)) + 100);
        monster.info.Defense.BaseValue = (int)((2 * monster.info.defBase + monster.info.DefensePotential.BaseValue) * (level / 25) * Mathf.Sqrt(1 + (level / 25)) + 100);
        //monster.info.atkStat = (int)((2 * monster.info.atkBase + monster.info.atkPot) * (level / 100) * Mathf.Sqrt(1 + (level / 100)) + 100);
        monster.info.Attack.BaseValue = (int)((2 * monster.info.atkBase + monster.info.AttackPotential.BaseValue) * (level / 25) * Mathf.Sqrt(1 + (level / 25)) + 100);
        //monster.info.speStat = (int)((2 * monster.info.speBase + monster.info.spePot) * (level / 100) * Mathf.Sqrt(1 + (level / 100)) + 100);
        monster.info.Speed.BaseValue = (int)((2 * monster.info.speBase + monster.info.SpeedPotential.BaseValue) * (level / 25) * Mathf.Sqrt(1 + (level / 25)) + 100);
        //monster.info.precStat = (int)((2 * monster.info.precBase + monster.info.precPot) * (level / 100) * Mathf.Sqrt(1 + (level / 100)) + 100);
        monster.info.Precision.BaseValue = (int)((2 * monster.info.precBase + monster.info.PrecisionPotential.BaseValue) * (level / 25) * Mathf.Sqrt(1 + (level / 25)) + 100);
        
        monster.info.critBase = (monster.info.precBase / 33) + Mathf.Sqrt(monster.info.Precision.BaseValue);


        monster.info.CoinGeneration.BaseValue = (int)(level * monster.info.coinGenBase);


        monster.info.evasionBase = Mathf.Round(((monster.info.Speed.BaseValue / monster.info.HP.BaseValue) / 5) * 100);
        monster.info.Stamina.BaseValue = (int)monster.info.staminaBase;
        monster.info.EnergyCost.BaseValue = (int)monster.info.energyCost;
        monster.info.EnergyGeneration.BaseValue = (monster.info.energyGenBase);


        monster.info.DropRateMod.BaseValue = 0;
        monster.info.EnergyDamageMod.BaseValue = 0;
        monster.info.ExplodeDamageMod.BaseValue = 0;
        monster.info.PhysicalDamageMod.BaseValue = 0;
        monster.info.PierceDamageMod.BaseValue = 0;



        //monster.info.attack1Name = attack1.name;
        //monster.info.attack1 = attack1;
        //monster.info.attack1.Power.BaseValue = attack1.power;
        //monster.info.attack1.Range.BaseValue = attack1.range;
        //monster.info.attack1.CritChance.BaseValue = attack1.critChance;
        //monster.info.attack1.CritMod.BaseValue = attack1.critMod;
        //monster.info.attack1.EffectChance.BaseValue = attack1.effectChance;
        //monster.info.attack1.AttackTime.BaseValue = attack1.attackTime;
        //monster.info.attack1.AttackSpeed.BaseValue = attack1.attackSpeed;
        //monster.info.attack1.AttackSlow.BaseValue = attack1.hitSlowTime;

        //monster.info.attack2Name = attack2.name;
        //monster.info.attack2 = attack2;
        //monster.info.attack2.Power.BaseValue = attack2.power;
        //monster.info.attack2.Range.BaseValue = attack2.range;
        //monster.info.attack2.CritChance.BaseValue = attack2.critChance;
        //monster.info.attack2.CritMod.BaseValue = attack2.critMod;
        //monster.info.attack2.EffectChance.BaseValue = attack2.effectChance;
        //monster.info.attack2.AttackTime.BaseValue = attack2.attackTime;
        //monster.info.attack2.AttackSpeed.BaseValue = attack2.attackSpeed;
        //monster.info.attack2.AttackSlow.BaseValue = attack2.hitSlowTime;


        monster.info.attack1Name = attack1.name;
        monster.info.baseAttack1.attack = attack1;
        monster.info.baseAttack1.attack.Power.BaseValue = attack1.power;
        monster.info.baseAttack1.attack.Range.BaseValue = attack1.range;
        monster.info.baseAttack1.attack.CritChance.BaseValue = attack1.critChance;
        monster.info.baseAttack1.attack.CritMod.BaseValue = attack1.critMod;
        monster.info.baseAttack1.attack.EffectChance.BaseValue = attack1.effectChance;
        monster.info.baseAttack1.attack.AttackTime.BaseValue = attack1.attackTime;
        monster.info.baseAttack1.attack.AttackSpeed.BaseValue = attack1.attackSpeed;
        monster.info.baseAttack1.attack.AttackSlow.BaseValue = attack1.hitSlowTime;

        monster.info.attack2Name = attack2.name;
        monster.info.baseAttack2.attack = attack2;
        monster.info.baseAttack2.attack.Power.BaseValue = attack2.power;
        monster.info.baseAttack2.attack.Range.BaseValue = attack2.range;
        monster.info.baseAttack2.attack.CritChance.BaseValue = attack2.critChance;
        monster.info.baseAttack2.attack.CritMod.BaseValue = attack2.critMod;
        monster.info.baseAttack2.attack.EffectChance.BaseValue = attack2.effectChance;
        monster.info.baseAttack2.attack.AttackTime.BaseValue = attack2.attackTime;
        monster.info.baseAttack2.attack.AttackSpeed.BaseValue = attack2.attackSpeed;
        monster.info.baseAttack2.attack.AttackSlow.BaseValue = attack2.hitSlowTime;

        monster.info.maxHP = monster.info.HP.Value;



        //sets the monster's temporary stats, using it's permanent stats as a base
        monster.tempStats.HP.BaseValue = monster.info.HP.Value;
        monster.tempStats.Defense.BaseValue = monster.info.Defense.Value;
        monster.tempStats.Attack.BaseValue = monster.info.Attack.Value;
        monster.tempStats.Speed.BaseValue = monster.info.Speed.Value;
        monster.tempStats.Precision.BaseValue = monster.info.Precision.Value;
        monster.tempStats.Stamina.BaseValue = monster.info.Stamina.Value;
        monster.tempStats.EnergyCost.BaseValue = monster.info.EnergyCost.Value;
        monster.tempStats.EnergyGeneration.BaseValue = monster.info.EnergyGeneration.Value;
        monster.tempStats.CoinGeneration.BaseValue = monster.info.CoinGeneration.Value;
        monster.tempStats.evasionBase = monster.info.evasionBase;
        monster.tempStats.critBase = monster.info.critBase;


        monster.tempStats.attack1 = attack1;
        monster.tempStats.attack1.Power.BaseValue = attack1.power;
        monster.tempStats.attack1.Range.BaseValue = monster.info.attack1.Range.BaseValue;
        monster.tempStats.attack1.CritChance.BaseValue = monster.info.attack1.CritChance.BaseValue;
        monster.tempStats.attack1.CritMod.BaseValue = monster.info.attack1.CritMod.BaseValue;
        monster.tempStats.attack1.EffectChance.BaseValue = monster.info.attack1.EffectChance.BaseValue;
        monster.tempStats.attack1.AttackTime.BaseValue = monster.info.attack1.AttackTime.BaseValue;
        monster.tempStats.attack1.AttackSpeed.BaseValue = monster.info.attack1.AttackSpeed.BaseValue;
        monster.tempStats.attack1.AttackSlow.BaseValue = monster.info.attack1.hitSlowTime;

        monster.tempStats.attack2 = attack2;
        monster.tempStats.attack2.Power.BaseValue = attack2.power;
        monster.tempStats.attack2.Range.BaseValue = monster.info.attack2.Range.BaseValue;
        monster.tempStats.attack2.CritChance.BaseValue = monster.info.attack2.CritChance.BaseValue;
        monster.tempStats.attack2.CritMod.BaseValue = monster.info.attack2.CritMod.BaseValue;
        monster.tempStats.attack2.EffectChance.BaseValue = monster.info.attack2.EffectChance.BaseValue;
        monster.tempStats.attack2.AttackTime.BaseValue = monster.info.attack2.AttackTime.BaseValue;
        monster.tempStats.attack2.AttackSpeed.BaseValue = monster.info.attack2.AttackSpeed.BaseValue;
        monster.tempStats.attack2.AttackSlow.BaseValue = monster.info.attack2.hitSlowTime;


        monster.tempStats.DropRateMod.BaseValue = monster.info.DropRateMod.Value;
        monster.tempStats.EnergyDamageMod.BaseValue = monster.info.EnergyDamageMod.Value;
        monster.tempStats.ExplodeDamageMod.BaseValue = monster.info.ExplodeDamageMod.Value;
        monster.tempStats.PhysicalDamageMod.BaseValue = monster.info.PhysicalDamageMod.Value;
        monster.tempStats.PierceDamageMod.BaseValue = monster.info.PierceDamageMod.Value;

        Monster = monster;

    }



    

}

[Serializable]
public class Stat
{

    public float BaseValue;

    public readonly List<StatModifier> statModifiers;

    // Add these variables
    protected bool isDirty = true;
    //protected float _value;
    public float _value;
    public readonly ReadOnlyCollection<StatModifier> StatModifiers; // Add this variable
    //protected float lastBaseValue = float.MinValue;
    public float lastBaseValue = float.MinValue;

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

        //Debug.Log(mod);
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
            //Debug.Log(mod.Value);
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
            //Debug.Log(statModifiers.Count);

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

        
        //return (float)Math.Round(finalValue, 0);
        return (float)Math.Round(finalValue, 2);
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

[Serializable]
public class StatModifier
{
    //public readonly float Value;
    //public readonly StatModType Type;
    //public readonly int Order;
    //public readonly object Source; // Added this variable
    //public readonly string ModName;

    public float Value;
    public StatModType Type;
    public int Order;
    public object Source; // Added this variable
    public string ModName;

    // "Main" constructor. Requires all variables.
    public StatModifier(float value, StatModType type, int order, object source, string name) // Added "source" input parameter
    {
        Value = value;
        Type = type;
        Order = order;
        Source = source; // Assign Source to our new input parameter
        ModName = name;


    }

    // Requires Value and Type. Calls the "Main" constructor and sets Order and Source to their default values: (int)type and null, respectively.
    public StatModifier(float value, StatModType type) : this(value, type, (int)type, null, null) { }

    // Requires Value, Type and Order. Sets Source to its default value: null
    public StatModifier(float value, StatModType type, int order) : this(value, type, order, null, null) { }

    // Requires Value, Type and Source. Sets Order to its default value: (int)Type
    public StatModifier(float value, StatModType type, object source, string name) : this(value, type, (int)type, source, name) { }
}


//*********************End Calcs***********************//



public class MapTileStatChange
{
    
    public void ApplyTileChanges(Monster monster, MapTile mapTile)
    {
        TileInfo tile = mapTile.info;

        if (tile.hpBonus != 0)
            //monster.hp += tile.hpBonus;
            monster.info.HP.AddModifier(new StatModifier(tile.hpBonus, StatModType.Flat, this, mapTile.tileNumber.ToString()));
        if (tile.atkBonus != 0)
            //monster.attack += tile.atkBonus;
            monster.info.Attack.AddModifier(new StatModifier(tile.atkBonus, StatModType.Flat, this, mapTile.tileNumber.ToString()));
        if (tile.defBonus != 0)
            //monster.defense += tile.defBonus;
            monster.info.Defense.AddModifier(new StatModifier(tile.defBonus, StatModType.Flat, this, mapTile.tileNumber.ToString()));
        if (tile.speedBonus != 0)
            //monster.speed += tile.speedBonus;
            monster.info.Speed.AddModifier(new StatModifier(tile.speedBonus, StatModType.Flat, this, mapTile.tileNumber.ToString()));

        if (tile.hpPercentBonus != 0)
            //monster.hp *= 1 + tile.hpPercentBonus;
            monster.info.HP.AddModifier(new StatModifier(tile.hpPercentBonus, StatModType.PercentMult, this, mapTile.tileNumber.ToString()));
        if (tile.atkPercentBonus != 0)
            //monster.attack *= 1 + tile.atkPercentBonus;
            monster.info.Attack.AddModifier(new StatModifier(tile.atkPercentBonus, StatModType.PercentMult, this, mapTile.tileNumber.ToString()));
        if (tile.defPercentBonus != 0)
            //if (tile.defPercentBonus != 0)
            monster.info.Defense.AddModifier(new StatModifier(tile.defPercentBonus, StatModType.PercentMult, this, mapTile.tileNumber.ToString()));
        if (tile.precPercentBonus != 0)
            //monster.defense *= 1 + tile.defPercentBonus;
            monster.info.Precision.AddModifier(new StatModifier(tile.precPercentBonus, StatModType.PercentMult, this, mapTile.tileNumber.ToString()));
        if (tile.spePercentBonus != 0)
            //monster.speed *= 1 + tile.spePercentBonus;
            monster.info.Speed.AddModifier(new StatModifier(tile.speedBonus, StatModType.PercentMult, this, mapTile.tileNumber.ToString()));

        

        monster.MonsterStatMods();
    }


    public void RemoveTileBoosts(Monster monster)
    {
        monster.info.HP.RemoveAllModifiersFromSource(this);
        monster.info.Attack.RemoveAllModifiersFromSource(this);
        monster.info.Defense.RemoveAllModifiersFromSource(this);
        monster.info.Speed.RemoveAllModifiersFromSource(this);
        monster.info.Precision.RemoveAllModifiersFromSource(this);

        monster.info.baseAttack1.Power.RemoveAllModifiersFromSource(this);
        monster.info.baseAttack1.Range.RemoveAllModifiersFromSource(this);
        monster.info.baseAttack1.AttackSpeed.RemoveAllModifiersFromSource(this);
        monster.info.baseAttack1.AttackTime.RemoveAllModifiersFromSource(this);
        monster.info.baseAttack1.CritChance.RemoveAllModifiersFromSource(this);
        monster.info.baseAttack1.CritMod.RemoveAllModifiersFromSource(this);
        monster.info.baseAttack1.EffectChance.RemoveAllModifiersFromSource(this);

        monster.info.baseAttack2.Power.RemoveAllModifiersFromSource(this);
        monster.info.baseAttack2.Range.RemoveAllModifiersFromSource(this);
        monster.info.baseAttack2.AttackSpeed.RemoveAllModifiersFromSource(this);
        monster.info.baseAttack2.AttackTime.RemoveAllModifiersFromSource(this);
        monster.info.baseAttack2.CritChance.RemoveAllModifiersFromSource(this);
        monster.info.baseAttack2.CritMod.RemoveAllModifiersFromSource(this);
        monster.info.baseAttack2.EffectChance.RemoveAllModifiersFromSource(this);


        //monster.tempStats.attack1.Power.RemoveAllModifiersFromSource(this);
        //monster.tempStats.attack1.Range.RemoveAllModifiersFromSource(this);
        //monster.tempStats.attack1.AttackSpeed.RemoveAllModifiersFromSource(this);
        //monster.tempStats.attack1.AttackTime.RemoveAllModifiersFromSource(this);
        //monster.tempStats.attack1.CritChance.RemoveAllModifiersFromSource(this);
        //monster.tempStats.attack1.CritMod.RemoveAllModifiersFromSource(this);
        //monster.tempStats.attack1.EffectChance.RemoveAllModifiersFromSource(this);

        //monster.tempStats.attack2.Power.RemoveAllModifiersFromSource(this);
        //monster.tempStats.attack2.Range.RemoveAllModifiersFromSource(this);
        //monster.tempStats.attack2.AttackSpeed.RemoveAllModifiersFromSource(this);
        //monster.tempStats.attack2.AttackTime.RemoveAllModifiersFromSource(this);
        //monster.tempStats.attack2.CritChance.RemoveAllModifiersFromSource(this);
        //monster.tempStats.attack2.CritMod.RemoveAllModifiersFromSource(this);
        //monster.tempStats.attack2.EffectChance.RemoveAllModifiersFromSource(this);

        monster.MonsterStatMods();
    }
}