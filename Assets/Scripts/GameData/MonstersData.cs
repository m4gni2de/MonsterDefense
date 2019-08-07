using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using System.Numerics;


//**************THIS SCRIPT IS FOR ALL OF THE MONSTER'S BASE DATA. EVERY MONSTER OF THE SAME SPECIES WILL USE THE VALUES HERE, BUT THEN WILL BE MODIFIED INDUVIDUALLY UPON BEING CREATED*********//
[System.Serializable]
public struct MonsterData
{
    public int id;
    public string species;
    public string type1;
    public string type2;
    public int hpBase;
    public int atkBase;
    public int defBase;
    public int speBase;
    public int precBase;
    public int maxLevel;
    public float levelConst;
    public string[] baseAttacks;

};

[System.Serializable]
public class MonsterDataRoot
{
    public MonsterData MonsterData;
}


public class AllMonsterData
{
    public MonsterData ElectricFiend = new MonsterData
    {
        id = 1,
        species = "ElectricFiend",
        type1 = "Electric",
        type2 = "none",
        hpBase = 65,
        atkBase = 70,
        defBase = 40,
        speBase = 55,
        precBase = 101,
        maxLevel = 100,
        levelConst = 1.8f,
        baseAttacks = new string[2] { "Thunder", "Volt Strike" },
    };

    public MonsterData MrMister = new MonsterData
    {
        id = 2,
        species = "MrMister",
        type1 = "Water",
        type2 = "Magic",
        hpBase = 98,
        atkBase = 44,
        defBase = 30,
        speBase = 39,
        precBase = 110,
        maxLevel = 100,
        levelConst = 1.8f,
        baseAttacks = new string[2] { "Misty Spray", "Aqua Dart" },
    };

    public MonsterData TerrorBite = new MonsterData
    {
        id = 3,
        species = "TerrorBite",
        type1 = "Shadow",
        type2 = "none",
        hpBase = 104,
        atkBase = 86,
        defBase = 53,
        speBase = 64,
        precBase = 133,
        maxLevel = 100,
        levelConst = 1.8f,
        baseAttacks = new string[2] { "Misty Spray", "Aqua Dart" },
    };
}

public class MonstersData : MonoBehaviour
{
    public GameObject[] monsterPrefabs;

    public AllMonsterData allMonsterData = new AllMonsterData();
    public MonsterTypeDetails allTypes = new MonsterTypeDetails();
    public Dictionary<string, MonsterData> monstersAllDict = new Dictionary<string, MonsterData>();
    public Dictionary<string, GameObject> monsterPrefabsDict = new Dictionary<string, GameObject>();
    public Dictionary<int, string> monstersByIdDict = new Dictionary<int, string>();

    public Dictionary<string, TypeInfo> typeChartDict = new Dictionary<string, TypeInfo>();

   

    private void Awake()
    {
        //create a dictionary of all of the monster names with their corresponding objects so any spawned monster can get all of their information from the dictionary
        AllMonsters();

        //create a dictionary of all of the type names with their corresponding objects so the type names can be used as objects
        AllTypes();

        

    }

    public void AllMonsters()
    {
        monstersAllDict.Add(allMonsterData.ElectricFiend.species, allMonsterData.ElectricFiend);
        monsterPrefabsDict.Add(allMonsterData.ElectricFiend.species, monsterPrefabs[0]);
        monstersByIdDict.Add(allMonsterData.ElectricFiend.id, allMonsterData.ElectricFiend.species);

        monstersAllDict.Add(allMonsterData.MrMister.species, allMonsterData.MrMister);
        monsterPrefabsDict.Add(allMonsterData.MrMister.species, monsterPrefabs[1]);
        monstersByIdDict.Add(allMonsterData.MrMister.id, allMonsterData.MrMister.species);

        monstersAllDict.Add(allMonsterData.TerrorBite.species, allMonsterData.TerrorBite);
        monsterPrefabsDict.Add(allMonsterData.TerrorBite.species, monsterPrefabs[2]);
        monstersByIdDict.Add(allMonsterData.TerrorBite.id, allMonsterData.TerrorBite.species);
    }

    public void AllTypes()
    {
        typeChartDict.Add(allTypes.Electric.name, allTypes.Electric);
        typeChartDict.Add(allTypes.Water.name, allTypes.Water);
        typeChartDict.Add(allTypes.Magic.name, allTypes.Magic);
        typeChartDict.Add(allTypes.Shadow.name, allTypes.Shadow);
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

};

[System.Serializable]
public class Types
{
    public TypeInfo TypeInfo;
}


public class MonsterTypeDetails
{
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





