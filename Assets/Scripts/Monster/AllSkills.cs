using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class PassiveSkill 
{
    public string name;
    public string description;
    public TriggerType triggerType;
    public EventTrigger trigger;
    public SkillEffects effects;
}


public class Skills
{
    public PassiveSkill ItemFinder = new PassiveSkill
    {
        name = "Item Finder",
        description = "Increases the chances of an item being dropped by an Enemy that this monster destroys by 20%."
        
    };

    public PassiveSkill TerrifyingGaze = new PassiveSkill
    {
        name = "Terrifying Gaze",
        description = "Cuts all monsters' Speed by 25%."
    };

    public PassiveSkill NaturalArmor = new PassiveSkill
    {
        name = "Natural Armor",
        description = "Raises this monster's defense by 3% for each Nature Tile on the field.",
        triggerType = TriggerType.TileChange,
    };
}

public class AllSkills: MonoBehaviour
{
    public Dictionary<string, PassiveSkill> allSkillsDict = new Dictionary<string, PassiveSkill>();
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

public class SkillEffects
{
    public PassiveSkill Skill;
    public Monster Owner;

    //this variable is used to delegate which ability method to use, given the name of the ability
    public delegate void SkillDelegate();
    public SkillDelegate skillMethod;

    public AllSkills allSkills;

    
    public int triggerCount;
    

    //keep track of the stat modifiers, if any, that a skill gives
    public List<StatModifier> mod = new List<StatModifier>();


    public SkillEffects(PassiveSkill skill, Monster owner)
    {
        Skill = skill;
        Owner = owner;
        Skill.effects = this;

        ActivateSkill();
    }

   
    public void ActivateSkill()
    {
        //get string of the name of the ability
        string name = string.Concat(Skill.name.Where(c => !char.IsWhiteSpace(c)));

        //convert string to a delegate to call the method of the name of the ability
        skillMethod = DelegateCreation(this, name);
        skillMethod.Invoke();
    }

    //create the method delegate for each ability
    SkillDelegate DelegateCreation(object target, string functionName)
    {
        SkillDelegate ab = (SkillDelegate)Delegate.CreateDelegate(typeof(SkillDelegate), target, functionName);
        //GameManager.Instance.SendNotificationToPlayer(Skill.name, 1, NotificationType.AbilityReady, Owner.info.species);
        return ab;
    }

    public void TriggerEvent()
    {
        RemovePassive();
        ActivateSkill();
    }


    public void RemovePassive()
    {
        foreach(StatModifier statMod in mod)
        {
            foreach(Stat stat in Owner.allStats)
            {
                stat.RemoveAllModifiersFromSource(this);
            }
        }

        mod.Clear();
    }

    //Below is the method for all of the abilities//
    public void ItemFinder()
    {
        Owner.info.DropRateMod.AddModifier(new StatModifier(.2f, StatModType.Flat, this, Skill.name));
        
    }

    public void TerrifyingGaze()
    {
        mod.Add(new StatModifier(-.5f, StatModType.PercentMult, this, Skill.name));
        GlobalStat terrifyingGaze = new GlobalStat(mod[0], "Speed", Owner, false, Skill.name, GlobalStatModType.Enemies);
        GameManager.Instance.activeMap.AddGlobalStat(terrifyingGaze);
        Owner.ownedGlobalStats.Add(terrifyingGaze);

    }

    public void NaturalArmor()
    {

        var tiles = GameManager.Instance.activeTiles;
        int total = 0;
        int tileCount = 0;

        foreach (KeyValuePair<int, MapTile> tile in tiles)
        {
            if (tile.Value.tileAtt == TileAttribute.Water)
            {
                total += 1;
            }

            tileCount += 1;

            if (tileCount >= tiles.Count)
            {
                float totalBoost = total * .03f;
                mod.Add(new StatModifier(totalBoost, StatModType.PercentAdd, this, Skill.name));
                if (Owner.isEnemy)
                {
                    Owner.GetComponent<Enemy>().stats.Defense.AddModifier(mod[0]);
                }
                else
                {
                    Owner.info.Defense.AddModifier(mod[0]);
                }
                
                break;
            }

        }
    }







    public void CancelSkill()
    {

    }
}
