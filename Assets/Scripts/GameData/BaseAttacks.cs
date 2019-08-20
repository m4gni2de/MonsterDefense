using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct BaseAttack
{
    public Stat Range;
    public Stat Power;
    public Stat CritChance;
    public Stat CritMod;
    public Stat EffectChance;
    public Stat AttackTime;
    public Stat AttackSpeed;


    public string name;
    public int id;
    public string description;
    public string type;
    public string effectName;
    public int range;
    public int power;
    public float critChance;
    public float critMod;
    public float effectChance;
    public float attackTime;
    public float attackSpeed;

    public GameObject attackAnimation;

};

[System.Serializable]
public class BaseAttackRoot
{
    public BaseAttack BaseAttack;
}

[System.Serializable]
//put all of the base attacks here
public class AllBaseAttacks
{
    public BaseAttack thunder = new BaseAttack
    {
        name = "Thunder",
        id = 0,
        description = "A quick jolt of electricity",
        type = "Electric",
        effectName = "Paralysis",
        range = 1,
        power = 95,
        critChance = 1f,
        critMod = 1f,
        attackTime = 0.4f,
        effectChance = .20f,
        attackSpeed = 10f,
        Range = new Stat(),
        Power = new Stat(),
        CritChance = new Stat(),
        CritMod = new Stat(),
        AttackTime = new Stat(),
        AttackSpeed = new Stat(),
        EffectChance = new Stat(),
        

    };

    public BaseAttack voltStrike = new BaseAttack
    {
        name = "Volt Strike",
        id = 1,
        description = "A powerful blast of electricity",
        type = "Electric",
        effectName = "Paralysis",
        range = 2,
        power = 130,
        critChance = 1f,
        critMod = 1f,
        attackTime = 2.4f,
        effectChance = .25f,
        attackSpeed = 10f,
        Range = new Stat(),
        Power = new Stat(),
        CritChance = new Stat(),
        CritMod = new Stat(),
        AttackTime = new Stat(),
        AttackSpeed = new Stat(),
        EffectChance = new Stat(),
    };

    public BaseAttack mistySpray = new BaseAttack
    {
        name = "Misty Spray",
        id = 2,
        description = "A soft spray of mist.",
        type = "Water",
        effectName = "Deafen",
        range = 2,
        power = 75,
        critChance = 1f,
        critMod = 1f,
        attackTime = 0.2f,
        effectChance = .15f,
        attackSpeed = 10f,
        Range = new Stat(),
        Power = new Stat(),
        CritChance = new Stat(),
        CritMod = new Stat(),
        AttackTime = new Stat(),
        AttackSpeed = new Stat(),
        EffectChance = new Stat(),
        
    };

    public BaseAttack aquaDart = new BaseAttack
    {
        name = "Aqua Dart",
        id = 3,
        description = "A soft spray of mist.",
        type = "Water",
        effectName = "Confusion",
        range = 3,
        power = 105,
        critChance = 1f,
        critMod = 1f,
        attackTime = 0.4f,
        effectChance = .10f,
        attackSpeed = 10f,
        Range = new Stat(),
        Power = new Stat(),
        CritChance = new Stat(),
        CritMod = new Stat(),
        AttackTime = new Stat(),
        AttackSpeed = new Stat(),
        EffectChance = new Stat(),
    };


}


public class BaseAttacks : MonoBehaviour
{
   

    public AllBaseAttacks allBaseAttacks = new AllBaseAttacks();
    public Dictionary<string, BaseAttack> baseAttackDict = new Dictionary<string, BaseAttack>();
    



    //create a dictionary of all of the attack names with their corresponding objects so enemies can get all of their attack information from the dictionary
    private void Awake()
    {
        SetBaseAttacks();
    }

    public void SetBaseAttacks()
    {
        baseAttackDict.Add(allBaseAttacks.thunder.name, allBaseAttacks.thunder);
        baseAttackDict.Add(allBaseAttacks.voltStrike.name, allBaseAttacks.voltStrike);
        baseAttackDict.Add(allBaseAttacks.aquaDart.name, allBaseAttacks.aquaDart);
        baseAttackDict.Add(allBaseAttacks.mistySpray.name, allBaseAttacks.mistySpray);


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
