using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public enum InfoDisplay
{
    Tower,
    Enemy
}
public class CurrentGameInfo : MonoBehaviour
{
    public GameObject monsterDisplayObject, monsterDisplayContent, enemyDisplayContent, monsterFullDisplay, enemyFullDisplay, enemyDisplayObject;
    public Sprite selectedSprite, unSelectedSprite;
    public Button towersButton, enemiesButton;
    public MapDetails mapDetails;

    //a  list to keep the monster display objects in this menu
    public List<GameObject> monsterDisplaysList = new List<GameObject>();
    //list to keep the enemy display objects in this menu
    public Dictionary<Enemy, GameObject> enemyDisplaysList = new Dictionary<Enemy, GameObject>();

    public InfoDisplay displayMode;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadYourTowers()
    {
       
        monsterDisplaysList.Clear();
        StopAllCoroutines();

        var towers = GameManager.Instance.activeTowers;

        foreach (KeyValuePair<int, Monster> tower in towers)
        {
            var x = Instantiate(monsterDisplayObject, monsterDisplayContent.transform, false);
            x.SetActive(true);
            x.GetComponent<DisplayMonsterObject>().DisplayMonster(tower.Value);
            x.transform.position = new Vector2(x.transform.position.x, x.transform.position.y - (monsterDisplaysList.Count * 80));
            x.GetComponent<DisplayMonsterObject>().StartCoroutine("UpdateMonster", 1f);
            monsterDisplaysList.Add(x);

        }
    }


    public void LoadEnemies()
    {
        var enemies = mapDetails.liveEnemies;

        foreach (KeyValuePair<Enemy, GameObject> e in enemyDisplaysList)
        {
            Destroy(e.Value);
        }

        enemyDisplaysList.Clear();


        foreach (Enemy enemy in enemies)
        {
            
            var x = Instantiate(enemyDisplayObject, enemyDisplayContent.transform, false);
            x.SetActive(true);
            x.GetComponent<DisplayMonsterObject>().EnemyDisplay(enemy.GetComponent<Monster>());
            x.transform.position = new Vector2(x.transform.position.x, x.transform.position.y - (enemyDisplaysList.Count * 70));
            //x.GetComponent<DisplayMonsterObject>().StartCoroutine("UpdateEnemy", 1f);
            enemyDisplaysList.Add(enemy, x);
        }
    }

    //when this menu becomes enables, create the objects
    public void OnEnable()
    {
        
        TowerDisplay();

        mapDetails = GameManager.Instance.activeMap;

    }

    //when this menu becomes enables, create the objects
    public void OnDisable()
    {
        foreach (GameObject o in monsterDisplaysList)
        {
            Destroy(o);
        }

        foreach (KeyValuePair<Enemy, GameObject> e in enemyDisplaysList)
        {
            Destroy(e.Value);
        }
    }

    public void TowerDisplay()
    {
        towersButton.GetComponent<Image>().sprite = selectedSprite;
        enemiesButton.GetComponent<Image>().sprite = unSelectedSprite;

        displayMode = InfoDisplay.Tower;

        if (monsterDisplaysList.Count > 0)
        {
            foreach (GameObject e in monsterDisplaysList)
            {
                Destroy(e);
            }
        }

        enemyFullDisplay.SetActive(false);
        monsterFullDisplay.SetActive(true);


        LoadYourTowers();  
    }

    public void EnemyDisplay()
    {
        towersButton.GetComponent<Image>().sprite = unSelectedSprite;
        enemiesButton.GetComponent<Image>().sprite = selectedSprite;
        displayMode = InfoDisplay.Enemy;

        enemyFullDisplay.SetActive(true);
        monsterFullDisplay.SetActive(false);


        StartCoroutine(ShowEnemies());
        
    }




   

    //this is to display your towers only
    public IEnumerator ShowEnemies()
    {

        do
        {
            LoadEnemies();
            yield return new WaitForSeconds(1f);

        } while (displayMode == InfoDisplay.Enemy);
        
    }
}
