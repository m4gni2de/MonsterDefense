using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class YourMonsters : MonoBehaviour
{

    //dictionary for your monsters as save tokens
    public Dictionary<int, string> yourMonstersDict = new Dictionary<int, string>();
    //dictionary for your defender monsters as save tokens
    public Dictionary<int, string> yourDefendersDict = new Dictionary<int, string>();
    //public Dictionary<int, Monster> yourMonstersAllInfo = new Dictionary<int, Monster>();

    //keeps track of all of your monsters and their coin generation
    public Dictionary<int, float> coinGenDict = new Dictionary<int, float>();
    //dictionary to hold all of your monsters

    //dictionary for your monsters as monster objects, not just their save tokens
    public Dictionary<int, Monster> yourMonstersComplete = new Dictionary<int, Monster>();

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
        coinGenDict.Clear();
        yourDefendersDict.Clear();
        GameManager.Instance.coinGeneration = 0;

        //var byPrefab = GameManager.Instance.monstersData.monsterPrefabsDict;
        var accountInfo = GameManager.Instance.GetComponent<YourAccount>().account;

        string[] monsters = new string[GameManager.Instance.monsterCount + 1];

        for (int i = 1; i <= GameManager.Instance.monsterCount; i++)
        {
            string json = PlayerPrefs.GetString(i.ToString());
            //yourMonsterTokens.Add(i, json);

            
            yourMonstersDict.Add(i, json);
            var t = JsonUtility.FromJson<MonsterSaveToken>(json);
            float coinGen = (int)(t.level * gameObject.GetComponent<MonstersData>().monstersAllDict[t.species].coinGenBase);
            coinGenDict.Add(t.index, coinGen);

            if (t.defenderIndex != 0)
            {
                if (!yourDefendersDict.ContainsKey(t.defenderIndex + 1))
                {
                    yourDefendersDict.Add(t.defenderIndex + 1, json);
                }

                Debug.Log(yourDefendersDict[t.defenderIndex + 1]);
            }
            
            //Debug.Log(yourMonstersDict[i]);
        }

        //get your coin generation total from all of your monsters
        foreach(KeyValuePair<int, float> coinGen in coinGenDict)
        {
            GameManager.Instance.coinGeneration += coinGen.Value;
        }

        //Debug.Log(GameManager.Instance.coinGeneration);



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

    //call this from other scripts to add or remove monsters from your list of monsters
    public void MonsterList(int index, Monster monster)
    {
        

        if (!yourMonstersComplete.ContainsKey(index))
        {
            yourMonstersComplete.Add(index, monster);
        }
        else
        {
            yourMonstersComplete.Remove(index);
            yourMonstersComplete.Add(index, monster);
        }

        
    }

   

    // Update is called once per frame
    void Update()
    {
        //foreach(KeyValuePair<int, Monster> monster in yourMonstersComplete)
        //{
        //    Debug.Log(monster.Value.info.name + ": " + monster.Value.info.Attack.Value);
        //}

       
    }
}


public class MonsterToken
{
    public Monster Monster;
    public MonsterSaveToken newToken;

    public void CreateMonsterToken(Monster monster)
    {
        

        newToken.index = monster.info.index;
        newToken.defenderIndex = monster.info.defenderIndex;
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
        newToken.equip1Exp = monster.info.equip1Exp;
        newToken.equip2 = monster.info.equip2Name;
        newToken.equip2Exp = monster.info.equip2Exp;
        newToken.koCount = monster.info.koCount;
        newToken.maxLevel = monster.info.maxLevel;
        newToken.specialAbility = monster.info.abilityName;
        newToken.passiveSkill = monster.info.skillName;
        newToken.equip1Slot = monster.info.equip1Slot;
        newToken.equip2Slot = monster.info.equip2Slot;
        newToken.isStar = monster.info.isStar;
        
    }

}
