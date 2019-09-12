using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct MonsterAttack
{
    //Stats are filled in when the monster using it becomes active, to account for stat buffs and drops
    public Stat Range;
    public Stat Power;
    public Stat CritChance;
    public Stat CritMod;
    public Stat EffectChance;
    public Stat AttackTime;
    public Stat AttackSpeed;
    public Stat AttackSlow;


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
    //length of time a monster is slowed when hit by this attack
    public float hitSlowTime;
    //either projectile or physical
    public AttackMode attackMode;

    public GameObject attackAnimation;
    
    

};

[System.Serializable]
public class BaseAttackRoot
{
    public MonsterAttack MonsterAttack;
}

public enum AttackMode
{
    Punch,
    Kick,
    Projectile,
    Physical,
}

[System.Serializable]
//put all of the base attacks here
public class AllAttacks
{
    public MonsterAttack thunder = new MonsterAttack
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
        attackSpeed = 1.0f,
        hitSlowTime = .2f,
        attackMode = AttackMode.Projectile,
        
        

    };

    public MonsterAttack voltStrike = new MonsterAttack
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
        attackTime = .4f,
        effectChance = .25f,
        attackSpeed = 14f,
        hitSlowTime = .6f,
        attackMode = AttackMode.Projectile,
       
    };

    public MonsterAttack mistySpray = new MonsterAttack
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
        attackTime = 0.3f,
        effectChance = .15f,
        attackSpeed = 7f,
        hitSlowTime = .25f,
        attackMode = AttackMode.Projectile,
        

    };

    public MonsterAttack aquaDart = new MonsterAttack
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
        attackTime = 0.15f,
        effectChance = .10f,
        attackSpeed = 12f,
        hitSlowTime = .15f,
        attackMode = AttackMode.Projectile,
        
    };

    public MonsterAttack shadowBP = new MonsterAttack
    {
        name = "Shadow Blaze Punch",
        id = 4,
        description = "A punch that calls upon power from the Underworld.",
        type = "Shadow",
        effectName = "Burn",
        range = 2,
        power = 115,
        critChance = 1f,
        critMod = 1f,
        attackTime = 0.7f,
        effectChance = .10f,
        attackSpeed = 13f,
        hitSlowTime = .35f,
        attackMode = AttackMode.Punch,

    };

    public MonsterAttack windCyclone = new MonsterAttack
    {
        name = "Wind Cyclone",
        id = 5,
        description = "A swirling storm of wind and destruction.",
        type = "Nature",
        effectName = "none",
        range = 2,
        power = 115,
        critChance = 1f,
        critMod = 1f,
        attackTime = 0.6f,
        effectChance = .10f,
        attackSpeed = 7f,
        hitSlowTime = .75f,
        attackMode = AttackMode.Projectile,
        
        
    };

    public MonsterAttack energyDragon = new MonsterAttack
    {
        name = "Energy Dragon",
        id = 6,
        description = "A blazing dragon of fire and fury.",
        type = "Fire",
        effectName = "Burn",
        range = 2,
        power = 140,
        critChance = 1f,
        critMod = 1f,
        attackTime = 0.8f,
        effectChance = .15f,
        attackSpeed = 5f,
        hitSlowTime = .85f,
        attackMode = AttackMode.Projectile,
        

    };


}


public class Attacks : MonoBehaviour
{
   

    public AllAttacks allAttacks = new AllAttacks();
    public Dictionary<string, MonsterAttack> attackDict = new Dictionary<string, MonsterAttack>();

    
    public List<string> waterAttacks = new List<string>();
    public List<string> electricAttacks = new List<string>();
    public List<string> shadowAttacks = new List<string>();
    public List<string> natureAttacks = new List<string>();
    public List<string> fireAttacks = new List<string>();



    //create a dictionary of all of the attack names with their corresponding objects so enemies can get all of their attack information from the dictionary
    private void Awake()
    {
        SetBaseAttacks();
       
    }

    public void SetBaseAttacks()
    {
        attackDict.Add(allAttacks.thunder.name, allAttacks.thunder);
        attackDict.Add(allAttacks.voltStrike.name, allAttacks.voltStrike);
        attackDict.Add(allAttacks.aquaDart.name, allAttacks.aquaDart);
        attackDict.Add(allAttacks.mistySpray.name, allAttacks.mistySpray);
        attackDict.Add(allAttacks.shadowBP.name, allAttacks.shadowBP);
        attackDict.Add(allAttacks.windCyclone.name, allAttacks.windCyclone);
        attackDict.Add(allAttacks.energyDragon.name, allAttacks.energyDragon);


        //loops through all of the attacks and separates them in to lists based on their type
        foreach (KeyValuePair<string, MonsterAttack> attack in attackDict)
        {
            attackDict[attack.Key].Power.BaseValue = attackDict[attack.Key].power;
            if (attack.Value.type == "Water")
            {
                waterAttacks.Add(attack.Key);
                
            }

            if (attack.Value.type == "Electric")
            {
                electricAttacks.Add(attack.Key);

            }

            if (attack.Value.type == "Shadow")
            {
                shadowAttacks.Add(attack.Key);

            }

            if (attack.Value.type == "Nature")
            {
                natureAttacks.Add(attack.Key);

            }

            if (attack.Value.type == "Fire")
            {
                fireAttacks.Add(attack.Key);

            }
        }


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
