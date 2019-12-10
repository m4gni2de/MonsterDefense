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
    public GameObject monsterSprite, type1, type2, attack1Border, attack2Border;
    public GameObject equipMenu, equipObject, monsterEditorMenu, monsterUpgradeMenu, monsterList;
    public TMP_Text monsterNameText, levelText, atkText, defText, speText, precText, typeText, toNextLevelText, evasionText, energyGenText, energyCostText, stamTxt, abilityNameText, abilityText, coinGenText, koCountText;
    public TMP_Text atkBoostText, defBoostText, speBoostText, precBoostText, evasBoostText, enGenBoostText, costBoostText, stamBoostText, coinGenBoost;
    public TMP_Text attack1, attack2;
    public TMP_Text atk1Attack, atk1Range, atk1Cool, atk1Slow, atk1Effect, atk1Stamina;
    public TMP_Text atk2Attack, atk2Range, atk2Cool, atk2Slow, atk2Effect, atk2Stamina;
    public TMP_Text skillName, skillDescription;
    public Slider expSlider;

    //images to represent the attack modes and damage forces of the monster's attacks
    public Image atk1Mode, atk1Force, atk1Status, atk2Mode, atk2Force, atk2Status;

    


    public Equipment equips1, equips2;
    public EquipmentScript e1, e2;
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

    void LateUpdate()
    {
        //if (gameObject.activeSelf == true)
        //{
        //    monsterList.SetActive(false);
        //}
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
        monster.transform.localScale = new Vector3(monster.transform.localScale.x * 2.8f, monster.transform.localScale.y * 2.8f, 1f);


        //monster.GetComponent<Monster>().frontModel.transform.localScale = new Vector3(18f, 18f, 1f);
        //clickedMonsterIcon.gameObject.GetComponentInChildren<MonsterIcon>().IconVisibility("Default");
        gameObject.GetComponentInParent<YourHome>().HideAllMonsters();

        if (types.ContainsKey(monster.info.type1))
        {
            type1.GetComponent<SpriteRenderer>().sprite = types[monster.info.type1].typeSprite;
            type1.name = monster.info.type1;
        }
        else
        {
            type1.name = "type1";
            type1.GetComponent<SpriteRenderer>().sprite = null;
            
        }

        if (types.ContainsKey(monster.info.type2))
        {
            type2.GetComponent<SpriteRenderer>().sprite = types[monster.info.type2].typeSprite;
            type2.name = monster.info.type2;
        }
        else
        {
            type2.name = "type2";
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

        //RefreshMonsterInfo(thisMonster);
        //RefreshEquipment();
        RefreshMonsterInfo(monster);
        
        

        

    }

    //refresh the stat boxes of the monster when something about the monster changes
    public void RefreshMonsterInfo(Monster thisMonster)
    {
        GameObject[] equips = GameObject.FindGameObjectsWithTag("Respawn");

        for (int e = 0; e < equips.Length; e++)
        {
            Destroy(equips[e]);
        }

        

        if (e1)
        {
            e1.DeactivateItem(e1, equip1Btn.gameObject);
        }

        if (e2)
        {
            e2.DeactivateItem(e2, equip2Btn.gameObject);
            
        }

       

        var allEquipment = GameManager.Instance.GetComponent<Items>().allEquipsDict;
        var abilities = GameManager.Instance.GetComponent<MonsterAbilities>().allAbilitiesDict;

        GetComponentInChildren<EnviromentCanvas>().GetEnviroment(thisMonster.info.type1);

        //show the equipment item for Slot 1
        //if (equipment.ContainsKey(thisMonster.info.equip1Name))
        if (allEquipment.ContainsKey(monster.info.equip1Name))
        {


            equip1Btn.GetComponent<Image>().color = Color.white;

            //equips1 = monster.info.equip1;
            equips1 = new Equipment(allEquipment[monster.info.equip1Name]);
            e1 = equips1.equipment;
            equips1.SetInventorySlot(monster.info.equip1.inventorySlot);
            equips1.GetStats();
            equips1.Equip(monster, 1);
            equip1Btn.interactable = false;
            equip1Btn.GetComponent<Image>().sprite = equips1.equipment.sprite;
            equip1Btn.name = allEquipment[monster.info.equip1Name].name;
            isEquip1 = true;
            

            equips1.equipment.ActivateItem(equips1.equipment, equip1Btn.gameObject);
        }
        else
        {

            equip1Btn.GetComponent<Image>().sprite = null;
            equip1Btn.GetComponent<Image>().color = Color.clear;
            equip1Btn.interactable = true;
            isEquip1 = false;
            equips1 = null;
            e1 = null;
        }


        //show the equipped item in slot 2
        if (allEquipment.ContainsKey(monster.info.equip2Name))
        {


            equip2Btn.GetComponent<Image>().color = Color.white;
            equips2 = new Equipment(allEquipment[monster.info.equip2Name]);
            e2 = equips2.equipment;
            equips2.SetInventorySlot(monster.info.equip2.inventorySlot);
            equips2.GetStats();
            equips2.Equip(monster, 2);
            equip2Btn.interactable = false;
            equip2Btn.GetComponent<Image>().sprite = equips2.equipment.sprite;
            equip2Btn.name = allEquipment[monster.info.equip2Name].name;
            isEquip2 = true;

            equips2.equipment.ActivateItem(equips2.equipment, equip2Btn.gameObject);
        }
        else
        {

            equip2Btn.GetComponent<Image>().sprite = null;
            equip2Btn.GetComponent<Image>().color = Color.clear;
            equip2Btn.interactable = true;
            isEquip2 = false;
            equips2 = null;
            e2 = null;
        }

        thisMonster = monster;
        monsterNameText.text = thisMonster.info.name;
        //typeText.text = thisMonster.info.type1 + "/" + thisMonster.info.type2;

        levelText.text = thisMonster.info.level.ToString();
        atkText.text = Math.Round(thisMonster.info.Attack.Value, 0).ToString();
        defText.text = Math.Round(thisMonster.info.Defense.Value, 0).ToString();
        speText.text = Math.Round(thisMonster.info.Speed.Value, 0).ToString();
        precText.text = Math.Round(thisMonster.info.Precision.Value, 0).ToString();
        evasionText.text = thisMonster.info.evasionBase + "%";
        //energyGenText.text = Math.Round(thisMonster.energyGeneration / 60, 2) + " /s";
        energyGenText.text = Math.Round(thisMonster.info.EnergyGeneration.Value / 60, 2) + " /s";
        energyCostText.text = thisMonster.info.EnergyCost.Value.ToString();
        stamTxt.text = Math.Round(thisMonster.info.Stamina.Value, 0).ToString();
        abilityNameText.text = thisMonster.info.abilityName;
        abilityText.text = abilities[thisMonster.info.abilityName].description;
        coinGenText.text = thisMonster.info.CoinGeneration.Value + " /h";
        koCountText.text = thisMonster.info.koCount.ToString();

        atkBoostText.text = "(+ " + Math.Round((thisMonster.info.Attack.Value - thisMonster.info.Attack.BaseValue), 0) + ")".ToString();
        defBoostText.text = "(+ " + Math.Round((thisMonster.info.Defense.Value - thisMonster.info.Defense.BaseValue), 0) + ")".ToString();
        speBoostText.text = "(+ " + Math.Round((thisMonster.info.Speed.Value - thisMonster.info.Speed.BaseValue), 0) + ")".ToString();
        precBoostText.text = "(+ " + Math.Round((thisMonster.info.Precision.Value - thisMonster.info.Precision.BaseValue), 0) + ")".ToString();
        evasBoostText.text = "(+ " + Math.Round((thisMonster.info.evasionBase - thisMonster.info.evasionBase), 0) + ")".ToString();
        enGenBoostText.text = "(+ " + Math.Round((thisMonster.info.EnergyGeneration.BaseValue - thisMonster.info.EnergyGeneration.Value), 2) + ")".ToString();
        costBoostText.text = "(+ " + Math.Round((thisMonster.info.EnergyCost.Value - thisMonster.info.EnergyCost.BaseValue), 0) + ")".ToString();
        stamBoostText.text = "(+ " + Math.Round((thisMonster.info.Stamina.Value - thisMonster.info.Stamina.BaseValue), 0) + ")".ToString();
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

        if (Math.Round(thisMonster.info.EnergyGeneration.BaseValue, 0) != Math.Round(thisMonster.info.EnergyGeneration.Value, 0))
        {
            energyGenText.color = Color.yellow;
        }
        else
        {
            energyGenText.color = Color.white;
        }

        if (Math.Round(thisMonster.info.EnergyCost.BaseValue, 0) != Math.Round(thisMonster.info.EnergyCost.Value, 0))
        {
            energyCostText.color = Color.yellow;
        }
        else
        {
            energyCostText.color = Color.white;
        }

        if (Math.Round(thisMonster.info.Stamina.BaseValue, 0) != Math.Round(thisMonster.info.Stamina.Value, 0))
        {
            stamTxt.color = Color.yellow;
        }
        else
        {
            stamTxt.color = Color.white;
        }

        if (Math.Round(thisMonster.info.CoinGeneration.BaseValue, 0) != Math.Round(thisMonster.info.CoinGeneration.Value, 0))
        {
            coinGenText.color = Color.yellow;
        }
        else
        {
            coinGenText.color = Color.white;
        }

        evasionText.color = Color.white;

        attack1.text = thisMonster.info.attack1Name;
        atk1Attack.text = thisMonster.info.baseAttack1.Power.Value.ToString();
        atk1Range.text = thisMonster.info.baseAttack1.Range.Value.ToString();
        atk1Cool.text = thisMonster.info.baseAttack1.AttackTime.Value.ToString();
        atk1Slow.text = thisMonster.info.baseAttack1.AttackSlow.Value.ToString();
        
        atk1Mode.sprite = GameManager.Instance.baseAttacks.atkModeDict[thisMonster.info.baseAttack1.attack.attackMode.ToString()];
        atk1Mode.name = thisMonster.info.baseAttack1.attack.attackMode.ToString();
        atk1Stamina.text = thisMonster.info.baseAttack1.StaminaGained.ToString();
        attack1Border.GetComponent<SpriteRenderer>().color = GameManager.Instance.typeColorDictionary[thisMonster.info.baseAttack1.type];

        ColorBlock cb = new ColorBlock();
        cb.normalColor = GameManager.Instance.typeColorDictionary[thisMonster.info.baseAttack1.type];
        cb.highlightedColor = GameManager.Instance.typeColorDictionary[thisMonster.info.baseAttack1.type];
        cb.pressedColor = new Color(.78f, .78f, .78f, 1f);
        cb.disabledColor = new Color(.78f, .78f, .78f, .5f);
        cb.colorMultiplier = 1f;
        attack1Btn.colors = cb;

        if (thisMonster.info.baseAttack1.effectName != "none")
        {
            atk1Effect.text = "+ " + thisMonster.info.baseAttack1.EffectChance.Value * 100 + "%";
            atk1Status.color = Color.white;
            atk1Status.sprite = GameManager.Instance.GetComponent<AllStatusEffects>().allStatusDict[thisMonster.info.baseAttack1.effectName].statusSprite;
            atk1Status.name = thisMonster.info.baseAttack1.effectName;
            
        }
        else
        {
            atk1Effect.text = "";
            atk1Status.sprite = null;
            atk1Status.color = Color.clear;
            atk1Status.name = "none";
        }

        if (thisMonster.info.baseAttack1.Power.BaseValue != thisMonster.info.baseAttack1.Power.Value)
        {
            atk1Attack.color = Color.yellow;
        }
        else
        {
            atk1Attack.color = Color.white;
        }

        if (thisMonster.info.baseAttack1.Range.BaseValue != thisMonster.info.baseAttack1.Range.Value)
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



        Color color2 = GameManager.Instance.typeColorDictionary[thisMonster.info.baseAttack2.type];

        attack2.text = thisMonster.info.attack2Name;
        atk2Attack.text = thisMonster.info.baseAttack2.Power.Value.ToString();
        atk2Range.text = thisMonster.info.baseAttack2.Range.Value.ToString();
        atk2Cool.text = thisMonster.info.baseAttack2.AttackTime.Value.ToString();
        atk2Slow.text = thisMonster.info.baseAttack2.AttackSlow.Value.ToString();
        atk2Stamina.text = thisMonster.info.baseAttack2.attack.staminaGained.ToString();
        attack2Border.GetComponent<SpriteRenderer>().color = color2;
        //attack2Panel.GetComponent<SpriteRenderer>().color = GameManager.Instance.typeColorDictionary[thisMonster.info.baseAttack2.type];

        ColorBlock cb2 = new ColorBlock();

        cb2.normalColor = color2;
        cb2.highlightedColor = color2;
        cb2.pressedColor = new Color(.78f, .78f, .78f, 1f);
        cb2.disabledColor = new Color(.78f, .78f, .78f, .5f);
        cb2.colorMultiplier = 1f;
        attack2Btn.colors = cb2;

        if (thisMonster.info.baseAttack2.effectName != "none")
        {
            atk2Effect.text = "+ " + thisMonster.info.baseAttack2.EffectChance.Value * 100 + "%";
            atk2Status.color = Color.white;
            atk2Status.sprite = GameManager.Instance.GetComponent<AllStatusEffects>().allStatusDict[thisMonster.info.baseAttack2.effectName].statusSprite;
            atk2Status.name = thisMonster.info.baseAttack2.effectName;
        }
        else
        {
            atk2Effect.text = "";
            atk2Status.sprite = null;
            atk2Status.color = Color.clear;
            atk2Status.name = "none";
        }


        atk2Mode.sprite = GameManager.Instance.baseAttacks.atkModeDict[thisMonster.info.baseAttack2.attack.attackMode.ToString()];
        atk2Mode.name = thisMonster.info.baseAttack2.attack.attackMode.ToString();

        if (thisMonster.info.baseAttack2.Power.BaseValue != thisMonster.info.baseAttack2.Power.Value)
        {
            atk2Attack.color = Color.yellow;
        }
        else
        {
            atk2Attack.color = Color.white;
        }

        if (thisMonster.info.baseAttack2.Range.BaseValue != thisMonster.info.baseAttack2.Range.Value)
        {
            atk2Range.color = Color.yellow;
        }
        else
        {
            atk2Range.color = Color.white;
        }

        //if (thisMonster.tempStats.baseAttack2.AttackTime.BaseValue != thisMonster.tempStats.baseAttack2.AttackTime.Value)
        //{
        //    atk2Cool.color = Color.yellow;
        //}
        //else
        //{
        //    atk2Cool.color = Color.white;
        //}

        
        if (GetComponentInParent<YourHome>().activeMonster.expToLevel.ContainsKey(thisMonster.info.level))
        {
            

            int toNextLevel = GetComponentInParent<YourHome>().activeMonster.expToLevel[thisMonster.info.level + 1];
            int totalNextLevel = GetComponentInParent<YourHome>().activeMonster.totalExpForLevel[thisMonster.info.level + 1];
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

        skillName.text = thisMonster.info.skillName;
        skillDescription.text = thisMonster.info.passiveSkill.skill.description;

        if (thisMonster.info.isStar)
        {
            thisMonster.bodyParts.StarMonster();
        }
    }


    //open the equipment menu to change the monster's Equipment #1
    public void Equipment1Button()
    {
        equipMenu.SetActive(true);
        equipMenu.GetComponent<EquipmentManager>().ChangeEquipment(GetComponentInParent<YourHome>().activeMonster.GetComponent<Monster>(), 1);
        if (isEquip1)
        {
            GetComponentInParent<YourHome>().activeMonster.GetComponent<Monster>().UnEquipItem(equips1, 1);
            equips1.UnEquip();
            isEquip1 = false;
            equip1Btn.GetComponent<Image>().sprite = null;
            LoadInfo(GetComponentInParent<YourHome>().activeMonster.GetComponent<Monster>());
            
        }
        
    }   

    //open the equipment menu to change the monster's Equipment #2
    public void Equipment2Button()
    {
        equipMenu.SetActive(true);
        equipMenu.GetComponent<EquipmentManager>().ChangeEquipment(GetComponentInParent<YourHome>().activeMonster.GetComponent<Monster>(), 2);
        if (isEquip2)
        {
            GetComponentInParent<YourHome>().activeMonster.GetComponent<Monster>().UnEquipItem(equips2, 2);
            equips2.UnEquip();
            isEquip2 = false;
            equip2Btn.GetComponent<Image>().sprite = null;
            LoadInfo(GetComponentInParent<YourHome>().activeMonster.GetComponent<Monster>());
            
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



        if (isEquip1)
        {
            equips1.UnEquip();
        }
        if (isEquip2)
        {
            equips2.UnEquip();

        }

        Destroy(monster.gameObject);


        GetComponentInParent<YourHome>().MonsterListActive();
        //GetComponentInParent<YourHome>().sorter.Sort(SortMode.Attack);
        
        GetComponentInParent<YourHome>().LoadMonsters();
        gameObject.SetActive(false);
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        var items = GameManager.Instance.items.fullItemList;
        var equips = GameManager.Instance.items.allEquipsDict;
        var types = GameManager.Instance.monstersData.typeChartDict;
        var effects = GameManager.Instance.GetComponent<AllStatusEffects>().allStatusDict;
        var atkModes = GameManager.Instance.GetComponent<Attacks>().atkModeDict;

        if (eventData.pointerEnter)
        {
            var tag = eventData.pointerEnter.gameObject.tag;
            var hit = eventData.pointerEnter.gameObject;


            
            if (tag == "Equipment")
            {
                
              
            }

            if (tag == "MonsterDisplay")
            {
                monster.monsterMotion.SetBool("isClicked", true);
                
            }

            //checks to see if the item hit was a type. if it was, fill the box with information about the type
            if (types.ContainsKey(hit.name))
            {
                GameManager.Instance.DisplayPopMenu(types[hit.name]);
            }

            //if the player clicks on an icon, open a window that describes what the icon is
            if (tag == "ScriptableObject")
            {
                
                

                //checks to see if the item hit was an item. if it was, fill the box with information about the item
                if (items.ContainsKey(hit.name))
                {
                    
                    if (equips.ContainsKey(hit.name))
                    {
                       
                        GameManager.Instance.DisplayPopMenu(equips[hit.name]);
                    }
                    else
                    {
                        GameManager.Instance.DisplayPopMenu(items[hit.name]);
                    }
                }

                if (effects.ContainsKey(hit.name))
                {
                    GameManager.Instance.DisplayPopMenu(effects[hit.name]);
                }

                if (atkModes.ContainsKey(hit.name))
                {

                    GameManager.Instance.DisplayPopMenu(hit);
                }


                //popMenu.SetActive(true);
                //popMenu.GetComponent<PopMenuObject>().AcceptObject(hit.name, hit);
            }

        }



    }

    //when the attack 1 button is pushed, have the monster shoot a sample of the attack
    public void Attack1Btn()
    {
        MonsterAttack attack = monster.info.baseAttack1.attack;

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
        MonsterAttack attack = monster.info.baseAttack2.attack;
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

    //call this from the Items Button on the Panel, with the objective of using a consumable item on the active monster for this panel
   public void OpenItems()
    {
        gameObject.GetComponentInParent<YourHome>().itemsListObject.SetActive(true);
        gameObject.GetComponentInParent<YourHome>().itemsListObject.GetComponent<ItemManager>().LoadMonsterItems();
        gameObject.GetComponentInParent<YourHome>().itemsListObject.GetComponent<ItemManager>().activeMonster = monster;
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
