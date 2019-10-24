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

    [HideInInspector] public EquipmentScript equipment;


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
        }

        
    }


    //trigger for when an enemy is KOd
    public void EnemyKO()
    {
       if (equipment != null)
        {
            //equipment.info.equippedMonster.UnEquipItem(equipment, equipment.info.equipSlot);
            //equipment.UnEquip();
            //equipment.EquipItem();
            //equipment.info.triggerCount += 1;

            equipment.TriggerEvent();
        }

        
    }

    //trigger for when a tile or tiles change on the map
    public void TileChange()
    {

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
}
