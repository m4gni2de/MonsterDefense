using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum TriggerType
{
    None,
    EnemyKO, 
    TileChange,
    TileLevelUp,
    ItemGet,
    LevelUp,
    AbilityUsed,
    TowerSummon,
    WeatherChange,
    GlobalStatMod,
    EnemySpawned,
}

public enum TriggerObject
{
    Item,
    Monster,
    Ability,
    Tile,
    Attack,
}

[System.Serializable]
public class EventTrigger 
{
    public TriggerType triggerType;
    public object triggerObject;
    public int id;

    //all of the possible objects that can have a trigger on them
    //[HideInInspector] public EquipmentScript equipment;
    //[HideInInspector] public Monster monster;
    //[HideInInspector] public PassiveSkill passiveSkill;


    //use this to set the EventTrigger's trigger type
    public EventTrigger(TriggerType type, object obj)
    {
        triggerType = type;
        triggerObject = obj;


        //Debug.Log(obj);
        
    }

    //activate the trigger
    public void ActivateTrigger(TriggerType type)
    {
        


        if (triggerType == type)
        {
            Debug.Log("Type Accepted: " + type + "   This Type: " + triggerType);

            if (triggerType == TriggerType.ItemGet)
            {
                ItemGet();

            }

            if (triggerType == TriggerType.LevelUp)
            {
                LevelUp();
            }

            if (triggerType == TriggerType.EnemyKO)
            {
                EnemyKO();
            }

            if (triggerType == TriggerType.TileChange)
            {
                TileChange();
            }

            if (triggerType == TriggerType.TowerSummon)
            {
                TowerSummon();
            }

            if (triggerType == TriggerType.WeatherChange)
            {
                WeatherChange();
            }

            if (triggerType == TriggerType.GlobalStatMod)
            {
                GlobalStatMod();
            }

            if (triggerType == TriggerType.EnemySpawned)
            {
                EnemySpawned();
            }
        }

        
    }


    //trigger for when an enemy is KOd
    public void EnemyKO()
    {
       //if (equipment != null)
       // {
       //     equipment.TriggerEvent();
       // }

        
    }

    //trigger for when a tile or tiles change on the map
    public void TileChange()
    {
        //if (equipment != null)
        //{
        //    equipment.TriggerEvent();
        //}

        //if (passiveSkill != null)
        //{
        //    passiveSkill.TriggerEvent();
        //}

        
    }

    //trigger for when a tile on the map increases level
    public void TileLevelUp()
    {

    }

    //trigger for when an item is aquired through mining or enemy drop
    public void ItemGet()
    {
       //Debug.Log()
    }

    //trigger for when a monster levels up
    public void LevelUp()
    {

    }

    //trigger for when a monster ability is used
    public void AbilityUsed()
    {

    }

    //trigger for when the weather is changed
    public void WeatherChange()
    {

    }

    //trigger for when a monster is summoned as a tower
    public void TowerSummon()
    {
        //if (equipment != null)
        //{
        //    equipment.TriggerEvent();
        //}
    }

    //trigger for when a global stat mod because active or inactive
    public void GlobalStatMod()
    {
        
    }

    public void EnemySpawned()
    {

    }
}
