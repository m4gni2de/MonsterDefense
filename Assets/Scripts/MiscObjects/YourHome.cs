
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class YourHome : MonoBehaviour
{
    public GameObject homeCanvas, infoMenu, deleteButton, menuCanvas, equipListObject, equipPlacement;
    public Renderer homeBg;

    //public TMP_Text monsterName, levelText, atkText, defText, speText, precText, typeText, toNextLevelText;
    public Monster activeMonster;

    public Slider expSlider;

    private float touchTime, acumTime, releaseTime;


    public GameObject monsterInfoMenu;

    // Start is called before the first frame update
    void Start()
    {
        expSlider.GetComponent<Slider>();
        LoadMonsters();
        
        //SummonItem();

    }

    public void LoadMonsters()
    {
        //if there are monsters that are showing, delete them to avoid duplicates
        GameObject[] mons = GameObject.FindGameObjectsWithTag("Monster");

        if (mons.Length > 0)
        {

            for (int m = 0; m < mons.Length; m++)
            {
                Destroy(mons[m]);

                if (m >= mons.Length)
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
        var byPrefab = GameManager.Instance.monstersData.monsterPrefabsDict;
        var active = GameManager.Instance.activeTowers;
        //int index = new int();

        List<int> indexes = new List<int>();

        for (int i = 1; i <= monsters.Count; i++)
        {
            //checks the Active Towers dictionary. If a monster appears in it, then it will add that monster to a local list of towers that are on the field
            if (active.ContainsKey(i))
            {
                Monster monster = active[i];
                indexes.Add(monster.info.index);
                Debug.Log(indexes[i]);
            }


            if (monsters.ContainsKey(i))
            {
                string monsterJson = monsters[i];
                var info = JsonUtility.FromJson<MonsterInfo>(monsterJson);


                string species = info.species;

                //if the monster appears on the Active Towers list, skip over the spawning of it
                if (indexes.Contains(i))
                {
                    //
                }
                else
                {
                    if (byPrefab.ContainsKey(species))
                    {
                        ///Location of the enemies that spawn
                        float x1 = homeBg.transform.position.x - homeBg.bounds.size.x / 2;
                        float x2 = homeBg.transform.position.x + homeBg.bounds.size.x / 2;
                        float y1 = homeBg.transform.position.y - homeBg.bounds.size.y / 2;
                        float y2 = homeBg.transform.position.y + homeBg.bounds.size.y / 2;
                        var spawnPoint = new Vector2(Random.Range(x1, x2), Random.Range(y1, y2));
                        var monster = Instantiate(byPrefab[species], transform.position, Quaternion.identity);
                        monster.transform.SetParent(homeCanvas.transform, true);
                        monster.transform.position = spawnPoint;
                        monster.tag = "Monster";
                        monster.GetComponent<Monster>().GetComponent<Enemy>().enemyCanvas.SetActive(false);
                        monster.GetComponent<Monster>().info = JsonUtility.FromJson<MonsterInfo>(monsters[i]);
                    }
                }


            }


            //*************************************88

        }
    }

    

    public void DeleteMonster()
    {
        Debug.Log(GameManager.Instance.GetComponent<YourMonsters>().yourMonstersDict.Count);

        if (GameManager.Instance.GetComponent<YourMonsters>().yourMonstersDict.Count > 1)
        {
            int indexDeleted = activeMonster.info.index;

            Debug.Log(indexDeleted);

            PlayerPrefs.DeleteKey(activeMonster.info.index.ToString());


            //checks all of your current monsters, and if their index is ABOVE that of the deleted monster, reduce them all by 1 
            for (int i = 1; i <= GameManager.Instance.monsterCount; i++)
            {
                string monsterJson = GameManager.Instance.GetComponent<YourMonsters>().yourMonstersDict[i];
                var info = JsonUtility.FromJson<MonsterInfo>(monsterJson);
                Debug.Log(i);

                if (info.index > indexDeleted)
                {
                    info.index -= 1;
                    PlayerPrefs.SetString(info.index.ToString(), JsonUtility.ToJson(info));

                }


            }

            PlayerPrefs.DeleteKey(GameManager.Instance.monsterCount.ToString());
            GameManager.Instance.monsterCount -= 1;
            PlayerPrefs.SetInt("MonsterCount", GameManager.Instance.monsterCount);
            GameManager.Instance.GetComponent<YourMonsters>().GetYourMonsters();
            Debug.Log(PlayerPrefs.GetInt("MonsterCount"));

            
            Destroy(activeMonster.gameObject);
            infoMenu.SetActive(false);
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
        var byPrefab = GameManager.Instance.monstersData.monsterPrefabsDict;

        //picks a random number. then translates that number to the Monsters by Id Dictionary. Then takes that number, and summons a prefab based on the name of the matching key
        if (byId.ContainsKey(random))
        {
            string species = byId[random];

            if (byPrefab.ContainsKey(species))
            {

                var monster = Instantiate(byPrefab[species], map.transform.position, Quaternion.identity);
                monster.transform.SetParent(map.gameObject.transform, true);
                monster.transform.position = new Vector3(0f, 0f, 10f);
                monster.tag = "Monster";
                monster.GetComponent<Monster>().SummonNewMonster(monster.GetComponent<Monster>().info.species);

                activeMonster = monster.GetComponent<Monster>();
                GameManager.Instance.GetComponent<YourMonsters>().GetYourMonsters();
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

        //these are the same thing, but I am using GetMouse for Unity/WebGL and the Touch for mobile
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Monster")
                {
                    var mon = JsonUtility.ToJson(hit.collider.gameObject.GetComponent<Monster>().info);
                    var monsterInfo = JsonUtility.FromJson<MonsterInfo>(mon);

                    infoMenu.SetActive(true);
                    activeMonster = hit.collider.gameObject.GetComponent<Monster>();

                    infoMenu.GetComponent<MonsterInfoPanel>().LoadInfo(activeMonster);

                    
                }
            }
        }
    }

    public void MoveMonster(Monster monster)
    {
        for (var i = 0; i < Input.touchCount; ++i)
        {
            
            var position = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            //Debug.Log(position);
            var x = position.x;
            var y = position.y;
            monster.transform.position = new Vector3(x, y, transform.position.z);

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                monster.transform.position = monster.transform.position;
                activeMonster = null;
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
        
        //var items = GameManager.Instance.GetComponent<YourItems>().yourItemsDict;
        //var equipIds = GameManager.Instance.GetComponent<YourItems>().equipIds;
        //var allEquips = GameManager.Instance.GetComponent<Items>().allEquipmentDict;
        //var equipByPrefab = GameManager.Instance.GetComponent<Items>().equipmentByPrefab;


        //for (int i = 1; i <= equipIds.Count; i++)
        //{

        //    if (equipIds.ContainsKey(i))
        //    {
        //        string name = equipIds[i];

        //        if (PlayerPrefs.HasKey(name))
        //        {
        //            Equipment item = allEquips[name];
        //            int itemCount = PlayerPrefs.GetInt(item.name);
        //            var x = Instantiate(equipByPrefab[item.name], new Vector2(equipPlacement.transform.position.x + (50 * (i - 1)), equipPlacement.transform.position.y), Quaternion.identity);
        //            x.transform.SetParent(equipListObject.transform, true);
        //            x.GetComponent<EquipmentItem>().EquipItemInfo(item);
        //            x.transform.localScale = Vector3.one;

        //        }                        
        //    }
        //}

        
    }

    public void CloseEquipment()
    {
        GameObject[] equips = GameObject.FindGameObjectsWithTag("Equipment");

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
