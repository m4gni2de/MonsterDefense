using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class YourHome : MonoBehaviour, IPointerDownHandler
{
    public GameObject homeCanvas, infoMenu, deleteButton, menuCanvas, equipListObject, equipPlacement, sortMenu, scrollContent, itemsListObject;

    public GameObject monsterSprite;
    public GameObject[] monsterSprites;

    //keep an array of monsters to be accessed on this script
    public GameObject[] monsterList;
    public int monsterSpriteTotal;

    //list of monsters in the array
    public List<GameObject> iconsList;

    //public TMP_Text monsterName, levelText, atkText, defText, speText, precText, typeText, toNextLevelText;
    public Monster activeMonster;

    public Slider expSlider;

    private float touchTime, acumTime, releaseTime;
    public GameObject monsterInfoMenu, accountInfoMenu, monsterScrollList, consumableObject;

    public SortMonsters sorter;
    public TMP_Text sortModeText;

    private void Awake()
    {
        monsterSpriteTotal = 0;
        

        for (int c = 0; c < 20; c++)
        {
            for (int r = 0; r < 5; r++)
            {
                monsterSprites[monsterSpriteTotal] = Instantiate(monsterSprite, homeCanvas.transform.position, Quaternion.identity);
                monsterSprites[monsterSpriteTotal].transform.SetParent(scrollContent.transform, true);
                monsterSprites[monsterSpriteTotal].transform.position = new Vector3(monsterSprite.transform.position.x + (r * 96), monsterSprite.transform.position.y - (c * 75), monsterSprite.transform.position.z);
                monsterSpriteTotal += 1;
            }

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        sorter.sortMode = SortMode.Index;

        

        expSlider.GetComponent<Slider>();
        LoadMonsters();

        //foreach(ConsumableItem item in GameManager.Instance.GetComponent<Items>().consumables)
        //{
        //    var x = Instantiate(consumableObject, homeCanvas.transform.position, Quaternion.identity);
        //    x.GetComponent<ConsumableObject>().consumableItem = item;
        //    x.GetComponent<ConsumableObject>().LoadItem();
        //    x.transform.localScale = new Vector3(x.transform.localScale.x * 100, x.transform.localScale.y * 100, x.transform.localScale.z);
        //}

        //SummonItem();

        GameManager.Instance.GetComponent<YourAccount>().account.coins += 1000;

    }

    public void LoadMonsters()
    {

        //if there are monsters that are showing, delete them to avoid duplicates
        GameObject[] mons = GameObject.FindGameObjectsWithTag("MonsterIcon");

        monsterSpriteTotal = 0;

        if (iconsList.Count > 0)
        {

            for (int m = 0; m < iconsList.Count; m++)
            {
                

                //monsterSprites[m].GetComponent<Image>().sprite = null;
                //monsterSprites[m].GetComponent<Image>().color = Color.clear;
                //monsterSprites[m].GetComponent<MonsterHomeIcon>().nameText.text = "";
                //monsterSprites[m].GetComponent<MonsterHomeIcon>().levelText.text = "";
                Destroy(iconsList[m]);

                //if (m >= iconsList.Count - 1)
                if (m >= iconsList.Count - 1)
                {
                    iconsList.Clear();
                    DisplayMonsters();
                    
                    return;
                }
            }
        }
        else
        {
            iconsList.Clear();
            DisplayMonsters();
        }


        //monsterSpriteTotal = 0;

        //if (mons.Length > 0)
        //{

        //    for (int m = 0; m <= mons.Length; m++)
        //    {
        //        Debug.Log(m);

        //        //monsterSprites[m].GetComponent<Image>().sprite = null;
        //        //monsterSprites[m].GetComponent<Image>().color = Color.clear;
        //        //monsterSprites[m].GetComponent<MonsterHomeIcon>().nameText.text = "";
        //        //monsterSprites[m].GetComponent<MonsterHomeIcon>().levelText.text = "";
        //        Destroy(mons[m]);

        //        if (m > mons.Length)
        //        {
        //            DisplayMonsters();
        //            break;
        //        }
        //    }
        //}
        //else
        //{
        //    DisplayMonsters();
        //}
    }

    public void DisplayMonsters()
    {
        GameManager.Instance.GetComponent<YourMonsters>().GetYourMonsters();

        
        var monsters = GameManager.Instance.GetComponent<YourMonsters>().yourMonstersDict;
        //var monsters = GameManager.Instance.GetComponent<YourMonsters>().yourMonsterTokens;
        var byId = GameManager.Instance.monstersData.monstersByIdDict;
        var monstersDict = GameManager.Instance.monstersData.monstersAllDict;
        var active = GameManager.Instance.activeTowers;

        //var playerInfo = GameManager.Instance.GetComponent<YourAccount>().account;
        //string yourMonsters = Application.persistentDataPath + "/Saves/" + playerInfo.username + "/monsters.txt";

        //string[] lines = File.ReadAllLines(yourMonsters);

        //StreamWriter monsterFile;




        //int index = new int();

        //List<int> indexes = new List<int>();

        string mode = PlayerPrefs.GetString("SortMode", "Index");
        sortModeText.text = "Sorted By: " + mode;
        

        for (int i = 1; i <= monsters.Count; i++)
        {
            ////checks the Active Towers dictionary. If a monster appears in it, then it will add that monster to a local list of towers that are on the field
            //if (active.ContainsKey(i))
            //{
            //    Monster monster = active[i];
            //    indexes.Add(monster.info.index);
            //    Debug.Log(indexes[i]);
            //}


            if (monsters.ContainsKey(i))
            {
                string monsterJson = monsters[i];
                var info = JsonUtility.FromJson<MonsterSaveToken>(monsterJson);
                //var info = JsonUtility.FromJson<MonsterInfo>(monsterJson);
                //monsterFile = File.AppendText(yourMonsters);
                //monsterFile.WriteLine(monsterJson);
                //monsterFile.Close();
            string species = info.species;

                if (monstersDict.ContainsKey(species))
                {
                var monster = Instantiate(monstersDict[species].monsterPrefab, monsterSprites[i - 1].transform.position, Quaternion.identity);
                monster.transform.SetParent(scrollContent.transform, true);

                    
                monster.GetComponent<Tower>().boneStructure.SetActive(false);
                monster.GetComponent<Monster>().monsterIcon.transform.localScale = new Vector3(20f, 20f, transform.localScale.z);
                monster.tag = "MonsterIcon";
                    


                monster.GetComponent<Monster>().saveToken = JsonUtility.FromJson<MonsterSaveToken>(monsters[i]);
                monster.GetComponent<Monster>().DisplayIcon();
                monster.GetComponent<Monster>().MonsterEquipment();
                 monster.GetComponent<Monster>().monsterIcon.GetComponentInChildren<MonsterIcon>().DisplayMonster(monster.GetComponent<Monster>());
                    monster.GetComponent<Monster>().monsterIcon.GetComponentInChildren<MonsterIcon>().DisplayCorrectText(sorter.sortMode);

                    iconsList.Add(monster);

                  
                monsterSpriteTotal += 1;

              
                }
                //}


            }


            //*************************************88

        }
    }

    

    public void DeleteMonster()
    {
        

        if (GameManager.Instance.GetComponent<YourMonsters>().yourMonstersDict.Count > 1)
        {
            int indexDeleted = activeMonster.info.index;

            Debug.Log(indexDeleted);

            
                //deletes the key of the monster being destroyed
                PlayerPrefs.DeleteKey(activeMonster.info.index.ToString());
                GameManager.Instance.GetComponent<YourMonsters>().yourMonstersComplete.Clear();

                //checks all of your current monsters, and if their index is ABOVE that of the deleted monster, reduce them all by 1 
                for (int i = 1; i <= GameManager.Instance.monsterCount; i++)
                {
                    string monsterJson = GameManager.Instance.GetComponent<YourMonsters>().yourMonstersDict[i];
                    //var info = JsonUtility.FromJson<MonsterInfo>(monsterJson);
                    var info = JsonUtility.FromJson<MonsterSaveToken>(monsterJson);
                    //Debug.Log(i);

                    if (info.index > indexDeleted)
                    {
                        info.index -= 1;
                        PlayerPrefs.SetString(info.index.ToString(), JsonUtility.ToJson(info));
                        

                    }

                    if (i >= GameManager.Instance.monsterCount)
                    {
                        PlayerPrefs.DeleteKey(GameManager.Instance.monsterCount.ToString());
                        GameManager.Instance.monsterCount -= 1;
                        PlayerPrefs.SetInt("MonsterCount", GameManager.Instance.monsterCount);
                        GameManager.Instance.GetComponent<YourMonsters>().GetYourMonsters();
                        //Debug.Log(PlayerPrefs.GetInt("MonsterCount"));
                        var position = activeMonster.transform.position;
                        Destroy(activeMonster.gameObject);
                        infoMenu.SetActive(false);
                        monsterScrollList.SetActive(true);
                        LoadMonsters();
                    }

                    
                }


            //PlayerPrefs.DeleteKey(GameManager.Instance.monsterCount.ToString());
            //GameManager.Instance.monsterCount -= 1;
            //PlayerPrefs.SetInt("MonsterCount", GameManager.Instance.monsterCount);
            //GameManager.Instance.GetComponent<YourMonsters>().GetYourMonsters();
            //Debug.Log(PlayerPrefs.GetInt("MonsterCount"));


           


        }
        else
        {

        }

    }

    //THIS SUMMONS A NEW MONSTER TO BE USED AS A TOWER AND CREATES A PLAYERPREF FOR IT
    public void SummonTower()
    {
        //var map = GameObject.FindGameObjectWithTag("Map");


        var random = Random.Range(1, GameManager.Instance.monstersData.monstersByIdDict.Count + 1);
        var byId = GameManager.Instance.monstersData.monstersByIdDict;
        var monstersDict = GameManager.Instance.monstersData.monstersAllDict;

        //picks a random number. then translates that number to the Monsters by Id Dictionary. Then takes that number, and summons a prefab based on the name of the matching key
        if (byId.ContainsKey(random))
        {
            string species = byId[random];

            if (monstersDict.ContainsKey(species))
            {
                var position = monsterSprites[monsterSpriteTotal].transform.position;
                //var monster = Instantiate(monstersDict[species].monsterPrefab, monsterSprites[monsterSpriteTotal - 1].transform.position, Quaternion.identity);



                //var monster = Instantiate(monstersDict[species].monsterPrefab, homeCanvas.transform.position, Quaternion.identity);
                //monsterList[monsterSpriteTotal] = Instantiate(monstersDict[species].monsterPrefab, scrollContent.transform.position, Quaternion.identity);
                var monster = Instantiate(monstersDict[species].monsterPrefab, scrollContent.transform.position, Quaternion.identity);
                //var monster = monsterList[monsterSpriteTotal];
                //monster.transform.SetParent(homeCanvas.transform, true);
                monster.transform.SetParent(scrollContent.transform, true);
                monster.GetComponent<Monster>().monsterIcon.SetActive(true);
                
                monster.GetComponent<Tower>().boneStructure.SetActive(false);
                monster.GetComponent<Monster>().GetComponent<Enemy>().enemyCanvas.SetActive(false);
                //monster.transform.position = new Vector3(0f, 0f, 10f);
                monster.transform.position = position;
                monster.tag = "MonsterIcon";
                monster.GetComponent<Monster>().MonsterSummon(monster.GetComponent<Monster>().info.species);
                //monster.GetComponent<Monster>().SummonNewMonster(monster.GetComponent<Monster>().info.species);
                //monster.transform.position = spawnPoint;
                monster.GetComponent<Monster>().monsterIcon.transform.localScale = new Vector3(transform.localScale.x * 3, transform.localScale.y * 3, transform.localScale.z);
                //monster.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
                iconsList.Add(monster);
                activeMonster = monster.GetComponent<Monster>();
                monsterSpriteTotal += 1;
                GameManager.Instance.GetComponent<YourAccount>().account.totalMonstersCollected += 1;
                
                LoadMonsters();
            }
        }

        


    }

    //THIS SUMMONS A NEW MONSTER TO BE USED AS A TOWER AND CREATES A PLAYERPREF FOR IT. THIS IS CALLED FROM AN ITEM OR ANOTHER OBJECT THAT HAS A SPECIFIC SET OF POSSIBLE SUMMON OPTIONS
    public void SummonMonster(Summon summon)
    {
        var monstersDict = GameManager.Instance.monstersData.monstersAllDict;

        var monster = Instantiate(monstersDict[summon.summonChoice.species].monsterPrefab, scrollContent.transform.position, Quaternion.identity);
        monster.GetComponent<Monster>().MonsterSummon(monster.GetComponent<Monster>().info.species);
        activeMonster = monster.GetComponent<Monster>();
        monsterSpriteTotal += 1;
        GameManager.Instance.GetComponent<YourAccount>().account.totalMonstersCollected += 1;
        
        LoadMonsters();
    }

    //use this to summon an item and add it to your inventory
    public void SummonItem()
    {
       
        var items = GameManager.Instance.GetComponent<Items>().allEquipsDict;
        int rand = Random.Range(1, items.Count);
        Dictionary<int, string> equipIds = new Dictionary<int, string>();
        int i = 1;

        //makes a dictionary of all of the equipment items and their id's
        foreach (KeyValuePair<string, EquipmentScript> equips in items)
        {
            equipIds.Add(equips.Value.id, equips.Key);     
            i += 1;
        }

        if (equipIds.ContainsKey(rand))
        {
            if (items.ContainsKey(equipIds[rand]))
            {
                EquipmentScript item = items[equipIds[rand]];
                int itemCount = PlayerPrefs.GetInt(item.name);
                PlayerPrefs.SetInt(item.name, itemCount + 1);
                GameManager.Instance.GetComponent<YourItems>().GetYourItems();
            }
        }
        
        

    }

    // Update is called once per frame
    void Update()
    {
        if (activeMonster)
        {
            //allow the player to move the monster around their Home
            MoveMonster(activeMonster);

        }

        

        ////these are the same thing, but I am using GetMouse for Unity/WebGL and the Touch for mobile
        //if (Input.GetMouseButtonDown(0))
        //{
        //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        //    RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        //    if (hit.collider != null)
        //    {
        //        if (hit.collider.gameObject.tag == "Monster")
        //        {
        //            var mon = JsonUtility.ToJson(hit.collider.gameObject.GetComponent<Monster>().info);
        //            var monsterInfo = JsonUtility.FromJson<MonsterInfo>(mon);

        //            infoMenu.SetActive(true);
        //            activeMonster = hit.collider.gameObject.GetComponent<Monster>();

        //            infoMenu.GetComponent<MonsterInfoPanel>().LoadInfo(activeMonster);


        //        }
        //    }
        //}
    }

    public void MoveMonster(Monster monster)
    {
        //for (var i = 0; i < Input.touchCount; ++i)
        //{
            
        //    var position = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
        //    //Debug.Log(position);
        //    var x = position.x;
        //    var y = position.y;
        //    monster.transform.position = new Vector3(x, y, transform.position.z);

        //    if (Input.GetTouch(0).phase == TouchPhase.Ended)
        //    {
        //        monster.transform.position = monster.transform.position;
        //        activeMonster = null;
        //    }

        //}

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        if (eventData.pointerEnter)
        {
            var tag = eventData.pointerEnter.gameObject.tag;
            var hit = eventData.pointerEnter.gameObject;


            //if the menu is opened with the purpose of Equipping a monster with an item, then allow it to be equipped. Otherwise, show the item's details
            if (tag == "Monster")
            {
                var mon = JsonUtility.ToJson(hit.gameObject.GetComponent<Monster>().info);
            var monsterInfo = JsonUtility.FromJson<MonsterInfo>(mon);

            infoMenu.SetActive(true);
            activeMonster = hit.gameObject.GetComponent<Monster>();

                monsterScrollList.SetActive(false);
                infoMenu.GetComponent<MonsterInfoPanel>().LoadInfo(activeMonster);

            }

            if (tag == "MonsterIcon")
            {
                var mon = JsonUtility.ToJson(hit.gameObject.GetComponentInParent<Monster>().info);
                var monsterInfo = JsonUtility.FromJson<MonsterInfo>(mon);

                infoMenu.SetActive(true);
                activeMonster = hit.gameObject.GetComponentInParent<Monster>();
                monsterScrollList.SetActive(false);
                infoMenu.GetComponent<MonsterInfoPanel>().LoadInfo(activeMonster);
            }
        }


    }


    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void AccountMenu()
    {
        accountInfoMenu.SetActive(true);
        monsterScrollList.SetActive(false);
        accountInfoMenu.GetComponent<AccountInfoMenu>().LoadAccountInfo();
    }

    
    //called from a button
    public void ShowEquipment()
    {

        equipListObject.SetActive(true);
        monsterScrollList.SetActive(false);
        equipListObject.GetComponent<EquipmentManager>().LoadEquipment();


    }

    //called from a button
    public void ShowItems()
    {
        
        itemsListObject.SetActive(true);
        monsterScrollList.SetActive(false);
        itemsListObject.GetComponent<ItemManager>().LoadItems();


    }

    public void CloseEquipment()
    {
        GameObject[] equips = GameObject.FindGameObjectsWithTag("Equipment");

        if (equips.Length == 0)
        {
            equipListObject.SetActive(false);

        }
        else
        {
            for (int i = 0; i < equips.Length; i++)
            {
                Destroy(equips[i]);

                if (i >= equips.Length - 1)
                {
                    monsterScrollList.SetActive(true);
                    equipListObject.SetActive(false);
                }
            }
        }
    }

    public void DeleteAllPrefs()
    {
        PlayerPrefs.DeleteAll();
    }


    public void CollectCoins()
    {
        GameManager.Instance.GetComponent<YourAccount>().GetCoins();
    }

    //call this from other objects to make the monster list active again
    public void MonsterListActive()
    {
        monsterScrollList.SetActive(true);
    }

    







    //called from the MonsterInfoPanel script to hide all of the monster Icons behind the new menu
    public void HideAllMonsters()
    {
        for (int i = 0; i < monsterSpriteTotal; i++)
        {

            //monsterList[i].gameObject.GetComponentInChildren<MonsterIcon>().IconVisibility("Default");
            //monsterList[i].gameObject.GetComponentInChildren<MonsterIcon>().gameObject.GetComponent<Image>().raycastTarget = false;
            //monsterList[i].gameObject.GetComponent<Image>().raycastTarget = false;
        }
    }

    //called from the MonsterInfoPanel script to put all of the Monster icons back and visible
    public void ShowAllMonsters()
    {
        for (int i = 0; i < monsterSpriteTotal; i++)
        {
        //    monsterList[i].gameObject.GetComponentInChildren<MonsterIcon>().IconVisibility("GameUI");
        //    monsterList[i].gameObject.GetComponentInChildren<MonsterIcon>().gameObject.GetComponent<Image>().raycastTarget = true;
        //    monsterList[i].gameObject.GetComponent<Image>().raycastTarget = true;
        }
    }


   
}
