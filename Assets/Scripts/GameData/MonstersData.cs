using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using System.Numerics;


//**************THIS SCRIPT IS FOR ALL OF THE MONSTER'S BASE DATA. EVERY MONSTER OF THE SAME SPECIES WILL USE THE VALUES HERE, BUT THEN WILL BE MODIFIED INDUVIDUALLY UPON BEING CREATED*********//
[System.Serializable]
public struct MonsterData
{
    public GameObject monsterPrefab;
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

    public int staminaBase;
    public int energyGenBase;
    public float energyCost;
    

};

[System.Serializable]
public class MonsterDataRoot
{
    public MonsterData MonsterData;
}

[System.Serializable]
public class AllMonsterData
{
    public MonsterData Lichenthrope = new MonsterData
    {
        id = 1,
        species = "Lichenthrope",
        type1 = "Nature",
        type2 = "none",
        hpBase = 131,
        atkBase = 120,
        defBase = 118,
        speBase = 44,
        precBase = 86,
        maxLevel = 100,
        levelConst = 1.9f,
        baseAttacks = new string[2] { "Thunder", "Misty Spray" },
        staminaBase = 70,
        energyGenBase = 77,
        energyCost = 7f,
        
    };


    //public MonsterData MrMister = new MonsterData
    //{
    //    id = 2,
    //    species = "MrMister",
    //    type1 = "Water",
    //    type2 = "Magic",
    //    hpBase = 98,
    //    atkBase = 44,
    //    defBase = 30,
    //    speBase = 39,
    //    precBase = 110,
    //    maxLevel = 100,
    //    levelConst = 1.8f,
    //    baseAttacks = new string[2] { "Misty Spray", "Aqua Dart" },
    //    staminaBase = 90,
    //    energyGenBase = 95,
    //    energyCost = 5.5f,
    //};

   

    
}

public class MonstersData : MonoBehaviour
{
    
    

    public AllMonsterData allMonsterData = new AllMonsterData();
    public MonsterTypeDetails allTypes = new MonsterTypeDetails();
    public Dictionary<string, MonsterData> monstersAllDict = new Dictionary<string, MonsterData>();
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
       
        monstersAllDict.Add(allMonsterData.Lichenthrope.species, allMonsterData.Lichenthrope);
        monstersByIdDict.Add(allMonsterData.Lichenthrope.id, allMonsterData.Lichenthrope.species);
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
        typeChartDict.Add(allTypes.Normal.name, allTypes.Mechanical);

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






