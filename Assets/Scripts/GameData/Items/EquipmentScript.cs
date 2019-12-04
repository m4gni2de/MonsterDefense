using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;


public enum EquipmentClass
{
    Rune, 
    Axe, 
    Glove, 
    Ring,
    Tome,
}

//to tell if the equipment's boost values can change
public enum EquipBoostVariance
{
    Static,
    Dynamic,
}

//if a sprite component is to be added, choose it from here
public enum EquipmentSpriteEffect
{
    None,
    ShinyReflectFX,
    PlasmaShieldFX,
    Wood2D,
    Hologram, 
    Hologram2, 
    Hologram3,
    Ice,
}


[System.Serializable]
public struct EquipmentInformation
{
    public int equipSlot;
    public int quantity;

    public int equipLevel;
    public int equipExp;
    public int equipLevelMax;

    //used to determine if the equipment item is in your inventory or equipped to a monster;
    public bool isEquipped;
    //used to determine is this item has been upgraded. if it has, it is considered a different item rather than a copy of an original
    public bool isUpgraded;
    public Monster equippedMonster;
    public List<string> boosts;
    public PocketItem inventorySlot;

    public int triggerCount;
    
}

[CreateAssetMenu]
public class EquipmentScript : ScriptableObject
{
    public string itemName;
    public int id;
    public string description;
    public Sprite sprite;
    public EquipmentClass equipClass;
    public string typeMonsterReq;
    public string typeMoveReq;
    public DamageForce forceReq;
    public AttackMode attackModeReq;
    public string[] boosts;
    public float cost;
    public EquipBoostVariance variance;

    //if the item can receive an event trigger
    public TriggerType triggerType;
    public EventTrigger trigger;

    public ItemRarity rarity;
    public float expConstant;
    public int equipLevelMax;
    public int level;
    public int expGiven;
    


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

    //these variables exist to affect the possible sprite effects that can be added
    public EquipmentSpriteEffect spriteEffect;
    //use these variables to change the properties of the sprite effect
    public float _Alpha;
    public float _TimeX;
    public Color _ColorX;
    public float Speed;
    public float Distortion;

    
   

    //the gameobject that the item spawns upon
    public GameObject GameObject;

   

    public EquipmentScript()
    {
        


        //info.equipLevel = level;
        //info.equipLevelMax = equipLevelMax;


        //expToLevel.Clear();
        //totalExpForLevel.Clear();
        //GetExpCurve();

        
    }

   
    

    ////used to equip the attached monster with this item
    //public void EquipItem()
    //{
    //    equip.Equip(info.equippedMonster, info.equipSlot, this);
    //}



    public void UnEquip()
    {
        //equip.Unequip(info.equippedMonster);
        


       
    }

  


    //use this to give a gameobject's renderer that this object is spawned on to the correct properties
    public void ActivateItem(EquipmentScript eq, GameObject g)
    {  
       

        //if the equipment has a type to be added, add it on the sprite
        if (eq.spriteEffect != EquipmentSpriteEffect.None)
        {

            g.AddComponent(Type.GetType(eq.spriteEffect.ToString()));
            Component comp = g.GetComponent(Type.GetType(eq.spriteEffect.ToString()));


            //checks the variable values for this equipment, and then make the values for the added component equal to the values
            foreach (FieldInfo fi in comp.GetType().GetFields())
            {
                object obj = (System.Object)comp;

                if (fi.Name == "_ColorX")
                {
                    fi.SetValue(obj, _ColorX);
                }

                if (fi.Name == "Distortion")
                {
                    fi.SetValue(obj, Distortion);
                }

                if (fi.Name == "Speed")
                {
                    fi.SetValue(obj, Speed);
                }
            }




        }
    }


    //remove the properties on the gameobject for this equipment
    public void DeactivateItem(EquipmentScript e, GameObject g)
    {
        //Debug.Log(e.itemName);

        //if the equipment has a type to be added, add it on the sprite
        if (e.spriteEffect != EquipmentSpriteEffect.None)
        {
            if (g.GetComponent(Type.GetType(e.spriteEffect.ToString(), true)))
            {
                Destroy(g.GetComponent(Type.GetType(e.spriteEffect.ToString())));
            }
            
        }
    }

   
}

[System.Serializable]
//the class that combines the EquipmentScript scriptable object with the effects from the item itself
public class Equipment
{
    public EquipmentScript equipment;
    public Monster monster;
    public int Slot;
    public PocketItem inventorySlot = new PocketItem();

    //this variable is used to delegate which item method to use, given the name of the item
    public delegate void EquipmentDelegate();
    public EquipmentDelegate equipMethod;

    public int level;
    public int exp;
    public int levelMax;
    public string itemName;
    public int expGiven;
    public float cost;

    public int toNextLevel, totalNextLevel, nextLevelDiff;
    //used to determine if the equipment item is in your inventory or equipped to a monster;
    public bool isEquipped;

    public EquipmentClass equipClass;
    public string typeMonsterReq;
    public string typeMoveReq;


    public Dictionary<int, int> expToLevel = new Dictionary<int, int>();
    public Dictionary<int, int> totalExpForLevel = new Dictionary<int, int>();


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

    //a dictionary of the item's stat boosts
    public Dictionary<string, float> statBoosts = new Dictionary<string, float>();

    //these variables exist to affect the possible sprite effects that can be added
    public EquipmentSpriteEffect spriteEffect;
    //use these variables to change the properties of the sprite effect
    public float alpha;
    public float timeX;
    public Color colorX;
    public float Speed;
    public float Distortion;

    public Equipment(EquipmentScript e)
    {
        
        equipment = e;

        hpBonus = e.hpBonus;
        atkBonus = e.atkBonus;
        defBonus = e.defBonus;
        speedBonus = e.speedBonus;
        precBonus = e.precBonus;
        atkPowerBonus = e.atkPowerBonus;
        atkTimeBonus = e.atkTimeBonus;
        atkRangeBonus = e.atkRangeBonus;
        critModBonus = e.critModBonus;
        critChanceBonus = e.critChanceBonus;
        staminaBonus = e.staminaBonus;

        hpPercentBonus = e.hpPercentBonus;
        atkPercentBonus = e.atkPercentBonus;
        defPercentBonus = e.defPercentBonus;
        spePercentBonus = e.spePercentBonus;
        atkPowerPercentBonus = e.atkPowerPercentBonus;
        atkTimePercentBonus = e.atkTimePercentBonus;
        evasionPercentBonus = e.evasionPercentBonus;
        staminaPercentBonus = e.staminaPercentBonus;

        level = e.level;
        levelMax = e.equipLevelMax;
        itemName = e.itemName;
        expGiven = e.expGiven;

        alpha = e._Alpha;
        timeX = e._TimeX;
        colorX = e._ColorX;
        Speed = e.Speed;
        Distortion = e.Distortion;

        equipClass = e.equipClass;
        typeMonsterReq = e.typeMonsterReq;
        typeMoveReq = e.typeMoveReq;

        cost = e.cost;


        


        GetExpCurve(); 
        
    }

    //create the method delegate for each item
    EquipmentDelegate DelegateCreation(object target, string functionName)
    {
        EquipmentDelegate eq = (EquipmentDelegate)Delegate.CreateDelegate(typeof(EquipmentDelegate), target, functionName);
        return eq;
    }

   

    public void Equip(Monster Monster, int slot)
    {

        monster = Monster;
        Slot = slot;

        
        //get string of the name of the item
        string name = string.Concat(itemName.Where(c => !char.IsWhiteSpace(c)));

        //convert string to a delegate to call the method of the name of the ability
        equipMethod = DelegateCreation(this, name);

        if (Slot == 1)
        {
            monster.info.equip1 = this;
        }
        else if (Slot == 2)
        {
            monster.info.equip2 = this;
        }

        isEquipped = true;

        
        //SetInventorySlot(inventorySlot);

        equipMethod.Invoke();
        //refresh the monster's list of stat modifiers
        monster.MonsterStatMods();
    }

    public void UnEquip()
    {
        monster.info.HP.RemoveAllModifiersFromSource(this);
        monster.info.Attack.RemoveAllModifiersFromSource(this);
        monster.info.Defense.RemoveAllModifiersFromSource(this);
        monster.info.Speed.RemoveAllModifiersFromSource(this);
        monster.info.Precision.RemoveAllModifiersFromSource(this);
        monster.info.Stamina.RemoveAllModifiersFromSource(this);
        monster.info.EnergyGeneration.RemoveAllModifiersFromSource(this);
        monster.info.EnergyCost.RemoveAllModifiersFromSource(this);
        monster.info.CoinGeneration.RemoveAllModifiersFromSource(this);



        monster.info.baseAttack1.Power.RemoveAllModifiersFromSource(this);
        monster.info.baseAttack1.Range.RemoveAllModifiersFromSource(this);
        monster.info.baseAttack1.AttackSpeed.RemoveAllModifiersFromSource(this);
        monster.info.baseAttack1.AttackTime.RemoveAllModifiersFromSource(this);
        monster.info.baseAttack1.CritChance.RemoveAllModifiersFromSource(this);
        monster.info.baseAttack1.CritMod.RemoveAllModifiersFromSource(this);
        monster.info.baseAttack1.EffectChance.RemoveAllModifiersFromSource(this);

        monster.info.baseAttack2.Power.RemoveAllModifiersFromSource(this);
        monster.info.baseAttack2.Range.RemoveAllModifiersFromSource(this);
        monster.info.baseAttack2.AttackSpeed.RemoveAllModifiersFromSource(this);
        monster.info.baseAttack2.AttackTime.RemoveAllModifiersFromSource(this);
        monster.info.baseAttack2.CritChance.RemoveAllModifiersFromSource(this);
        monster.info.baseAttack2.CritMod.RemoveAllModifiersFromSource(this);
        monster.info.baseAttack2.EffectChance.RemoveAllModifiersFromSource(this);

        //ScriptableObject.Destroy(equipment);

        
        isEquipped = false;
        monster.MonsterStatMods();
    }


    //if an equipment item has dynamic stats, call this to trigger its stat change
    public void TriggerEvent()
    {
        UnEquip();
        Equip(monster, Slot);
    }

    public void GetExpCurve()
    {
        int totalExp = new int();



        for (int i = 1; i <= levelMax; i++)
        {
            if (i == 1)
            {

                expToLevel.Add(i, 0);
                totalExpForLevel.Add(i, 0);


            }
            else
            {
                toNextLevel = Mathf.FloorToInt(Mathf.Pow(i, equipment.expConstant));
                expToLevel.Add(i, (int)Mathf.Round(toNextLevel));
                totalExp += (int)Mathf.Round(toNextLevel);
                totalExpForLevel.Add(i, totalExp);
            }



            if (i >= levelMax)
            {

                SetExp();
            }
        }

    }

    public void SetExp()
    {
        if (expToLevel.ContainsKey(level))
        {
            toNextLevel = expToLevel[level + 1];
            totalNextLevel = totalExpForLevel[level + 1];
            nextLevelDiff = totalNextLevel - exp;
        }
    }

    ////this is called from the ItemUpgrade script
    //public void GainEXP(int expGained)
    //{
    //    exp += (int)Mathf.Round(expGained);
    //    if (expToLevel.ContainsKey(level))
    //    {
    //        toNextLevel = expToLevel[level + 1];
    //        totalNextLevel = totalExpForLevel[level + 1];
    //        nextLevelDiff = totalNextLevel - exp;


    //        if (expGained >= nextLevelDiff)
    //        {
    //            OnLevelUp();

    //            return;
    //        }  
    //    }
    //}

    //public void OnLevelUp()
    //{
    //    level += 1;
    //    inventorySlot.itemExp = exp;
    //    inventorySlot.itemLevel = level;
        
    //}

    public void NatureRune()
    {
        if (monster.info.type1 == "Nature" || monster.info.type2 == "Nature")
        {
            monster.info.Attack.AddModifier(new StatModifier(atkPercentBonus, StatModType.PercentMult, this, itemName));
        }


    }

    public void WaterRune()
    {
        if (monster.info.type1 == "Water" || monster.info.type2 == "Water")
        {
            monster.info.Attack.AddModifier(new StatModifier(atkPercentBonus, StatModType.PercentMult, this, itemName));
        }
    }

    public void ShadowRune()
    {
        if (monster.info.type1 == "Shadow" || monster.info.type2 == "Shadow")
        {
            monster.info.Attack.AddModifier(new StatModifier(atkPercentBonus, StatModType.PercentMult, this, itemName));
        }
    }

    public void FireRune()
    {
        if (monster.info.type1 == "Fire" || monster.info.type2 == "Fire")
        {
            monster.info.Attack.AddModifier(new StatModifier(atkPercentBonus, StatModType.PercentMult, this, itemName));
        }
    }

    public void MechanicalRune()
    {
        if (monster.info.type1 == "Mechanical" || monster.info.type2 == "Mechanical")
        {
            monster.info.Attack.AddModifier(new StatModifier(atkPercentBonus, StatModType.PercentMult, this, itemName));
        }
    }

    public void MagicRune()
    {
        if (monster.info.type1 == "Magic" || monster.info.type2 == "Magic")
        {
            monster.info.Attack.AddModifier(new StatModifier(atkPercentBonus, StatModType.PercentMult, this, itemName));
        }
    }

    public void IceRune()
    {

        if (monster.info.type1 == "Ice" || monster.info.type2 == "Ice")
        {
            monster.info.Attack.AddModifier(new StatModifier(atkPercentBonus, StatModType.PercentMult, this, itemName));

        }
    }

    public void ThunderRune()
    {
        if (monster.info.type1 == "Electric" || monster.info.type2 == "Electric")
        {
            monster.info.Attack.AddModifier(new StatModifier(atkPercentBonus, StatModType.PercentMult, this, itemName));
        }
    }

    public void NormalRune()
    {
        if (monster.info.type1 == "Normal" || monster.info.type2 == "Normal")
        {
            monster.info.Attack.AddModifier(new StatModifier(atkPercentBonus, StatModType.PercentMult, this, itemName));
        }
    }

    public void WoodAxe()
    {
        monster.info.baseAttack1.Power.AddModifier(new StatModifier(atkPowerBonus, StatModType.Flat, this, itemName));
        monster.info.baseAttack2.Power.AddModifier(new StatModifier(atkPowerBonus, StatModType.Flat, this, itemName));
    }

    public void SpikedKnuckles()
    {
        if (monster.info.baseAttack1.attack.attackMode == AttackMode.Punch)
        {
            monster.info.baseAttack1.Power.AddModifier(new StatModifier(atkPowerPercentBonus, StatModType.PercentMult, this, itemName));
        }

        if (monster.info.baseAttack2.attack.attackMode == AttackMode.Punch)
        {
            monster.info.baseAttack2.Power.AddModifier(new StatModifier(atkPowerPercentBonus, StatModType.PercentMult, this, itemName));
        }
    }

    public void RingOfFluctuation()
    {
        monster.info.Speed.AddModifier(new StatModifier(spePercentBonus, StatModType.PercentMult, this, itemName));
    }


    public void FrostAxe()
    {
        var tiles = GameManager.Instance.activeTiles;
        int total = 0;
        int tileCount = 0;

        foreach (KeyValuePair<int, MapTile> tile in tiles)
        {
            if (tile.Value.tileAtt == TileAttribute.Ice)
            {
                total += atkBonus;
            }

            tileCount += 1;

            if (tileCount >= tiles.Count)
            {
                monster.info.Attack.AddModifier(new StatModifier(total, StatModType.Flat, this, itemName));
                break;
            }

        }


    }

    public void TomeOfTheBlade()
    {
        monster.info.Attack.AddModifier(new StatModifier(atkPercentBonus, StatModType.PercentMult, this, itemName));
        monster.info.Precision.AddModifier(new StatModifier(precPercentBonus, StatModType.PercentMult, this, itemName));
    }



    public void SetInventorySlot(PocketItem e)
    {
        
        inventorySlot = e;
        exp = e.itemExp;
        level = e.itemLevel;

        
    }

    public void GetStats()
    {
        
        statBoosts.Clear();

        EquipmentLevelStats Stats = new EquipmentLevelStats(this, level);

    }


    public void AddToInventory(int quantity)
    {
        PocketItem item = new PocketItem();
        item.itemExp = exp;
        item.itemLevel = level;
        item.pocketName = "Equipment";
        item.itemName = itemName;
        item.slotIndex = GameManager.Instance.Inventory.EquipmentPocket.items.Count;
        inventorySlot = item;
        
 
        GameManager.Instance.Inventory.AddEquipment(inventorySlot, quantity);
    }

    public void RemoveFromInventory()
    {
        GameManager.Instance.Inventory.RemoveEquipment(inventorySlot);
    }


}


public class EquipmentLevelStats
{
    public Equipment Equipment;
    public int level;

    public EquipmentLevelStats(Equipment e, int l)
    {
        Equipment = e;
        level = l;

        if (Equipment.hpBonus != 0)
        {
            Equipment.hpBonus = Equipment.equipment.hpBonus + (int)(5 * (level - 1));
            Equipment.statBoosts.Add("HP", (float)Equipment.hpBonus);
        }
        if (Equipment.atkBonus != 0)
        {
            Equipment.atkBonus = Equipment.equipment.atkBonus + (int)(5 * (level - 1));
            Equipment.statBoosts.Add("Attack", (float)Equipment.atkBonus);
        }
        if (Equipment.defBonus != 0)
        {
            Equipment.defBonus = Equipment.equipment.defBonus + (int)(5 * (level - 1));
            Equipment.statBoosts.Add("HP", (float)Equipment.defBonus);
        }
        if (Equipment.speedBonus != 0)
        {
            Equipment.speedBonus = Equipment.equipment.speedBonus + (int)(5 * (level - 1));
            Equipment.statBoosts.Add("Speed", (float)Equipment.speedBonus);
        }
        if (Equipment.precBonus != 0)
        {
            Equipment.precBonus = Equipment.equipment.precBonus + (int)(5 * (level - 1));
            Equipment.statBoosts.Add("Precision", (float)Equipment.precBonus);
        }
        if (Equipment.atkPowerBonus != 0)
        {
            Equipment.atkPowerBonus = Equipment.equipment.atkPowerBonus + (int)(5 * (level - 1));
            Equipment.statBoosts.Add("Attack Power", (float)Equipment.atkPowerBonus);
        }

        if (Equipment.atkTimeBonus != 0)
        {
            Equipment.atkTimeBonus = Equipment.equipment.atkTimeBonus + (int)(5 * (level - 1));
            Equipment.statBoosts.Add("Attack Time", (float)Equipment.atkTimeBonus);
        }
        if (Equipment.atkRangeBonus != 0)
        {
            Equipment.atkRangeBonus = Equipment.equipment.atkRangeBonus + (int)(5 * (level - 1));
            Equipment.statBoosts.Add("Attack Range", (float)Equipment.atkRangeBonus);
        }
        if (Equipment.critModBonus != 0)
        {
            Equipment.critModBonus = Equipment.equipment.critModBonus + (int)(5 * (level - 1));
            Equipment.statBoosts.Add("Critical Hit Multiplier", (float)Equipment.critModBonus);
        }
        if (Equipment.critChanceBonus != 0)
        {
            Equipment.critChanceBonus = Equipment.equipment.critChanceBonus + (int)(5 * (level - 1));
            Equipment.statBoosts.Add("Critical Hit Chance", (float)Equipment.critChanceBonus);
        }
        if (Equipment.staminaBonus != 0)
        {
            Equipment.staminaBonus = Equipment.equipment.staminaBonus + (int)(5 * (level - 1));
            Equipment.statBoosts.Add("Stamina", (float)Equipment.staminaBonus);
        }

        if (Equipment.hpPercentBonus != 0)
        {
            Equipment.hpPercentBonus = Equipment.equipment.hpPercentBonus + (float)(.05f * (level - 1));
            Equipment.statBoosts.Add("HP", Equipment.hpPercentBonus);
        }
        if (Equipment.atkPercentBonus != 0)
        {
            Equipment.atkPercentBonus = Equipment.equipment.atkPercentBonus + (float)(.05f * (level - 1));
            Equipment.statBoosts.Add("Attack", Equipment.atkPercentBonus);
        }
        if (Equipment.defPercentBonus != 0)
        {
            Equipment.defPercentBonus = Equipment.equipment.defPercentBonus + (float)(.05f * (level - 1));
            Equipment.statBoosts.Add("Defense", Equipment.defPercentBonus);
        }
        if (Equipment.spePercentBonus != 0)
        {
            Equipment.spePercentBonus = Equipment.equipment.spePercentBonus + (float)(.05f * (level - 1));
            Equipment.statBoosts.Add("Speed", Equipment.spePercentBonus);
        }
        if (Equipment.atkPowerPercentBonus != 0)
        {
            Equipment.atkPowerPercentBonus = Equipment.equipment.atkPowerPercentBonus + (float)(.05f * (level - 1));
            Equipment.statBoosts.Add("Attack Power", Equipment.atkPowerPercentBonus);
        }
        if (Equipment.atkTimePercentBonus != 0)
        {
            Equipment.atkTimePercentBonus = Equipment.equipment.atkTimePercentBonus + (float)(.05f * (level - 1));
            Equipment.statBoosts.Add("Attack Time", Equipment.atkTimeBonus);
        }
        if (Equipment.evasionPercentBonus != 0)
        {
            Equipment.evasionPercentBonus = Equipment.equipment.evasionPercentBonus + (float)(.05f * (level - 1));
            Equipment.statBoosts.Add("Evasion", Equipment.evasionPercentBonus);
        }
        if (Equipment.staminaPercentBonus != 0)
        {
            Equipment.staminaPercentBonus = Equipment.equipment.staminaPercentBonus + (int)(5 * (level - 1));
            Equipment.statBoosts.Add("Stamina", Equipment.staminaPercentBonus);
        }

        Equipment.cost *= 1 + (.2f * (level - 1));

        
        //Debug.Log(Equipment.atkPercentBonus);
    }
}

