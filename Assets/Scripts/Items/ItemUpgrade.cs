using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class ItemUpgrade : MonoBehaviour
{
    public GameObject item, item2, item3, itemScroll, activeItemSprite, equipmentObject, upgradeMenu, yourItemBase, equipmentMenu;
    public Equipment activeEquip, tempEquip;
    public TMP_Text nameText, levelText, expText;
    public Slider expSlider;
    public List<GameObject> upgradeOptions = new List<GameObject>();
    public Dictionary<int, int> upgradeTributes = new Dictionary<int, int>();
    public Button xButton;

    //text boxes to hold the possible different number of boosts an equipment can have
    public TMP_Text[] boostTexts;

    public GameObject[] itemButtons;

    //the number of the button being pushed
    public int buttonNumber;

    public Dictionary<string, float> itemBoosts = new Dictionary<string, float>();

    private void Awake()
    {
        for (int i = 0; i < itemButtons.Length; i++)
        {
            itemButtons[i].GetComponent<Button>().onClick.AddListener(delegate { DisplayEquipment(); });

        }
    }
    // give each button an onclick that corresponds with the button they are
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    

    public void ActiveEquipment(Equipment equip)
    {
        
        equipmentMenu.SetActive(false);
        upgradeMenu.SetActive(true);
        activeEquip = equip;
        tempEquip = new Equipment(activeEquip.equipment);
        
        //tempEquip.GetExpCurve();
        activeItemSprite.GetComponent<EquipmentObject>().equipment = tempEquip;
        tempEquip.SetInventorySlot(activeEquip.inventorySlot);
        UpdateItemStats();
        activeItemSprite.GetComponent<Button>().GetComponent<Image>().color = Color.white;
        activeItemSprite.GetComponent<Button>().GetComponent<Image>().sprite = tempEquip.equipment.sprite;
        tempEquip.equipment.ActivateItem(tempEquip.equipment, activeItemSprite);

        nameText.text = tempEquip.itemName;
        levelText.text = "Lv: " + tempEquip.level;

        

       

        
        

        if (tempEquip.expToLevel.ContainsKey(tempEquip.level) && tempEquip.level < tempEquip.levelMax)
        {
            tempEquip.toNextLevel = tempEquip.expToLevel[tempEquip.level + 1];
            tempEquip.totalNextLevel = tempEquip.totalExpForLevel[tempEquip.level + 1];
            tempEquip.nextLevelDiff = tempEquip.totalNextLevel - tempEquip.exp;

            Debug.Log(tempEquip.nextLevelDiff);

            expSlider.maxValue = tempEquip.toNextLevel;
            expSlider.value = tempEquip.toNextLevel - tempEquip.nextLevelDiff;


            expText.text = "EXP Until Level Up: " + tempEquip.nextLevelDiff.ToString();
        }
        else if (tempEquip.level == tempEquip.levelMax)
        {
            tempEquip.toNextLevel = 0;
            tempEquip.totalNextLevel = 0;
            tempEquip.nextLevelDiff = 0;

            levelText.text = "Lv: " + tempEquip.level;
            expSlider.maxValue = tempEquip.toNextLevel;
            expSlider.value = tempEquip.toNextLevel - tempEquip.nextLevelDiff;
            expText.text = "Level Max!";
        }

        upgradeOptions.Clear();


    }

    
    //this is called when you click on a button to add an item to be sacrificed
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
            if (e.slotIndex != activeEquip.inventorySlot.slotIndex && !upgradeTributes.ContainsKey(i))
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
                    option.GetComponentInChildren<TMP_Text>().text = option.GetComponent<EquipmentObject>().equipment.level.ToString();
                    option.name = e.itemName + "(" + option.GetComponent<EquipmentObject>().equipment.level + ")";

                    upgradeOptions.Add(option);
                //}

            }
        }
    }


    //from the list of selected equipment, the one clicked on is added to the corresponded spot in the item tributes
    public void SelectEquipment(EquipmentObject obj)
    {
        if (itemButtons[buttonNumber].GetComponent<Button>().GetComponent<Image>().sprite == null)
        {
            upgradeTributes.Add(obj.equipment.inventorySlot.slotIndex, obj.equipment.expGiven);
            itemButtons[buttonNumber].GetComponent<Button>().GetComponent<Image>().color = Color.white;
            itemButtons[buttonNumber].GetComponent<Button>().GetComponent<Image>().sprite = obj.equipment.equipment.sprite;
            obj.equipment.equipment.ActivateItem(obj.equipment.equipment, itemButtons[buttonNumber].gameObject);
            itemButtons[buttonNumber].name = obj.equipment.itemName + "(" + activeEquip.inventorySlot.itemLevel + ")";
            itemButtons[buttonNumber].GetComponentInChildren<TMP_Text>().text = "Lv. " + obj.equipment.level;

            CheckEXP(obj.equipment.expGiven);
            DisplayEquipment();

            itemButtons[buttonNumber].GetComponent<Button>().onClick.RemoveListener(delegate { DisplayEquipment(); });
            itemButtons[buttonNumber].GetComponent<Button>().onClick.AddListener(delegate { RemoveTribute(obj); });
        }
        
        

    }

    //adds exp to the item, then checks to see if that added exp will level up the item or not
    public void CheckEXP(int exp)
    {
        tempEquip.exp += exp;

        if (tempEquip.expToLevel.ContainsKey(tempEquip.level) && tempEquip.level < tempEquip.levelMax)
        {
            tempEquip.toNextLevel = tempEquip.expToLevel[tempEquip.level + 1];
            tempEquip.totalNextLevel = tempEquip.totalExpForLevel[tempEquip.level + 1];
            tempEquip.nextLevelDiff = tempEquip.totalNextLevel - tempEquip.exp;

            levelText.text = "Lv: " + tempEquip.level;
            expSlider.maxValue = tempEquip.toNextLevel;
            expSlider.value = tempEquip.toNextLevel - tempEquip.nextLevelDiff;

            if (exp >= tempEquip.nextLevelDiff)
            {
                tempEquip.level += 1;
                SetExp();
                
            }

            expText.text = "EXP Until Level Up: " + tempEquip.nextLevelDiff.ToString();
        }
        else if (tempEquip.level == tempEquip.levelMax)
        {
            tempEquip.toNextLevel = 0;
            tempEquip.totalNextLevel = 0;
            tempEquip.nextLevelDiff = 0;

            levelText.text = "Lv: " + tempEquip.level;
            expSlider.maxValue = tempEquip.toNextLevel;
            expSlider.value = tempEquip.toNextLevel - tempEquip.nextLevelDiff;
            expText.text = "Level Max!";
        }


        UpdateItemStats();

    }

    //if the item levels up, calculate the new level up here
    public void SetExp()
    {
        if (tempEquip.expToLevel.ContainsKey(tempEquip.level) && tempEquip.level < tempEquip.levelMax)
        {
            tempEquip.toNextLevel = tempEquip.expToLevel[tempEquip.level + 1];
            tempEquip.totalNextLevel = tempEquip.totalExpForLevel[tempEquip.level + 1];
            tempEquip.nextLevelDiff = tempEquip.totalNextLevel - tempEquip.exp;

            levelText.text = "Lv: " + tempEquip.level;
            expSlider.maxValue = tempEquip.toNextLevel;
            expSlider.value = tempEquip.toNextLevel - tempEquip.nextLevelDiff;

            if (tempEquip.nextLevelDiff <= 0)
            {

                tempEquip.level += 1;
                UpdateItemStats();
                SetExp();
                
            }

            expText.text = "EXP Until Level Up: " + tempEquip.nextLevelDiff.ToString();
        }
        else if (tempEquip.level == tempEquip.levelMax)
        {
            tempEquip.toNextLevel = 0;
            tempEquip.totalNextLevel = 0;
            tempEquip.nextLevelDiff = 0;

            levelText.text = "Lv: " + tempEquip.level;
            expSlider.maxValue = tempEquip.toNextLevel;
            expSlider.value = tempEquip.toNextLevel - tempEquip.nextLevelDiff;
            expText.text = "Level Max!";
        }

        
    }

    //confirm the upgrade. all items used for the upgrade are removed from your inventory
    public void ConfirmUpgrade()
    {
        var yourEquips = GameManager.Instance.Inventory.EquipmentPocket.items;
        activeEquip.inventorySlot.itemExp = tempEquip.exp;
        activeEquip.inventorySlot.itemLevel = tempEquip.level;
        activeEquip.SetInventorySlot(activeEquip.inventorySlot);

        foreach (KeyValuePair<int, int> upgrades in upgradeTributes)
        {
            GameManager.Instance.Inventory.RemoveEquipment(yourEquips[upgrades.Key]);
        }

        upgradeTributes.Clear();

        for (int i = 0; i < itemButtons.Length; i++)
        {
            //Debug.Log(i);
            //itemButtons[i].GetComponent<Button>().onClick.RemoveListener(delegate { RemoveTribute(obj); });
            //itemButtons[i].GetComponent<Button>().onClick.AddListener(delegate { DisplayEquipment(); });
            itemButtons[i].GetComponent<Button>().GetComponent<Image>().color = Color.white;
            itemButtons[i].GetComponent<Button>().GetComponent<Image>().sprite = null;
            itemButtons[i].GetComponentInChildren<TMP_Text>().text = "";
        }
        GameManager.Instance.Inventory.SaveInventory();
        ActiveEquipment(activeEquip);
        
    }

    //use this to update the stat text boxes of an item so you can see what it's stats will be upon upgrade
    public void UpdateItemStats()
    {
        itemBoosts.Clear();

        
        tempEquip.GetStats();

        for (int i = 0; i < boostTexts.Length; i++)
        {
            boostTexts[i].text = "";
        }

        int boostCount = 0;


        //get the actual boosts that this item has, and put them in to a new dictionary just for this item
        foreach (KeyValuePair<string, float> boost in tempEquip.statBoosts)
        {
            if (boost.Value != 0)
            {
                itemBoosts.Add(boost.Key, (float)boost.Value);
                if (boost.Value < 1)
                {
                    float value = boost.Value * 100;
                    boostTexts[boostCount].text = boost.Key + "  +  " + value + "%";
                }
                else
                {
                    boostTexts[boostCount].text = boost.Key + "  +  " + boost.Value;
                }

                boostCount += 1;
            }
        }
    }

    //when a button is clicked with this method, set this script's button number to that same number
    public void GetButtonNumber(int b)
    {
        buttonNumber = b;
    }
    
    void RemoveTribute(EquipmentObject obj)
    {
        var yourEquips = GameManager.Instance.Inventory.EquipmentPocket.items;


        upgradeTributes.Remove(obj.equipment.inventorySlot.slotIndex);
        itemButtons[buttonNumber].GetComponent<Button>().GetComponent<Image>().color = Color.white;
        itemButtons[buttonNumber].GetComponent<Button>().GetComponent<Image>().sprite = null;
        obj.equipment.equipment.DeactivateItem(obj.equipment.equipment, itemButtons[buttonNumber].gameObject);
        itemButtons[buttonNumber].name = "equip" + buttonNumber + 1;
        itemButtons[buttonNumber].GetComponentInChildren<TMP_Text>().text = "";

        //make a new copy of the equipment being upgraded, then re-add all of the items already as a tribute so you can remove the exp the removed tribute was giving
        tempEquip = new Equipment(tempEquip.equipment);
        tempEquip.SetInventorySlot(activeEquip.inventorySlot);
        int newExp = 0;

        foreach (KeyValuePair<int, int> tribute in upgradeTributes)
        {
            newExp += tribute.Value;
        }

        CheckEXP(newExp);
        DisplayEquipment();

        itemButtons[buttonNumber].GetComponent<Button>().onClick.RemoveListener(delegate { RemoveTribute(obj); });
        itemButtons[buttonNumber].GetComponent<Button>().onClick.AddListener(delegate { DisplayEquipment(); });
    }

    public void CloseMenu()
    {
        GameObject[] options = GameObject.FindGameObjectsWithTag("Respawn");

        for (int i = 0; i < options.Length; i++)
        {
            Destroy(options[i]);
        }

        for (int i = 0; i < boostTexts.Length; i++)
        {
            boostTexts[i].text = "";
        }

        for (int i = 0; i < itemButtons.Length; i++)
        {
            //itemButtons[buttonNumber].GetComponent<Button>().onClick.RemoveListener(delegate { RemoveTribute(obj); });
            itemButtons[i].GetComponent<Button>().onClick.AddListener(delegate { DisplayEquipment(); });
            itemButtons[i].GetComponent<Button>().GetComponent<Image>().color = Color.white;
            itemButtons[i].GetComponent<Button>().GetComponent<Image>().sprite = null;
            itemButtons[i].GetComponentInChildren<TMP_Text>().text = "";
        }

        equipmentMenu.gameObject.SetActive(true);
        equipmentMenu.GetComponent<EquipmentManager>().CloseEquipment();
        upgradeMenu.SetActive(false);
    }
}

    

