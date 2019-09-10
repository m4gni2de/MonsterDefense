using System.Collections;
using System.Collections.Generic;
using System.IO;
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

        //var byPrefab = GameManager.Instance.monstersData.monsterPrefabsDict;
        var accountInfo = GameManager.Instance.GetComponent<YourAccount>().account;

        string[] monsters = new string[GameManager.Instance.monsterCount + 1];

        for (int i = 1; i <= GameManager.Instance.monsterCount; i++)
        {
            string json = PlayerPrefs.GetString(i.ToString());
            yourMonstersDict.Add(i, json);
            //Debug.Log(yourMonstersDict[i]);
        }

        //string playerDirectory = Application.persistentDataPath + "/Saves/" + accountInfo.username;
        //string accountText = playerDirectory + "/player.txt";
        //string monsterText = playerDirectory + "/monsters.txt";

        ////gets the account information from the json string in the textfile and loads in to the player information
        //if (File.Exists(monsterText))
        //{
        //    string[] lines = File.ReadAllLines(monsterText);


        //    string[] monsters = new string[GameManager.Instance.monsterCount + 1];

        //    for (int i = 0; i < GameManager.Instance.monsterCount; i++)
        //    {
        //        string json = lines[i];
        //        yourMonstersDict.Add(i +1, json);
        //    }

            
        //}
    }

    // Update is called once per frame
    void Update()
    {

    }
}
