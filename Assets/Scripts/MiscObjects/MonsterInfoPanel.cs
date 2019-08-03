﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterInfoPanel : MonoBehaviour
{
    public GameObject monsterSprite, equip1, equip2, type1, type2;
    public GameObject equipMenu, equipObject;
    public TMP_Text monsterNameText, levelText, atkText, defText, speText, precText, typeText, toNextLevelText;
    public Slider expSlider;
    

    private GameObject e1, e2;
    public bool isEquip1, isEquip2;
    private Monster monster;


    public Button equip1Btn, equip2Btn, removeEquipBtn;
    // Start is called before the first frame update
    void Start()
    {
        expSlider.GetComponent<Slider>();
        equip1Btn.GetComponent<Button>();
        equip2Btn.GetComponent<Button>();
        
        //deleteButton.GetComponent<Button>();

        
    }

    // Update is called once per frame
    void Update()
    {
        TouchManager();
    }

    public void TouchManager()
    {
        //if (Input.GetMouseButtonDown(0))
        //{

        //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        //    RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        //    //if this tower is clicked on, it can be placed
        //    if (hit.collider != null)
        //    {
        //        if (hit.collider.gameObject.tag == "Equipment")
        //        {
        //            var equipment = hit.collider.gameObject.GetComponent<EquipmentItem>();
        //            GetComponentInParent<YourHome>().activeMonster.GetComponent<Monster>().UnEquipItem(equipment.equip, equipment.equipDetails.equipSlot);
        //            gameObject.SetActive(false);
        //        }
        //    }
        //}
    }

    public void LoadInfo(Monster thisMonster)
    {
        if (monster)
        {
            Destroy(monster.gameObject);
        }


        if (e1)
        {
            Destroy(e1.gameObject);
        }

        if (e2)
        {
            Destroy(e2.gameObject);
        }

        equip1Btn.GetComponent<Image>().sprite = null;
        equip2Btn.GetComponent<Image>().sprite = null;


        monster = Instantiate(thisMonster, transform.position, Quaternion.identity);
        var equips = GameManager.Instance.GetComponent<Items>().equipmentByPrefab;
        var equipment = GameManager.Instance.GetComponent<Items>().allEquipmentDict;
        

        //show the equipment item for Slot 1
        if (equips.ContainsKey(monster.info.equip1Name))
        {
            e1 = Instantiate(equips[monster.info.equip1Name], equipObject.transform.position, Quaternion.identity);
            e1.transform.SetParent(equipObject.transform, false);
            e1.transform.position = new Vector3(-1000f, -1000f, -1000f);
            e1.GetComponent<EquipmentItem>().GetEquipInfo(equipment[thisMonster.info.equip1Name], thisMonster, 1);
            equip1Btn.interactable = false;
            equip1Btn.GetComponent<Image>().sprite = e1.GetComponent<SpriteRenderer>().sprite;
            //Destroy(e1.gameObject);
            isEquip1 = true;
        }
        else
        {
            
            equip1Btn.interactable = true;
            isEquip1 = false;
        }

        //show the equipment item for Slot 2
        if (equips.ContainsKey(monster.info.equip2Name))
        {
            e2 = Instantiate(equips[monster.info.equip2Name], equipObject.transform.position, Quaternion.identity);
            e2.transform.SetParent(equipObject.transform, false);
            e2.transform.position = new Vector3(-1000f, -1000f, -1000f);
            e2.GetComponent<EquipmentItem>().GetEquipInfo(equipment[thisMonster.info.equip2Name], thisMonster, 2);
            equip2Btn.GetComponent<Image>().sprite = e2.GetComponent<SpriteRenderer>().sprite;
            equip2Btn.interactable = false;
            //Destroy(e2.gameObject);
            isEquip2 = true;
        }
        else
        {
            
            equip2Btn.interactable = true;
            isEquip2 = false;
        }

        monster.transform.SetParent(transform, true);
        monster.transform.position = monsterSprite.transform.position;

        //make this monster's sorting layer higher than everything else so it shows up on the panel
        SpriteRenderer[] monsterParts = monster.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < monsterParts.Length; i++)
        {
            monsterParts[i].sortingLayerName = "GameUI";
            monsterParts[i].sortingOrder = monsterParts[i].sortingOrder + transform.GetComponent<SpriteRenderer>().sortingOrder;
        }

        monsterNameText.text = thisMonster.info.name;

        levelText.text = thisMonster.info.level.ToString();
        atkText.text = thisMonster.info.Attack.Value.ToString();
        defText.text = thisMonster.info.Defense.Value.ToString();
        speText.text = thisMonster.info.Speed.Value.ToString();
        precText.text = thisMonster.info.Precision.Value.ToString();
        typeText.text = thisMonster.info.type1 + "/" + thisMonster.info.type2;

        if (thisMonster.expToLevel.ContainsKey(thisMonster.info.level))
        {
            int toNextLevel = thisMonster.expToLevel[thisMonster.info.level + 1];
            int totalNextLevel = thisMonster.totalExpForLevel[thisMonster.info.level + 1];
            int nextLevelDiff = totalNextLevel - thisMonster.info.totalExp;

            expSlider.maxValue = toNextLevel;
            expSlider.value = toNextLevel - nextLevelDiff;

            toNextLevelText.text = "EXP Until Level Up: " + nextLevelDiff.ToString();
        }

    }


    //open the equipment menu to change the monster's Equipment #1
    public void Equipment1Button()
    {
        equipMenu.SetActive(true);
        equipMenu.GetComponent<EquipmentManager>().ChangeEquipment(GetComponentInParent<YourHome>().activeMonster.GetComponent<Monster>(), 1);
        if (isEquip1)
        {
            GetComponentInParent<YourHome>().activeMonster.GetComponent<Monster>().UnEquipItem(e1.GetComponent<EquipmentItem>().equip, 1);
        }
        
    }   

    //open the equipment menu to change the monster's Equipment #2
    public void Equipment2Button()
    {
        equipMenu.SetActive(true);
        equipMenu.GetComponent<EquipmentManager>().ChangeEquipment(GetComponentInParent<YourHome>().activeMonster.GetComponent<Monster>(), 2);
        if (isEquip2)
        {
            GetComponentInParent<YourHome>().activeMonster.GetComponent<Monster>().UnEquipItem(e2.GetComponent<EquipmentItem>().equip, 2);
        }
        
    }


    public void RemoveEquipmentButton()
    {
        //make the equipment changing buttons active
        equip1Btn.interactable = true;
        equip2Btn.interactable = true;
    }




    private void OnDisable()
    {
        Destroy(monster.gameObject);


        if (e1)
        {
            Destroy(e1.gameObject);
        }

        if (e2)
        {
            Destroy(e2.gameObject);
        }

    }


}
