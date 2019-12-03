using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public enum EquipmentType
{
    Rune,
    Axe,
    Glove,
    Ring,
    Tome,
}

public enum ItemType
{
    Equipment,
    Consumable, 
    Cell
}

public enum ItemRarity
{
    Common,
    Rare,
    Mythic,
    Legendary,
    Immortal,
}





//[System.Serializable]
//public struct Equipment
//{
//    public string name;
//    public int id;
//    public GameObject equipPrefab;
//    public string description;
//    public EquipmentType equipType;
//    public string typeMonsterReq;
//    public string typeMoveReq;
//    public DamageForce forceReq;
//    public AttackMode attackModeReq;
//    public string[] boosts;
//    public float cost;
//    public int equipSlot;

//    public ItemRarity rarity;
//    public int equipLevel;
//    public int equipLevelMax;
//    public int equipExp;
    
    

//    public int hpBonus;
//    public int atkBonus;
//    public int defBonus;
//    public int speedBonus;
//    public int precBonus;
//    public int atkPowerBonus;
//    public int atkTimeBonus;
//    public int atkRangeBonus;
//    public int critModBonus;
//    public int critChanceBonus;
//    public int staminaBonus;

//    public float hpPercentBonus;
//    public float atkPercentBonus;
//    public float defPercentBonus;
//    public float spePercentBonus;
//    public float precPercentBonus;
//    public float atkPowerPercentBonus;
//    public float atkTimePercentBonus;
//    public float evasionPercentBonus;
//    public int staminaPercentBonus;



    
    
//};


[System.Serializable]
public struct MonsterCell
{
    public string name;
    public string hostMonster;
    public int id;
    public string description;
    public float cost;

    

};





//[System.Serializable]
//public class AllEquipment 
//{
//    public Equipment[] allEquipment;
//}


public class AllMonsterCells
{
    public MonsterCell LichenthropeCell = new MonsterCell
    {
        name = "Lichenthrope Cell",
        hostMonster = "Lichenthrope",
        id = 1,
        description = "A cell from Lichenthrope."

    };
}



public class Items: MonoBehaviour
{
    

    //public AllEquipment allEquipment = new AllEquipment();
    public AllMonsterCells allMonsterCells = new AllMonsterCells();

    //list of equipment that are affected by event triggers
    public List<EquipmentScript> triggerEquips = new List<EquipmentScript>();


    public Dictionary<string, ItemType> fullItemList = new Dictionary<string, ItemType>();

   
    public Dictionary<string, MonsterCell> allMonsterCellsDict = new Dictionary<string, MonsterCell>();


    //use lists to manually add non scriptable objects
    public List<ConsumableItem> consumables = new List<ConsumableItem>();
    //add the consumable items to a dictionary so they can be looked up
    public Dictionary<string, ConsumableItem> allConsumablesDict = new Dictionary<string, ConsumableItem>();
    //use lists to manually add non scriptable objects
    public List<EquipmentScript> equipment = new List<EquipmentScript>();
    public Dictionary<string, EquipmentScript> allEquipsDict = new Dictionary<string, EquipmentScript>();



    private void Awake()
    {
       
        
        
        AddItems();
        AddEquipment();
        AddCells();

        



    }

   
    
    
   //creates dictionary for all items together
   void AddItems()
    {

        
        

        //foreach (KeyValuePair<string, AllItem> item in allItemsDict)
        //{
        //    if (item.Value.itemType == ItemType.Equipment)
        //    {
        //        equipmentList.Add(item.Key);
        //    }



        //    if (item.Value.itemType == ItemType.Cell)
        //    {
        //        cellList.Add(item.Key);
        //    }

        //}

        foreach (ConsumableItem cItem in consumables)
        {
            fullItemList.Add(cItem.itemName, ItemType.Consumable);
            allConsumablesDict.Add(cItem.itemName, cItem);

            //Debug.Log(cItem.itemName);
        }




    }

    
    void AddEquipment()
    {

        foreach (EquipmentScript equip in equipment)
        {
            //add them all to a dictionary of all of the equipment
            allEquipsDict.Add(equip.itemName, equip);

            //give them each an EquipManager and EquipmentInformation
            //equip.equip = new EquipManager();
            //equip.info = new EquipmentInformation();

            //equip.info.equipLevel = equip.level;
            //equip.info.equipLevelMax = equip.equipLevelMax;
            //equip.info.equipLevel = 1;
            //equip.info.equipLevelMax = equip.equipLevelMax;
            //info.equipLevel = level;
            //info.equipLevelMax = equipLevelMax;
            //equip.GetExpCurve();

            //if the item has an event trigger, set the trigger on the item itself
            if (equip.triggerType != TriggerType.None)
            {
                //set their event trigger with their type of trigger
                equip.trigger = new EventTrigger(equip.triggerType, equip);
                //add the item to the list of triggerable equipment
                triggerEquips.Add(equip);
            }
            fullItemList.Add(equip.itemName, ItemType.Equipment);
        }

        
       
        
    }


   

    void AddCells()
    {
        allMonsterCellsDict.Add(allMonsterCells.LichenthropeCell.hostMonster, allMonsterCells.LichenthropeCell);

        
       
    }

}
