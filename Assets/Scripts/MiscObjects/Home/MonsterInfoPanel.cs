using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.ObjectModel;
using TMPro;
using Puppet2D;
using UnityEngine.EventSystems;

public class MonsterInfoPanel : MonoBehaviour, IPointerDownHandler
{
    public GameObject monsterSprite, type1, type2;
    public GameObject equipMenu, equipObject, monsterEditorMenu;
    public TMP_Text monsterNameText, levelText, atkText, defText, speText, precText, typeText, toNextLevelText, evasionText, energyGenText, energyCostText;
    public TMP_Text atkBoostText, defBoostText, speBoostText, precBoostText, evasBoostText, enGenBoostText, costBoostText;
    public TMP_Text attack1, attack2;
    public TMP_Text atk1Attack, atk1Range, atk1Cool, atk1Slow, atk1Effect, atk1EffectChance;
    public TMP_Text atk2Attack, atk2Range, atk2Cool, atk2Slow, atk2Effect, atk2EffectChance;
    public Slider expSlider;

    

    private Equipment equip1, equip2;
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


        equip1Btn.GetComponent<Image>().sprite = null;
        equip2Btn.GetComponent<Image>().sprite = null;

        clickedMonsterIcon = thisMonster;

        //summon a copy of the monster and it's bone structure so it can do idle animations
        monster = Instantiate(thisMonster, transform.position, Quaternion.identity);
        //monster.GetComponent<Monster>().frontModel.SetActive(true);
        monster.GetComponent<Tower>().boneStructure.SetActive(true);

        //loop through all of the meshes of the body and make their sorting layer higher than this menu so they are visible
        for (int i = 0; i < monster.bodyParts.bodyMeshes.Length; i++)
        {
            monster.bodyParts.bodyMeshes[i].GetComponent<Renderer>().sortingLayerName = "GameUI";

            monster.bodyParts.bodyMeshes[i].GetComponent<Renderer>().sortingOrder += 100;
        }
        
        monster.GetComponent<Image>().raycastTarget = false;
        monster.transform.localScale = new Vector3(monster.transform.localScale.x * 3.5f, monster.transform.localScale.y * 3.5f, 1f);


        //monster.GetComponent<Monster>().frontModel.transform.localScale = new Vector3(18f, 18f, 1f);
        //clickedMonsterIcon.gameObject.GetComponentInChildren<MonsterIcon>().IconVisibility("Default");
        gameObject.GetComponentInParent<YourHome>().HideAllMonsters();

        if (types.ContainsKey(monster.info.type1))
        {
            type1.GetComponent<SpriteRenderer>().sprite = types[monster.info.type1].typeSprite;
        }
        else
        {
            type1.GetComponent<SpriteRenderer>().sprite = null;
            
        }

        if (types.ContainsKey(monster.info.type2))
        {
            type2.GetComponent<SpriteRenderer>().sprite = types[monster.info.type2].typeSprite;
            
        }
        else
        {
            type2.GetComponent<SpriteRenderer>().sprite = null;
           
        }

        var equipment = GameManager.Instance.GetComponent<Items>().allEquipmentDict;


        //show the equipment item for Slot 1
        //if (equipment.ContainsKey(monster.info.equip1Name))
        //{
        //    e1 = Instantiate(equipment[monster.info.equip1Name].equipPrefab, equipObject.transform.position, Quaternion.identity);
        //    e1.transform.SetParent(equipObject.transform, false);
        //    e1.transform.position = new Vector3(-1000f, -1000f, -1000f);
        //    e1.GetComponent<EquipmentItem>().GetEquipInfo(equipment[thisMonster.info.equip1Name], thisMonster, 1);
        //    equip1Btn.interactable = false;
        //    equip1Btn.GetComponent<Image>().sprite = e1.GetComponent<SpriteRenderer>().sprite;
        //    //Destroy(e1.gameObject);
        //    isEquip1 = true;
        //}
        if (equipment.ContainsKey(monster.info.equip1Name))
        {
            equip1 = equipment[monster.info.equip1Name];
            equip1Btn.interactable = false;
            equip1.equipPrefab.GetComponent<EquipmentItem>().GetEquipInfo(equipment[thisMonster.info.equip1Name], thisMonster, 1);
            equip1Btn.GetComponent<Image>().sprite = equip1.equipPrefab.GetComponent<Image>().sprite;
            equip1Btn.name = equipment[monster.info.equip1Name].name;
            isEquip1 = true;
        }
        else
        {
            
            equip1Btn.interactable = true;
            isEquip1 = false;
        }

        //show the equipment item for Slot 2
        //if (equipment.ContainsKey(monster.info.equip2Name))
        //{
        //    e2 = Instantiate(equipment[monster.info.equip2Name].equipPrefab, equipObject.transform.position, Quaternion.identity);
        //    e2.transform.SetParent(equipObject.transform, false);
        //    e2.transform.position = new Vector3(-1000f, -1000f, -1000f);
        //    e2.GetComponent<EquipmentItem>().GetEquipInfo(equipment[thisMonster.info.equip2Name], thisMonster, 2);
        //    equip2Btn.GetComponent<Image>().sprite = e2.GetComponent<SpriteRenderer>().sprite;
        //    equip2Btn.interactable = false;
        //    //Destroy(e2.gameObject);
        //    isEquip2 = true;
        //}
        if (equipment.ContainsKey(monster.info.equip2Name))
        {
            equip2 = equipment[monster.info.equip2Name];
            equip2Btn.interactable = false;
            equip2Btn.name = equipment[monster.info.equip2Name].name;
            equip2.equipPrefab.GetComponent<EquipmentItem>().GetEquipInfo(equipment[thisMonster.info.equip2Name], thisMonster, 2);
            equip2Btn.GetComponent<Image>().sprite = equip2.equipPrefab.GetComponent<Image>().sprite;
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

        RefreshMonsterInfo(thisMonster);
        
        

        

    }

    //refresh the stat boxes of the monster when something about the monster changes
    public void RefreshMonsterInfo(Monster thisMonster)
    {
        

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
        atk1Attack.text = thisMonster.tempStats.attack1.Power.Value.ToString();
        atk1Range.text = thisMonster.tempStats.attack1.Range.Value.ToString();
        atk1Cool.text = thisMonster.tempStats.attack1.attackTime.ToString();
        atk1Slow.text = thisMonster.tempStats.attack1.hitSlowTime.ToString();
        atk1Effect.text = thisMonster.tempStats.attack1.effectName;
        atk1EffectChance.text = thisMonster.tempStats.attack1.effectChance * 100 + "%";

        if (thisMonster.tempStats.attack1.Power.BaseValue != thisMonster.tempStats.attack1.Power.Value)
        {
            atk1Attack.color = Color.yellow;
        }
        else
        {
            atk1Attack.color = Color.white;
        }

        if (thisMonster.tempStats.attack1.Range.BaseValue != thisMonster.tempStats.attack1.Range.Value)
        {
            atk1Range.color = Color.yellow;
        }
        else
        {
            atk1Range.color = Color.white;
        }

        //if (thisMonster.tempStats.attack1.AttackTime.BaseValue != thisMonster.tempStats.attack1.AttackTime.Value)
        //{
        //    atk1Cool.color = Color.yellow;
        //}
        //else
        //{
        //    atk1Cool.color = Color.white;
        //}

       

        

        attack2.text = thisMonster.info.attack2Name;
        atk2Attack.text = thisMonster.tempStats.attack2.Power.Value.ToString();
        atk2Range.text = thisMonster.tempStats.attack2.Range.Value.ToString();
        atk2Cool.text = thisMonster.tempStats.attack2.attackTime.ToString();
        atk2Slow.text = thisMonster.tempStats.attack2.hitSlowTime.ToString();
        atk2Effect.text = thisMonster.tempStats.attack2.effectName;
        atk2EffectChance.text = thisMonster.tempStats.attack2.effectChance * 100 + "%";

        if (thisMonster.tempStats.attack2.Power.BaseValue != thisMonster.tempStats.attack2.Power.Value)
        {
            atk2Attack.color = Color.yellow;
        }
        else
        {
            atk2Attack.color = Color.white;
        }

        if (thisMonster.tempStats.attack2.Range.BaseValue != thisMonster.tempStats.attack2.Range.Value)
        {
            atk2Range.color = Color.yellow;
        }
        else
        {
            atk2Range.color = Color.white;
        }

        //if (thisMonster.tempStats.attack2.AttackTime.BaseValue != thisMonster.tempStats.attack2.AttackTime.Value)
        //{
        //    atk2Cool.color = Color.yellow;
        //}
        //else
        //{
        //    atk2Cool.color = Color.white;
        //}

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
            GetComponentInParent<YourHome>().activeMonster.GetComponent<Monster>().UnEquipItem(equip1.equipPrefab.GetComponent<EquipmentItem>().equip, 1);
            isEquip1 = false;
            equip1Btn.GetComponent<Image>().sprite = null;
            RefreshMonsterInfo(GetComponentInParent<YourHome>().activeMonster.GetComponent<Monster>());
        }
        
    }   

    //open the equipment menu to change the monster's Equipment #2
    public void Equipment2Button()
    {
        equipMenu.SetActive(true);
        equipMenu.GetComponent<EquipmentManager>().ChangeEquipment(GetComponentInParent<YourHome>().activeMonster.GetComponent<Monster>(), 2);
        if (isEquip2)
        {
            GetComponentInParent<YourHome>().activeMonster.GetComponent<Monster>().UnEquipItem(equip2.equipPrefab.GetComponent<EquipmentItem>().equip, 2);
            isEquip2 = false;
            equip2Btn.GetComponent<Image>().sprite = null;
            RefreshMonsterInfo(GetComponentInParent<YourHome>().activeMonster.GetComponent<Monster>());
        }
        
    }



    public void RemoveEquipmentButton()
    {
        //make the equipment changing buttons active
        equip1Btn.interactable = true;
        equip2Btn.interactable = true;
    }



    public void ClosePanel()
    {
        //clickedMonsterIcon.gameObject.GetComponentInChildren<MonsterIcon>().IconVisibility("GameUI");

        gameObject.GetComponentInParent<YourHome>().ShowAllMonsters();
        Destroy(monster.gameObject);

        gameObject.SetActive(false);
    }



    public void OnPointerDown(PointerEventData eventData)
    {

        if (eventData.pointerEnter)
        {
            var tag = eventData.pointerEnter.gameObject.tag;
            var hit = eventData.pointerEnter.gameObject;


            //if the menu is opened with the purpose of Equipping a monster with an item, then allow it to be equipped. Otherwise, show the item's details
            if (tag == "Equipment")
            {
                var equipment = hit.gameObject.GetComponent<EquipmentItem>();

                Debug.Log(equipment.equipDetails.description);
              
            }

        }



    }

    //when the attack 1 button is pushed, have the monster shoot a sample of the attack
    public void Attack1Btn()
    {
        //get a list of all of the animation events on the monster. 
        AnimationClip[] clips = monster.monsterMotion.runtimeAnimatorController.animationClips;
        for (int i = 0; i < clips.Length; i++)
        {
            //get a list of all animations that have at least 1 event
            if (clips[i].events.Length > 0)
            {
                AnimationEvent[] evt = clips[i].events;
                for (int e = 0; e < clips[i].events.Length; e++)
                {
                    //figure out where the "start attack" event is, and delay the attack by this amount, so that the attack spawns when the attack animation tells it to
                    if (evt[e].functionName == "StartAttack")
                    {
                        StartCoroutine(Attack1(evt[e].time));
                    }
                }
            }
        }

       
    }

    public IEnumerator Attack1(float time)
    {
       

        MonsterAttack attack = monster.tempStats.attack1;


        monster.GetComponent<Tower>().attackNumber = 1;
        monster.monsterMotion.SetBool("isAttacking", true);

        Debug.Log(time + (time * (1 -monster.monsterMotion.speed)));
        yield return new WaitForSeconds(time + (time * (1 -monster.monsterMotion.speed)));

        var attack1 = Instantiate(monster.tempStats.attack1.attackAnimation, monster.GetComponent<Tower>().attackPoint.transform.position, Quaternion.identity);
        attack1.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "PopMenu";
        //attack1.GetComponent<AttackEffects>().delay = .3f;
        attack1.transform.localScale = new Vector3(attack1.transform.localScale.x * 1.5f, attack1.transform.localScale.y * 1.5f, attack1.transform.localScale.z);
        attack1.GetComponent<AttackEffects>().FromAttacker(attack, attack.name, attack.type, monster.tempStats.Attack.Value, (int)attack.Power.Value, monster.info.level, attack.CritChance.Value, attack.CritMod.Value, gameObject.GetComponent<Monster>());
        attack1.GetComponent<AttackEffects>().AttackMotion(Vector2.right * 25);
    }

    //open the attack editor for the monster's first attack
    public void Atk1Edit()
    {
        monsterEditorMenu.SetActive(true);
        monsterEditorMenu.GetComponent<MonsterEditor>().LoadMonster(GetComponentInParent<YourHome>().activeMonster, 1);
    }


    //when the attack 2 button is pushed, have the monster shoot a sample of the attack
    public void Attack2Btn()
    {
        //get a list of all of the animation events on the monster. 
        AnimationClip[] clips = monster.monsterMotion.runtimeAnimatorController.animationClips;
        for (int i = 0; i < clips.Length; i++)
        {
            //get a list of all animations that have at least 1 event
            if (clips[i].events.Length > 0)
            {
                AnimationEvent[] evt = clips[i].events;
                for (int e = 0; e < clips[i].events.Length; e++)
                {
                    //figure out where the "start attack" event is, and delay the attack by this amount, so that the attack spawns when the attack animation tells it to
                    if (evt[e].functionName == "StartAttack")
                    {
                        StartCoroutine(Attack2(evt[e].time));
                    }
                }
            }
        }
    }

    public IEnumerator Attack2(float time)
    {

        MonsterAttack attack = monster.tempStats.attack2;


        monster.GetComponent<Tower>().attackNumber = 2;
        monster.monsterMotion.SetBool("isAttacking", true);

        yield return new WaitForSeconds(time + (time * (1 -monster.monsterMotion.speed)));

        var attack2 = Instantiate(monster.tempStats.attack2.attackAnimation, monster.GetComponent<Tower>().attackPoint.transform.position, Quaternion.identity);
        attack2.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "PopMenu";
        //attack2.GetComponent<AttackEffects>().delay = .3f;
        attack2.transform.localScale = new Vector3(attack2.transform.localScale.x * 1.5f, attack2.transform.localScale.y * 1.5f, attack2.transform.localScale.z);
        attack2.GetComponent<AttackEffects>().FromAttacker(attack, attack.name, attack.type, monster.tempStats.Attack.Value, (int)attack.Power.Value, monster.info.level, attack.CritChance.Value, attack.CritMod.Value, gameObject.GetComponent<Monster>());
        attack2.GetComponent<AttackEffects>().AttackMotion(Vector2.right * 25);
    }

    //open the attack editor for the monster's first attack
    public void Atk2Edit()
    {
        monsterEditorMenu.SetActive(true);
        monsterEditorMenu.GetComponent<MonsterEditor>().LoadMonster(GetComponentInParent<YourHome>().activeMonster, 2);
    }

}
