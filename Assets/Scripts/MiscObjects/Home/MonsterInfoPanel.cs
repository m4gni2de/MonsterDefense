using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.ObjectModel;
using TMPro;
using Puppet2D;
using UnityEngine.EventSystems;
using System.Reflection;
using System.Linq;

public class MonsterInfoPanel : MonoBehaviour, IPointerDownHandler
{
    public GameObject monsterSprite, type1, type2;
    public GameObject equipMenu, equipObject, monsterEditorMenu, monsterUpgradeMenu, popMenu;
    public TMP_Text monsterNameText, levelText, atkText, defText, speText, precText, typeText, toNextLevelText, evasionText, energyGenText, energyCostText, stamTxt, abilityNameText, abilityText, coinGenText, koCountText;
    public TMP_Text atkBoostText, defBoostText, speBoostText, precBoostText, evasBoostText, enGenBoostText, costBoostText, stamBoostText, coinGenBoost;
    public TMP_Text attack1, attack2;
    public TMP_Text atk1Attack, atk1Range, atk1Cool, atk1Slow, atk1Effect, atk1Stamina;
    public TMP_Text atk2Attack, atk2Range, atk2Cool, atk2Slow, atk2Effect, atk2Stamina;
    public Slider expSlider;

    //images to represent the attack modes and damage forces of the monster's attacks
    public Image atk1Mode, atk1Force, atk1Status, atk2Mode, atk2Force, atk2Status;



    private Equipment equip1, equip2;
    public bool isEquip1, isEquip2;
    private Monster monster, clickedMonsterIcon;


    public Button equip1Btn, equip2Btn, removeEquipBtn, attack1Btn, attack2Btn;

    public GameObject[] rankSprites;
    //public GameObject equip1Image, equip2Image;
    // Start is called before the first frame update
    void Start()
    {
        expSlider.GetComponent<Slider>();
        equip1Btn.GetComponent<Button>();
        equip2Btn.GetComponent<Button>();
        attack1Btn.GetComponent<Button>();
        attack2Btn.GetComponent<Button>();


        attack1Btn.interactable = true;
        attack2Btn.interactable = true;

        //deleteButton.GetComponent<Button>();


    }

    // Update is called once per frame
    void Update()
    {
        if (monster)
        {
            if (monster.isAttacking)
            {
                attack1Btn.interactable = false;
                attack2Btn.interactable = false;
                
            }
            else
            {
                attack1Btn.interactable = true;
                attack2Btn.interactable = true;
            }
        }
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
        monster.GetComponent<Monster>().monsterIcon.SetActive(false);
        monster.tag = "MonsterDisplay";
        monster.GetComponent<Tower>().boneStructure.SetActive(true);

        //loop through all of the meshes of the body and make their sorting layer higher than this menu so they are visible
        for (int i = 0; i < monster.bodyParts.bodyMeshes.Length; i++)
        {
            monster.bodyParts.bodyMeshes[i].GetComponent<Renderer>().sortingLayerName = "GameUI";

            monster.bodyParts.bodyMeshes[i].GetComponent<Renderer>().sortingOrder += 100;
        }
        
        //monster.GetComponent<Image>().raycastTarget = false;
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

       

        monster.transform.SetParent(transform, true);
        monster.transform.position = monsterSprite.transform.position;

        ////make this monster's sorting layer higher than everything else so it shows up on the panel
        //SpriteRenderer[] monsterParts = monster.GetComponentsInChildren<SpriteRenderer>();
        //for (int i = 0; i < monsterParts.Length; i++)
        //{
        //    monsterParts[i].sortingLayerName = "GameUI";
        //    monsterParts[i].sortingOrder = monsterParts[i].sortingOrder + transform.GetComponent<SpriteRenderer>().sortingOrder;
        //}

        RefreshMonsterInfo(thisMonster);
        
        

        

    }

    //refresh the stat boxes of the monster when something about the monster changes
    public void RefreshMonsterInfo(Monster thisMonster)
    {
        GameObject[] equips = GameObject.FindGameObjectsWithTag("Respawn");

        for (int e = 0; e < equips.Length; e++)
        {
            Destroy(equips[e]);
        }

        var equipment = GameManager.Instance.GetComponent<Items>().allEquipmentDict;
        var abilities = GameManager.Instance.GetComponent<MonsterAbilities>().allAbilitiesDict;

        GetComponentInChildren<EnviromentCanvas>().GetEnviroment(thisMonster.info.type1);

        //show the equipment item for Slot 1

        if (equipment.ContainsKey(thisMonster.info.equip1Name))
        {
            GameObject e1 = Instantiate(equipment[monster.info.equip1Name].equipPrefab, equipObject.transform.position, Quaternion.identity);
            e1.transform.SetParent(equipObject.transform);
            //e1.GetComponent<Image>().color = Color.clear;
            e1.GetComponent<Image>().raycastTarget = false;
            e1.transform.position = equip1Btn.transform.position;
            e1.transform.localScale = new Vector2(60f, 60f);
            e1.tag = "Respawn";


            equip1 = equipment[monster.info.equip1Name];
            equip1Btn.interactable = false;
            equip1.equipPrefab.GetComponent<EquipmentItem>().GetEquipInfo(equipment[thisMonster.info.equip1Name], thisMonster, 1);
            //equip1Btn.GetComponent<Image>().sprite = equip1.equipPrefab.GetComponent<Image>().sprite;

           
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
        if (equipment.ContainsKey(thisMonster.info.equip2Name))
        {
            GameObject e2 = Instantiate(equipment[monster.info.equip2Name].equipPrefab, equipObject.transform.position, Quaternion.identity);
            e2.transform.SetParent(equipObject.transform);
            //e2.GetComponent<Image>().color = Color.clear;
            e2.GetComponent<Image>().raycastTarget = false;
            e2.transform.position = equip2Btn.transform.position;
            e2.transform.localScale = new Vector2(60f, 60f);
            e2.tag = "Respawn";

            equip2 = equipment[monster.info.equip2Name];
            equip2Btn.interactable = false;
            equip2Btn.name = equipment[monster.info.equip2Name].name;
            equip2.equipPrefab.GetComponent<EquipmentItem>().GetEquipInfo(equipment[thisMonster.info.equip2Name], thisMonster, 2);
            //equip2Btn.GetComponent<Image>().sprite = equip2.equipPrefab.GetComponent<Image>().sprite;
            isEquip2 = true;
            equip2Btn.name = equipment[monster.info.equip2Name].name;

        }
        else
        {

            equip2Btn.interactable = true;
            isEquip2 = false;
        }

        monsterNameText.text = thisMonster.info.name;
        //typeText.text = thisMonster.info.type1 + "/" + thisMonster.info.type2;

        levelText.text = thisMonster.info.level.ToString();
        atkText.text = thisMonster.info.Attack.Value.ToString();
        defText.text = thisMonster.info.Defense.Value.ToString();
        speText.text = thisMonster.info.Speed.Value.ToString();
        precText.text = thisMonster.info.Precision.Value.ToString();
        evasionText.text = thisMonster.info.evasionBase + "%";
        //energyGenText.text = Math.Round(thisMonster.energyGeneration / 60, 2) + " /s";
        energyGenText.text = Math.Round(thisMonster.info.EnergyGeneration.Value / 60, 2) + " /s";
        energyCostText.text = thisMonster.info.EnergyCost.Value.ToString();
        stamTxt.text = thisMonster.info.Stamina.Value.ToString();
        abilityNameText.text = thisMonster.info.abilityName;
        abilityText.text = abilities[thisMonster.info.abilityName].description;
        coinGenText.text = thisMonster.info.CoinGeneration.Value + " /h";
        koCountText.text = thisMonster.info.koCount.ToString();

        atkBoostText.text = "(+ " + (thisMonster.info.Attack.Value - thisMonster.info.Attack.BaseValue) + ")".ToString();
        defBoostText.text = "(+ " + (thisMonster.info.Defense.Value - thisMonster.info.Defense.BaseValue) + ")".ToString();
        speBoostText.text = "(+ " + (thisMonster.info.Speed.Value - thisMonster.info.Speed.BaseValue) + ")".ToString();
        precBoostText.text = "(+ " + (thisMonster.info.Precision.Value - thisMonster.info.Precision.BaseValue) + ")".ToString();
        evasBoostText.text = "(+ " + (thisMonster.info.evasionBase - thisMonster.info.evasionBase) + ")".ToString();
        enGenBoostText.text = "(+ " + (thisMonster.info.EnergyGeneration.BaseValue - thisMonster.info.EnergyGeneration.Value) + ")".ToString();
        costBoostText.text = "(+ " + (thisMonster.info.EnergyCost.Value - thisMonster.info.EnergyCost.BaseValue) + ")".ToString();
        stamBoostText.text = "(+ " + (thisMonster.info.Stamina.Value - thisMonster.info.Stamina.BaseValue) + ")".ToString();
        enGenBoostText.text = "(+ 0)";
        coinGenBoost.text = "(+ 0)";
        

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
        atk1Attack.text = thisMonster.info.attack1.Power.Value.ToString();
        atk1Range.text = thisMonster.info.attack1.range.ToString();
        atk1Cool.text = thisMonster.info.attack1.attackTime.ToString();
        atk1Slow.text = thisMonster.info.attack1.hitSlowTime.ToString();
        
        atk1Mode.sprite = GameManager.Instance.baseAttacks.atkModeDict[thisMonster.info.attack1.attackMode.ToString()];
        atk1Mode.name = thisMonster.info.attack1.attackMode.ToString();
        atk1Stamina.text = thisMonster.info.attack1.staminaGained.ToString();

        if (thisMonster.info.attack1.effectName != "none")
        {
            atk1Effect.text = "+ " + thisMonster.info.attack1.effectChance * 100 + "%";
            atk1Status.color = Color.white;
            atk1Status.sprite = GameManager.Instance.GetComponent<AllStatusEffects>().allStatusDict[thisMonster.info.attack1.effectName].statusSprite;
            atk1Status.name = thisMonster.info.attack1.effectName;
            
        }
        else
        {
            atk1Effect.text = "";
            atk1Status.sprite = null;
            atk1Status.color = Color.clear;
            atk1Status.name = "none";
        }

        if (thisMonster.info.attack1.Power.BaseValue != thisMonster.info.attack1.Power.Value)
        {
            atk1Attack.color = Color.yellow;
        }
        else
        {
            atk1Attack.color = Color.white;
        }

        if (thisMonster.info.attack1.Range.BaseValue != thisMonster.info.attack1.Range.Value)
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
        atk2Attack.text = thisMonster.info.attack2.Power.Value.ToString();
        atk2Range.text = thisMonster.info.attack2.range.ToString();
        atk2Cool.text = thisMonster.info.attack2.attackTime.ToString();
        atk2Slow.text = thisMonster.info.attack2.hitSlowTime.ToString();
        atk2Stamina.text = thisMonster.info.attack2.staminaGained.ToString();

        if (thisMonster.info.attack2.effectName != "none")
        {
            atk2Effect.text = "+ " + thisMonster.info.attack2.effectChance * 100 + "%";
            atk2Status.color = Color.white;
            atk2Status.sprite = GameManager.Instance.GetComponent<AllStatusEffects>().allStatusDict[thisMonster.info.attack2.effectName].statusSprite;
            atk2Status.name = thisMonster.info.attack2.effectName;
        }
        else
        {
            atk2Effect.text = "";
            atk2Status.sprite = null;
            atk2Status.color = Color.clear;
            atk2Status.name = "none";
        }


        atk2Mode.sprite = GameManager.Instance.baseAttacks.atkModeDict[thisMonster.info.attack2.attackMode.ToString()];
        atk2Mode.name = thisMonster.info.attack2.attackMode.ToString();

        if (thisMonster.tempStats.attack2.Power.BaseValue != thisMonster.tempStats.attack2.Power.Value)
        {
            atk2Attack.color = Color.yellow;
        }
        else
        {
            atk2Attack.color = Color.white;
        }

        if (thisMonster.info.attack2.Range.BaseValue != thisMonster.info.attack2.Range.Value)
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

        //show the correct number of rank stars
        for (int i = 0; i < rankSprites.Length; i++)
        {
            if (thisMonster.saveToken.rank > i)
            {
                rankSprites[i].gameObject.SetActive(true);
            }
            else
            {
                rankSprites[i].gameObject.SetActive(false);
            }
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
            equip1.equipPrefab.GetComponent<EquipmentItem>().UnEquip();
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
            equip2.equipPrefab.GetComponent<EquipmentItem>().UnEquip();
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
        

        if (equip1.equipPrefab)
        {
            equip1.equipPrefab.GetComponent<EquipmentItem>().UnEquip();
        }
        if (equip2.equipPrefab)
        {
            equip2.equipPrefab.GetComponent<EquipmentItem>().UnEquip();
        }


        


        gameObject.SetActive(false);
    }



    public void OnPointerDown(PointerEventData eventData)
    {

        if (eventData.pointerEnter)
        {
            var tag = eventData.pointerEnter.gameObject.tag;
            var hit = eventData.pointerEnter.gameObject;


            
            if (tag == "Equipment")
            {
                var equipment = hit.gameObject.GetComponent<EquipmentItem>();

                Debug.Log(equipment.equipDetails.description);
              
            }

            if (tag == "MonsterDisplay")
            {
                monster.monsterMotion.SetBool("isClicked", true);
                
            }

            //if the player clicks on an icon, open a window that describes what the icon is
            if (tag == "ScriptableObject")
            {
                popMenu.SetActive(true);
                popMenu.GetComponent<PopMenuObject>().AcceptObject(hit.name, hit);
            }

        }



    }

    //when the attack 1 button is pushed, have the monster shoot a sample of the attack
    public void Attack1Btn()
    {
        MonsterAttack attack = monster.tempStats.attack1;

        monster.GetComponent<Tower>().attackNumber = 1;
        monster.monsterMotion.SetBool("isAttacking", true);
        monster.GetComponent<Tower>().isAttacking = true;
        monster.GetComponent<Tower>().boneStructure.GetComponent<MotionControl>().AttackModeCheck(attack.attackMode);
        monster.isAttacking = true;


        ////get a list of all of the animation events on the monster. 
        //AnimationClip[] clips = monster.monsterMotion.runtimeAnimatorController.animationClips;
        //for (int i = 0; i < clips.Length; i++)
        //{
        //    //get a list of all animations that have at least 1 event
        //    if (clips[i].events.Length > 0)
        //    {
        //        AnimationEvent[] evt = clips[i].events;
        //        for (int e = 0; e < clips[i].events.Length; e++)
        //        {
        //            //figure out where the "start attack" event is, and delay the attack by this amount, so that the attack spawns when the attack animation tells it to
        //            if (evt[e].functionName == "StartAttack")
        //            {
        //                StartCoroutine(Attack1(evt[e].time));
        //                return;
        //            }
        //        }
        //    }
        //}
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
        MonsterAttack attack = monster.info.attack2;
        monster.GetComponent<Tower>().attackNumber = 2;
        monster.monsterMotion.SetBool("isAttacking", true);
        monster.GetComponent<Tower>().isAttacking = true;
        monster.GetComponent<Tower>().boneStructure.GetComponent<MotionControl>().AttackModeCheck(attack.attackMode);
        monster.isAttacking = true;

    }


    //open the attack editor for the monster's first attack
    public void Atk2Edit()
    {
        monsterEditorMenu.SetActive(true);
        monsterEditorMenu.GetComponent<MonsterEditor>().LoadMonster(GetComponentInParent<YourHome>().activeMonster, 2);
    }

    //use this to open the menu to upgrade the current monster
    public void MonsterUpgradeBtn()
    {
        monsterUpgradeMenu.SetActive(true);
        monsterUpgradeMenu.GetComponent<MonsterUpgrade>().SelectMonster(GetComponentInParent<YourHome>().activeMonster);
    }

   


    public void OnDisable()
    {


        //if (equip1.equipPrefab)
        //{
        //    equip1.equipPrefab.GetComponent<EquipmentItem>().UnEquip();
        //}
        //if (equip2.equipPrefab)
        //{
        //    equip2.equipPrefab.GetComponent<EquipmentItem>().UnEquip();
        //}


    }


    public void OnEnable()
    {

       
    }

}
