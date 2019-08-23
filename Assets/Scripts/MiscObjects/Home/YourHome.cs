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
    public GameObject homeCanvas, infoMenu, deleteButton, menuCanvas, equipListObject, equipPlacement;
    public Renderer spawnArea;

    public GameObject monsterSprite;
    public GameObject[] monsterSprites;
    public int monsterSpriteTotal;

    //public TMP_Text monsterName, levelText, atkText, defText, speText, precText, typeText, toNextLevelText;
    public Monster activeMonster;

    public Slider expSlider;

    private float touchTime, acumTime, releaseTime;


    public GameObject monsterInfoMenu;


    private void Awake()
    {
        monsterSpriteTotal = 0;
        

        for (int c = 0; c < 3; c++)
        {
            for (int r = 0; r < 5; r++)
            {
                monsterSprites[monsterSpriteTotal] = Instantiate(monsterSprite, homeCanvas.transform.position, Quaternion.identity);
                monsterSprites[monsterSpriteTotal].transform.SetParent(homeCanvas.transform, true);
                monsterSprites[monsterSpriteTotal].transform.position = new Vector3(monsterSprite.transform.position.x + (r * 100), monsterSprite.transform.position.y - (c * 100), monsterSprite.transform.position.z);
                monsterSpriteTotal += 1;
            }

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        expSlider.GetComponent<Slider>();
        LoadMonsters();

        

        //SummonItem();

    }

    public void LoadMonsters()
    {
        monsterSpriteTotal = 0;
        //if there are monsters that are showing, delete them to avoid duplicates
        GameObject[] mons = GameObject.FindGameObjectsWithTag("Monster");


        
        if (mons.Length > 0)
        {

            for (int m = 0; m < mons.Length; m++)
            {
                Destroy(mons[m]);

                if (m >= mons.Length - 1)
                {
                    DisplayMonsters();
                }
            }
        }
        else
        {

            DisplayMonsters();
        }
    }

    public void DisplayMonsters()
    {
        var monsters = GameManager.Instance.GetComponent<YourMonsters>().yourMonstersDict;
        var byId = GameManager.Instance.monstersData.monstersByIdDict;
        var monstersDict = GameManager.Instance.monstersData.monstersAllDict;
        var active = GameManager.Instance.activeTowers;

        //var playerInfo = GameManager.Instance.GetComponent<YourAccount>().account;
        //string yourMonsters = Application.persistentDataPath + "/Saves/" + playerInfo.username + "/monsters.txt";
        
        //string[] lines = File.ReadAllLines(yourMonsters);

        //StreamWriter monsterFile;

        


        //int index = new int();

        //List<int> indexes = new List<int>();

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
                var info = JsonUtility.FromJson<MonsterInfo>(monsterJson);
                //monsterFile = File.AppendText(yourMonsters);
                //monsterFile.WriteLine(monsterJson);
                //monsterFile.Close();




                string species = info.species;

                ////if the monster appears on the Active Towers list, skip over the spawning of it
                //if (indexes.Contains(i))
                //{
                //    //
                //}
                //else
                //{
                    if (monstersDict.ContainsKey(species))
                    {
                        ///Location of the enemies that spawn
                        float x1 = spawnArea.transform.position.x - spawnArea.bounds.size.x / 2;
                        float x2 = spawnArea.transform.position.x + spawnArea.bounds.size.x / 2;
                        float y1 = spawnArea.transform.position.y - spawnArea.bounds.size.y / 2;
                        float y2 = spawnArea.transform.position.y + spawnArea.bounds.size.y / 2;
                        var spawnPoint = new Vector2(Random.Range(x1, x2), Random.Range(y1, y2));
                    //var monster = Instantiate(byPrefab[species], transform.position, Quaternion.identity);
                        monsterSprites[i -1] = Instantiate(monstersDict[species].monsterPrefab, monsterSprites[i - 1].transform.position, Quaternion.identity);
                        var monster = monsterSprites[i - 1];
                        monster.transform.SetParent(homeCanvas.transform, true);
                        monster.GetComponent<Monster>().monsterIcon.SetActive(true);
                        monster.GetComponent<Tower>().boneStructure.SetActive(false);
                        //monster.transform.position = spawnPoint;
                        //monster.transform.position = monsterSprites[i - 1].transform.position;
                        monster.GetComponent<Monster>().monsterIcon.transform.localScale = new Vector3(transform.localScale.x * 3, transform.localScale.y * 3, transform.localScale.z);
                        monster.tag = "Monster";
                        monster.GetComponent<Monster>().GetComponent<Enemy>().enemyCanvas.SetActive(false);
                        monster.GetComponent<Monster>().info = JsonUtility.FromJson<MonsterInfo>(monsters[i]);
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


                //checks all of your current monsters, and if their index is ABOVE that of the deleted monster, reduce them all by 1 
                for (int i = 1; i <= GameManager.Instance.monsterCount; i++)
                {
                    string monsterJson = GameManager.Instance.GetComponent<YourMonsters>().yourMonstersDict[i];
                    var info = JsonUtility.FromJson<MonsterInfo>(monsterJson);
                    Debug.Log(i);

                    if (info.index >= indexDeleted)
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
                        Debug.Log(PlayerPrefs.GetInt("MonsterCount"));
                        var position = activeMonster.transform.position;
                        Destroy(activeMonster.gameObject);
                        infoMenu.SetActive(false);
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
        var map = GameObject.FindGameObjectWithTag("Map");


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
                monsterSprites[monsterSpriteTotal] = Instantiate(monstersDict[species].monsterPrefab, monsterSprites[monsterSpriteTotal].transform.position, Quaternion.identity);
                var monster = monsterSprites[monsterSpriteTotal];
               
                //var monster = Instantiate(monstersDict[species].monsterPrefab, homeCanvas.transform.position, Quaternion.identity);
                monster.transform.SetParent(homeCanvas.transform, true);
                monster.GetComponent<Monster>().monsterIcon.SetActive(true);
                monster.GetComponent<Tower>().boneStructure.SetActive(false);
                monster.GetComponent<Monster>().GetComponent<Enemy>().enemyCanvas.SetActive(false);
                //monster.transform.position = new Vector3(0f, 0f, 10f);
                monster.transform.position = position;
                monster.tag = "Monster";
                monster.GetComponent<Monster>().SummonNewMonster(monster.GetComponent<Monster>().info.species);
                //monster.transform.position = spawnPoint;
                monster.GetComponent<Monster>().monsterIcon.transform.localScale = new Vector3(transform.localScale.x * 3, transform.localScale.y * 3, transform.localScale.z);
                //monster.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
                activeMonster = monster.GetComponent<Monster>();
                monsterSpriteTotal += 1;
                //GameManager.Instance.GetComponent<YourMonsters>().GetYourMonsters();
            }
        }


    }

    //use this to summon an item and add it to your inventory
    public void SummonItem()
    {
       
        var items = GameManager.Instance.GetComponent<Items>().allEquipmentDict;
        int rand = Random.Range(1, items.Count);
        Dictionary<int, string> equipIds = new Dictionary<int, string>();
        int i = 1;

        //makes a dictionary of all of the equipment items and their id's
        foreach (KeyValuePair<string, Equipment> equips in items)
        {
            equipIds.Add(equips.Value.id, equips.Key);     
            i += 1;
        }

        if (equipIds.ContainsKey(rand))
        {
            if (items.ContainsKey(equipIds[rand]))
            {
                Equipment item = items[equipIds[rand]];
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

            infoMenu.GetComponent<MonsterInfoPanel>().LoadInfo(activeMonster);

            }
        }


    }


    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    

    public void ShowEquipment()
    {
        equipListObject.SetActive(true);

        equipListObject.GetComponent<EquipmentManager>().LoadEquipment();

        
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
                    equipListObject.SetActive(false);
                }
            }
        }
    }

    public void DeleteAllPrefs()
    {
        PlayerPrefs.DeleteAll();
    }


    //called from the MonsterInfoPanel script to hide all of the monster Icons behind the new menu
    public void HideAllMonsters()
    {
        for (int i = 0; i < monsterSpriteTotal; i++)
        {
            
            monsterSprites[i].gameObject.GetComponentInChildren<MonsterIcon>().IconVisibility("Default");
        }
    }

    //called from the MonsterInfoPanel script to put all of the Monster icons back and visible
    public void ShowAllMonsters()
    {
        for (int i = 0; i < monsterSpriteTotal; i++)
        {
            monsterSprites[i].gameObject.GetComponentInChildren<MonsterIcon>().IconVisibility("GameUI");
        }
    }
}
