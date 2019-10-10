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



[System.Serializable]
public struct Equipment
{
    public string name;
    public int id;
    public GameObject equipPrefab;
    public string description;
    public EquipmentType equipType;
    public string typeMonsterReq;
    public string typeMoveReq;
    public DamageForce forceReq;
    public AttackMode attackModeReq;
    public string[] boosts;
    public float cost;
    public int equipSlot;

    public ItemRarity rarity;
    public int equipLevel;
    public int equipLevelMax;
    public int equipExp;
    
    

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

    public EquippableItem equippable;

    
    
};


[System.Serializable]
public struct MonsterCell
{
    public string name;
    public string hostMonster;
    public int id;
    public string description;
    public float cost;

    

};





[System.Serializable]
public class AllEquipment 
{
    public Equipment[] allEquipment;
}


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
    

    public AllEquipment allEquipment = new AllEquipment();
    public AllMonsterCells allMonsterCells = new AllMonsterCells();
    



    public Dictionary<string, ItemType> fullItemList = new Dictionary<string, ItemType>();

    

    
    public Dictionary<string, Equipment> allEquipmentDict = new Dictionary<string, Equipment>();
    public Dictionary<string, MonsterCell> allMonsterCellsDict = new Dictionary<string, MonsterCell>();

    //public List<string> equipmentList = new List<string>();
    //public List<string> cellList = new List<string>();

    //use lists to manually add non scriptable objects
    public List<ConsumableItem> consumables = new List<ConsumableItem>();

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
            
        }




    }

    
    void AddEquipment()
    {
       

        foreach(Equipment equipment in allEquipment.allEquipment)
        {
            allEquipmentDict.Add(equipment.name, equipment);
            fullItemList.Add(equipment.name, ItemType.Equipment);
        }
       
        
    }


   

    void AddCells()
    {
        allMonsterCellsDict.Add(allMonsterCells.LichenthropeCell.hostMonster, allMonsterCells.LichenthropeCell);

        
       
    }

}
