using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//what monsters does the ability target
public enum AbilityTarget
{
    SingleEnemy,
    MultiEnemy,
    AllEnemy,
    Self,
    SingleAlly,
    MultiAlly,
    AllAlly,
};

//what the ability does
public enum AbilityType
{
    MonsterAttack,
    StatBuff,
    StatNerf,

};

//how the ability determines the targets, if any determination is needed
public enum AbilityTargetParameter
{
    None,
    ByClass,
    ByType,
    BySpecies,
    
}

[System.Serializable]
public class Ability
{
    public string name;
    public string description;
    public AbilityTarget target;
    public AbilityType type;
    public AbilityTargetParameter parameter;

    public Type typeParameter;
    public MonsterClass classParameter;
    public string speciesParameter;


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

    //how amount of times this ability can be used in one match
    public int castingAmmo, castingCount;
    //how long before this ability can be used again, as long as it has the ammo
    public float cooldown;
    //if the ability targets is "Multi", then this number says how many targets there are
    public int targetNumber;
    

    //Stats are filled in when the monster using it becomes active, to account for stat buffs and drops
    public Stat Range;
    public Stat Power;
    public Stat CritChance;
    public Stat CritMod;
    public Stat EffectChance;
    public Stat AttackTime;
    public Stat AttackSpeed;
    public Stat AttackSlow;

    public int range;
    public int power;
    public float critChance;
    public float critMod;
    public float effectChance;
    //modifier that affects the length of time between this attack's uses
    public float attackTime;
    //how fast the attack moves towards enemies
    public float attackSpeed;
    //length of time a monster is slowed when hit by this attack
    public float hitSlowTime;
    //either projectile or physical
    public AttackMode attackMode;

}


public class AllAbilities
{
    public Ability ofAFeather = new Ability
    {
        name = "Of A Feather",
        description = "All Flying Class Towers gain +5 attack",
        target = AbilityTarget.AllAlly,
        type = AbilityType.StatBuff,
        parameter = AbilityTargetParameter.ByClass,
        classParameter = MonsterClass.Flying,
        castingAmmo = 1,
        atkBonus = 5,


    };
}

public class MonsterAbilities: MonoBehaviour
{
    public Dictionary<string, Ability> allAbilitiesDict = new Dictionary<string, Ability>();
    public AllAbilities allAbilities = new AllAbilities();


    private void Awake()
    {
        AddAllAbilities();
    }


    void AddAllAbilities()
    {
        allAbilitiesDict.Add(allAbilities.ofAFeather.name, allAbilities.ofAFeather);
    }
}


public class MonsterAbility
{
    public Ability Ability;
    public Monster Owner;

    public MonsterAbility(Ability ability, Monster owner)
    {
        Ability = ability;
        Owner = owner;

        if (ability.target == AbilityTarget.AllAlly)
        {
            AllAlly();
        }

    }


    public void AllAlly()
    {
        var towers = Owner.GetComponent<Tower>().Map.GetComponent<MapDetails>().liveTowers;
        var enemies = Owner.GetComponent<Tower>().Map.GetComponent<MapDetails>().liveEnemies;


        //if there is a parameter on who the ability targets, figure out the targets
        if (Ability.parameter != AbilityTargetParameter.None)
        {
            //if your ability affects all allies of a class, do this
            if (Ability.parameter == AbilityTargetParameter.ByClass)
            {
                foreach(Monster ally in towers)
                {
                    if (ally.info.Class == Ability.classParameter)
                    {
                        ChangeStats(ally);
                    }
                }
            }
        }
    }







    public void ChangeStats(Monster monster)
    {
        if (Ability.hpBonus != 0)
            monster.info.HP.AddModifier(new StatModifier(Ability.hpBonus, StatModType.Flat, this, Ability.name));
        if (Ability.atkBonus != 0)
            monster.info.Attack.AddModifier(new StatModifier(Ability.atkBonus, StatModType.Flat, this, Ability.name));
        if (Ability.defBonus != 0)
            monster.info.Defense.AddModifier(new StatModifier(Ability.defBonus, StatModType.Flat, this, Ability.name));
        if (Ability.speedBonus != 0)
            monster.info.Speed.AddModifier(new StatModifier(Ability.speedBonus, StatModType.Flat, this, Ability.name));

        if (Ability.hpPercentBonus != 0)
            monster.info.HP.AddModifier(new StatModifier(Ability.hpPercentBonus, StatModType.PercentMult, this, Ability.name));
        if (Ability.atkPercentBonus != 0)
            monster.info.Attack.AddModifier(new StatModifier(Ability.atkPercentBonus, StatModType.PercentMult, this, Ability.name));
        if (Ability.defPercentBonus != 0)
            monster.info.Defense.AddModifier(new StatModifier(Ability.defPercentBonus, StatModType.PercentMult, this, Ability.name));
        if (Ability.spePercentBonus != 0)
            monster.info.Speed.AddModifier(new StatModifier(Ability.spePercentBonus, StatModType.PercentMult, this, Ability.name));




        ////if the Abilityment item is Type protected, check and make sure the types match. If they do, apply bonuses
        //if (Ability.typeMoveReq == monster.info.attack1.type || Ability.typeMoveReq == "none")
        //{


        //    if (Ability.atkPowerBonus != 0)
        //        monster.info.attack1.Power.AddModifier(new StatModifier(Ability.atkPowerBonus, StatModType.Flat, this, Ability.name));
        //    if (Ability.atkRangeBonus != 0)
        //        monster.info.attack1.Range.AddModifier(new StatModifier(Ability.atkRangeBonus, StatModType.Flat, this, Ability.name));
        //    if (Ability.atkTimeBonus != 0)
        //        monster.info.attack1.AttackTime.AddModifier(new StatModifier(Ability.atkTimeBonus, StatModType.Flat, this, Ability.name));
        //    if (Ability.critChanceBonus != 0)
        //        monster.info.attack1.CritChance.AddModifier(new StatModifier(Ability.critChanceBonus, StatModType.Flat, this, Ability.name));
        //    if (Ability.critModBonus != 0)
        //        monster.info.attack1.CritMod.AddModifier(new StatModifier(Ability.critModBonus, StatModType.Flat, this, Ability.name));

        //    if (Ability.atkPowerPercentBonus != 0)
        //        monster.info.attack1.Power.AddModifier(new StatModifier(Ability.atkPowerPercentBonus, StatModType.PercentMult, this, Ability.name));
        //    if (Ability.atkTimePercentBonus != 0)
        //        monster.info.attack1.AttackTime.AddModifier(new StatModifier(Ability.atkTimePercentBonus, StatModType.PercentMult, this, Ability.name));



        //}


        //if (Ability.typeMoveReq == monster.info.attack2.type || Ability.typeMoveReq == "none")
        //{
        //    if (Ability.atkPowerBonus != 0)
        //        monster.info.attack2.Power.AddModifier(new StatModifier(Ability.atkPowerBonus, StatModType.Flat, this, Ability.name));
        //    if (Ability.atkRangeBonus != 0)
        //        monster.info.attack2.Range.AddModifier(new StatModifier(Ability.atkRangeBonus, StatModType.Flat, this, Ability.name));
        //    if (Ability.atkTimeBonus != 0)
        //        monster.info.attack2.AttackTime.AddModifier(new StatModifier(Ability.atkTimeBonus, StatModType.Flat, this, Ability.name));
        //    if (Ability.critChanceBonus != 0)
        //        monster.info.attack2.CritChance.AddModifier(new StatModifier(Ability.critChanceBonus, StatModType.Flat, this, Ability.name));
        //    if (Ability.critModBonus != 0)
        //        monster.info.attack2.CritMod.AddModifier(new StatModifier(Ability.critModBonus, StatModType.Flat, this, Ability.name));

        //    if (Ability.atkPowerPercentBonus != 0)
        //        monster.info.attack2.Power.AddModifier(new StatModifier(Ability.atkPowerPercentBonus, StatModType.PercentMult, this, Ability.name));
        //    if (Ability.atkTimePercentBonus != 0)
        //        monster.info.attack2.AttackTime.AddModifier(new StatModifier(Ability.atkTimePercentBonus, StatModType.PercentMult, this, Ability.name));

        //}
    }

    public void RemoveStatChanges(Monster monster)
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









