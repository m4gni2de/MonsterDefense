using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class MonsterInfoMenus : MonoBehaviour
{
    public Monster activeMonster;
    private GameObject map;

    //variable for the window that pops up to show your list of unused towers, ready to place them
    public GameObject towerMenu, infoMenu, enemyInfoMenu;
    public GameObject menuContentView;
    public GameObject menuCanvas;

    //variables for the motion of the Tower Menu
    public GameObject loadTowerBtn;
    //used to count the amount of pixels the menu has moved
    public int menuMovements;
    public bool isClicked;
    

    public bool isChecking;

    public GameObject towerBase;

    public TMP_Text monsterName, attack1BtnText, attack2BtnText, levelText, atkText, defText, speText, precText, typeText, toLevelText, evasText, enGenText, enCostText;
    public Slider expSlider;

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
                //checks the Active Towers dictionary. If a monster appears in it, then it will add that monster to a local list of towers that are on the field
                //if (active.ContainsKey(i - 1))
                //{
                //    Monster monster = active[i - 1];
                //    indexes.Add(monster.info.index);
                //    Debug.Log(indexes[i]);
                //}


                if (monsters.ContainsKey(i))
                {
                    string monsterJson = monsters[i];
                    var info = JsonUtility.FromJson<MonsterInfo>(monsterJson);


                    string species = info.species;

                    //if the monster appears on the Active Towers list, skip over the spawning of it
                    //if (indexes.Contains(i))
                    //{
                    //    //
                    //}
                    //else
                    //{
                        if (monstersDict.ContainsKey(species))
                        {

                            //int towerCount = GameManager.Instance.activeTowers.Count;
                            var tower = Instantiate(monstersDict[species].monsterPrefab, menuContentView.transform.position, Quaternion.identity);
                            //tower.transform.position = new Vector3(towers[i - 1].transform.position.x, towers[i - 1].transform.position.y, tower.transform.position.z);
                            tower.transform.SetParent(menuContentView.transform, false);
                            tower.transform.position = new Vector3(towerBase.transform.position.x, towerBase.transform.position.y - ((i - 1) * 60), tower.transform.position.z);
                            tower.transform.localScale = new Vector3(tower.transform.localScale.x * 2, tower.transform.localScale.y * 2, tower.transform.localScale.z);
                            //***************************HERE************************
                            tower.GetComponent<Monster>().isTower = true;
                            tower.GetComponent<Monster>().GetComponent<Enemy>().enemyCanvas.SetActive(false);
                            tower.GetComponent<Monster>().info = JsonUtility.FromJson<MonsterInfo>(monsters[i]);
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
                    
                }
                
            }
        }


        CheckMonster();

        if (activeMonster)
        {


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
            atkText.text = "Atk: " + Math.Round(activeMonster.tempStats.Attack.Value, 0).ToString();
            defText.text = "Def: " + Math.Round(activeMonster.tempStats.Defense.Value, 0).ToString();
            speText.text = "Speed: " + Math.Round(activeMonster.tempStats.Speed.Value, 0).ToString();
            precText.text = "Prec: " + Math.Round(activeMonster.tempStats.Precision.Value, 0).ToString();
            typeText.text = "Type: " + activeMonster.info.type1 + "/" + activeMonster.info.type2;
            evasText.text = "Evasion: " + Math.Round(activeMonster.tempStats.evasionBase, 0) + "%";
            enGenText.text = "En Gen: " + Math.Round((activeMonster.tempStats.EnergyGeneration.Value / 60), 2) + " /s";
            enCostText.text = "Cost: " + activeMonster.tempStats.EnergyCost;

            if (activeMonster.expToLevel.ContainsKey(activeMonster.info.level))
            {
                int toNextLevel = activeMonster.expToLevel[activeMonster.info.level + 1];
                int totalNextLevel = activeMonster.totalExpForLevel[activeMonster.info.level + 1];
                int nextLevelDiff = totalNextLevel - activeMonster.info.totalExp;

                expSlider.maxValue = toNextLevel;
                expSlider.value = toNextLevel - nextLevelDiff;

                toLevelText.text = "EXP Until Level Up: " + nextLevelDiff.ToString();
            }


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
            }
            else
            {
                CancelInvoke("MoveTowerMenu");
                menuMovements = 0;
                loadTowerBtn.GetComponent<Button>().interactable = true;
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
            }
            else
            {
                CancelInvoke("MoveTowerMenu");
                menuMovements = 0;
                loadTowerBtn.GetComponent<Button>().interactable = true;
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
        towerMenu.SetActive(false);
        //GameManager.Instance.GetComponentInChildren<CameraMotion>().isFree = true;
    }

    public void DeleteTower()
    {

    }

    //THIS SUMMONS A NEW MONSTER TO BE USED AS A TOWER AND CREATES A PLAYERPREF FOR IT
    public void SummonTower()
    {
        map = GameObject.FindGameObjectWithTag("Map");


        var random = UnityEngine.Random.Range(1, GameManager.Instance.monstersData.monstersByIdDict.Count + 1);
        var byId = GameManager.Instance.monstersData.monstersByIdDict;
        var monstersDict = GameManager.Instance.monstersData.monstersAllDict;

        //picks a random number. then translates that number to the Monsters by Id Dictionary. Then takes that number, and summons a prefab based on the name of the matching key
        if (byId.ContainsKey(random))
        {
            string species = byId[random];

            if (monstersDict.ContainsKey(species))
            {

                var monster = Instantiate(monstersDict[species].monsterPrefab, map.transform.position, Quaternion.identity);
                monster.transform.SetParent(map.gameObject.transform, true);
                monster.transform.position = new Vector3(0f, 0f, 10f);


                monster.GetComponent<Monster>().SummonNewMonster(monster.GetComponent<Monster>().info.species);
            }
        }


    }

    public void InfoMenuOpen()
    {

    }

    public void InfoMenuClose()
    {

    }


    //use this to select the monster's first attack
    public void Attack1Btn()
    {
        activeMonster.GetComponent<Tower>().attackNumber = 1;
        monsterName.text = activeMonster.info.name;
        activeMonster.GetComponent<Tower>().AttackRangeUI();
    }

    //use this to select the monster's first attack
    public void Attack2Btn()
    {
        activeMonster.GetComponent<Tower>().attackNumber = 2;
        monsterName.text = activeMonster.info.name;
        activeMonster.GetComponent<Tower>().AttackRangeUI();
    }

    //use this to snap the camera to the active monster
    public void FindMonsterBtn()
    {
        
        mainCamera.transform.position = new Vector3(activeMonster.transform.position.x, activeMonster.transform.position.y, -10f);
        mainCamera.orthographicSize = 35;
        
    }

}
