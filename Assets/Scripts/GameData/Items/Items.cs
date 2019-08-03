using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum EquipmentType
{
    Rune,
}
[System.Serializable]
public struct Equipment
{
    public string name;
    public int id;
    public string description;
    public EquipmentType equipType;
    public string typeMonsterReq;
    public string typeMoveReq;
    public string[] boosts;
    public float cost;
    public int equipSlot;

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

    public EquippableItem equip;
    

    



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
        equip = new EquippableItem(),
        

    };

    public Equipment ThunderRune = new Equipment
    {
        name = "Thunder Rune",
        id = 2,
        description = "A Rune that boosts Electric Type monster's attack stat by 6",
        equipType = EquipmentType.Rune,
        boosts = new string[1] { "+6 Attack" },
        typeMonsterReq = "Electric",
        atkBonus = 6,
        equip = new EquippableItem()

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
    public MonsterCell TerrorBiteCell = new MonsterCell
    {
        name = "Terror Bite Cell",
        hostMonster = "TerrorBite",
        id = 1,
        description = "A cell from Terror Bite."

    };
}

public class Items: MonoBehaviour
{
    public GameObject[] equipmentPrefabs;

    public AllEquipment allEquipment = new AllEquipment();
    public AllConsumables allConsumables = new AllConsumables();
    public AllMonsterCells allMonsterCells = new AllMonsterCells();

    public Dictionary<string, GameObject> equipmentByPrefab = new Dictionary<string, GameObject>();
    public Dictionary<string, Equipment> allEquipmentDict = new Dictionary<string, Equipment>();


    public Dictionary<string, Consumable> allConsumablesDict = new Dictionary<string, Consumable>();
    public Dictionary<string, MonsterCell> allMonsterCellsDict = new Dictionary<string, MonsterCell>();

    private void Awake()
    {
        //equipEffects = GetComponent<EquipEffects>();

        
        AddEquipment();
        AddConsumables();
        AddCells();
    }
    
    
   

    void AddEquipment()
    {
        allEquipmentDict.Add(allEquipment.AquaRune.name, allEquipment.AquaRune);
        allEquipmentDict.Add(allEquipment.ThunderRune.name, allEquipment.ThunderRune);

        equipmentByPrefab.Add(allEquipment.AquaRune.name, equipmentPrefabs[0]);
        equipmentByPrefab.Add(allEquipment.ThunderRune.name, equipmentPrefabs[1]);
    }


    void AddConsumables()
    {
        allConsumablesDict.Add(allConsumables.ExpBoost.name, allConsumables.ExpBoost);
    }

    void AddCells()
    {
        allMonsterCellsDict.Add(allMonsterCells.TerrorBiteCell.hostMonster, allMonsterCells.TerrorBiteCell);
       
    }

}
