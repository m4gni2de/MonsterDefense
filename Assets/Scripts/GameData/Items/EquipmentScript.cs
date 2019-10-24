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

    //used to determine if the equipment item is in your inventory or equipped to a monster;
    public bool isEquipped;
    public Monster equippedMonster;
    public List<string> boosts;

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
    public int equipLevelMax;

    


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

    //public EquipManager equip = new EquipManager();
    //public EquipmentInformation info = new EquipmentInformation();
    public EquipManager equip;
    public EquipmentInformation info;

    //the gameobject that the item spawns upon
    public GameObject GameObject;

    
    public EquipmentScript()
    {
        equip = new EquipManager();
    }

    //use this when the equipment is equipped to a monster
    public void GetEquipInfo(Monster Monster, int EquipSlot)
    {
        
        
        info.equipSlot = EquipSlot;
        info.equippedMonster = Monster;

        info.isEquipped = true;

        EquipItem();


        
    }

    

    //used to equip the attached monster with this item
    public void EquipItem()
    {
        equip.Equip(info.equippedMonster, info.equipSlot, this);
    }



    public void UnEquip()
    {
        equip.Unequip(info.equippedMonster);
        


       
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
        

        //if the equipment has a type to be added, add it on the sprite
        if (e.spriteEffect != EquipmentSpriteEffect.None)
        {
            if (g.GetComponent(Type.GetType(e.spriteEffect.ToString(), true)))
            {
                Destroy(g.GetComponent(Type.GetType(e.spriteEffect.ToString())));
            }
            
        }
    }

    //if an equipment item has dynamic stats, call this to trigger its stat change
    public void TriggerEvent()
    {
        //UnEquip();
        info.triggerCount += 1;
        EquipItem();
    }
}


public class EquipManager
{
    public MonsterAttack attack1;
    public MonsterAttack attack2;

    public Monster monster;

    public EquipmentScript equipment;
    public int Slot;


    


    public void Equip(Monster Monster, int slot, EquipmentScript equip)
    {
        monster = Monster;
        Slot = slot;
        equipment = equip;

        EquipEffect effect = new EquipEffect(monster, equipment, Slot);

        if (slot == 1)
        {
            monster.info.equipment1 = equip;
        }
        else if (slot == 2)
        {
            monster.info.equipment2 = equip;
        }
        

    }


    




    public void EquipmentStatChanges()
    {
        if (equipment.hpBonus != 0)
            monster.info.HP.AddModifier(new StatModifier(equipment.hpBonus, StatModType.Flat, this, equipment.name));
        if (equipment.atkBonus != 0)
            monster.info.Attack.AddModifier(new StatModifier(equipment.atkBonus, StatModType.Flat, this, equipment.name));
        if (equipment.defBonus != 0)
            monster.info.Defense.AddModifier(new StatModifier(equipment.defBonus, StatModType.Flat, this, equipment.name));
        if (equipment.speedBonus != 0)
            monster.info.Speed.AddModifier(new StatModifier(equipment.speedBonus, StatModType.Flat, this, equipment.name));

        if (equipment.hpPercentBonus != 0)
            monster.info.HP.AddModifier(new StatModifier(equipment.hpPercentBonus, StatModType.PercentMult, this, equipment.name));
        if (equipment.atkPercentBonus != 0)
            monster.info.Attack.AddModifier(new StatModifier(equipment.atkPercentBonus, StatModType.PercentMult, this, equipment.name));
        if (equipment.defPercentBonus != 0)
            monster.info.Defense.AddModifier(new StatModifier(equipment.defPercentBonus, StatModType.PercentMult, this, equipment.name));
        if (equipment.spePercentBonus != 0)
            monster.info.Speed.AddModifier(new StatModifier(equipment.spePercentBonus, StatModType.PercentMult, this, equipment.name));


    }


    //invoke this to remove all of the stat boosts when unequipping an equipment item
    public void Unequip(Monster monster)
    {
        monster.info.HP.RemoveAllModifiersFromSource(this);
        monster.info.Attack.RemoveAllModifiersFromSource(this);
        monster.info.Defense.RemoveAllModifiersFromSource(this);
        monster.info.Speed.RemoveAllModifiersFromSource(this);
        monster.info.Precision.RemoveAllModifiersFromSource(this);


        monster.info.attack1.Power.RemoveAllModifiersFromSource(this);
        monster.info.attack1.Range.RemoveAllModifiersFromSource(this);
        monster.info.attack1.AttackSpeed.RemoveAllModifiersFromSource(this);
        monster.info.attack1.AttackTime.RemoveAllModifiersFromSource(this);
        monster.info.attack1.CritChance.RemoveAllModifiersFromSource(this);
        monster.info.attack1.CritMod.RemoveAllModifiersFromSource(this);
        monster.info.attack1.EffectChance.RemoveAllModifiersFromSource(this);

        monster.info.attack2.Power.RemoveAllModifiersFromSource(this);
        monster.info.attack2.Range.RemoveAllModifiersFromSource(this);
        monster.info.attack2.AttackSpeed.RemoveAllModifiersFromSource(this);
        monster.info.attack2.AttackTime.RemoveAllModifiersFromSource(this);
        monster.info.attack2.CritChance.RemoveAllModifiersFromSource(this);
        monster.info.attack2.CritMod.RemoveAllModifiersFromSource(this);
        monster.info.attack2.EffectChance.RemoveAllModifiersFromSource(this);

        //monster.UnEquipItem(equip, slot);
    }
}


