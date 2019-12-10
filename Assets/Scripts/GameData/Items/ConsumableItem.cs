using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum ConsumableType
{
    PlayerUse, 
    MonsterUse,
}


[CreateAssetMenu]
public class ConsumableItem : ScriptableObject
{
    public string itemName;
    //public int id;
    public string description;
    public float cost;
    public Sprite sprite;
    public ConsumableType itemType;
    public TileAttribute tileMinedFrom;

    public PocketItem inventorySlot;
    public Monster targetMonster;
    
    //this variable is used to delegate which item method to use, given the name of the item
    public delegate void ItemDelegate();
    public ItemDelegate itemMethod;

    public void SetTarget(Monster monster)
    {
        targetMonster = monster;
    }

    public void UseItem()
    {
        //get string of the name of the item
        string name = string.Concat(itemName.Where(c => !char.IsWhiteSpace(c)));

        //convert string to a delegate to call the method of the name of the ability
        itemMethod = DelegateCreation(this, name);
        itemMethod.Invoke();
        
    }

    //create the method delegate for each item
    ItemDelegate DelegateCreation(object target, string functionName)
    {
        ItemDelegate it = (ItemDelegate)Delegate.CreateDelegate(typeof(ItemDelegate), target, functionName);
        return it;
    }


    public void AddToInventory(int quantity)
    {
        PocketItem item = new PocketItem();
        item.itemExp = 0;
        item.itemLevel = 0;
        item.itemName = itemName;
        item.pocketName = "Consumables";

        inventorySlot = item;

        GameManager.Instance.Inventory.AddConsumable(item, quantity);
    }

    public void RemoveFromInventory()
    {
        GameManager.Instance.Inventory.RemoveConsumable(inventorySlot);
    }

   



    public void NatureShard()
    {
        Summon summon = new Summon("Nature");
        summon.MonsterSummon();
    }

    public void MechanicalShard()
    {
        Summon summon = new Summon("Mechanical");
        summon.MonsterSummon();
    }

    public void ShadowShard()
    {
        Summon summon = new Summon("Shadow");
        summon.MonsterSummon();
    }

    public void FireShard()
    {
        Summon summon = new Summon("Fire");
        summon.MonsterSummon();
    }

    public void IceShard()
    {
        Summon summon = new Summon("Ice");
        summon.MonsterSummon();
    }

    public void StarCharm()
    {
        targetMonster.info.isStar = true;
        targetMonster.UseItem();
    }
}

//public class Consumable
//{
//    public ConsumableItem itemData;

//    public string itemName;
//    //public int id;
//    public string description;
//    public float cost;
//    public Sprite sprite;
//    public ConsumableType itemType;
//    public TileAttribute tileMinedFrom;

//    public PocketItem inventorySlot;
//}
