using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class YourMonsters : MonoBehaviour
{


    public Dictionary<int, string> yourMonstersDict = new Dictionary<int, string>();
    //public Dictionary<int, string> yourMonstersAllInfo = new Dictionary<int, string>();

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
        //yourMonstersAllInfo.Clear();

        //var byPrefab = GameManager.Instance.monstersData.monsterPrefabsDict;
        var accountInfo = GameManager.Instance.GetComponent<YourAccount>().account;

        string[] monsters = new string[GameManager.Instance.monsterCount + 1];

        for (int i = 1; i <= GameManager.Instance.monsterCount; i++)
        {
            string json = PlayerPrefs.GetString(i.ToString());
            //yourMonsterTokens.Add(i, json);

            //LoadMonsterFromToken(json);
            yourMonstersDict.Add(i, json);
            Debug.Log(yourMonstersDict[i]);
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

    public void LoadMonsterFromToken(string tokenJson)
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}


public class MonsterToken
{
    public Monster Monster;
    public MonsterSaveToken newToken;

    public void CreateMonsterToken(Monster monster)
    {
        

        newToken.index = monster.info.index;
        newToken.species = monster.info.species;
        newToken.name = monster.info.name;
        newToken.level = monster.info.level;
        newToken.totalExp = monster.info.totalExp;
        newToken.atkPot = (int)monster.info.AttackPotential.BaseValue;
        newToken.defPot = (int)monster.info.DefensePotential.BaseValue;
        newToken.spePot = (int)monster.info.SpeedPotential.BaseValue;
        newToken.precPot = (int)monster.info.PrecisionPotential.BaseValue;
        newToken.hpPot = (int)monster.info.HPPotential.BaseValue;
        newToken.rank = monster.info.monsterRank;
        newToken.attack1 = monster.info.attack1Name;
        newToken.attack2 = monster.info.attack2Name;
        newToken.equip1 = monster.info.equip1Name;
        newToken.equip2 = monster.info.equip2Name;
        newToken.koCount = monster.info.koCount;
        newToken.maxLevel = monster.info.maxLevel;
    }

}
