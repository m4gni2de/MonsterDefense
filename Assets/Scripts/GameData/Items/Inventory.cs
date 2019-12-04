using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
//class for each type of item in your inventory
public class Pocket
{
    public string pocketName;
    public ItemType pocketType;
    public int slotMax;
    public int slotCount;
    public List<PocketItem> items;
}

[Serializable]
//each pocket item can only have one item type. if Pocket = Equipment, then only an EquipmentScript can exist in the PocketItem
public class PocketItem
{
    public int slotIndex;
    public string itemName;
    public int itemLevel;
    public int itemExp;
    public string pocketName;
}




public class Inventory : MonoBehaviour
{
    public List<Pocket> AllPockets = new List<Pocket>();

    public Pocket EquipmentPocket = new Pocket
    {
        pocketName = "Equipment",
        pocketType = ItemType.Equipment,
        slotMax = 100,
        slotCount = 0,
        items = new List<PocketItem>(),
    };

    public Pocket ConsumablePocket = new Pocket
    {
        pocketName = "Consumables",
        pocketType = ItemType.Consumable,
        slotMax = 100,
        slotCount = 0,
        items = new List<PocketItem>(),
    };

    public Pocket CellPocket = new Pocket
    {
        pocketName = "Cells",
        pocketType = ItemType.Cell,
        slotMax = 100,
        slotCount = 0,
        items = new List<PocketItem>(),
    };

    private void Awake()
    {
        AllPockets.Add(EquipmentPocket);
        AllPockets.Add(ConsumablePocket);
        AllPockets.Add(CellPocket);

        LoadInventory();
    }

    public void LoadInventory()
    {
        for (int i = 0; i < AllPockets.Count; i++)
        {

            if (PlayerPrefs.HasKey(AllPockets[i].pocketName))
            {
                string inventory = PlayerPrefs.GetString(AllPockets[i].pocketName.ToString());
                var info = JsonUtility.FromJson<Pocket>(inventory);
                AllPockets[i] = info;


            }
        }

        EquipmentPocket = AllPockets[0];
        ConsumablePocket = AllPockets[1];
        CellPocket = AllPockets[2];

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    public void AddPocketItem(PocketItem item, int quantity)
    {
        if (item.pocketName == "Equipment")
        {
            AddEquipment(item, quantity);
            return;
        }

        if (item.pocketName == "Consumables")
        {
            AddConsumable(item, quantity);
            return;
        }

        if (item.pocketName == "Cells")
        {
            AddCell(item, quantity);
            return;
        }
    }

   

    public void AddEquipment(PocketItem item, int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            
            item.slotIndex = EquipmentPocket.items.Count + i;
            EquipmentPocket.items.Add(item);
            EquipmentPocket.slotCount += 1;
        }


        SaveInventory();
    }

    public void AddConsumable(PocketItem item, int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            item.slotIndex = ConsumablePocket.items.Count + i;
            ConsumablePocket.items.Add(item);
            ConsumablePocket.slotCount += 1;
        }

        SaveInventory();
    }

    public void AddCell(PocketItem item, int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            item.slotIndex = ConsumablePocket.items.Count + i;
            CellPocket.items.Add(item);
            CellPocket.slotCount += 1;
        }

        SaveInventory();
    }


    public void RemoveEquipment(PocketItem item)
    {
        EquipmentPocket.items.Remove(item);
        EquipmentPocket.slotCount = EquipmentPocket.items.Count;
        int index = item.slotIndex;

        for(int i = 0; i < EquipmentPocket.slotCount; i++)
        {
            if (EquipmentPocket.items[i].slotIndex > index)
            {
                EquipmentPocket.items[i].slotIndex -= 1;
            }
        }


        SaveInventory();
    }

    public void RemoveConsumable(PocketItem item)
    {
        ConsumablePocket.items.Remove(item);
        ConsumablePocket.slotCount = ConsumablePocket.items.Count;

        int index = item.slotIndex;

        for (int i = 0; i < ConsumablePocket.slotCount; i++)
        {
            if (ConsumablePocket.items[i].slotIndex > index)
            {
                ConsumablePocket.items[i].slotIndex -= 1;
            }
        }

        SaveInventory();
    }


    public void SaveInventory()
    {
        foreach (Pocket pocket in AllPockets)
        {
            PlayerPrefs.SetString(pocket.pocketName, JsonUtility.ToJson(pocket));
        }
    }
}
