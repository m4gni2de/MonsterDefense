using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;



public struct EquipmentInfo
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

public class Equipment
{
    public EquipmentScript equipment;
    public EquipmentInfo info = new EquipmentInfo();

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

    //the gameobject that the item spawns upon
    public GameObject GameObject;

    public Equipment()
    {
       
    }

    public void SetEquipment(EquipmentScript equip)
    {
        foreach(FieldInfo f in this.GetType().GetFields())
        {
            foreach(FieldInfo fi in equip.GetType().GetFields())
            {
                if (f.Name == fi.Name)
                {
                    f.SetValue(equip, fi.GetValue(equip));

                    Debug.Log(f.GetValue(this));
                }
            }
        }
    }
    
    public void Equip()
    {

    }
}
