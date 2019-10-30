using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CurrentGameInfo : MonoBehaviour
{
    public GameObject monsterDisplayObject, monsterDisplayContent;

    //a  list to keep the monster display objects in this menu
    public List<GameObject> monsterDisplaysList = new List<GameObject>();


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

    //when this menu becomes enables, create the objects
    public void OnEnable()
    {
        LoadYourTowers();
    }

    //when this menu becomes enables, create the objects
    public void OnDisable()
    {
        foreach(GameObject o in monsterDisplaysList)
        {
            Destroy(o);
        }
    }
}
