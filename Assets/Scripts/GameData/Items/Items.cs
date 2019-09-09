using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public enum EquipmentType
{
    Rune,
}

public enum ItemType
{
    Equipment,
    Consumable, 
    Cell
}


public struct AllItem
{
    public string name;
    public ItemType itemType;
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
    public string[] boosts;
    public float cost;
    public int equipSlot;

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

    public float hpPercentBonus;
    public float atkPercentBonus;
    public float defPercentBonus;
    public float spePercentBonus;
    public float precPercentBonus;
    public float atkPowerPercentBonus;
    public float atkTimePercentBonus;
    public float evasionPercentBonus;
    
    
};


public struct Consumable
{
    public string name;
    public int id;
    public string description;
    public float cost;

};

public struct MonsterCell
{
    public string name;
    public string hostMonster;
    public int id;
    public string description;
    public float cost;

};

[System.Serializable]
public class ItemsRoot
{
    public Equipment Equipment;
    public Consumable Consumable;
    public MonsterCell MonsterCell;
}

[System.Serializable]
public class AllEquipment 
{
    public Equipment AquaRune = new Equipment
    {
        name = "Aqua Rune",
        id = 1,
        description = "A Rune that boosts Water Type attacks from Water Type monsters by 8%",
        equipType = EquipmentType.Rune,
        typeMonsterReq = "Water",
        typeMoveReq = "Water",
        boosts = new string[1] { "Water Type Attacks + 8% Power"},
        atkPowerPercentBonus = .08f,
        cost = 200,
        
        

    };

    public Equipment ThunderRune = new Equipment
    {
        name = "Thunder Rune",
        id = 2,
        description = "A Rune that boosts an Electric Type monster's attack stat by 10%",
        equipType = EquipmentType.Rune,
        boosts = new string[1] { "+10% Attack" },
        typeMonsterReq = "Electric",
        atkPercentBonus = .10f,
        cost = 200,
        

    };

    public Equipment NatureRune = new Equipment
    {
        name = "Nature Rune",
        id = 3,
        description = "A Rune that boosts a Nature Type monster's attack stat by 10%",
        equipType = EquipmentType.Rune,
        boosts = new string[1] { "+10% Attack" },
        typeMonsterReq = "Nature",
        atkPercentBonus = .10f,
        cost = 200,
        

    };

    public Equipment MagicRune = new Equipment
    {
        name = "Magic Rune",
        id = 4,
        description = "A Rune that boosts a Magic Type monster's attack stat by 10%",
        equipType = EquipmentType.Rune,
        boosts = new string[1] { "+10% Attack" },
        typeMonsterReq = "Magic",
        atkPercentBonus = .10f,
        cost = 200,
        

    };

    public Equipment ShadowRune = new Equipment
    {
        name = "Shadow Rune",
        id = 5,
        description = "A Rune that boosts a Shadow Type monster's attack stat by 10%",
        equipType = EquipmentType.Rune,
        boosts = new string[1] { "+10% Attack" },
        typeMonsterReq = "Shadow",
        atkPercentBonus = .10f,
        cost = 200,
        

    };
}


public class AllItems
{
    public AllItem AquaRune = new AllItem
    {
        name = "Aqua Rune",
        itemType = ItemType.Equipment,
    };

    public AllItem ThunderRune = new AllItem
    {
        name = "Thunder Rune",
        itemType = ItemType.Equipment,
    };

    public AllItem NatureRune = new AllItem
    {
        name = "Nature Rune",
        itemType = ItemType.Equipment,
    };

    public AllItem MagicRune = new AllItem
    {
        name = "Magic Rune",
        itemType = ItemType.Equipment,
    };

    public AllItem ShadowRune = new AllItem
    {
        name = "Shadow Rune",
        itemType = ItemType.Equipment,
    };

    public AllItem ExpBoost = new AllItem
    {
        name = "Exp Boost",
        itemType = ItemType.Consumable,
    };

    public AllItem LichenthropeCell = new AllItem
    {
        name = "Lichenthrope Cell",
        itemType = ItemType.Cell,
    };
}


public class AllConsumables
{
    public Consumable ExpBoost = new Consumable
    {
        name = "Exp Boost",
        id = 1,
        description = "The monster that consumes this gains an extra 20% EXP when it defeats an enemy for the remainder of this stage."

    };
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
    public AllConsumables allConsumables = new AllConsumables();
    public AllMonsterCells allMonsterCells = new AllMonsterCells();
    public AllItems allItems = new AllItems();


    public Dictionary<string, AllItem> allItemsDict = new Dictionary<string, AllItem>();

    
    public Dictionary<string, Equipment> allEquipmentDict = new Dictionary<string, Equipment>();
    public Dictionary<string, Consumable> allConsumablesDict = new Dictionary<string, Consumable>();
    public Dictionary<string, MonsterCell> allMonsterCellsDict = new Dictionary<string, MonsterCell>();

    public List<string> equipmentList = new List<string>();
    public List<string> consumableList = new List<string>();
    public List<string> cellList = new List<string>();

    private void Awake()
    {
        //equipEffects = GetComponent<EquipEffects>();

        AddItems();
        AddEquipment();
        AddConsumables();
        AddCells();
        
    }

   
    
    
   //creates dictionary for all items together
   void AddItems()
    {
        allItemsDict.Add(allItems.AquaRune.name, allItems.AquaRune);
        allItemsDict.Add(allItems.ThunderRune.name, allItems.ThunderRune);
        allItemsDict.Add(allItems.NatureRune.name, allItems.NatureRune);
        allItemsDict.Add(allItems.MagicRune.name, allItems.MagicRune);
        allItemsDict.Add(allItems.ShadowRune.name, allItems.MagicRune);
        allItemsDict.Add(allItems.ExpBoost.name, allItems.ExpBoost);
        allItemsDict.Add(allItems.LichenthropeCell.name, allItems.LichenthropeCell);


        foreach (KeyValuePair<string, AllItem> item in allItemsDict)
        {
            if (item.Value.itemType == ItemType.Equipment)
            {
                equipmentList.Add(item.Key);
            }

            if (item.Value.itemType == ItemType.Consumable)
            {
                consumableList.Add(item.Key);
            }

            if (item.Value.itemType == ItemType.Cell)
            {
                cellList.Add(item.Key);
            }

        }
    }

    
    void AddEquipment()
    {
        allEquipmentDict.Add(allEquipment.AquaRune.name, allEquipment.AquaRune);
        allEquipmentDict.Add(allEquipment.ThunderRune.name, allEquipment.ThunderRune);
        allEquipmentDict.Add(allEquipment.NatureRune.name, allEquipment.NatureRune);
        allEquipmentDict.Add(allEquipment.MagicRune.name, allEquipment.MagicRune);
        allEquipmentDict.Add(allEquipment.ShadowRune.name, allEquipment.ShadowRune);


    }


    void AddConsumables()
    {
        allConsumablesDict.Add(allConsumables.ExpBoost.name, allConsumables.ExpBoost);
    }

    void AddCells()
    {
        allMonsterCellsDict.Add(allMonsterCells.LichenthropeCell.hostMonster, allMonsterCells.LichenthropeCell);
       
    }

}
