using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YourItems : MonoBehaviour
{
    public Dictionary<string, int> yourItemsDict = new Dictionary<string, int>();
    public Dictionary<int, string> equipIds = new Dictionary<int, string>();


    // Start is called before the first frame update
    void Start()
    {
        GetYourItems();
    }


    public void GetYourItems()
    {
        yourItemsDict.Clear();

        var items = GameManager.Instance.GetComponent<Items>().allEquipmentDict;
        
        
        int i = 1;

        //makes a dictionary of all of the equipment items and their id's
        foreach (KeyValuePair<string, Equipment> equips in items)
        {
            equipIds.Add(equips.Value.id, equips.Key);
            

            if (equipIds.ContainsKey(i))
            {
                if (items.ContainsKey(equipIds[i]))
                {
                    Equipment item = items[equipIds[i]];
                    int itemCount = PlayerPrefs.GetInt(item.name);
                    yourItemsDict.Add(item.name, itemCount);
                }
            }

            i += 1;
        }

       

        
       

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
