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
    

   

    //public void ActivateSkill(Monster monster)
    //{
    //    Owner = monster;
    //    skillManager.ActivateSkill(monster, this);
        

    //}


    //public void TriggerEvent()
    //{
    //    skillManager.RemoveSkill();
    //    skillManager.ActivateSkill(Owner, this);
       
    //}
    
}


//public class SkillManager
//{
//    public Monster monster;
//    public PassiveSkill passiveSkill;

//    public void ActivateSkill(Monster Monster, PassiveSkill skill)
//    {
//        monster = Monster;
//        passiveSkill = skill;

//        SkillEffects effect = new SkillEffects(monster, skill);
//    }

//    public void RemoveSkill()
//    {
//        foreach (Stat stat in monster.allStats)
//        {
//            stat.RemoveAllModifiersFromSource(passiveSkill);
//        }
//    }
//}


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
        skillMethod.Invoke();
    }

    //if this skill is called from a trigger, do this
    public void TriggerEvent()
    {
        foreach (StatModifier m in mod)
        {
            foreach (Stat stat in Owner.allStats)
            {
                stat.RemoveAllModifiersFromSource(this);
            }
        }

        mod.Clear();
        skillMethod.Invoke();
    }


    //Below is the method for all of the abilities//
    public void ItemFinder()
    {
        Owner.info.DropRateMod.AddModifier(new StatModifier(.2f, StatModType.Flat, this, skill.name));
        
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
                mod.Add(new StatModifier(totalBoost, StatModType.PercentMult, this, skill.name));

                Owner.info.Defense.AddModifier(mod[0]);
                break;
            }

        }
    }







    public void CancelSkill()
    {

    }
}
