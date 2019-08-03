using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YourMonsters : MonoBehaviour
{


    public Dictionary<int, string> yourMonstersDict = new Dictionary<int, string>();

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        GetYourMonsters();

    }

    //make a Dictionary of all of the PlayerPrefs monsters' index and their json object information
    public void GetYourMonsters()
    {
        yourMonstersDict.Clear();

        var byPrefab = GameManager.Instance.monstersData.monsterPrefabsDict;

        string[] monsters = new string[GameManager.Instance.monsterCount + 1];

        for (int i = 1; i <= GameManager.Instance.monsterCount; i++)
        {

            string json = PlayerPrefs.GetString(i.ToString());
            yourMonstersDict.Add(i, json);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
