using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YourItems : MonoBehaviour
{
    

    //make a Dictionary of all of your items
    public Dictionary<string, int> yourInventory = new Dictionary<string, int>();
    public Dictionary<string, int> yourEquipment = new Dictionary<string, int>();
    public Dictionary<string, int> yourConsumables = new Dictionary<string, int>();


    // Start is called before the first frame update
    void Start()
    {
        GetYourItems();

        
    }


    public void GetYourItems()
    {
        yourInventory.Clear();
        yourEquipment.Clear();
        yourConsumables.Clear();

        //var equips = GameManager.Instance.items.allEquipmentDict;
        var equips = GameManager.Instance.items.allEquipsDict;
        var consumables = GameManager.Instance.items.allConsumablesDict;
        var allItems = GameManager.Instance.items.fullItemList;



        //loops through all the items in the game, checks them against a playerpref of the same name. if the playerpref exists, then the player has at least 1 of that item. Add those items to a Dictionary of your items
        foreach (KeyValuePair<string, ItemType> item in allItems)
        {

            if (PlayerPrefs.HasKey(item.Key))
            {
                yourInventory.Add(item.Key, PlayerPrefs.GetInt(item.Key));

                if (allItems.ContainsKey(item.Key))
                {
                    //get the type of the item from the all item list
                    ItemType type = allItems[item.Key];

                    //if the item is an equipment, check the all Equipment Dictionary
                    if (type == ItemType.Equipment)
                    {
                        if (equips.ContainsKey(item.Key))
                        {

                            yourEquipment.Add(item.Key, PlayerPrefs.GetInt(item.Key));

                        }
                    }


                    if (type == ItemType.Consumable)
                    {
                        if (consumables.ContainsKey(item.Key))
                        {
                            yourConsumables.Add(item.Key, PlayerPrefs.GetInt(item.Key));
                        }
                    }
                }

                

                

                //Debug.Log(item.Key);
                
              
            }
        }






    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
