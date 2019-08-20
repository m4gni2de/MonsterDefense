﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.ObjectModel;
using TMPro;

public class MonsterInfoPanel : MonoBehaviour
{
    public GameObject monsterSprite, equip1, equip2, type1, type2;
    public GameObject equipMenu, equipObject;
    public TMP_Text monsterNameText, levelText, atkText, defText, speText, precText, typeText, toNextLevelText, evasionText, energyGenText, energyCostText;
    public TMP_Text atkBoostText, defBoostText, speBoostText, precBoostText, evasBoostText, enGenBoostText, costBoostText;
    public TMP_Text attack1, attack2;
    public Slider expSlider;
    

    private GameObject e1, e2;
    public bool isEquip1, isEquip2;
    private Monster monster, clickedMonsterIcon;


    public Button equip1Btn, equip2Btn, removeEquipBtn, attack1Btn, attack2Btn;
    // Start is called before the first frame update
    void Start()
    {
        expSlider.GetComponent<Slider>();
        equip1Btn.GetComponent<Button>();
        equip2Btn.GetComponent<Button>();
        attack1Btn.GetComponent<Button>();
        attack2Btn.GetComponent<Button>();

        
        
        
        //deleteButton.GetComponent<Button>();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TouchManager()
    {
        
    }

    public void LoadInfo(Monster thisMonster)
    {
        var types = GameManager.Instance.monstersData.typeChartDict;

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

        clickedMonsterIcon = thisMonster;
        monster = Instantiate(thisMonster, transform.position, Quaternion.identity);
        monster.GetComponent<Monster>().frontModel.SetActive(true);
        monster.GetComponent<Image>().raycastTarget = false;
        monster.GetComponent<Monster>().frontModel.transform.localScale = new Vector3(18f, 18f, 1f);
        //clickedMonsterIcon.gameObject.GetComponentInChildren<MonsterIcon>().IconVisibility("Default");
        gameObject.GetComponentInParent<YourHome>().HideAllMonsters();

        if (types.ContainsKey(monster.info.type1))
        {
            type1.GetComponent<SpriteRenderer>().sprite = types[monster.info.type1].typeSprite;
        }
        
        var equipment = GameManager.Instance.GetComponent<Items>().allEquipmentDict;
        

        //show the equipment item for Slot 1
        if (equipment.ContainsKey(monster.info.equip1Name))
        {
            e1 = Instantiate(equipment[monster.info.equip1Name].equipPrefab, equipObject.transform.position, Quaternion.identity);
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
        if (equipment.ContainsKey(monster.info.equip2Name))
        {
            e2 = Instantiate(equipment[monster.info.equip2Name].equipPrefab, equipObject.transform.position, Quaternion.identity);
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
        typeText.text = thisMonster.info.type1 + "/" + thisMonster.info.type2;

        levelText.text = thisMonster.info.level.ToString();
        atkText.text = thisMonster.info.Attack.Value.ToString();
        defText.text = thisMonster.info.Defense.Value.ToString();
        speText.text = thisMonster.info.Speed.Value.ToString();
        precText.text = thisMonster.info.Precision.Value.ToString();
        evasionText.text = thisMonster.info.evasionBase + "%";
        //energyGenText.text = Math.Round(thisMonster.energyGeneration / 60, 2) + " /s";
        energyGenText.text = Math.Round(thisMonster.tempStats.EnergyGeneration.Value / 60, 2) + " /s";
        energyCostText.text = thisMonster.info.EnergyCost.Value.ToString();

        atkBoostText.text = "(+ " + (thisMonster.info.Attack.Value - thisMonster.info.Attack.BaseValue) + ")".ToString();
        defBoostText.text = "(+ " + (thisMonster.info.Defense.Value - thisMonster.info.Defense.BaseValue) + ")".ToString();
        speBoostText.text = "(+ " + (thisMonster.info.Speed.Value - thisMonster.info.Speed.BaseValue) + ")".ToString();
        precBoostText.text = "(+ " + (thisMonster.info.Precision.Value - thisMonster.info.Precision.BaseValue) + ")".ToString();
        evasBoostText.text = "(+ " + (thisMonster.info.evasionBase - thisMonster.info.evasionBase) + ")".ToString();
        enGenBoostText.text = "(+ " + (thisMonster.info.EnergyGeneration.BaseValue - thisMonster.info.EnergyGeneration.Value) + ")".ToString();
        costBoostText.text = "(+ " + (thisMonster.info.EnergyCost.Value - thisMonster.info.EnergyCost.BaseValue) + ")".ToString();

        if (thisMonster.info.Attack.BaseValue != thisMonster.info.Attack.Value)
        {
            atkText.color = Color.yellow;
        }
        else
        {
            atkText.color = Color.white;
        }

        if (thisMonster.info.Defense.BaseValue != thisMonster.info.Defense.Value)
        {
            defText.color = Color.yellow;
        }
        else
        {
            defText.color = Color.white;
        }

        if (thisMonster.info.Speed.BaseValue != thisMonster.info.Speed.Value)
        {
            speText.color = Color.yellow;
        }
        else
        {
            speText.color = Color.white;
        }

        if (thisMonster.info.Precision.BaseValue != thisMonster.info.Precision.Value)
        {
            precText.color = Color.yellow;
        }
        else
        {
            precText.color = Color.white;
        }

        //if (thisMonster.info.Evasion.BaseValue != thisMonster.info.Evasion.Value)
        //{
        //    evasionText.color = Color.yellow;
        //}
        //else
        //{
        //    evasionText.color = Color.white;
        //}

        //if (thisMonster.info.EnergyGeneration.BaseValue != thisMonster.info.EnergyGeneration.Value)
        //{
        //    energyGenText.color = Color.yellow;
        //}
        //else
        //{
        //    energyGenText.color = Color.white;
        //}

        if (thisMonster.info.EnergyCost.BaseValue != thisMonster.info.EnergyCost.Value)
        {
            energyCostText.color = Color.yellow;
        }
        else
        {
            energyCostText.color = Color.white;
        }

        attack1.text = thisMonster.info.attack1Name;
        attack2.text = thisMonster.info.attack2Name;

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

        //clickedMonsterIcon.gameObject.GetComponentInChildren<MonsterIcon>().IconVisibility("GameUI");
        gameObject.GetComponentInParent<YourHome>().ShowAllMonsters();
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
