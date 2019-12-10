using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//this script is used to display pop menu boxes 
public class PopMenuObject : MonoBehaviour
{
    public TMP_Text menuText, titleText, secondaryText;

    //set an active monster to this object so that it can display information about that monster
    public Monster activeMonster;
    public Equipment activeEquipment;
    public ConsumableItem activeItem;

    public GameObject equipUpgrade;

    public GameObject upgradeButton;

    // Start is called before the first frame update
    void Start()
    {
        menuText.GetComponent<TMP_Text>();
        titleText.GetComponent<TMP_Text>();
        secondaryText.GetComponent<TMP_Text>();

       

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void FillBox(string desc)
    {
        
        menuText.text = desc;
    }

    //use this to display equipment information
    public void AcceptEquipment(Equipment equip)
    {
        upgradeButton.SetActive(true);
        upgradeButton.GetComponent<Button>().onClick.RemoveAllListeners();
        upgradeButton.GetComponent<Button>().onClick.AddListener(delegate { UpgradeEquipment(); });
        upgradeButton.GetComponent<Button>().GetComponentInChildren<TMP_Text>().text = "Upgrade";

        titleText.text = equip.itemName;
        menuText.text = equip.equipment.description;
        secondaryText.text = "";

        activeEquipment = equip;
        
    }

    //use this to display status information
    public void AcceptStatus(Status status)
    {
        upgradeButton.SetActive(false);
        

        titleText.text = status.name;
        menuText.text = status.description;
        secondaryText.text = "";
    }

    //use this to display status information
    public void AcceptType(TypeInfo type)
    {
        upgradeButton.SetActive(false);
        

        titleText.text = "Type";
        menuText.text = type.name;
        secondaryText.text = "";
    }

    //use this to display status information
    public void AcceptAttackMode(string mode)
    {
        upgradeButton.SetActive(false);
        

        titleText.text = "Attack Mode";
        menuText.text = mode;
        secondaryText.text = "";
    }

    //use this to look at an item in your inventory
    public void AcceptItem(PocketItem item)
    {
        var allItems = GameManager.Instance.items.allConsumablesDict;
        var yourItems = GameManager.Instance.Inventory.ConsumablePocket.items;

        upgradeButton.SetActive(true);
        upgradeButton.GetComponent<Button>().onClick.RemoveAllListeners();
        upgradeButton.GetComponent<Button>().onClick.AddListener(delegate { UseItem(); });
        upgradeButton.GetComponent<Button>().GetComponentInChildren<TMP_Text>().text = "Use Item";

        activeItem = Instantiate(allItems[item.itemName]);
        activeItem.inventorySlot = item;
        

        titleText.text = activeItem.itemName;
        menuText.text = activeItem.description;
        secondaryText.text = "";

        
       
    }




    //invoke this from the button on the PopMenu to trigger the equipment upgrade menu
    public void UpgradeEquipment()
    {
        //equipUpgrade = GameManager.Instance.activeScene.GetRootGameObjects
        GameObject[] items = GameManager.Instance.activeScene.GetRootGameObjects();
        foreach (GameObject item in items)
        {
            if (item.name == "HomeObject")
            {
                item.GetComponentInChildren<ItemUpgrade>().ActiveEquipment(activeEquipment);
                gameObject.SetActive(false);
                return;
            }
        }
        
        //equipUpgrade.gameObject.SetActive(true);
        //itemUpgrade.GetComponent<ItemUpgrade>().ActiveEquipment(activeEquipment);
    }

    //invoke this from the button on GameManager's pop menu to use an item
    public void UseItem()
    {
        
        GameObject[] items = GameManager.Instance.activeScene.GetRootGameObjects();

        foreach (GameObject item in items)
        {
            if (item.name == "HomeObject")
            {
                item.GetComponent<YourHome>().itemsListObject.SetActive(true);
                item.GetComponentInChildren<ItemManager>().UseItem();
                CloseWindow();
                return;
            }
        }
    }

    //invoke this from the button on the PopMenu to trigger the closing of this window
    public void CloseWindow()
    {
        activeMonster = null;
        gameObject.SetActive(false);
    }




}
