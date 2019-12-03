using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemUpgrade : MonoBehaviour
{
    public GameObject item, item2, item3, itemScroll, activeItemSprite, equipmentObject, upgradeMenu, yourItemBase;
    public Equipment activeEquip;
    public TMP_Text nameText, levelText, expText;
    public Slider expSlider;
    public List<GameObject> upgradeOptions = new List<GameObject>();
    public List<int> upgradeTributes = new List<int>();
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    

    public void ActiveEquipment(Equipment equip)
    {
        upgradeMenu.SetActive(true);
        activeEquip = equip;
        
        //activeEquip.GetExpCurve();
        activeItemSprite.GetComponent<EquipmentObject>().equipment = activeEquip;
        activeEquip.SetInventorySlot(activeEquip.inventorySlot);
        activeItemSprite.GetComponent<Button>().GetComponent<Image>().color = Color.white;
        activeItemSprite.GetComponent<Button>().GetComponent<Image>().sprite = activeEquip.equipment.sprite;
        activeEquip.equipment.ActivateItem(activeEquip.equipment, activeItemSprite);

        nameText.text = activeEquip.itemName;
        levelText.text = "Lv: " + activeEquip.level;
        
        
        
        if (activeEquip.expToLevel.ContainsKey(activeEquip.level) && activeEquip.level < activeEquip.levelMax)
        {
            activeEquip.toNextLevel = activeEquip.expToLevel[activeEquip.level + 1];
            activeEquip.totalNextLevel = activeEquip.totalExpForLevel[activeEquip.level + 1];
            activeEquip.nextLevelDiff = activeEquip.totalNextLevel - activeEquip.exp;

            Debug.Log(activeEquip.nextLevelDiff);

            expSlider.maxValue = activeEquip.toNextLevel;
            expSlider.value = activeEquip.toNextLevel - activeEquip.nextLevelDiff;


            expText.text = "EXP Until Level Up: " + activeEquip.nextLevelDiff.ToString();
        }
        else if (activeEquip.level == activeEquip.levelMax)
        {
            activeEquip.toNextLevel = 0;
            activeEquip.totalNextLevel = 0;
            activeEquip.nextLevelDiff = 0;

            levelText.text = "Lv: " + activeEquip.level;
            expSlider.maxValue = activeEquip.toNextLevel;
            expSlider.value = activeEquip.toNextLevel - activeEquip.nextLevelDiff;
            expText.text = "Level Max!";
        }

        upgradeOptions.Clear();


    }

    public void DisplayEquipment()
    {
        var yourEquips = GameManager.Instance.Inventory.EquipmentPocket.items;
        var allEquips = GameManager.Instance.items.allEquipsDict;

        GameObject[] options = GameObject.FindGameObjectsWithTag("Respawn");

        for (int i = 0; i < options.Length; i++)
        {
            Destroy(options[i]);
        }

        upgradeOptions.Clear();
        
        for (int i = 0; i < yourEquips.Count; i++)
        {
            //EquipmentScript e = Instantiate(allEquips[yourEquips[i].itemName]);
            PocketItem e = yourEquips[i];


            //makes sure the active equipment can't select itself as a possible upgrade option
            if (e.slotIndex != activeEquip.inventorySlot.slotIndex && !upgradeTributes.Contains(i))
            {

                //checks your equipment for other items of the same name and level
                //if (e.itemName == activeEquip.inventorySlot.itemName && e.itemLevel == activeEquip.level)
                //{

                    
                    //activeEquip.inventorySlot = e;
                    var option = Instantiate(yourItemBase, new Vector3(yourItemBase.transform.position.x, yourItemBase.transform.position.y - (upgradeOptions.Count * (yourItemBase.GetComponent<RectTransform>().rect.height * yourItemBase.transform.localScale.y)), yourItemBase.transform.position.z), Quaternion.identity);
                    option.gameObject.SetActive(true);
                    option.transform.tag = "Respawn";
                    option.transform.SetParent(itemScroll.transform, true);
                    option.transform.localScale = new Vector3(1f, 1f, 1f);
                    option.GetComponent<EquipmentObject>().equipment = new Equipment(allEquips[e.itemName]);
                    option.GetComponent<EquipmentObject>().equipment.SetInventorySlot(e);
                    option.GetComponent<Button>().GetComponent<Image>().color = Color.white;
                    option.GetComponent<Button>().GetComponent<Image>().sprite = allEquips[e.itemName].sprite;
                    option.GetComponent<EquipmentObject>().equipment.equipment.ActivateItem(allEquips[e.itemName], option.gameObject);
                    option.name = e.itemName + "(" + activeEquip.inventorySlot.itemLevel + ")";

                    upgradeOptions.Add(option);
                //}

            }
        }
    }


    public void SelectEquipment(EquipmentObject obj)
    {
        upgradeTributes.Add(obj.equipment.inventorySlot.slotIndex);
        item.GetComponent<Button>().GetComponent<Image>().color = Color.white;
        item.GetComponent<Button>().GetComponent<Image>().sprite = obj.equipment.equipment.sprite;
        obj.equipment.equipment.ActivateItem(obj.equipment.equipment, item.gameObject);
        item.name = obj.equipment.itemName + "(" + activeEquip.inventorySlot.itemLevel + ")";

        CheckEXP(obj.equipment.expGiven);
        DisplayEquipment();
    }

    public void CheckEXP(int exp)
    {
        activeEquip.exp += exp;

        if (activeEquip.expToLevel.ContainsKey(activeEquip.level) && activeEquip.level < activeEquip.levelMax)
        {
            activeEquip.toNextLevel = activeEquip.expToLevel[activeEquip.level + 1];
            activeEquip.totalNextLevel = activeEquip.totalExpForLevel[activeEquip.level + 1];
            activeEquip.nextLevelDiff = activeEquip.totalNextLevel - activeEquip.exp;



            if (exp >= activeEquip.nextLevelDiff)
            {
                activeEquip.level += 1;

                SetExp();
                
            }
        }
        else if (activeEquip.level == activeEquip.levelMax)
        {
            activeEquip.toNextLevel = 0;
            activeEquip.totalNextLevel = 0;
            activeEquip.nextLevelDiff = 0;

            levelText.text = "Lv: " + activeEquip.level;
            expSlider.maxValue = activeEquip.toNextLevel;
            expSlider.value = activeEquip.toNextLevel - activeEquip.nextLevelDiff;
            expText.text = "Level Max!";
        }

    }

    public void SetExp()
    {
        if (activeEquip.expToLevel.ContainsKey(activeEquip.level) && activeEquip.level < activeEquip.levelMax)
        {
            activeEquip.toNextLevel = activeEquip.expToLevel[activeEquip.level + 1];
            activeEquip.totalNextLevel = activeEquip.totalExpForLevel[activeEquip.level + 1];
            activeEquip.nextLevelDiff = activeEquip.totalNextLevel - activeEquip.exp;

            levelText.text = "Lv: " + activeEquip.level;
            expSlider.maxValue = activeEquip.toNextLevel;
            expSlider.value = activeEquip.toNextLevel - activeEquip.nextLevelDiff;

            if (activeEquip.nextLevelDiff >= activeEquip.toNextLevel)
            {

                activeEquip.level += 1;
                SetExp();

            }

            expText.text = "EXP Until Level Up: " + activeEquip.nextLevelDiff.ToString();
        }
        else if (activeEquip.level == activeEquip.levelMax)
        {
            activeEquip.toNextLevel = 0;
            activeEquip.totalNextLevel = 0;
            activeEquip.nextLevelDiff = 0;

            levelText.text = "Lv: " + activeEquip.level;
            expSlider.maxValue = activeEquip.toNextLevel;
            expSlider.value = activeEquip.toNextLevel - activeEquip.nextLevelDiff;
            expText.text = "Level Max!";
        }
    }

    public void ConfirmUpgrade()
    {
        var yourEquips = GameManager.Instance.Inventory.EquipmentPocket.items;
        activeEquip.inventorySlot.itemExp = activeEquip.exp;
        activeEquip.inventorySlot.itemLevel = activeEquip.level;
        activeEquip.SetInventorySlot(activeEquip.inventorySlot);

        foreach (int upgrades in upgradeTributes)
        {
            GameManager.Instance.Inventory.RemoveEquipment(yourEquips[upgrades]);
        }

        upgradeTributes.Clear();
        item.GetComponent<Button>().GetComponent<Image>().color = Color.clear;
        item.GetComponent<Button>().GetComponent<Image>().sprite = null;
        GameManager.Instance.Inventory.SaveInventory();
        ActiveEquipment(activeEquip);
    }

    
}
