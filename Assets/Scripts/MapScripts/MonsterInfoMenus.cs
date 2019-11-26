﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class MonsterInfoMenus : MonoBehaviour, IPointerDownHandler
{
    public Monster activeMonster;
    private GameObject map;

    //variable for the window that pops up to show your list of unused towers, ready to place them
    public GameObject towerMenu, infoMenu, enemyInfoMenu;
    public GameObject menuContentView;
    public GameObject menuCanvas;
    public GameObject popMenuCanvas;
    public GameObject towerButton;



    //objects related to the active monster's target moe
    public TMP_Dropdown targetModeDropdown;
    public TargetMode targetMode = new TargetMode();

    //variables for the motion of the Tower Menu
    public GameObject loadTowerBtn;
    //used to count the amount of pixels the menu has moved
    public int menuMovements;
    public bool isClicked;

    public Button findMonsterBtn, showTowersBtn, hideTowersBtn;

    public bool isChecking;

    public GameObject towerBase;

    public TMP_Text monsterName, attack1BtnText, attack2BtnText, levelText, atkText, defText, speText, precText, typeText, toLevelText, evasText, enGenText, enCostText, staminaText;
    public TMP_Text atk1Power, atk1Range, atk2Power, atk2Range;
    public SpriteRenderer atk1TypeSp, atk2TypeSp;
    public Slider expSlider;
    public Image type1, type2;
    public GameObject equip1, equip2;
    public EquipmentScript e1, e2;
    public EnergyBar staminaBar;

    //bool to make sure the towers in the list of towers only spawns one time
    public bool towersFilled;

    //the main Camera on the map
    public Camera mainCamera;

    //list that holds the sprite effects on the items and monster
    public List<string> effectTypes = new List<string>();

    //use this to preload a tile that a monster can be placed on, if it isn't already. this value is gotten from the MapTileMenu script
    public MapTile tileToBePlaced;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {

        

        List<string> targetModes = new List<string>();
        //the cap is 9 because there are currently 8 Target Modes, so this will fill them all
        for (int i = 0; i < 9; i++)
        {
            string mode = Enum.ToObject(typeof(TargetMode), i).ToString();
            targetModes.Add(mode);
        }

        targetModeDropdown.AddOptions(targetModes);
        targetModeDropdown.value = 0;

        e1 = ScriptableObject.CreateInstance<EquipmentScript>();
        e2 = ScriptableObject.CreateInstance<EquipmentScript>();

    }

    public void LoadYourTowers()
    {
        if (!towersFilled)
        {
            towersFilled = true;
            var monsters = GameManager.Instance.GetComponent<YourMonsters>().yourMonstersDict;
            var byId = GameManager.Instance.monstersData.monstersByIdDict;
            var monstersDict = GameManager.Instance.monstersData.monstersAllDict;
            var active = GameManager.Instance.activeTowers;
            //int index = new int();

            List<int> indexes = new List<int>();

            for (int i = 1; i <= monsters.Count; i++)
            {

                if (monsters.ContainsKey(i))
                {
                    string monsterJson = monsters[i];
                    var info = JsonUtility.FromJson<MonsterInfo>(monsterJson);


                    string species = info.species;
                    if (monstersDict.ContainsKey(species))
                    {
                        var tower = Instantiate(monstersDict[species].monsterPrefab, menuContentView.transform.position, Quaternion.identity);
                        tower.transform.SetParent(menuContentView.transform, false);
                        tower.transform.position = new Vector3(towerBase.transform.position.x, towerBase.transform.position.y - ((i - 1) * 60), -2f);
                        tower.transform.localScale = new Vector3(tower.transform.localScale.x * 3.5f, tower.transform.localScale.y * 3.5f, tower.transform.localScale.z);
                        tower.GetComponent<Monster>().isTower = true;
                        tower.GetComponent<Monster>().GetComponent<Enemy>().enemyCanvas.SetActive(false);
                        //tower.GetComponent<Monster>().info = JsonUtility.FromJson<MonsterInfo>(monsters[i]);
                        tower.GetComponent<Monster>().saveToken = JsonUtility.FromJson<MonsterSaveToken>(monsters[i]);
                        tower.GetComponent<Monster>().LoadMonsterToken(tower.GetComponent<Monster>().saveToken);
                        tower.GetComponent<Monster>().MonsterEquipment();
                        tower.GetComponent<Monster>().monsterIcon.GetComponentInChildren<MonsterIcon>().DisplayMonster(tower.GetComponent<Monster>());
                        tower.GetComponent<Monster>().monsterIcon.GetComponentInChildren<MonsterIcon>().IconVisibility("GameUI");

                        tower.gameObject.tag = "Tower";
                        tower.gameObject.name = tower.GetComponent<Monster>().info.species + " " + tower.GetComponent<Monster>().info.index;

                        //SpriteRenderer[] sprites = tower.GetComponentsInChildren<SpriteRenderer>();

                        //for (int s = 0; s < sprites.Length; s++)
                        //{
                        //    sprites[s].sortingLayerName = "GameUI";
                        //}
                        //}
                    }


                }


                //*************************************88

            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            var equips = GameManager.Instance.items.allEquipsDict;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            //if this tower is clicked on, it can be placed
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Tower")
                {
                    //GameManager.Instance.OverworldMenuControl();
                    //GameManager.Instance.overworldMenu.GetComponentInChildren<OverworldInfoMenu>().activeMonster = hit.collider.gameObject.GetComponent<Monster>();

                    infoMenu.SetActive(true);
                    enemyInfoMenu.SetActive(false);
                    activeMonster = hit.collider.gameObject.GetComponent<Monster>();

                    //checks the monster's equipment and displays the matching sprites if there is equipment on the monster
                    if (equips.ContainsKey(activeMonster.info.equip1Name))
                    {
                        e1.DeactivateItem(e1, equip1);
                        e1 = equips[activeMonster.info.equip1Name];
                        //equip1.GetComponent<Image>().color = Color.white;
                        equip1.name = activeMonster.info.equip1Name;
                        equip1.GetComponent<SpriteRenderer>().sprite = equips[activeMonster.info.equip1Name].sprite;

                        e1.ActivateItem(e1, equip1);
                    }
                    else
                    {
                        equip1.GetComponent<SpriteRenderer>().sprite = null;
                        //equip1.GetComponent<Image>().color = Color.clear;
                        equip1.name = "Equip1";
                        e1.DeactivateItem(e1, equip1);
                        //e1 = null;
                        
                    }

                    if (equips.ContainsKey(activeMonster.info.equip2Name))
                    {
                        e2.DeactivateItem(e2, equip2);
                        e2 = equips[activeMonster.info.equip2Name];
                        //equip2.GetComponent<Image>().color = Color.white;
                        equip2.name = activeMonster.info.equip2Name;
                        equip2.GetComponent<SpriteRenderer>().sprite = equips[activeMonster.info.equip2Name].sprite;
                        e2.ActivateItem(e2, equip2);
                    }
                    else
                    {
                        equip2.GetComponent<SpriteRenderer>().sprite = null;
                        equip2.name = "Equip2";
                        e2.DeactivateItem(e2, equip2);
                        //e2 = null;
                        //equip2.GetComponent<Image>().color = Color.clear;
                        
                    }

                }

            }
        }


        CheckMonster();

        if (activeMonster)
        {
            type1.GetComponent<Image>();
            type2.GetComponent<Image>();


            var types = GameManager.Instance.monstersData.typeChartDict;
            var equips = GameManager.Instance.items.allEquipsDict;

            //monsterName.text = activeMonster.info.name;
            //attack1BtnText.text = activeMonster.info.attack1Name;
            //atk1Power.text = activeMonster.info.attack1.Power.Value.ToString();
            //atk1Range.text = activeMonster.info.attack1.Range.Value.ToString();
            //atk1TypeSp.sprite = types[activeMonster.info.attack1.type].typeSprite;

            //attack2BtnText.text = activeMonster.info.attack2Name;
            //atk2Power.text = activeMonster.info.attack2.Power.Value.ToString();
            //atk2Range.text = activeMonster.info.attack2.Range.Value.ToString();
            //atk2TypeSp.sprite = types[activeMonster.info.attack2.type].typeSprite;

            monsterName.text = activeMonster.info.name;
            attack1BtnText.text = activeMonster.info.attack1Name;
            atk1Power.text = activeMonster.info.baseAttack1.Power.Value.ToString();
            atk1Range.text = activeMonster.info.baseAttack1.Range.Value.ToString();
            atk1TypeSp.sprite = types[activeMonster.info.baseAttack1.type].typeSprite;

            attack2BtnText.text = activeMonster.info.attack2Name;
            atk2Power.text = activeMonster.info.baseAttack2.Power.Value.ToString();
            atk2Range.text = activeMonster.info.baseAttack2.Range.Value.ToString();
            atk2TypeSp.sprite = types[activeMonster.info.baseAttack2.type].typeSprite;


            //levelText.text = "Level: " + activeMonster.info.level.ToString();
            //atkText.text = "Atk: " + Math.Round(activeMonster.attack, 0).ToString();
            //defText.text = "Def: " + Math.Round(activeMonster.defense, 0).ToString();
            //speText.text = "Speed: " + Math.Round(activeMonster.speed, 0).ToString();
            //precText.text = "Prec: " + Math.Round(activeMonster.precision, 0).ToString();
            //typeText.text = "Type: " + activeMonster.info.type1 + "/" + activeMonster.info.type2;
            //evasText.text = "Evasion: " + Math.Round(activeMonster.evasion, 0) + "%";
            //enGenText.text = "En Gen: " + Math.Round((activeMonster.energyGeneration / 60), 2) + " /s";
            //enCostText.text = "Cost: " + activeMonster.energyCost;

            levelText.text = "Level: " + activeMonster.info.level.ToString();
            atkText.text = "Atk: " + Math.Round(activeMonster.info.Attack.Value, 0).ToString();
            defText.text = "Def: " + Math.Round(activeMonster.info.Defense.Value, 0).ToString();
            speText.text = "Speed: " + Math.Round(activeMonster.info.Speed.Value, 0).ToString();
            precText.text = "Prec: " + Math.Round(activeMonster.info.Precision.Value, 0).ToString();
            //typeText.text = "Type: " + activeMonster.info.type1 + "/" + activeMonster.info.type2;
            evasText.text = "Evasion: " + Math.Round(activeMonster.info.evasionBase, 0) + "%";
            enGenText.text = "En Gen: " + Math.Round((activeMonster.info.EnergyGeneration.Value / 60), 2) + " /s";
            enCostText.text = "Cost: " + activeMonster.info.EnergyCost.Value;
            staminaText.text = "Stamina: " + activeMonster.tempStats.Stamina.Value;

            

            if (activeMonster.expToLevel.ContainsKey(activeMonster.info.level))
            {
                int toNextLevel = activeMonster.expToLevel[activeMonster.info.level + 1];
                int totalNextLevel = activeMonster.totalExpForLevel[activeMonster.info.level + 1];
                int nextLevelDiff = totalNextLevel - activeMonster.info.totalExp;

                expSlider.maxValue = toNextLevel;
                expSlider.value = toNextLevel - nextLevelDiff;

                
                toLevelText.text = "EXP Until Level Up: " + nextLevelDiff.ToString();
            }

            //checks the monster's type against the type sprites and changes their images to match the correct type sprite
            if (types.ContainsKey(activeMonster.info.type1))
            {
                type1.GetComponent<Image>().sprite = types[activeMonster.info.type1].typeSprite;
                type1.GetComponent<Image>().color = Color.white;
                //type1.transform.localScale = new Vector3(3.5f, 1.25f, type1.transform.localScale.z);
                type1.name = activeMonster.info.type1;
            }
            else
            {
                type1.GetComponent<Image>().sprite = null;
                type1.name = "Type1";
            }

            if (types.ContainsKey(activeMonster.info.type2))
            {
                type2.GetComponent<Image>().sprite = types[activeMonster.info.type2].typeSprite;
                type2.GetComponent<Image>().color = Color.white;
                //type2.transform.localScale = new Vector3(3.5f, 1.25f, type2.transform.localScale.z);
                type2.name = activeMonster.info.type2;
            }
            else
            {
                type2.GetComponent<Image>().sprite = null;
                type2.GetComponent<Image>().color = Color.clear;
                type2.name = "Type2";
            }

            targetModeDropdown.value = (int)Enum.ToObject(typeof(TargetMode), activeMonster.GetComponent<Tower>().targetMode);
            staminaBar.BarProgress = activeMonster.GetComponent<Tower>().staminaBar.BarProgress;


            //set the status of the find/place monster button
            if (activeMonster.GetComponent<Tower>().mapTileOn != null)
            {
                findMonsterBtn.interactable = true;
                findMonsterBtn.GetComponentInChildren<TMP_Text>().text = "Find Monster";
            }
            else
            {
                findMonsterBtn.GetComponentInChildren<TMP_Text>().text = "Summon Monster";


                if (tileToBePlaced != null)
                {
                    findMonsterBtn.interactable = true;
                   
                }
                else
                {
                    findMonsterBtn.interactable = false;
                }
            }

            

            //if (activeMonster.GetComponent<Tower>().
            //indicator.transform.position = new Vector2(activeMonster.specs.head.transform.position.x, activeMonster.specs.head.transform.position.y + 40);
        }
    }

    //brings up the current tower's stats on the window if you click on the monster
    public void CheckMonster()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isChecking)
            {
                isChecking = true;
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

                
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.tag == "Tower")
                    {
                        activeMonster = hit.collider.gameObject.GetComponent<Monster>();
                        int index = activeMonster.activeIndex;
                        int activeAttack = activeMonster.GetComponent<Tower>().attackNumber;

                        if (GameManager.Instance.activeTowers.ContainsKey(index))
                        {
                            monsterName.text = GameManager.Instance.activeTowers[index].info.name;
                            attack1BtnText.text = GameManager.Instance.activeTowers[index].info.attack1Name;
                            attack2BtnText.text = GameManager.Instance.activeTowers[index].info.attack2Name;

                        }

                    }
                }
            }
            //if the monster is showing its attack ranges, when you click off of it, the ranges go away
            else
            {
                isChecking = false;
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);


                if (hit.collider != null && activeMonster != null)
                {

                    activeMonster.GetComponent<Tower>().RangeUIOff();

                }
            }
        }
    }

    



    


    public void DeleteTower()
    {

    }

    ////THIS SUMMONS A NEW MONSTER TO BE USED AS A TOWER AND CREATES A PLAYERPREF FOR IT
    //public void SummonTower()
    //{
    //    map = GameObject.FindGameObjectWithTag("Map");


    //    var random = UnityEngine.Random.Range(1, GameManager.Instance.monstersData.monstersByIdDict.Count + 1);
    //    var byId = GameManager.Instance.monstersData.monstersByIdDict;
    //    var monstersDict = GameManager.Instance.monstersData.monstersAllDict;

    //    //picks a random number. then translates that number to the Monsters by Id Dictionary. Then takes that number, and summons a prefab based on the name of the matching key
    //    if (byId.ContainsKey(random))
    //    {
    //        string species = byId[random];

    //        if (monstersDict.ContainsKey(species))
    //        {

    //            var monster = Instantiate(monstersDict[species].monsterPrefab, map.transform.position, Quaternion.identity);
    //            monster.transform.SetParent(map.gameObject.transform, true);
    //            monster.transform.position = new Vector3(0f, 0f, 10f);


    //            monster.GetComponent<Monster>().SummonNewMonster(monster.GetComponent<Monster>().info.species);
    //        }
    //    }


    //}

    public void InfoMenuOpen()
    {

    }

    public void InfoMenuClose()
    {

    }


    //use this to change the target mode of the monster to match the value of the drop down
    public void MonsterTargetMode()
    {
        activeMonster.GetComponent<Tower>().targetMode = (TargetMode)Enum.ToObject(typeof(TargetMode), targetModeDropdown.value);
    }


    //use this to select the monster's first attack
    public void Attack1Btn()
    {
        activeMonster.GetComponent<Tower>().attackNumber = 1;
        monsterName.text = activeMonster.info.name;
        activeMonster.GetComponent<Tower>().AttackRangeUI();
        activeMonster.monsterMotion.GetComponent<MotionControl>().ResetAttacks();
    }

    //use this to select the monster's first attack
    public void Attack2Btn()
    {
        activeMonster.GetComponent<Tower>().attackNumber = 2;
        monsterName.text = activeMonster.info.name;
        activeMonster.GetComponent<Tower>().AttackRangeUI();
        activeMonster.monsterMotion.GetComponent<MotionControl>().ResetAttacks();

    }

    //use this to snap the camera to the active monster
    public void FindMonsterBtn()
    {
        //if the monster is already placed on the map, move the camera to it. if it's yet to be placed, the button summons the monster to the active tile
        if (activeMonster.GetComponent<Tower>().isPlaced)
        {
            mainCamera.transform.position = new Vector3(activeMonster.transform.position.x, activeMonster.transform.position.y, -10f);
            mainCamera.orthographicSize = 35;

            return;
        }
        
        //if the active monster is not yet placed, and there is a tile loaded, place the monster on that tile
        if (!activeMonster.GetComponent<Tower>().isPlaced && tileToBePlaced != null && !tileToBePlaced.isRoad && !tileToBePlaced.hasMonster)
        {
            activeMonster.GetComponent<Tower>().mapTileOn = tileToBePlaced;
            activeMonster.GetComponent<Tower>().StartCoroutine("PlaceTower");
            
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        if (eventData.pointerEnter)
        {
            var tag = eventData.pointerEnter.gameObject.tag;
            var hit = eventData.pointerEnter.gameObject;
            var name = eventData.pointerEnter.gameObject.name;



            //objects with the scriptable object tag are image sprites that can be touched by the player to reveal information about the thing touched in a pop menu
            if (tag == "ScriptableObject" || tag == "Item")
            {
                var items = GameManager.Instance.items.fullItemList;
                var equips = GameManager.Instance.items.allEquipsDict;
                var types = GameManager.Instance.monstersData.typeChartDict;

                Debug.Log(name);

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

                //checks to see if the item hit was a type. if it was, fill the box with information about the type
                if (types.ContainsKey(hit.name))
                {
                    
                    GameManager.Instance.DisplayPopMenu(types[hit.name]);
                }

            }




           
        }



    }



    public void OnDisable()
    {
       
    }


    //use this to disable the camera during certain events
    public void DisableCamera()
    {
        //mainCamera.GetComponent<CameraMotion>().oldTouchPositions[0] = null;
        //mainCamera.GetComponent<CameraMotion>().oldTouchPositions[1] = null;
        //mainCamera.GetComponent<CameraMotion>().isFree = false;

    }

    //use this to disable the camera during certain events
    public void EnableCamera()
    {
        mainCamera.GetComponent<CameraMotion>().isFree = true;
    }



}
