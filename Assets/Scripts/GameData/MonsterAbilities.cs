using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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
    SingleTile, 
    AllTiles, 
    MultiTile,
};

//what the ability does
public enum AbilityType
{
    MonsterAttack,
    StatBuff,
    StatNerf,
    TileChange,
};

//how the ability determines the targets, if any determination is needed
public enum AbilityTargetParameter
{
    None,
    ByClass,
    ByType,
    BySpecies,
    
}

//if the ability's values are not constant, determine it here
public enum AbilityParameterScope
{
    None,
    PerEnemyByClass,
    PerAllyByClass,
    PerEnemyByType,
    PerAllyByType,
    PerTileByType,
    


}

[System.Serializable]
public class Ability
{
    public string name;
    public string description;
    public AbilityTarget target;
    public AbilityType type;
    public AbilityTargetParameter parameter;
    public AbilityParameterScope scope;

    public string typeParameter;
    public MonsterClass classParameter;
    public string speciesParameter;

    public delegate void AbilityDelegate();
    public AbilityDelegate abilityMethod;
    

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
        scope = AbilityParameterScope.None,
        castingAmmo = 1,
        atkBonus = 5,
       
        
    };

    public Ability BeastSlayer = new Ability
    {
        name = "Beast Slayer",
        description = "All Beast Class Enemies on the field have their defense stat cut in half.",
        target = AbilityTarget.AllEnemy,
        type = AbilityType.StatNerf,
        parameter = AbilityTargetParameter.ByClass,
        classParameter = MonsterClass.Beast,
        scope = AbilityParameterScope.None,
        castingAmmo = 1,
        defPercentBonus = -.5f,
    };

    public Ability NaturalQuake = new Ability
    {
        name = "Natural Quake",
        description = "For each Nature Type Enemy on the field, raise this monster's attack by 5.",
        target = AbilityTarget.Self,
        type = AbilityType.StatBuff,
        parameter = AbilityTargetParameter.None,
        typeParameter = "Nature",
        scope = AbilityParameterScope.PerEnemyByType,
        castingAmmo = 1,
        atkBonus = 5,
    };


    public Ability IceStorm = new Ability
    {
        name = "Ice Storm",
        description = "Converts up to 10 non-Ice Tiles to Ice Tiles",
        target = AbilityTarget.AllTiles,
        type = AbilityType.TileChange,
        parameter = AbilityTargetParameter.None,
        scope = AbilityParameterScope.None,
        castingAmmo = 1,
    };

    public Ability SerpentineVenom = new Ability
    {
        name = "Serpentine Venom",
        description = "Inflicts all non-Serpentine enemies with Poison.",
        target = AbilityTarget.MultiEnemy,
        type = AbilityType.StatNerf,
        parameter = AbilityTargetParameter.ByClass,
        scope = AbilityParameterScope.PerEnemyByClass,
        castingAmmo = 1,
    };

    public Ability DraconicBoom = new Ability
    {
        name = "Draconic Boom",
        description = "All enemies take 6% damage for each Dragon Class Tower you control.",
        target = AbilityTarget.AllEnemy,
        type = AbilityType.MonsterAttack,
        parameter = AbilityTargetParameter.None,
        scope = AbilityParameterScope.None,
        castingAmmo = 1,
    };
}

public class MonsterAbilities: MonoBehaviour
{
    public Dictionary<string, Ability> allAbilitiesDict = new Dictionary<string, Ability>();
    public AllAbilities allAbilities = new AllAbilities();


    //public Monster owner;
    //public Ability ability;
    //public MonsterAbility MonsterAbility;

    private void Awake()
    {
        AddAllAbilities();
    }


    void AddAllAbilities()
    {
        allAbilitiesDict.Add(allAbilities.ofAFeather.name, allAbilities.ofAFeather);
        allAbilitiesDict.Add(allAbilities.BeastSlayer.name, allAbilities.BeastSlayer);
        allAbilitiesDict.Add(allAbilities.NaturalQuake.name, allAbilities.NaturalQuake);
        allAbilitiesDict.Add(allAbilities.IceStorm.name, allAbilities.IceStorm);
        allAbilitiesDict.Add(allAbilities.SerpentineVenom.name, allAbilities.SerpentineVenom);
        allAbilitiesDict.Add(allAbilities.DraconicBoom.name, allAbilities.DraconicBoom);
    }

}




public class MonsterAbility
{
    public Ability Ability;
    public Monster Owner;

    public int castingCount;
    public int castingAmmo;
    public string abilityName;
    public string description;

    //this variable is used to delegate which ability method to use, given the name of the ability
    public delegate void AbilityDelegate();
    public AbilityDelegate abilityMethod;

    public MonsterAbilities abilities;


    public MonsterAbility(Ability ability, Monster owner)
    {
        Ability = ability;
        Owner = owner;

        castingAmmo = ability.castingAmmo;
        castingCount = ability.castingCount;
        abilityName = ability.name;
        description = ability.description;

       
        //get string of the name of the ability
        string name = string.Concat(ability.name.Where(c => !char.IsWhiteSpace(c)));

        //convert string to a delegate to call the method of the name of the ability
        abilityMethod = DelegateCreation(this, name);
        
    }

    public void ActivateAbility()
    {
        GameManager.Instance.SendNotificationToPlayer(Ability.name, 1, NotificationType.AbilityReady, Owner.info.species);
        abilityMethod.Invoke();
    }


    //create the method delegate for each ability
    AbilityDelegate DelegateCreation(object target, string functionName)
    {
        AbilityDelegate ab = (AbilityDelegate)Delegate.CreateDelegate(typeof(AbilityDelegate), target, functionName);
        return ab;
    }



    //Below is the method for all of the abilities//
    public void OfAFeather()
    {
        var towers = Owner.GetComponent<Tower>().mapDetails.liveTowers;

        foreach (Monster ally in towers)
        {
            if (ally.info.Class == MonsterClass.Flying)
            {
                ChangeStats(ally);
            }
        }
    }


    public void BeastSlayer()
    {
        var enemies = Owner.GetComponent<Tower>().mapDetails.liveEnemies;

        foreach (Enemy enemy in enemies)
        {
            if (enemy.monster.info.Class == MonsterClass.Beast)
            {
                ChangeStats(enemy.GetComponent<Monster>());
            }
        }
    }


    public void NaturalQuake()
    {
        var enemies = Owner.GetComponent<Tower>().mapDetails.liveEnemies;


        foreach (Enemy enemy in enemies)
        {
            if (enemy.monster.info.type1 == "Nature" || enemy.monster.info.type2 == "Nature")
            {
                ChangeStats(Owner);
            }
        }
    
    }


    public void IceStorm()
    {
        var tiles = GameManager.Instance.activeTiles;

        for (int i = 0; i < 10; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, tiles.Count);

            if (tiles[randomIndex].info.attribute != "Ice")
            {
                tiles[randomIndex].ClearAttribute();
                tiles[randomIndex].GetAttribute(7);
            }

        }
    }

    public void SerpentineVenom()
    {
        var enemies = Owner.GetComponent<Tower>().mapDetails.liveEnemies;
        AllStatuses status = new AllStatuses();

        foreach (Enemy enemy in enemies)
        {
            if (enemy.monster.info.Class != MonsterClass.Serpentine)
            {
                Status poison = status.Poison;
                enemy.GetComponent<Monster>().AddStatus(poison);
            }
        }
    }

    public void DraconicBoom()
    {
        var enemies = Owner.GetComponent<Tower>().mapDetails.liveEnemies;
        var towers = Owner.GetComponent<Tower>().mapDetails.liveTowers;
        int count = 0;
        int dragons = 0;

        

        foreach (Monster tower in towers)
        {
            if (tower.info.Class == MonsterClass.Dragon)
            {
                dragons += 1;
            }

            count += 1;

            if (count >= towers.Count - 1)
            {
                float damageDealt = dragons * .06f;
                foreach (Enemy enemy in enemies)
                {
                    float enemyHP = enemy.GetComponent<Monster>().info.maxHP;
                    float totalDamage = damageDealt * enemyHP;

                    enemy.TakeDamage(totalDamage, Owner);
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




       
    }

    public void RemoveStatChanges(Monster monster)
    {
        monster.info.HP.RemoveAllModifiersFromSource(this);
        monster.info.Attack.RemoveAllModifiersFromSource(this);
        monster.info.Defense.RemoveAllModifiersFromSource(this);
        monster.info.Speed.RemoveAllModifiersFromSource(this);
        monster.info.Precision.RemoveAllModifiersFromSource(this);
        monster.info.CoinGeneration.RemoveAllModifiersFromSource(this);


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









