using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MonsterAttack
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
    //modifier that affects the length of time between this attack's uses
    public float attackTime;
    //how fast the attack moves towards enemies
    public float attackSpeed;
    //length of time a monster is slowed when hit by this attack
    public float hitSlowTime;
    //either projectile or physical
    public AttackMode attackMode;

    public GameObject attackAnimation;


    public DamageForce forceType;

    //every time this attack is used, the monster using it gains this amount of stamina
    public float staminaGained;

    
    

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
    Mystical,
}

public enum DamageForce
{
    Energy, 
    Physical, 
    Pierce, 
    Explode,
}


public struct AttackStats
{
    public Stat Range;
    public Stat Power;
    public Stat CritChance;
    public Stat CritMod;
    public Stat EffectChance;
    public Stat AttackTime;
    public Stat AttackSpeed;
    public Stat AttackSlow;
    public Stat StaminaGained;
}
[System.Serializable]
//use this for the object of the attack, so that each monster can have their own instances of the attacks
public class BaseAttack
{
    public MonsterAttack attack;
    public Monster owner;
    public AttackStats stats;
   

    public string type;
    public DamageForce forceType;
    public string effectName;
    public AttackMode attackMode;


    public Stat Range = new Stat();
    public Stat Power = new Stat();
    public Stat CritChance = new Stat();
    public Stat CritMod = new Stat();
    public Stat EffectChance = new Stat();
    public Stat AttackTime = new Stat();
    public Stat AttackSpeed = new Stat();
    public Stat AttackSlow = new Stat();
    public Stat StaminaGained = new Stat();

    public BaseAttack(MonsterAttack Attack, Monster Owner)
    {
        attack = Attack;
        owner = Owner;

        //var attacks = GameManager.Instance.baseAttacks.attackDict;

        //attack = attacks[Attack.name];

        //Debug.Log(attack.range);

        SetAttack();
       
    }

    public void SetAttack()
    {
        Range.BaseValue = attack.range;
        Power.BaseValue = attack.power;
        CritChance.BaseValue = attack.critChance;
        CritMod.BaseValue = attack.critMod;
        EffectChance.BaseValue = attack.effectChance;
        AttackTime.BaseValue = attack.attackTime;
        AttackSpeed.BaseValue = attack.attackSpeed;
        AttackSlow.BaseValue = attack.hitSlowTime;
        StaminaGained.BaseValue = attack.staminaGained;
        type = attack.type;
        forceType = attack.forceType;
        effectName = attack.effectName;
        attackMode = attack.attackMode;
    }
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
        staminaGained = .06f,
        
        

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
        staminaGained = .06f,

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
        attackTime = 1.2f,
        effectChance = .15f,
        attackSpeed = 7f,
        hitSlowTime = .25f,
        attackMode = AttackMode.Projectile,
        staminaGained = .06f,


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
        staminaGained = .06f,

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
        attackTime = 1.3f,
        effectChance = .10f,
        attackSpeed = 13f,
        hitSlowTime = .35f,
        attackMode = AttackMode.Punch,
        staminaGained = .06f,

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
        attackTime = 1.0f,
        effectChance = .10f,
        attackSpeed = 7f,
        hitSlowTime = .75f,
        attackMode = AttackMode.Punch,
        staminaGained = .06f,


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
        attackTime = 1.1f,
        effectChance = .15f,
        attackSpeed = 5f,
        hitSlowTime = .85f,
        attackMode = AttackMode.Punch,
        staminaGained = .06f,


    };

    public MonsterAttack darknessPike = new MonsterAttack
    {
        name = "Darkness Pike",
        id = 7,
        description = "Spirits from departed souls pierce the living veil.",
        type = "Shadow",
        effectName = "none",
        range = 3,
        power = 75,
        critChance = 1f,
        critMod = 1f,
        attackTime = 1.2f,
        effectChance = .15f,
        attackSpeed = 7f,
        hitSlowTime = .63f,
        attackMode = AttackMode.Punch,
        staminaGained = .06f,


    };

    public MonsterAttack Fireball = new MonsterAttack
    {
        name = "Fireball",
        id = 8,
        description = "An orb of fire.",
        type = "Fire",
        effectName = "Burn",
        range = 2,
        power = 105,
        critChance = 1f,
        critMod = 1f,
        attackTime = 0.8f,
        effectChance = .15f,
        attackSpeed = 7f,
        hitSlowTime = .41f,
        attackMode = AttackMode.Projectile,
        staminaGained = .06f,


    };

    public MonsterAttack FrostShot = new MonsterAttack
    {
        name = "Frost Shot",
        id = 9,
        description = "A frigid blast of sub-arctic energy.",
        type = "Ice",
        effectName = "none",
        range = 3,
        power = 85,
        critChance = 1f,
        critMod = 1f,
        attackTime = 0.7f,
        effectChance = .15f,
        attackSpeed = 5f,
        hitSlowTime = .84f,
        attackMode = AttackMode.Projectile,
        staminaGained = .08f,


    };

    public MonsterAttack VirusBlaster = new MonsterAttack
    {
        name = "Virus Blaster",
        id = 10,
        description = "An orb of energy from the digital plane.",
        type = "Mechanical",
        effectName = "Glitched",
        range = 2,
        power = 115,
        critChance = 1f,
        critMod = 1f,
        attackTime = 0.7f,
        effectChance = .15f,
        attackSpeed = 6.5f,
        hitSlowTime = .77f,
        attackMode = AttackMode.Projectile,
        staminaGained = .08f,


    };

    public MonsterAttack ArcaneCutter = new MonsterAttack
    {
        name = "Arcane Cutter",
        id = 11,
        description = "A magical blast of arcane energy.",
        type = "Magic",
        effectName = "none",
        range = 3,
        power = 90,
        critChance = 1f,
        critMod = 1f,
        attackTime = 0.6f,
        effectChance = .15f,
        attackSpeed = 9.1f,
        hitSlowTime = .57f,
        attackMode = AttackMode.Projectile,
        staminaGained = .05f,


    };

    public MonsterAttack NoxiousTwister = new MonsterAttack
    {
        name = "Noxious Twister",
        id = 12,
        description = "Tainted gases form together with wicked intensity.",
        type = "Poison",
        effectName = "Poison",
        range = 3,
        power = 75,
        critChance = 1f,
        critMod = 1f,
        attackTime = 1.1f,
        effectChance = .45f,
        attackSpeed = 10.2f,
        hitSlowTime = 1.2f,
        attackMode = AttackMode.Projectile,
        staminaGained = .11f,


    };


}


public class Attacks : MonoBehaviour
{
   

    public AllAttacks allAttacks = new AllAttacks();
    public Dictionary<string, MonsterAttack> attackDict = new Dictionary<string, MonsterAttack>();

    //this is populated in the Inspector
    public List<Sprite> attackModeSprites;
    //this is filled on Awake
    public Dictionary<string, Sprite> atkModeDict = new Dictionary<string, Sprite>();

    
    public List<string> waterAttacks = new List<string>();
    public List<string> electricAttacks = new List<string>();
    public List<string> shadowAttacks = new List<string>();
    public List<string> natureAttacks = new List<string>();
    public List<string> fireAttacks = new List<string>();
    public List<string> iceAttacks = new List<string>();
    public List<string> poisonAttacks = new List<string>();
    public List<string> magicAttacks = new List<string>();
    public List<string> mechAttacks = new List<string>();
    public List<string> normalAttacks = new List<string>();



    //create a dictionary of all of the attack names with their corresponding objects so enemies can get all of their attack information from the dictionary
    private void Awake()
    {
        SetBaseAttacks();
        SetSprites();
       
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
        attackDict.Add(allAttacks.darknessPike.name, allAttacks.darknessPike);
        attackDict.Add(allAttacks.Fireball.name, allAttacks.Fireball);
        attackDict.Add(allAttacks.FrostShot.name, allAttacks.FrostShot);
        attackDict.Add(allAttacks.VirusBlaster.name, allAttacks.VirusBlaster);
        attackDict.Add(allAttacks.ArcaneCutter.name, allAttacks.ArcaneCutter);
        attackDict.Add(allAttacks.NoxiousTwister.name, allAttacks.NoxiousTwister);


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

            if (attack.Value.type == "Poison")
            {
                poisonAttacks.Add(attack.Key);

            }

            if (attack.Value.type == "Magic")
            {
                magicAttacks.Add(attack.Key);

            }

            if (attack.Value.type == "Ice")
            {
                iceAttacks.Add(attack.Key);

            }

            if (attack.Value.type == "Mechanical")
            {
                mechAttacks.Add(attack.Key);

            }

            if (attack.Value.type == "Normal")
            {
                normalAttacks.Add(attack.Key);

            }
        }


    }


    //set the sprite dictionaries for the attack effects
    public void SetSprites()
    {
        atkModeDict.Add("Punch", attackModeSprites[0]);
        atkModeDict.Add("Kick", attackModeSprites[1]);
        atkModeDict.Add("Projectile", attackModeSprites[2]);
        atkModeDict.Add("Physical", attackModeSprites[3]);
        atkModeDict.Add("Mystical", attackModeSprites[4]);
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
