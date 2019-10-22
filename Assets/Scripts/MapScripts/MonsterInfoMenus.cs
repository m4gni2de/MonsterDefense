using System.Collections;
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
    public GameObject popMenuCanvas, popMenuObject;
    public GameObject towerButton;
    
    

    //objects related to the active monster's target moe
    public TMP_Dropdown targetModeDropdown;
    public TargetMode targetMode = new TargetMode();

    //variables for the motion of the Tower Menu
    public GameObject loadTowerBtn;
    //used to count the amount of pixels the menu has moved
    public int menuMovements;
    public bool isClicked;
    

    public bool isChecking;

    public GameObject towerBase;

    public TMP_Text monsterName, attack1BtnText, attack2BtnText, levelText, atkText, defText, speText, precText, typeText, toLevelText, evasText, enGenText, enCostText, staminaText;
    public Slider expSlider;
    public Image type1, type2;
    public GameObject equip1, equip2;
    public EnergyBar staminaBar;

    //bool to make sure the towers in the list of towers only spawns one time
    public bool towersFilled;

    //the main Camera on the map
    public Camera mainCamera;

    

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
        LoadYourTowers();

        List<string> targetModes = new List<string>();
        //the cap is 9 because there are currently 8 Target Modes, so this will fill them all
        for (int i = 0; i < 9; i++)
        {
            string mode = Enum.ToObject(typeof(TargetMode), i).ToString();
            targetModes.Add(mode);
        }

        targetModeDropdown.AddOptions(targetModes);
        targetModeDropdown.value = 0;

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
                            tower.gameObject.tag = "Tower";
                            tower.gameObject.name = tower.GetComponent<Monster>().info.species + " " + tower.GetComponent<Monster>().info.index;

                            SpriteRenderer[] sprites = tower.GetComponentsInChildren<SpriteRenderer>();

                            for (int s = 0; s < sprites.Length; s++)
                            {
                                sprites[s].sortingLayerName = "GameUI";
                            }
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

                    var equips = GameManager.Instance.items.allEquipmentDict;

                    GameObject[] e = GameObject.FindGameObjectsWithTag("Item");

                    for (int i = 0; i < e.Length; i++)
                    {
                        Destroy(e[i]);
                    }

                    //checks the monster's equipment and displays the matching sprites if there is equipment on the monster
                    if (equips.ContainsKey(activeMonster.info.equip1Name))
                    {
                        var e1 = Instantiate(equips[activeMonster.info.equip1Name].equipPrefab, equip1.transform.position, Quaternion.identity);
                        e1.transform.SetParent(equip1.transform);
                        e1.transform.tag = "Item";
                        e1.name = activeMonster.info.equip1Name;
                    }

                    //checks the monster's equipment and displays the matching sprites if there is equipment on the monster
                    if (equips.ContainsKey(activeMonster.info.equip2Name))
                    {
                        var e2 = Instantiate(equips[activeMonster.info.equip2Name].equipPrefab, equip2.transform.position, Quaternion.identity);
                        e2.transform.SetParent(equip1.transform);
                        e2.transform.tag = "Item";
                        e2.name = activeMonster.info.equip2Name;
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

            monsterName.text = activeMonster.info.name;
            attack1BtnText.text = activeMonster.info.attack1Name;
            attack2BtnText.text = activeMonster.info.attack2Name;

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


            //checks the monster's equipment and displays the matching sprites if there is equipment on the monster
            if (equips.ContainsKey(activeMonster.info.equip1Name))
            {
                equip1.GetComponent<Image>().color = Color.white;
                equip1.name = activeMonster.info.equip1Name;
                equip1.GetComponent<Image>().sprite = equips[activeMonster.info.equip1Name].sprite;
            }
            else
            {
                equip1.GetComponent<Image>().sprite = null;
                equip1.GetComponent<Image>().color = Color.clear;
                equip1.name = "Equip1";
            }

            if (equips.ContainsKey(activeMonster.info.equip2Name))
            {
                equip2.GetComponent<Image>().color = Color.white;
                equip2.name = activeMonster.info.equip2Name;
                equip2.GetComponent<Image>().sprite = equips[activeMonster.info.equip2Name].sprite;
            }
            else
            {
                equip2.GetComponent<Image>().sprite = null;
                equip2.GetComponent<Image>().color = Color.clear;
                equip2.name = "Equip2";
            }


            targetModeDropdown.value = (int)Enum.ToObject(typeof(TargetMode), activeMonster.GetComponent<Tower>().targetMode);
            staminaBar.BarProgress = activeMonster.GetComponent<Tower>().staminaBar.BarProgress;

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

    //use this to slide the tower menu back and forth towards its resting place or its open place
    public void MoveTowerMenu()
    {
        if (isClicked)
        {
            menuMovements += 1;

            if (menuMovements <= (170 * menuCanvas.transform.localScale.x))
            {
                loadTowerBtn.GetComponent<Button>().interactable = false;
                towerMenu.transform.Translate(Vector3.left, Space.World);
                loadTowerBtn.transform.Translate(Vector3.left, Space.World);
                mainCamera.GetComponent<CameraMotion>().isFree = false;
            }
            else
            {
                CancelInvoke("MoveTowerMenu");
                menuMovements = 0;
                loadTowerBtn.GetComponent<Button>().interactable = true;
                //mainCamera.GetComponent<CameraMotion>().isFree = !mainCamera.GetComponent<CameraMotion>().isFree;
            }
        }
        else
        {
            menuMovements += 1;


            if (menuMovements <= (170 * menuCanvas.transform.localScale.x))
            {
                loadTowerBtn.GetComponent<Button>().interactable = false;
                towerMenu.transform.Translate(Vector3.right, Space.World);
                loadTowerBtn.transform.Translate(Vector3.right, Space.World);
                mainCamera.GetComponent<CameraMotion>().isFree = false;
            }
            else
            {
                CancelInvoke("MoveTowerMenu");
                menuMovements = 0;
                loadTowerBtn.GetComponent<Button>().interactable = true;
                //mainCamera.GetComponent<CameraMotion>().isFree = !mainCamera.GetComponent<CameraMotion>().isFree;
            }
        }

    }



    //map this to a button to open the tower menu for players in game and load your available towers
    public void TowerMenuBtn()
    {
        
        towerMenu.SetActive(true);
        isClicked = !isClicked;
        InvokeRepeating("MoveTowerMenu", 0f, .001f);
        

        //GameManager.Instance.GetComponentInChildren<CameraMotion>().isFree = false;
    }

    public void TowerMenuClose()
    {
        mainCamera.GetComponent<CameraMotion>().isFree = true;
        towerMenu.SetActive(false);
        
        //GameManager.Instance.GetComponentInChildren<CameraMotion>().isFree = true;
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
        
        mainCamera.transform.position = new Vector3(activeMonster.transform.position.x, activeMonster.transform.position.y, -10f);
        mainCamera.orthographicSize = 35;
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if (eventData.pointerEnter)
        {
            var tag = eventData.pointerEnter.gameObject.tag;
            var hit = eventData.pointerEnter.gameObject;

            

            //objects with the scriptable object tag are image sprites that can be touched by the player to reveal information about the thing touched in a pop menu
            if (tag == "ScriptableObject" || tag == "Item")
            {
                var items = GameManager.Instance.items.fullItemList;
                var types = GameManager.Instance.monstersData.typeChartDict;

                Debug.Log(tag);

                //checks to see if the item hit was an item. if it was, fill the box with information about the item
                if (items.ContainsKey(hit.name))
                {
                        popMenuObject.SetActive(true);
                        popMenuObject.GetComponent<PopMenuObject>().AcceptObject(hit.name, items[hit.name]);
                }

                //checks to see if the item hit was a type. if it was, fill the box with information about the type
                if (types.ContainsKey(hit.name))
                {
                    popMenuObject.SetActive(true);
                    popMenuObject.GetComponent<PopMenuObject>().AcceptObject(hit.name, types[hit.name]);
                }

            }


            //if a UI object is tapped, turn the camera off
            if (hit.layer == 5)
            {
                mainCamera.GetComponent<CameraMotion>().isFree = false;
            }

        }



    }



    public void OnDisable()
    {
        GameObject[] e = GameObject.FindGameObjectsWithTag("Item");

        for (int i = 0; i < e.Length; i++)
        {
            Destroy(e[i]);
        }
    }


    //use this to disable the camera during certain events
    public void DisableCamera()
    {
        mainCamera.GetComponent<CameraMotion>().isFree = false;
    }

    //use this to disable the camera during certain events
    public void EnableCamera()
    {
        mainCamera.GetComponent<CameraMotion>().isFree = true;
    }



}
