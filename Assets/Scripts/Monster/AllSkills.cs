using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SkillData 
{
    public string name;
    public string description;
    public TriggerType triggerType;
    public EventTrigger trigger;
    
}




public class Skills
{
    public SkillData ItemFinder = new SkillData
    {
        name = "Item Finder",
        description = "Increases the chances of an item being dropped by an Enemy that this monster destroys by 20%."
        
    };

    public SkillData TerrifyingGaze = new SkillData
    {
        name = "Terrifying Gaze",
        description = "Cuts all monsters' Speed by 25%."
    };

    public SkillData NaturalArmor = new SkillData
    {
        name = "Natural Armor",
        description = "Raises this monster's defense by 3% for each Nature Tile on the field.",
        triggerType = TriggerType.TileChange,
    };

    public SkillData IgnitionTrigger = new SkillData
    {
        name = "Ignition Trigger",
        description = "Raises this monster's fire type attacks by 7% for each Fire Type Tower you control.",
        triggerType = TriggerType.TowerSummon,
    };
}


public class AllSkills: MonoBehaviour
{
    public Dictionary<string, SkillData> allSkillsDict = new Dictionary<string, SkillData>();
    public Skills allSkills = new Skills();



    private void Awake()
    {
        AddAllSkills();
    }


    void AddAllSkills()
    {
        allSkillsDict.Add(allSkills.ItemFinder.name, allSkills.ItemFinder);
        allSkillsDict.Add(allSkills.TerrifyingGaze.name, allSkills.TerrifyingGaze);
        allSkillsDict.Add(allSkills.NaturalArmor.name, allSkills.NaturalArmor);
        allSkillsDict.Add(allSkills.IgnitionTrigger.name, allSkills.IgnitionTrigger);
    }
}

//the class that the monster has for the skill. it combines the data for the skill, plus a trigger to make this new skill object
[System.Serializable]
public class PassiveSkill
{
    public Monster Owner;
    public SkillData skill;
    //this variable is used to delegate which ability method to use, given the name of the ability
    public delegate void SkillDelegate();
    public SkillDelegate skillMethod;
    public int triggerCount;
    

    //keep track of the stat modifiers, if any, that a skill gives
    public List<StatModifier> mod = new List<StatModifier>();

    public PassiveSkill(Monster monster, SkillData data)
    {
        Owner = monster;
        skill = data;

        //get string of the name of the item
        string name = string.Concat(skill.name.Where(c => !char.IsWhiteSpace(c)));

        //convert string to a delegate to call the method of the name of the ability
        skillMethod = DelegateCreation(this, name);
        
    }

    SkillDelegate DelegateCreation(object target, string functionName)
    {
        SkillDelegate eq = (SkillDelegate)Delegate.CreateDelegate(typeof(SkillDelegate), target, functionName);
        return eq;
    }


    public void ActivateSkill()
    {
        mod.Clear();
        skillMethod.Invoke();
        Owner.MonsterStatMods();
    }

    //if this skill is called from a trigger, do this
    public void TriggerEvent()
    {
        
        RemoveSkillBoosts(Owner);
        ActivateSkill();
    }


    //Below is the method for all of the abilities//
    public void ItemFinder()
    {
        Owner.info.DropRateMod.AddModifier(new StatModifier(.2f, StatModType.Flat, skill, skill.name));
        
    }

    public void TerrifyingGaze()
    {
        mod.Add(new StatModifier(-.5f, StatModType.PercentMult, this, skill.name));
        if (Owner.isEnemy)
        {
            GlobalStat terrifyingGaze = new GlobalStat(mod[0], "Speed", Owner, skill.name, GlobalStatModType.Towers);
            GameManager.Instance.activeMap.AddGlobalStat(terrifyingGaze);
            Owner.ownedGlobalStats.Add(terrifyingGaze);
        }
        else
        {
            GlobalStat terrifyingGaze = new GlobalStat(mod[0], "Speed", Owner, skill.name, GlobalStatModType.Enemies);
            GameManager.Instance.activeMap.AddGlobalStat(terrifyingGaze);
            Owner.ownedGlobalStats.Add(terrifyingGaze);
        }
       

    }

    public void NaturalArmor()
    {

        var tiles = GameManager.Instance.activeMap.allTiles;
        int total = 0;
        int tileCount = 0;

        foreach (MapTile tile in tiles)
        {
            if (tile.tileAtt == TileAttribute.Nature)
            {
                total += 1;
            }

            tileCount += 1;

            if (tileCount >= tiles.Count)
            {
                float totalBoost = total * .03f;
                mod.Add(new StatModifier(totalBoost, StatModType.PercentMult, skill, skill.name));

                Owner.info.Defense.AddModifier(mod[0]);
                break;
            }

        }
    }

    public void IgnitionTrigger()
    {
        var towers = GameManager.Instance.activeMap.liveTowers;
        int total = 0;
        int towerCount = 0;


        foreach (Monster monster in towers)
        {
            if (monster.info.type1 == "Fire" || monster.info.type2 == "Fire")
            {
                towerCount += 1;
            }

            total += 1;

            if (total == towers.Count)
            {
                //float totalBoost = towerCount * .07f;
                float totalBoost = towerCount + 7;
                Debug.Log(totalBoost);
                mod.Add(new StatModifier(totalBoost, StatModType.Flat, skill, skill.name));

                if (Owner.info.baseAttack1.type == "Fire")
                {
                    Owner.info.baseAttack1.Power.AddModifier(mod[0]);
                }

                if (Owner.info.baseAttack2.type == "Fire")
                {
                    Owner.info.baseAttack2.Power.AddModifier(mod[0]);
                }

                
                break;
            }

            
        }
    }







    public void RemoveSkillBoosts(Monster monster)
    {
        monster.info.HP.RemoveAllModifiersFromSource(skill);
        monster.info.Attack.RemoveAllModifiersFromSource(skill);
        monster.info.Defense.RemoveAllModifiersFromSource(skill);
        monster.info.Speed.RemoveAllModifiersFromSource(skill);
        monster.info.Precision.RemoveAllModifiersFromSource(skill);
        monster.info.Stamina.RemoveAllModifiersFromSource(skill);
        monster.info.EnergyGeneration.RemoveAllModifiersFromSource(skill);
        monster.info.EnergyCost.RemoveAllModifiersFromSource(skill);
        monster.info.CoinGeneration.RemoveAllModifiersFromSource(skill);
       

        monster.info.baseAttack1.Power.RemoveAllModifiersFromSource(skill);
        monster.info.baseAttack1.Range.RemoveAllModifiersFromSource(skill);
        monster.info.baseAttack1.AttackSpeed.RemoveAllModifiersFromSource(skill);
        monster.info.baseAttack1.AttackTime.RemoveAllModifiersFromSource(skill);
        monster.info.baseAttack1.CritChance.RemoveAllModifiersFromSource(skill);
        monster.info.baseAttack1.CritMod.RemoveAllModifiersFromSource(skill);
        monster.info.baseAttack1.EffectChance.RemoveAllModifiersFromSource(skill);

        monster.info.baseAttack2.Power.RemoveAllModifiersFromSource(skill);
        monster.info.baseAttack2.Range.RemoveAllModifiersFromSource(skill);
        monster.info.baseAttack2.AttackSpeed.RemoveAllModifiersFromSource(skill);
        monster.info.baseAttack2.AttackTime.RemoveAllModifiersFromSource(skill);
        monster.info.baseAttack2.CritChance.RemoveAllModifiersFromSource(skill);
        monster.info.baseAttack2.CritMod.RemoveAllModifiersFromSource(skill);
        monster.info.baseAttack2.EffectChance.RemoveAllModifiersFromSource(skill);


       
        monster.MonsterStatMods();
    }
}
