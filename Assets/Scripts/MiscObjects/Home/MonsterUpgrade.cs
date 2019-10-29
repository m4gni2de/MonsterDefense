using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MonsterUpgrade : MonoBehaviour, IPointerDownHandler
{
    public Monster activeMonster, m2, m3;
    //public List<MonsterSaveToken> upgradeOptions = new List<MonsterSaveToken>();
    public Dictionary<GameObject, MonsterSaveToken> upgradeOptions = new Dictionary<GameObject, MonsterSaveToken>();
    public GameObject monsterSprite, monsterScroll, upgradePopMenu;

    public TMP_Text hp1, atk1, def1, spe1, prec1, maxLevel1, hp2, atk2, def2, spe2, prec2, maxLevel2, hp3, atk3, def3, spe3, prec3, maxLevel3, rank1, rank2, rank3;
    public GameObject monster1, monster2, monster3;

    public Button[] objectButtons;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SelectMonster(Monster monster)
    {
        activeMonster = monster;

       

        upgradeOptions.Clear();

        var monsters = GameManager.Instance.GetComponent<YourMonsters>().yourMonstersDict;
        var monstersDict = GameManager.Instance.monstersData.monstersAllDict;

        //checks all of the monsters in your monsters dictionary
        for (int i = 1; i <= monsters.Count; i++)
        {
            
            if (monsters.ContainsKey(i))
            {
                string monsterJson = monsters[i];
                var info = JsonUtility.FromJson<MonsterSaveToken>(monsterJson);
                string species = info.species;

                //makes sure the active monster can't select itself as a possible upgrade option
                if (info.index != activeMonster.info.index)
                {

                    //checks your monsters for other monsters of the same species as the active monster
                    if (info.species == activeMonster.info.species && info.rank == activeMonster.info.monsterRank)
                    {
                        //var option = Instantiate(monsterSprite, new Vector3(monsterSprite.transform.position.x, monsterSprite.transform.position.y - (upgradeOptions.Count * (monsterSprite.GetComponent<RectTransform>().rect.height * monsterSprite.transform.localScale.y)), monsterSprite.transform.position.z), Quaternion.identity);
                        //option.transform.SetParent(monsterScroll.transform, true);
                        //option.GetComponent<Image>().sprite = monstersDict[species].iconSprite;
                        //upgradeOptions.Add(option, info);
                        //option.transform.localScale = new Vector2(45f, 25f);
                        //option.tag = "Respawn";
                        //option.name = info.index.ToString();
                        //option.GetComponent<MonsterHomeIcon>().nameText.text = info.name;
                        //option.GetComponent<MonsterHomeIcon>().levelText.text = info.level.ToString();

                        var option = Instantiate(monstersDict[species].monsterPrefab, new Vector3(monsterSprite.transform.position.x, monsterSprite.transform.position.y - (upgradeOptions.Count * (monsterSprite.GetComponent<RectTransform>().rect.height * monsterSprite.transform.localScale.y)), monsterSprite.transform.position.z), Quaternion.identity);
                        option.transform.SetParent(monsterScroll.transform, true);
                        option.GetComponent<Tower>().boneStructure.SetActive(false);
                        option.GetComponent<Monster>().monsterIcon.transform.localScale = new Vector3(25, 25, transform.localScale.z);
                        option.tag = "Respawn";
                        option.name = info.index.ToString();
                        option.GetComponent<Monster>().GetComponent<Enemy>().enemyCanvas.SetActive(false);
                        option.GetComponent<Monster>().saveToken = JsonUtility.FromJson<MonsterSaveToken>(monsters[i]);
                        option.GetComponent<Monster>().LoadMonsterToken(option.GetComponent<Monster>().saveToken);
                        option.GetComponent<Monster>().monsterIcon.SetActive(true);
                        option.GetComponent<Monster>().monsterIcon.GetComponentInChildren<MonsterIcon>().IconVisibility("Overlays");
                        option.GetComponent<Monster>().monsterIcon.GetComponentInChildren<MonsterIcon>().DisplayMonster(option.GetComponent<Monster>());
                        upgradeOptions.Add(option, info);

                    }


                }
                


                ////if the monster appears on the Active Towers list, skip over the spawning of it
                //if (indexes.Contains(i))
                //{
                //    //
                //}
                //else
                //{
                if (monstersDict.ContainsKey(species))
                {

                    //monsterList[i - 1] = Instantiate(monstersDict[species].monsterPrefab, monsterSprites[i - 1].transform.position, Quaternion.identity);
                    //var monster = monsterList[i - 1];
                    //monster.transform.SetParent(scrollContent.transform, true);

                    //monster.GetComponent<Monster>().monsterIcon.SetActive(false);
                    //monster.GetComponent<Tower>().boneStructure.SetActive(false);
                    //monster.GetComponent<Monster>().monsterIcon.transform.localScale = new Vector3(transform.localScale.x * 3, transform.localScale.y * 3, transform.localScale.z);
                    //monster.tag = "Monster";
                    //monster.GetComponent<Monster>().GetComponent<Enemy>().enemyCanvas.SetActive(false);
                    //monster.GetComponent<Monster>().saveToken = JsonUtility.FromJson<MonsterSaveToken>(monsters[i]);
                    //monster.GetComponent<Monster>().LoadMonsterToken(monster.GetComponent<Monster>().saveToken);
                    ////monster.GetComponent<Monster>().info = JsonUtility.FromJson<MonsterInfo>(monsters[i]);
                    //monsterSprites[i - 1].GetComponent<Image>().color = Color.white;
                    //monsterSprites[i - 1].GetComponent<Image>().sprite = monstersDict[species].iconSprite;
                    //monsterSprites[i - 1].GetComponent<MonsterHomeIcon>().nameText.text = monster.GetComponent<Monster>().info.name;
                    //monsterSprites[i - 1].GetComponent<MonsterHomeIcon>().levelText.text = monster.GetComponent<Monster>().info.level.ToString();

                    //monsterSpriteTotal += 1;

                   


                }


            }
        }
    }


    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnable()
    {
        Button[] buttons = GameObject.FindObjectsOfType<Button>();

        for (int i = 0; i < buttons.Length; i++)
        {

            buttons[i].interactable = false;
        }

        for (int b = 0; b < objectButtons.Length; b++)
        {
            objectButtons[b].interactable = true;
        }
    }

    public void OnDisable()
    {
       
    }



    public void OnPointerDown(PointerEventData eventData)
    {
        

        if (eventData.pointerEnter)
        {
            var tag = eventData.pointerEnter.gameObject.tag;
            var hit = eventData.pointerEnter.gameObject;
            

            //if the menu is opened with the purpose of Equipping a monster with an item, then allow it to be equipped. Otherwise, show the item's details
            if (tag == "Respawn")
            {
                //var mon = JsonUtility.ToJson(hit.gameObject.GetComponent<Monster>().info);
                //var monsterInfo = JsonUtility.FromJson<MonsterInfo>(mon);

                //upgradePopMenu.SetActive(true);
                //infoMenu.GetComponent<MonsterInfoPanel>().LoadInfo(activeMonster);

                Debug.Log(upgradeOptions.Keys);

                //if (upgradeOptions.ContainsKey(hit))
                //{
                    upgradePopMenu.SetActive(true);
                    UpgradeMonster(hit);
                //}

            }

        }


    }

    public void UpgradeMonster(GameObject hit)
    {
        var monstersDict = GameManager.Instance.monstersData.monstersAllDict;
        var monsters = GameManager.Instance.GetComponent<YourMonsters>().yourMonstersDict;



        //monster1.GetComponent<Image>().sprite = monstersDict[activeMonster.info.species].iconSprite;
        //monster1.GetComponent<MonsterHomeIcon>().nameText.text = activeMonster.info.name;
        //monster1.GetComponent<MonsterHomeIcon>().levelText.text = activeMonster.info.level.ToString();
        //monster1.GetComponent<MonsterHomeIcon>().rankText.text = activeMonster.info.monsterRank.ToString();



        //monster1 = Instantiate(activeMonster.monsterIcon, monster1.transform.position, Quaternion.identity);
        var monster = Instantiate(activeMonster.monsterIcon, monster1.transform.position, Quaternion.identity);
        monster.GetComponentInChildren<MonsterIcon>().IconVisibility("Overlays");

        //var monster = monster1;
        monster.transform.SetParent(upgradePopMenu.transform, true);
        monster.tag = "Tower";


        //monster.GetComponent<Tower>().boneStructure.SetActive(false);
        //monster.GetComponent<Monster>().monsterIcon.SetActive(true);
        //monster.GetComponent<Monster>().monsterIcon.GetComponentInChildren<MonsterIcon>().DisplayMonster(activeMonster);



        hp1.text = "HP: " + activeMonster.info.HP.Value.ToString();
        atk1.text = "Atk: " + activeMonster.info.Attack.Value.ToString();
        def1.text = "Def: " + activeMonster.info.Defense.Value.ToString();
        spe1.text = "Spe: " + activeMonster.info.Speed.Value.ToString();
        prec1.text = "Prec: " + activeMonster.info.Precision.Value.ToString();
        maxLevel1.text = "Max Level: " + activeMonster.info.maxLevel.ToString();
        rank1.text = "Rank: " + activeMonster.info.monsterRank.ToString();

        

        //monster2.GetComponent<Image>().sprite = monstersDict[upgradeOptions[hit].species].iconSprite;
        //monster2.GetComponent<MonsterHomeIcon>().nameText.text = upgradeOptions[hit].name;
        //monster2.GetComponent<MonsterHomeIcon>().levelText.text = upgradeOptions[hit].level.ToString();
        //monster2.GetComponent<MonsterHomeIcon>().rankText.text = upgradeOptions[hit].rank.ToString();

        //create a temporary monster for the 2nd monster, to avoid having to spawn an entirely new monster
        if (monsters.ContainsKey(upgradeOptions[hit].index))
        {
            //monster2 = Instantiate(activeMonster.monsterIcon, monster2.transform.position, Quaternion.identity);
            var tempMonster2 = Instantiate(activeMonster.monsterIcon, monster2.transform.position, Quaternion.identity);
            tempMonster2.GetComponentInChildren<MonsterIcon>().IconVisibility("Overlays");
            tempMonster2.tag = "Tower";

            //var m = monster2;
            tempMonster2.transform.SetParent(upgradePopMenu.transform, true);


            string monsterJson = monsters[upgradeOptions[hit].index];
            MonsterSaveToken info = JsonUtility.FromJson<MonsterSaveToken>(monsterJson);
            string species = info.species;

            m2 = monstersDict[species].monsterPrefab.GetComponent<Monster>();
            m2.saveToken = info;
            m2.LoadMonsterToken(m2.saveToken);
            tempMonster2.GetComponentInChildren<MonsterIcon>().DisplayMonster(m2);

            
            hp2.text = "HP: " + m2.info.HP.Value.ToString();
            atk2.text = "Atk: " + m2.info.Attack.Value.ToString();
            def2.text = "Def: " + m2.info.Defense.Value.ToString();
            spe2.text = "Spe: " + m2.info.Speed.Value.ToString();
            prec2.text = "Prec: " + m2.info.Precision.Value.ToString();
            maxLevel2.text = "Max Level: " + m2.info.maxLevel.ToString();
            rank2.text = "Rank: " + m2.info.monsterRank.ToString();

        }

        //set the changes that will occur for the newly upgraded monster
        m3 = activeMonster;

        //monster3 = Instantiate(activeMonster.monsterIcon, monster3.transform.position, Quaternion.identity);
        var newMonster3 = Instantiate(activeMonster.monsterIcon, monster3.transform.position, Quaternion.identity);
        newMonster3.GetComponentInChildren<MonsterIcon>().IconVisibility("Overlays");
        newMonster3.tag = "Tower";

        //var m = monster3;
        newMonster3.transform.SetParent(upgradePopMenu.transform, true);

        m3.saveToken.rank = activeMonster.saveToken.rank +1;
        m3.saveToken.maxLevel = activeMonster.saveToken.maxLevel + (10 * (m3.saveToken.rank - 1));

        m3.CheckStats(m3.saveToken);

        hp3.text = "HP: " + m3.info.HP.Value.ToString();
        atk3.text = "Atk: " + m3.info.Attack.Value.ToString();
        def3.text = "Def: " + m3.info.Defense.Value.ToString();
        spe3.text = "Spe: " + m3.info.Speed.Value.ToString();
        prec3.text = "Prec: " + m3.info.Precision.Value.ToString();
        maxLevel3.text = "Max Level: " + m3.info.maxLevel.ToString();
        rank3.text = "Rank: " + m3.info.monsterRank.ToString();

        newMonster3.GetComponentInChildren<MonsterIcon>().DisplayMonster(m3);
        //monster3.GetComponent<Image>().sprite = monstersDict[activeMonster.info.species].iconSprite;
        //monster3.GetComponent<MonsterHomeIcon>().nameText.text = activeMonster.info.name;
        //monster3.GetComponent<MonsterHomeIcon>().levelText.text = activeMonster.info.level.ToString();
        //monster3.GetComponent<MonsterHomeIcon>().rankText.text = activeMonster.saveToken.rank.ToString();



    }


    //this is the final button click to upgrade the monsters
    public void UpgradeButton()
    {
        activeMonster.saveToken = m3.saveToken;
        activeMonster.SaveMonsterToken();
        activeMonster.LoadMonsterToken(activeMonster.saveToken);
        GameManager.Instance.GetComponent<YourMonsters>().GetYourMonsters();

        Debug.Log(m2.info.index);
        if (GameManager.Instance.GetComponent<YourMonsters>().yourMonstersDict.Count > 1)
        {
            int indexDeleted = m2.info.index;

            //Debug.Log(indexDeleted);


            //deletes the key of the monster being destroyed
            PlayerPrefs.DeleteKey(m2.info.index.ToString());


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
                    
                    
                    
                   
                    
                    CloseMenuBtn();
                }


            }


            //PlayerPrefs.DeleteKey(GameManager.Instance.monsterCount.ToString());
            //GameManager.Instance.monsterCount -= 1;
            //PlayerPrefs.SetInt("MonsterCount", GameManager.Instance.monsterCount);
            //GameManager.Instance.GetComponent<YourMonsters>().GetYourMonsters();
            //Debug.Log(PlayerPrefs.GetInt("MonsterCount"));





        }

        
       
    }

    //close this menu
    public void CloseMenuBtn()
    {
        Button[] buttons = GameObject.FindObjectsOfType<Button>();

        //activeMonster = null;
        //m2 = null;
        //m3 = null;

        for (int i = 0; i < buttons.Length; i++)
        {

            buttons[i].interactable = true;
        }

        GameObject[] respawn = GameObject.FindGameObjectsWithTag("Tower");

        for (int a = 0; a < respawn.Length; a++)
        {
            Debug.Log("here");
            Destroy(respawn[a]);
        }

        foreach (KeyValuePair<GameObject, MonsterSaveToken> icon in upgradeOptions)
        {
            Destroy(icon.Key);
        }


        GetComponentInParent<YourHome>().infoMenu.GetComponent<MonsterInfoPanel>().LoadInfo(activeMonster);
        GetComponentInParent<YourHome>().LoadMonsters();
        Destroy(activeMonster.gameObject);
        upgradePopMenu.SetActive(false);
        gameObject.SetActive(false);
    }
}
