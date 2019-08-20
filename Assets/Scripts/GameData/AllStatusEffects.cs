using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public class Status
{
    public string name;
    public string effectType;

    public float hpDrop;
    public float atkDrop;
    public float defDrop;
    public float speDrop;
    public float precDrop;
    public float atkPowerDrop;
    public float atkTimeDrop;
    public float evasionDrop;
    public float enGenDrop;
    public float speedDrop;

    public float duration;
    public float interval;
    public Sprite statusSprite;
    
}
 
[System.Serializable]
public class AllStatuses
{
    public Status Burn = new Status
    {
        name = "Burn",
        effectType = "Fire",
        hpDrop = -.04f,
        duration = 5f,
        interval = 3f,
    };


    public Status FrostBite = new Status
    {
        name = "Frost Bite",
        effectType = "Ice",
        defDrop = -.20f,
        speDrop = -.15f,
        duration = 30f,
        interval = 30f,
    };


    public Status Paralysis = new Status
    {
        name = "Paralysis",
        effectType = "Electric",
        evasionDrop = -.45f,
        speDrop = -.25f,
        duration = 30f,
        interval = 30f,
    };


    public Status Poison = new Status
    {
        name = "Poison",
        effectType = "Nature",
        evasionDrop = -.45f,
        hpDrop = -.02f,
        defDrop = -.01f,
        duration = 8f,
        interval = 4f,
    };


    public Status Deafen = new Status
    {
        name = "Deafen",
        effectType = "Shadow",
        evasionDrop = -.06f,
        speDrop = -.03f,
        duration = 30f,
        interval = 4f,
    };

    public Status Confusion = new Status
    {
        name = "Confusion",
        effectType = "Magic",
        evasionDrop = -.30f,
        speDrop = -.55f,
        duration = 30f,
        interval = 30f,
    };
}

public class AllStatusEffects : MonoBehaviour
{
    public AllStatuses allStatuses = new AllStatuses();
    public Dictionary<string, Status> allStatusDict = new Dictionary<string, Status>();

    private void Awake()
    {
        AddStatuses();
    }

    public void AddStatuses()
    {
        allStatusDict.Add(allStatuses.Burn.name, allStatuses.Burn);
        allStatusDict.Add(allStatuses.FrostBite.name, allStatuses.FrostBite);
        allStatusDict.Add(allStatuses.Paralysis.name, allStatuses.Paralysis);
        allStatusDict.Add(allStatuses.Poison.name, allStatuses.Poison);
        allStatusDict.Add(allStatuses.Deafen.name, allStatuses.Deafen);
        allStatusDict.Add(allStatuses.Confusion.name, allStatuses.Confusion);
    }
}



public class StatusTimer
{
    public float interval;
    public float duration;

    public Monster Monster;
    public Status Status;
    public float acumTime;
    public StatusEffects statusEffects;
    public bool isEnemy;


    public StatusTimer(Monster monster, Status status)
    {
        
        interval = status.interval;
        duration = status.duration;
        Monster = monster;
        Status = status;

        if (Monster.isEnemy)
        {
            isEnemy = true;
        }
        else
        {
            isEnemy = false;
        }
            
        StatusEffects StatusEffects = new StatusEffects();
        statusEffects = StatusEffects;


    }
    //called from the Monster script to trigger another iteration of the status
    public IEnumerator TriggerEffect()
    {
        
        yield return new WaitForSeconds(interval);
        acumTime += interval;
        Monster.GetComponent<Monster>().TriggerStatus(Status, this, acumTime, statusEffects);


    }


}



public class StatusEffects 
{
    public Monster Monster;
    public Enemy Enemy;

    //public StatusEffects(Monster monster, Status status, bool isEnemy)
    //{

       
    //}

    public void ProcEffect(StatusTimer timer)
    {
        Monster monster = timer.Monster;
        //Enemy Enemy = timer.Enemy;
        Status status = timer.Status;
        bool isEnemy = timer.isEnemy;

        //if it's an enemy being inflicted with the status, change the enemy.stats instead of the monster.info, since an enemy derives its stats from the enemy.stats struct
        if (!isEnemy)
        {
            //monster.attack -= monster.attack * status.atkDrop;
            //monster.defense -= monster.defense * status.defDrop;
            //monster.evasion -= monster.evasion * status.evasionDrop;
            //monster.hp -= (monster.hp * status.hpDrop);
            //monster.speed -= monster.speed * status.speedDrop;
            //monster.energyGeneration -= monster.energyGeneration * status.enGenDrop;
            //monster.precision -= monster.precision * status.precDrop;

            if (status.hpDrop != 0)
                monster.tempStats.HP.AddModifier(new StatModifier(status.hpDrop, StatModType.PercentMult, this));
            if (status.atkDrop != 0)
                monster.tempStats.Attack.AddModifier(new StatModifier(status.atkDrop, StatModType.PercentMult, this));
            if (status.defDrop != 0)
                monster.tempStats.Defense.AddModifier(new StatModifier(status.defDrop, StatModType.PercentMult, this));
            if (status.speedDrop != 0)
                monster.tempStats.Speed.AddModifier(new StatModifier(status.speedDrop, StatModType.PercentMult, this));
            if (status.enGenDrop != 0)
                monster.tempStats.EnergyGeneration.AddModifier(new StatModifier(status.enGenDrop, StatModType.PercentMult, this));
            if (status.precDrop != 0)
                monster.tempStats.Precision.AddModifier(new StatModifier(status.precDrop, StatModType.PercentMult, this));

            Monster = monster;
        }
        else
        {
            Enemy enemy = monster.GetComponent<Enemy>();

            //enemy.stats.def -= (enemy.stats.def * status.defDrop);
            //enemy.stats.evasion -= (enemy.stats.evasion * status.evasionDrop);
            //enemy.stats.currentHp -= enemy.stats.currentHp * status.hpDrop;
            //enemy.stats.speed -= enemy.stats.speed * status.speDrop;

            if (status.hpDrop != 0)
                enemy.stats.HP.AddModifier(new StatModifier(status.hpDrop, StatModType.PercentMult, this));
            if (status.atkDrop != 0)
                enemy.stats.Attack.AddModifier(new StatModifier(status.atkDrop, StatModType.PercentMult, this));
            if (status.defDrop != 0)
                enemy.stats.Defense.AddModifier(new StatModifier(status.defDrop, StatModType.PercentMult, this));
            if (status.speedDrop != 0)
                enemy.stats.Speed.AddModifier(new StatModifier(status.speedDrop, StatModType.PercentMult, this));
            if (status.enGenDrop != 0)
                enemy.stats.EnergyGeneration.AddModifier(new StatModifier(status.enGenDrop, StatModType.PercentMult, this));
            if (status.precDrop != 0)
                enemy.stats.Precision.AddModifier(new StatModifier(status.precDrop, StatModType.PercentMult, this));

            Enemy = enemy;
        }
    }


    public void HealStatus(Monster monster, Status status, bool isEnemy)
    {
        Monster = monster;

        
        Monster.GetComponent<Monster>().RemoveStatus(status);

        if (!isEnemy)
        {
            monster.tempStats.HP.RemoveAllModifiersFromSource(this);
            monster.tempStats.Attack.RemoveAllModifiersFromSource(this);
            monster.tempStats.Defense.RemoveAllModifiersFromSource(this);
            monster.tempStats.Speed.RemoveAllModifiersFromSource(this);
            monster.tempStats.Precision.RemoveAllModifiersFromSource(this);
        }
        else
        {
            monster.GetComponent<Enemy>().stats.HP.RemoveAllModifiersFromSource(this);
            monster.GetComponent<Enemy>().stats.Attack.RemoveAllModifiersFromSource(this);
            monster.GetComponent<Enemy>().stats.Defense.RemoveAllModifiersFromSource(this);
            monster.GetComponent<Enemy>().stats.Speed.RemoveAllModifiersFromSource(this);
            monster.GetComponent<Enemy>().stats.Precision.RemoveAllModifiersFromSource(this);
        }
    }

    
}
