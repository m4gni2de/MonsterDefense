using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemUpgrade : MonoBehaviour
{
    public GameObject item, item2, item3, itemScroll, activeItemSprite, equipmentObject, upgradeMenu, yourItemBase;
    public EquipmentScript activeEquip;
    public TMP_Text nameText, levelText, expText;
    public Slider expSlider;
    public List<GameObject> upgradeOptions = new List<GameObject>();
    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActiveEquipment(EquipmentScript equip)
    {
        upgradeMenu.SetActive(true);
        activeEquip = Instantiate(equip);
        activeEquip.GetExpCurve();
        activeItemSprite.GetComponent<EquipmentObject>().equipment = activeEquip;
        activeItemSprite.GetComponent<Button>().GetComponent<Image>().color = Color.white;
        activeItemSprite.GetComponent<Button>().GetComponent<Image>().sprite = activeEquip.sprite;
        activeEquip.ActivateItem(activeEquip, activeItemSprite);

        nameText.text = activeEquip.itemName;
        levelText.text = "Lv: " + equip.info.equipLevel;

        
        
        if (equip.expToLevel.ContainsKey(equip.info.equipLevel))
        {
            int toNextLevel = equip.expToLevel[equip.info.equipLevel + 1];
            int totalNextLevel = equip.totalExpForLevel[equip.info.equipLevel + 1];
            int nextLevelDiff = totalNextLevel - equip.info.equipExp;

            Debug.Log(nextLevelDiff);

            expSlider.maxValue = toNextLevel;
            expSlider.value = toNextLevel - nextLevelDiff;


            expText.text = "EXP Until Level Up: " + nextLevelDiff.ToString();
        }

        

        SelectEquipment(equip);
    }

    public void SelectEquipment(EquipmentScript equip)
    {
        var yourEquips = GameManager.Instance.Inventory.EquipmentPocket.items;
        var allEquips = GameManager.Instance.items.allEquipsDict;

        upgradeOptions.Clear();
        //Debug.Log(yourEquips.Count);
        //checks all of the monsters in your monsters dictionary
        for (int i = 0; i < yourEquips.Count; i++)
        {
            //EquipmentScript e = Instantiate(allEquips[yourEquips[i].itemName]);
            PocketItem e = yourEquips[i];
            

                //makes sure the active monster can't select itself as a possible upgrade option
                if (e.slotIndex != activeEquip.info.inventorySlot.slotIndex)
                {

                    //checks your monsters for other monsters of the same species as the active monster
                    if (e.itemName == activeEquip.info.inventorySlot.itemName && e.itemLevel == activeEquip.info.equipLevel)
                    {

                        EquipmentScript eq = Instantiate(allEquips[e.itemName]);
                        eq.info.inventorySlot = e;
                        var option = Instantiate(yourItemBase, new Vector3(yourItemBase.transform.position.x, yourItemBase.transform.position.y - (upgradeOptions.Count * (yourItemBase.GetComponent<RectTransform>().rect.height * yourItemBase.transform.localScale.y)), yourItemBase.transform.position.z), Quaternion.identity);
                        option.gameObject.SetActive(true);    
                        option.transform.SetParent(itemScroll.transform, true);
                        option.transform.localScale = new Vector3(1f, 1f, 1f);
                        option.GetComponent<EquipmentObject>().equipment = eq;
                        option.GetComponent<Button>().GetComponent<Image>().color = Color.white;
                        option.GetComponent<Button>().GetComponent<Image>().sprite = eq.sprite;
                        eq.ActivateItem(eq, option.gameObject);
                        option.name = e.itemName + "(" + eq.info.inventorySlot.itemLevel + ")";

                        upgradeOptions.Add(option);
                }

            }
        }
    }
}
