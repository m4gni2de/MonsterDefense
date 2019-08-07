using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShop : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var allEquips = GameManager.Instance.items.allEquipmentDict;
        var allCells = GameManager.Instance.items.allMonsterCellsDict;
        var allConsumables = GameManager.Instance.items.allConsumablesDict;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
