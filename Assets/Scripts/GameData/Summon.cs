using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//use this class to summon a monster with specific parameters
public class Summon
{
    public string type;
    public List<MonsterData> summonOptions = new List<MonsterData>();
    public MonsterData summonChoice;

    public Summon(string Type)
    {
        summonOptions.Clear();

        var allMonsters = GameManager.Instance.monstersData.monstersAllDict;

        type = Type;
        
        //loops through all of the monsters and checks for the correct type
        foreach(KeyValuePair<string, MonsterData> monster in allMonsters)
        {
            if (monster.Value.type1 == type || monster.Value.type2 == type)
            {
                summonOptions.Add(monster.Value);
            }
        }
    }

    public void MonsterSummon()
    {
        var allMonsters = GameManager.Instance.monstersData.monstersAllDict;

        int rand = Random.Range(0, summonOptions.Count);

       summonChoice = summonOptions[rand];

        GameObject[] items = GameManager.Instance.activeScene.GetRootGameObjects();
        foreach (GameObject item in items)
        {
            if (item.name == "HomeObject")
            {
                item.GetComponentInChildren<YourHome>().SummonMonster(this);
                return;
            }
        }
        
    }
}
