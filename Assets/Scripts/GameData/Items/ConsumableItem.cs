using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ConsumableType
{
    Shard,
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
}
