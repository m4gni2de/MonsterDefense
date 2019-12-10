using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using System.Numerics;


//**************THIS SCRIPT IS FOR ALL OF THE MONSTER'S BASE DATA. EVERY MONSTER OF THE SAME SPECIES WILL USE THE VALUES HERE, BUT THEN WILL BE MODIFIED INDUVIDUALLY UPON BEING CREATED*********//
public enum MonsterClass
{
    Flying,
    Beast,
    Humanoid,
    Serpentine,
    Dragon,
}


//use this as the basis for monster's data objects. these are contained within a "Monster Data Object" scriptable object. 
//when a new monster is made, add it's Monster Data Object to the list of MDO's in this script's list of them
[System.Serializable]
public struct MonsterData
{
    public GameObject monsterPrefab;
    //public Sprite iconSprite;
    public Sprite frontIcon;
    public int id;
    public string species;
    public string type1;
    public string type2;
    //the class of monster, such as Flying, Beast, etc
    public MonsterClass Class;
    public int hpBase;
    public int atkBase;
    public int defBase;
    public int speBase;
    public int precBase;
    public int staminaBase;
    public int energyGenBase;
    public float energyCost;
    //the rate at which the monster generates coins for the player to spend
    public float coinGenBase;
    public int maxLevel;
    //used as a multiplier for calculating experience points needed to level up. higher the number, more exp needed, which usually means a stronger monster
    public float levelConst;
    //which attacks the monster can learn, either via levelup or another method
    public string[] baseAttacks;


    //which abilities the most can have when it's summoned
    public string[] abilities;
    public string[] skills;

    //rate at which an item will even drop in the first place
    public float dropRate;
    //what items this monster can drop on defeat
    public string[] itemDrops;
    //chance that this monster will be summoned as a Star Monster. Base value is 1/500 chance to be a Star Monster
    public int starChance;
};

[System.Serializable]
public class AllMonsterData
{
    public MonsterData Lichenthrope = new MonsterData
    {
        

    };


    public MonsterData Armordan = new MonsterData
    {
        

    };

    public MonsterData Fowitzer = new MonsterData
    {
        

    };

    public MonsterData Iceros = new MonsterData
    {
        
    };

    public MonsterData Vypior = new MonsterData
    {
        
    };

    public MonsterData Skaeren = new MonsterData
    {
        

    };




}


public class MonstersData : MonoBehaviour
{
    
    

    public AllMonsterData allMonsterData = new AllMonsterData();
    public MonsterTypeDetails allTypes = new MonsterTypeDetails();

    public List<MonsterDataObject> allMonsters = new List<MonsterDataObject>();
    public Dictionary<string, MonsterDataObject> monsterDataObjectsDict = new Dictionary<string, MonsterDataObject>();
    public Dictionary<string, MonsterData> monstersAllDict = new Dictionary<string, MonsterData>();
    public Dictionary<int, string> monstersByIdDict = new Dictionary<int, string>();

    public Dictionary<string, TypeInfo> typeChartDict = new Dictionary<string, TypeInfo>();

    public GameObject monsterAvatar;

    

    private void Awake()
    {
        //create a dictionary of all of the monster names with their corresponding objects so any spawned monster can get all of their information from the dictionary
        AllMonsters();

        //create a dictionary of all of the type names with their corresponding objects so the type names can be used as objects
        AllTypes();

        

    }

    public void AllMonsters()
    {
        foreach (MonsterDataObject data in allMonsters)
        {

            //monstersAllDict.Add(data.monsterData.species, data.monsterData);
            //monstersByIdDict.Add(data.monsterData.id, data.monsterData.species);


            monsterDataObjectsDict.Add(data.monsterData.species, data);

            
        }

        monstersAllDict.Add(allMonsterData.Lichenthrope.species, allMonsterData.Lichenthrope);
        monstersAllDict.Add(allMonsterData.Armordan.species, allMonsterData.Armordan);
        monstersAllDict.Add(allMonsterData.Fowitzer.species, allMonsterData.Fowitzer);
        monstersAllDict.Add(allMonsterData.Iceros.species, allMonsterData.Iceros);
        monstersAllDict.Add(allMonsterData.Vypior.species, allMonsterData.Vypior);
        monstersAllDict.Add(allMonsterData.Skaeren.species, allMonsterData.Skaeren);

        


        monstersByIdDict.Add(allMonsterData.Lichenthrope.id, allMonsterData.Lichenthrope.species);
        monstersByIdDict.Add(allMonsterData.Armordan.id, allMonsterData.Armordan.species);
        monstersByIdDict.Add(allMonsterData.Fowitzer.id, allMonsterData.Fowitzer.species);
        monstersByIdDict.Add(allMonsterData.Iceros.id, allMonsterData.Iceros.species);
        monstersByIdDict.Add(allMonsterData.Vypior.id, allMonsterData.Vypior.species);
        monstersByIdDict.Add(allMonsterData.Skaeren.id, allMonsterData.Skaeren.species);

        
        //foreach (KeyValuePair<string, MonsterData> monster in monstersAllDict)
        //{
        //    if (monsterDataObjectsDict.ContainsKey(monster.Key))
        //    {
        //        monstersAllDict.Remove(monster.Key);
        //        monstersAllDict.Add(monster.Key, monsterDataObjectsDict[monster.Key].monsterData);
        //    }
        //}
    }

    public void AllTypes()
    {
        typeChartDict.Add(allTypes.Electric.name, allTypes.Electric);
        typeChartDict.Add(allTypes.Water.name, allTypes.Water);
        typeChartDict.Add(allTypes.Magic.name, allTypes.Magic);
        typeChartDict.Add(allTypes.Shadow.name, allTypes.Shadow);
        typeChartDict.Add(allTypes.Nature.name, allTypes.Nature);
        typeChartDict.Add(allTypes.Fire.name, allTypes.Fire);
        typeChartDict.Add(allTypes.Ice.name, allTypes.Ice);
        typeChartDict.Add(allTypes.Mechanical.name, allTypes.Mechanical);
        typeChartDict.Add(allTypes.Normal.name, allTypes.Normal);
        typeChartDict.Add(allTypes.Poison.name, allTypes.Poison);

    }



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


}


//********************************MONSTER TYPINGS*****************************//
[System.Serializable]
public struct TypeInfo
{
    public string name;
    public int id;
    public string[] noChange;
    public string[] resist;
    public string[] weakTo;
    public string[] immune;
    public Sprite typeSprite;

};

[System.Serializable]
public class Types
{
    public TypeInfo TypeInfo;
}

[System.Serializable]
public class MonsterTypeDetails
{
    public TypeInfo Normal = new TypeInfo
    {
        name = "Normal",
        id = 0,
        noChange = new string[9] { "Normal", "Fire", "Water", "Electric", "Ice", "Mechanical", "Shadow", "Nature", "Magic" },
        resist = new string[0] { },
        weakTo = new string[0] { },
        immune = new string[0] { },
    };


    public TypeInfo Electric = new TypeInfo
    {
        name = "Electric",
        id = 1,
        noChange = new string[5] { "Normal", "Fire", "Shadow", "Ice", "Water" },
        resist = new string[3] { "Electric", "Mechanical", "Magic" },
        weakTo = new string[2] { "Nature", "Fire" },
        immune = new string[0] { },
    };

    public TypeInfo Water = new TypeInfo
    {
        name = "Water",
        id = 2,
        noChange = new string[4] { "Normal", "Magic", "Shadow", "Nature" },
        resist = new string[2] { "Fire", "Water" },
        weakTo = new string[3] { "Electric", "Mechanical", "Ice" },
        immune = new string[0] { },
    };

    public TypeInfo Magic = new TypeInfo
    {
        name = "Magic",
        id = 3,
        noChange = new string[6] { "Normal", "Fire", "Water", "Mechanical", "Magic", "Ice" },
        resist = new string[1] { "Electric" },
        weakTo = new string[2] { "Shadow", "Nature" },
        immune = new string[0] { },
    };

    public TypeInfo Shadow = new TypeInfo
    {
        name = "Shadow",
        id = 4,
        noChange = new string[5] { "Normal", "Fire", "Water", "Electric", "Ice" },
        resist = new string[3] { "Mechanical", "Shadow", "Nature" },
        weakTo = new string[1] { "Magic" },
        immune = new string[0] { },
    };

    public TypeInfo Nature = new TypeInfo
    {
        name = "Nature",
        id = 5,
        noChange = new string[5] { "Normal", "Fire", "Water", "Electric", "Ice" },
        resist = new string[3] { "Mechanical", "Shadow", "Nature" },
        weakTo = new string[1] { "Magic" },
        immune = new string[0] { },
    };

    public TypeInfo Ice = new TypeInfo
    {
        name = "Ice",
        id = 6,
        noChange = new string[9] { "Normal", "Fire", "Water", "Electric", "Ice", "Mechanical", "Shadow", "Nature", "Magic" },
        resist = new string[0] { },
        weakTo = new string[0] { },
        immune = new string[0] { },
    };

    public TypeInfo Mechanical = new TypeInfo
    {
        name = "Mechanical",
        id = 7,
        noChange = new string[9] { "Normal", "Fire", "Water", "Electric", "Ice", "Mechanical", "Shadow", "Nature", "Magic" },
        resist = new string[0] { },
        weakTo = new string[0] { },
        immune = new string[0] { },
    };

    public TypeInfo Fire = new TypeInfo
    {
        name = "Fire",
        id = 8,
        noChange = new string[9] { "Normal", "Fire", "Water", "Electric", "Ice", "Mechanical", "Shadow", "Nature", "Magic" },
        resist = new string[0] { },
        weakTo = new string[0] { },
        immune = new string[0] { },
    };

    public TypeInfo Poison = new TypeInfo
    {
        name = "Poison",
        id = 9,
        noChange = new string[8] { "Normal", "Water", "Electric", "Ice", "Mechanical", "Shadow", "Nature", "Magic" },
        resist = new string[1] { "Fire" },
        weakTo = new string[0] { },
        immune = new string[0] { },
    };
}

public class TypeChart
{
    public float totalDamage;
    public float typeModifier;
    


    public TypeChart(TypeInfo attacking, TypeInfo defending, float force, float resistance)
    {
        totalDamage = 0;
        
        //Debug.Log(attacking.name);

        for (int i = 0; i < defending.noChange.Length; i++)
        {
            if (defending.noChange[i] == attacking.name)
            {
                typeModifier = 1;
                totalDamage = Mathf.Round(force / resistance);
            }
        }

        for (int i = 0; i < defending.resist.Length; i++)
        {
            if (defending.resist[i] == attacking.name)
            {
                typeModifier = .5f;
                totalDamage = Mathf.Round(force / resistance);

            }

        }

        for (int i = 0; i < defending.weakTo.Length; i++)
        {
            if (defending.weakTo[i] == attacking.name)
            {
                typeModifier = 2f;
                totalDamage = Mathf.Round(force / resistance);
            }

        }

        for (int i = 0; i < defending.immune.Length; i++)
        {
            if (defending.immune[i] == attacking.name)
            {
                typeModifier = 0f;
                totalDamage = Mathf.Round(force / resistance);
            }

        }

        

        //Debug.Log(typeModifier);
        //Debug.Log(totalDamage);

    }
}

//call this to determine if a defeated monster drops an item
public class MonsterItemDrop
{
    public List<string> droppableItems = new List<string>();

    public MonsterItemDrop(Enemy enemy, Monster attacker)
    {
        var allMonsters = GameManager.Instance.monstersData.monstersAllDict;
        var allEquips = GameManager.Instance.items.allEquipsDict;
        var allConsumables = GameManager.Instance.items.allConsumablesDict;

        Monster thisMonster = enemy.GetComponent<Monster>();
        float dropRate = allMonsters[enemy.monster.info.species].dropRate + attacker.tempStats.DropRateMod.Value;

        float rand = UnityEngine.Random.Range(0f, 1f);

        //check if an item will be dropped 
        if (rand <= dropRate)
        {
            //make a list of all of the possible drops the monster can drop
            for (int d = 0; d < allMonsters[enemy.monster.info.species].itemDrops.Length; d++)
            {
                droppableItems.Add(allMonsters[enemy.monster.info.species].itemDrops[d]);

                //when the list is made, choose one of the items at random to drop
                if (d >= allMonsters[enemy.monster.info.species].itemDrops.Length - 1)
                {
                    int itemRand = UnityEngine.Random.Range(0, allMonsters[enemy.monster.info.species].itemDrops.Length);

                    if (allEquips.ContainsKey(droppableItems[itemRand]))
                    {
                        Equipment e = new Equipment(allEquips[droppableItems[itemRand]]);
                        e.AddToInventory(1);
                    }
                    else if (allConsumables.ContainsKey(droppableItems[itemRand]))
                    {
                        GameManager.Instance.items.allConsumablesDict[droppableItems[itemRand]].AddToInventory(1);
                    }

                    GameManager.Instance.SendNotificationToPlayer(allMonsters[enemy.monster.info.species].itemDrops[itemRand], 1, NotificationType.MonsterDrop, allMonsters[enemy.monster.info.species].species);
                    GameManager.Instance.GetComponent<YourItems>().GetYourItems();
                    //bool hasKey = PlayerPrefs.HasKey(allMonsters[enemy.monster.info.species].itemDrops[d]);

                    //if (hasKey || !hasKey)
                    //{
                    //    int itemAmount = PlayerPrefs.GetInt(allMonsters[enemy.monster.info.species].itemDrops[d], 0);


                    //    PlayerPrefs.SetInt(allMonsters[enemy.monster.info.species].itemDrops[d], itemAmount + 1);
                    //    //Debug.Log("Defeated " + enemy.monster.info.species + " dropped a " + allMonsters[enemy.monster.info.species].itemDrops[d] + "! You now have " + (itemAmount + 1) + " of these!");
                    //    GameManager.Instance.SendNotificationToPlayer(allMonsters[enemy.monster.info.species].itemDrops[d], 1, NotificationType.MonsterDrop, allMonsters[enemy.monster.info.species].species);
                    //    GameManager.Instance.GetComponent<YourItems>().GetYourItems();
                    //}
                }
            }
        }

    }

}










