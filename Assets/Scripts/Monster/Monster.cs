
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Puppet2D;
using TMPro;


[System.Serializable]
public struct MonsterSaveToken
{

    [Header("SaveToken")]
    public int index;
    public string species;
    public string name;
    public int level;
    public int totalExp;
    public int rank;
    public string attack1;
    public string attack2;
    public string equip1;
    public string equip2;
    public int hpPot;
    public int atkPot;
    public int defPot;
    public int spePot;
    public int precPot;
    public int koCount;
    public int maxLevel;
    public string specialAbility;
    public string passiveSkill;

}

[System.Serializable]
public struct MonsterInfo
{
    [Header("Monster Stats")]

    public int index;
    public string species;
    public string name;
    public string type1;
    public string type2;
    public int dexId;
    public int hpBase;
    public int atkBase;
    public int defBase;
    public int speBase;
    public int precBase;
    public float critBase;
    public float evasionBase;
    public float coinGenBase;
    public int staminaBase;
    public int staminaMax;
    public int level;
    public int totalExp;
    public int maxLevel;
    public float levelConst;
    public float energyGenBase;
    public float energyCost;
    public MonsterClass Class;
    public float maxHP;
    public float currentHP;
    

    [Header("Monster Attacks")]
    public string attack1Name;
    public MonsterAttack attack1;
    public BaseAttack baseAttack1;
    public string attack2Name;
    public MonsterAttack attack2;
    public BaseAttack baseAttack2;

    [Header("Monster Ability")]
    public string abilityName;
    public Ability specialAbility;

    [Header("Monster Skill")]
    public string skillName;
    public PassiveSkill passiveSkill;

    [Header("Monster Equips")]
    public string equip1Name;
    public string equip2Name;

    public EquipmentScript equipment1;
    public EquipmentScript equipment2;

    public Equipment equip1;
    public Equipment equip2;
    


    [Header("Monster Stat Values")]
    public Stat HP;
    public Stat HPPotential;
    public Stat Attack;
    public Stat AttackPotential;
    public Stat Defense;
    public Stat DefensePotential;
    public Stat Speed;
    public Stat SpeedPotential;
    public Stat Precision;
    public Stat PrecisionPotential;
    

    
    
    public Stat Stamina;
    public Stat EnergyGeneration;
    public Stat EnergyCost;
    public Stat CoinGeneration;

    public int koCount;

    public bool isEquipped;

    public int monsterRank;

    //use this when sorting the monsters from your home, and then display them according to this index
    public int sortIndex;


    //new stats that need to be worked on
    
    public Stat EnergyDamageMod;
    public Stat PhysicalDamageMod;
    public Stat ExplodeDamageMod;
    public Stat PierceDamageMod;
    public Stat DropRateMod;

};

//these stats can change during a map without effecting it's permanent stats
[System.Serializable]
public struct TempStats
{
    [Header("Monster Temp Stats")]
    public Stat HP;
    public Stat HPPotential;
    public Stat Attack;
    public Stat AttackPotential;
    public Stat Defense;
    public Stat DefensePotential;
    public Stat Speed;
    public Stat SpeedPotential;
    public Stat Precision;
    public Stat PrecisionPotential;
    public Stat Stamina;
    public Stat EnergyGeneration;
    public Stat CoinGeneration;
    public Stat EnergyCost;
    public float evasionBase;
    public float critBase;

    
    public MonsterAttack attack1;
    public MonsterAttack attack2;

    //temporary monster stat modifiers
    public Stat EnergyDamageMod;
    public Stat PhysicalDamageMod;
    public Stat ExplodeDamageMod;
    public Stat PierceDamageMod;
    public Stat DropRateMod;

    public float energyDamageMod;
    public float physicalDamageMod;
    public float explodeDamageMod;
    public float pierceDamageMod;
    public float dropRateMod;

}

[System.Serializable]
public struct MonsterSpecs
{
    [Header("Monster Body Specs")]
    //used as the position anchors for tile placement so it appears that the monster's legs are on the tiles instead of the center of its body
    public GameObject[] legs;
    public GameObject head;
    public Vector3[] legPos;
};
public class Monster : MonoBehaviour
{


    public MonsterInfo info = new MonsterInfo();
    public MonsterSpecs specs = new MonsterSpecs();
    public TempStats tempStats = new TempStats();
    public MonsterSaveToken saveToken = new MonsterSaveToken();
    //public AllBaseAttacks allBaseAttacks = new AllBaseAttacks();
    //private BaseAttacks baseAttacks;

    private Tower tower;
    private Enemy enemy;

    //the number this monster is in the activeTowers Dictionary on the GameManager
    public int activeIndex;

    //checks if the monster being spawned is an enemy or not
    public bool isEnemy, isTower, isAttacking;

    //all of the values in regards to leveling up this monster
    public Dictionary<int, int> expToLevel = new Dictionary<int, int>();
    public Dictionary<int, int> totalExpForLevel = new Dictionary<int, int>();

    public Animator monsterMotion;

    //the puppet controller script that acts as the trigger for a monster's motions in the animator
    public Puppet2D_GlobalControl puppet;

    public GameObject monsterIcon, frontModel, shadow;

    //list to keep track of the tiles that are boosting this monster while it's on the map. this list is added to from the maptile script itself
    public List<MapTile> boostTiles = new List<MapTile>();

    public List<Status> statuses = new List<Status>();
    //use these icons to display a monster's current statuses
    public GameObject[] statusIcons;

    //script used to access the meshes that make up the monster
    public MeshBodyParts bodyParts;

    //lists to display all of the monster's stat mods
    public List<Stat> allStats = new List<Stat>();
    public List<StatModifier> statMods = new List<StatModifier>();

    //variable that contains all of the KOs the monster has gotten during this round
    public int currentMapKOs;

    //a list of global stats that this monster owns
    public List<GlobalStat> ownedGlobalStats = new List<GlobalStat>();
    //the global stats currently affecting this monster, to prevent duplicates of the same stat from being added
    public List<GlobalStat> activeGlobalStats = new List<GlobalStat>();

    private void Awake()
    {
        allStats.Clear();

        allStats.Add(info.HP);
        allStats.Add(info.HPPotential);
        allStats.Add(info.Attack);
        allStats.Add(info.AttackPotential);
        allStats.Add(info.Defense);
        allStats.Add(info.DefensePotential);
        allStats.Add(info.Speed);
        allStats.Add(info.SpeedPotential);
        allStats.Add(info.Precision);
        allStats.Add(info.PrecisionPotential);
        allStats.Add(info.Stamina);
        allStats.Add(info.EnergyGeneration);
        allStats.Add(info.EnergyCost);
        allStats.Add(info.CoinGeneration);
        allStats.Add(info.DropRateMod);
        allStats.Add(info.attack1.Power);
        allStats.Add(info.attack2.Power);


        


    }
    // Start is called before the first frame update
    public void Start()
    {
        

        tower = GetComponent<Tower>();
        enemy = GetComponent<Enemy>();


        monsterMotion.GetComponent<Animator>();
        puppet.GetComponent<Puppet2D_GlobalControl>();
        boostTiles.Clear();


        GetExpCurve();

        if (isEnemy)
        {
            enemy.enemyCanvas.SetActive(true);
            //expCanvas.SetActive(false);
            enemy.enabled = true;
            tower.enabled = false;
            //SetMonsterStats();
            monsterMotion.SetBool("isEnemy", true);

            

        }
        if (isTower)
        {
            enemy.enemyCanvas.SetActive(false);
            //expCanvas.SetActive(true);
            enemy.enabled = false;
            tower.enabled = true;
        }
        if (!isEnemy && !isTower)
        {
            enemy.enemyCanvas.SetActive(false);
            //expCanvas.SetActive(true);
            enemy.enabled = false;
            tower.enabled = false;
        }

        



        //EquipmentBoosts();
        var equipment = GameManager.Instance.GetComponent<Items>().allEquipsDict;
        var attacks = GameManager.Instance.baseAttacks.attackDict;

        if (equipment.ContainsKey(info.equip1Name))
        {
            EquipmentScript equip1 = Instantiate(equipment[info.equip1Name]);
            info.equipment1 = equip1;
            info.equipment1.info.equippedMonster = this;
        }
        else
        {
            info.equipment1 = null;
        }

        if (equipment.ContainsKey(info.equip2Name))
        {
            EquipmentScript equip2 = Instantiate(equipment[info.equip2Name]);
            info.equipment2 = equip2;
            info.equipment2.info.equippedMonster = this;
        }
        else
        {
            info.equipment2 = null;
        }


        info.sortIndex = info.index;


        if (attacks.ContainsKey(info.attack1Name))
        {
            info.baseAttack1 = new BaseAttack(attacks[info.attack1Name], this);
        }
        else
        {
            info.baseAttack1 = null;
        }

        if (attacks.ContainsKey(info.attack2Name))
        {
            info.baseAttack2 = new BaseAttack(attacks[info.attack2Name], this);
        }
        else
        {
            info.baseAttack2 = null;
        }
        //info.baseAttack1 = new BaseAttack(null, this);
        //info.baseAttack2 = new BaseAttack(null, this);
    }


   




    // Update is called once per frame
    void Update()
    {
        if (isEnemy)
        {
            enemy.enemyCanvas.SetActive(true);
        }
        else
        {
            enemy.enemyCanvas.SetActive(false);
        }

        

    }

   
    //Equip a new item to this monster
    public void EquipItem(EquipmentScript equip, int slot)
    {


        if (slot == 1)
        {
            info.equip1Name = equip.itemName;
            info.equipment1 = equip;
        }

        if (slot == 2)
        {
            

            info.equip2Name = equip.itemName;
            info.equipment2 = equip;

          
        }

        MonsterStatMods();
        SaveMonsterToken();
        GameManager.Instance.GetComponent<YourMonsters>().yourMonstersDict.Remove(info.index);
        GameManager.Instance.GetComponent<YourMonsters>().yourMonstersDict.Add(info.index, PlayerPrefs.GetString(info.index.ToString()));

    }

    //unequip an item from this monster
    public void UnEquipItem(EquipmentScript equip, int slot)
    {



        if (slot == 1)
        {
            info.equip1Name = "none";
            info.equipment1 = null;

        }
        if (slot == 2)
        {
            info.equip2Name = "none";
            info.equipment2 = null;
        }

        int itemCount = PlayerPrefs.GetInt(equip.name);
        PlayerPrefs.SetInt(equip.name, itemCount + 1);

        MonsterStatMods();
        SaveMonsterToken();
        GameManager.Instance.GetComponent<YourMonsters>().yourMonstersDict.Remove(info.index);
        GameManager.Instance.GetComponent<YourMonsters>().yourMonstersDict.Add(info.index, PlayerPrefs.GetString(info.index.ToString()));
        


    }


    ////when a new monster is being summoned, it's stats are randomized within the bounds of its species. Creates a new Playerpref for the amount of monsters, along with a playerpref string for the json object for the newly summoned object
    //public void SummonNewMonster(string name)
    //{
    //    bool haskey = PlayerPrefs.HasKey("MonsterCount");


    //    if (haskey)
    //    {
    //        int monsterCount = GameManager.Instance.monsterCount + 1;

    //        var attacksDict = GameManager.Instance.baseAttacks.attackDict;

    //        var dict = GameManager.Instance.monstersData.monstersAllDict;



    //        //get the monster's species dependent stats from the Game Manager
    //        if (dict.ContainsKey(name))
    //        {
    //            info.type1 = dict[name].type1;
    //            info.type2 = dict[name].type2;
    //            info.dexId = dict[name].id;
    //            info.hpBase = dict[name].hpBase;
    //            info.atkBase = dict[name].atkBase;
    //            info.defBase = dict[name].defBase;
    //            info.speBase = dict[name].speBase;
    //            info.precBase = dict[name].precBase;
    //            info.staminaBase = dict[name].staminaBase;
    //            info.level = 1;
    //            info.totalExp = 0;
    //            info.AttackPotential.BaseValue = Random.Range(0, 26);
    //            info.DefensePotential.BaseValue = Random.Range(0, 26);
    //            info.SpeedPotential.BaseValue = Random.Range(0, 26);
    //            info.PrecisionPotential.BaseValue = Random.Range(0, 26);
    //            info.HPPotential.BaseValue = Random.Range(0, 26);
    //            info.index = monsterCount;
    //            info.maxLevel = dict[name].maxLevel;
    //            info.levelConst = dict[name].levelConst;
    //            info.monsterRank = 0;



    //            PlayerPrefs.SetInt("MonsterCount", monsterCount);
    //            GameManager.Instance.monsterCount = monsterCount;

    //            //for each of the attacks this monster has in its base attack array, choose 2 at random to give to this monster
    //            //int rand = Random.Range(0, dict[name].baseAttacks.Length - 1);
    //            //int rand2 = Random.Range(0, dict[name].baseAttacks.Length - 1);


    //            int rand = Random.Range(0, dict[name].baseAttacks.Length - 1);
    //            info.attack1Name = dict[name].baseAttacks[rand];
    //            int rand2 = Random.Range(0, dict[name].baseAttacks.Length - 1);

    //            //make sure the 2 attacks aren't the same
    //            if (rand2 == rand)
    //            {
    //                if (rand2 == 0)
    //                {
    //                    info.attack2Name = dict[name].baseAttacks[rand + 1];
    //                }
    //                if (rand2 == dict[name].baseAttacks.Length - 1)
    //                {
    //                    info.attack2Name = dict[name].baseAttacks[rand - 1];
    //                }
    //            }
    //            else
    //            {
    //                info.attack2Name = dict[name].baseAttacks[rand];
    //            }


    //        }



    //        //sets the monster's nickname to its name if there isn't a nickname
    //        if (info.name != null || info.name == "")
    //        {
    //            info.name = info.species;
    //        }

    //        //load in the attacks of the monster
    //        if (attacksDict.ContainsKey(info.attack1Name))
    //        {
    //            //MonsterAttack attack = attacksDict[info.attack1Name];
    //            tempStats.attack1 = info.attack1;
    //            info.attack1 = attacksDict[info.attack1Name];
               

    //        }

    //        if (attacksDict.ContainsKey(info.attack2Name))
    //        {
    //            //MonsterAttack attack = attacksDict[info.attack2Name];
    //            //tempStats.attack2 = attack;
    //            info.attack2 = attacksDict[info.attack2Name];
    //            tempStats.attack2 = info.attack2;
    //        }

    //        SetMonsterStats();
            
    //    }
    //}



    ////gets the monster's information from the Game Manager and creates the monster's stats
    //public void SetMonsterStats()
    //{

    //    var attacksDict = GameManager.Instance.baseAttacks.attackDict;

    //    var equip = GameManager.Instance.items.allEquipmentDict;
    //    var dict = GameManager.Instance.monstersData.monstersAllDict;
    //    var name = info.species;


    //    //get the monster's species dependent stats from the Game Manager
    //    if (dict.ContainsKey(name))
    //    {
    //        info.type1 = dict[name].type1;
    //        info.type2 = dict[name].type2;
    //        info.dexId = dict[name].id;
    //        info.hpBase = dict[name].hpBase;
    //        info.atkBase = dict[name].atkBase;
    //        info.defBase = dict[name].defBase;
    //        info.speBase = dict[name].speBase;
    //        info.precBase = dict[name].precBase;
    //        info.staminaBase = dict[name].staminaBase;
    //        info.maxLevel = dict[name].maxLevel;
    //        info.levelConst = dict[name].levelConst;
    //        info.energyCost = dict[name].energyCost;
    //        info.energyGenBase = dict[name].energyGenBase;

    //        //for each of the attacks this monster has in its base attack array, choose 2 at random to give to this monster
    //        int rand = Random.Range(0, dict[name].baseAttacks.Length - 1);
    //        info.attack1Name = dict[name].baseAttacks[rand];
    //        int rand2 = Random.Range(0, dict[name].baseAttacks.Length - 1);

    //        //make sure the 2 attacks aren't the same
    //        if (rand2 == rand)
    //        {
    //            if (rand2 == 0)
    //            {
    //                info.attack2Name = dict[name].baseAttacks[rand + 1];
    //            }
    //            if (rand2 == dict[name].baseAttacks.Length - 1)
    //            {
    //                info.attack2Name = dict[name].baseAttacks[rand - 1];
    //            }
    //        }
    //        else
    //        {
    //            info.attack2Name = dict[name].baseAttacks[rand];
    //        }


    //    }




    //    //sets the monster's nickname to its name if there isn't a nickname
    //    if (info.name != null || info.name == "")
    //    {
    //        info.name = info.species;
    //    }

    //    //load in the attacks of the monster
    //    if (attacksDict.ContainsKey(info.attack1Name))
    //    {

    //        MonsterAttack attack = attacksDict[info.attack1Name];


    //        tempStats.attack1.Power.BaseValue = attack.power;
    //        tempStats.attack1.Range.BaseValue = attack.range;
    //        tempStats.attack1.CritChance.BaseValue = attack.critChance;
    //        tempStats.attack1.CritMod.BaseValue = attack.critMod;
    //        tempStats.attack1.EffectChance.BaseValue = attack.effectChance;
    //        tempStats.attack1.AttackTime.BaseValue = attack.attackTime;
    //        tempStats.attack1.AttackSpeed.BaseValue = attack.attackSpeed;
    //        tempStats.attack1.AttackSlow.BaseValue = attack.hitSlowTime;



    //    }

    //    if (attacksDict.ContainsKey(info.attack2Name))
    //    {
    //        MonsterAttack attack = attacksDict[info.attack2Name];

    //        tempStats.attack2.Power.BaseValue = attack.power;
    //        tempStats.attack2.Range.BaseValue = attack.range;
    //        tempStats.attack2.CritChance.BaseValue = attack.critChance;
    //        tempStats.attack2.CritMod.BaseValue = attack.critMod;
    //        tempStats.attack2.EffectChance.BaseValue = attack.effectChance;
    //        tempStats.attack2.AttackTime.BaseValue = attack.attackTime;
    //        tempStats.attack2.AttackSpeed.BaseValue = attack.attackSpeed;
    //        tempStats.attack2.AttackSlow.BaseValue = attack.hitSlowTime;
    //    }




    //    //if the monster has equipment attached to it, set their value to "none" to avoid Null Exceptions
    //    if (info.equip1Name == null)
    //    {
    //        info.equip1Name = "none";
    //    }
    //    if (info.equip2Name == null)
    //    {
    //        info.equip2Name = "none";
    //    }





    //    //if this monster is an enemy, then fill in the stats for the enemy with randomized values for Potential
    //    if (isEnemy)
    //    {

    //        //enemy.SetEnemyStats();

    //    }
    //    //if it is NOT an enemy, then it already has prefab stats, so get those
    //    else
    //    {

            
    //        StatsCalc stats = new StatsCalc(gameObject.GetComponent<Monster>());
    //        GetStats(stats);

    //    }


    //}



    


    //****************The following methods are for calculating the stats of the monster***********//
    public void GetStats(StatsCalc stats)
    {

        
        info = stats.Monster.info;
        tempStats = stats.Monster.tempStats;
        SaveMonsterToken();

        
        //MonsterTokenSet();
        //PlayerPrefs.SetString(info.index.ToString(), JsonUtility.ToJson(info));
        //GameManager.Instance.GetComponent<YourMonsters>().GetYourMonsters();
        //SaveMonsterToken();




    }

   

    //statuses are added from monster attacks or from tiles on the map. they are added here and the statuses being calculating
    public void AddStatus(Status status)
    {
        StatusTimer timer = new StatusTimer(gameObject.GetComponent<Monster>(), status);
        StartCoroutine(timer.TriggerEffect());
        if (statuses.Contains(status))
        {
            //
        }
        else
        {
            statuses.Add(status);
            if (GameManager.Instance.GetComponent<AllStatusEffects>().allStatusDict.ContainsKey(status.name))
            {
                statusIcons[statuses.Count - 1].GetComponent<SpriteRenderer>().sprite = GameManager.Instance.GetComponent<AllStatusEffects>().allStatusDict[status.name].statusSprite;
            }
        }


    }

    //removing a status is also called from the AllStatusEffects script. Remove the called status and set the status icons to show the correct statuses
    public void RemoveStatus(Status status)
    {
        for (int i = 0; i < statusIcons.Length; i++)
        {
            statusIcons[i].GetComponent<SpriteRenderer>().sprite = null;
        }

        statuses.Remove(status);

        for (int i = 0; i < statuses.Count; i++)
        {
            statusIcons[i].GetComponent<SpriteRenderer>().sprite = GameManager.Instance.GetComponent<AllStatusEffects>().allStatusDict[statuses[i].name].statusSprite;
        }

    }

    //called from the StatusTimer
    public void TriggerStatus(Status status, StatusTimer timer, float acumTime, StatusEffects effect)
    {
       
        if (isEnemy)
        {
            effect.ProcEffect(timer);
            enemy.CalculateStatus(effect);
            

            //if the monster has been inflicted with the status for longer than the duration that the status can be inflicted for, then it is no longer afflicted. if not, run the status trigger again
            if (acumTime <= status.duration)
            {
                StartCoroutine(timer.TriggerEffect());
                var display = Instantiate(enemy.damageText, transform.position, Quaternion.identity);
                display.transform.SetParent(enemy.enemyCanvas.transform, true);
                display.GetComponentInChildren<TMP_Text>().text = "-" + status.name;
                Destroy(display, display.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);

            }
            else
            {
                if (statuses.Contains(status))
                {
                    effect.HealStatus(this, status);
                }
            }
        }
        else
        {
            //StatusEffects effect = new StatusEffects(this, status, false);
            effect.ProcEffect(timer);
            CalculateStatus(effect);

            //if the monster has been inflicted with the status for longer than the duration that the status can be inflicted for, then it is no longer afflicted. if not, run the status trigger again
            if (acumTime <= status.duration)
            {
                
                StartCoroutine(timer.TriggerEffect());
                var display = Instantiate(enemy.damageText, transform.position, Quaternion.identity);
                display.transform.SetParent(enemy.enemyCanvas.transform, false);
                display.GetComponentInChildren<TMP_Text>().text = "-" + status.name;
                Destroy(display, display.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            }
            else
            {
                if (statuses.Contains(status))
                {
                    effect.HealStatus(this, status);
                }
            }
        }


        
    }

    public void CalculateStatus(StatusEffects effect)
    {

        //tempStats = effect.Monster.tempStats;
        info = effect.Monster.info;
        tempStats = effect.Monster.tempStats;
 
    }



    //create the monster's exp curve given it's information from the data scripts
    public void GetExpCurve()
    {
        int totalExp = new int();

        for (int i = 1; i <= info.maxLevel; i++)
        {
            if (i == 1)
            {

                expToLevel.Add(i, 0);
                totalExpForLevel.Add(i, 0);
            }
            else
            {
                int toNextLevel = Mathf.FloorToInt(Mathf.Pow(i, info.levelConst));
                expToLevel.Add(i, (int)Mathf.Round(toNextLevel));
                totalExp += (int)Mathf.Round(toNextLevel);
                totalExpForLevel.Add(i, totalExp);
            }


            
            if (i >= info.maxLevel)
            {
                SetExp();
            }
        }

        
    }

    public void SetExp()
    {
        if (expToLevel.ContainsKey(info.level))
        {
            int toNextLevel = expToLevel[info.level + 1];
            int totalNextLevel = totalExpForLevel[info.level + 1];
            int nextLevelDiff = totalNextLevel - info.totalExp;
        }
    }

    //this is called from the defeated enemy
    public void GainEXP(int expGained)
    {
        

        info.totalExp += (int)Mathf.Round(expGained);
        
        info.koCount += 1;
        


        if (expToLevel.ContainsKey(info.level))
        {
            int toNextLevel = expToLevel[info.level + 1];
            int totalNextLevel = totalExpForLevel[info.level + 1];
            int nextLevelDiff = totalNextLevel - info.totalExp;

            
            if (expGained >= nextLevelDiff)
            {
                OnLevelUp();

                return;
            }


            SaveMonsterToken();
        }

        


    }

    public void OnLevelUp()
    {
        info.level += 1;
        saveToken.level = info.level;

        SetExp();
        StatsCalc stats = new StatsCalc(gameObject.GetComponent<Monster>());
        GetStats(stats);
        GameManager.Instance.GetComponent<YourMonsters>().yourMonstersDict.Remove(info.index);
        GameManager.Instance.GetComponent<YourMonsters>().yourMonstersDict.Add(info.index, PlayerPrefs.GetString(info.index.ToString()));
        GameManager.Instance.SendNotificationToPlayer(info.species, info.level, NotificationType.LevelUp, "none");
        
    }




    //use this when summoning a monster
    public void MonsterSummon(string name)
    {
        bool hasKey = PlayerPrefs.HasKey("MonsterCount");


        if (hasKey)
        {
            int monsterCount = GameManager.Instance.monsterCount + 1;

            var monsters = GameManager.Instance.monstersData.monstersAllDict;
            

            //get the monster's species dependent stats from the Game Manager
            if (monsters.ContainsKey(name))
            {
                MonsterToken token = new MonsterToken();

                info.name = name;
                info.species = name;
                info.level = 1;
                info.totalExp = 0;
                info.AttackPotential.BaseValue = Random.Range(0, 26);
                info.DefensePotential.BaseValue = Random.Range(0, 26);
                info.SpeedPotential.BaseValue = Random.Range(0, 26);
                info.PrecisionPotential.BaseValue = Random.Range(0, 26);
                info.HPPotential.BaseValue = Random.Range(0, 26);
                info.index = monsterCount;
                info.maxLevel = monsters[name].maxLevel;
                info.levelConst = monsters[name].levelConst;
                info.monsterRank = 1;
                
                

                PlayerPrefs.SetInt("MonsterCount", monsterCount);
                GameManager.Instance.monsterCount = monsterCount;

                //set the monster's first 2 base attacks
                int rand = Random.Range(0, monsters[name].baseAttacks.Length - 1);
                info.attack1Name = monsters[name].baseAttacks[rand];
                int rand2 = Random.Range(0, monsters[name].baseAttacks.Length - 1);
                info.attack2Name = monsters[name].baseAttacks[rand2];

                //set the monster's ability'
                if (monsters[name].abilities.Length > 1)
                {
                    int ab = Random.Range(0, monsters[name].abilities.Length - 1);
                    info.abilityName = monsters[name].abilities[ab];
                }
                else
                {
                    info.abilityName = monsters[name].abilities[0];
                }

                //set the monster's skill
                if (monsters[name].skills.Length > 1)
                {
                    int sk = Random.Range(0, monsters[name].skills.Length - 1);
                    info.skillName = monsters[name].skills[sk];
                }
                else
                {
                    info.skillName = monsters[name].skills[0];
                }

                info.equip1Name = "none";
                info.equip2Name = "none";


                token.CreateMonsterToken(this);
                saveToken = token.newToken;

                
                SaveMonsterToken();
                LoadMonsterToken(saveToken);
                GameManager.Instance.GetComponent<YourMonsters>().GetYourMonsters();
                
            }

            
        }
    }



    public void LoadMonsterToken(MonsterSaveToken m)
    {
        MonsterStatsSet();

        StatsCalc stats = new StatsCalc(gameObject.GetComponent<Monster>());
        GetStats(stats);
        
    }


    public void SaveMonsterToken()
    {
        MonsterToken token = new MonsterToken();
        token.CreateMonsterToken(this);
        saveToken = token.newToken;

        
        PlayerPrefs.SetString(saveToken.index.ToString(), JsonUtility.ToJson(saveToken));

        //change this monster's dictionary item in your monster list dictionary
        GameManager.Instance.GetComponent<YourMonsters>().MonsterList(info.index, this);

        
        //GameManager.Instance.GetComponent<YourMonsters>().GetYourMonsters();
        //GameManager.Instance.GetComponent<YourMonsters>().MonsterList(this);

    }

   


    //set the monster's stats from the values of the save token
    public void MonsterStatsSet()
    {
        var monsters = GameManager.Instance.monstersData.monstersAllDict;
        var attacks = GameManager.Instance.baseAttacks.attackDict;
        var abilities = GameManager.Instance.GetComponent<MonsterAbilities>().allAbilitiesDict;
        var skills = GameManager.Instance.GetComponent<AllSkills>().allSkillsDict;
        var allEquips = GameManager.Instance.items.allEquipsDict;

        info.type1 = monsters[info.species].type1;
        info.type2 = monsters[info.species].type2;
        info.maxLevel = monsters[info.species].maxLevel;
        info.levelConst = monsters[info.species].levelConst;
        info.energyCost = monsters[info.species].energyCost;
        info.energyGenBase = monsters[info.species].energyGenBase;
        info.type1 = monsters[info.species].type1;
        info.type2 = monsters[info.species].type2;
        info.dexId = monsters[info.species].id;
        info.hpBase = monsters[info.species].hpBase;
        info.atkBase = monsters[info.species].atkBase;
        info.defBase = monsters[info.species].defBase;
        info.speBase = monsters[info.species].speBase;
        info.precBase = monsters[info.species].precBase;
        info.staminaBase = monsters[info.species].staminaBase;
        info.Class = monsters[info.species].Class;
        info.coinGenBase = monsters[info.species].coinGenBase;

        info.attack1Name = saveToken.attack1;
        info.attack2Name = saveToken.attack2;
        info.index = saveToken.index;
        info.level = saveToken.level;
        info.totalExp = saveToken.totalExp;
        info.monsterRank = saveToken.rank;
        info.koCount = saveToken.koCount;
        info.AttackPotential.BaseValue = saveToken.atkPot;
        info.DefensePotential.BaseValue = saveToken.defPot;
        info.SpeedPotential.BaseValue = saveToken.spePot;
        info.PrecisionPotential.BaseValue = saveToken.precPot;
        info.HPPotential.BaseValue = saveToken.hpPot;
        info.name = saveToken.name;
        info.equip1Name = saveToken.equip1;
        info.equip2Name = saveToken.equip2;
        info.maxLevel = saveToken.maxLevel;
        
        

        if (allEquips.ContainsKey(info.equip1Name))
        {
            info.equipment1 = allEquips[info.equip1Name];
        }
        else
        {
            info.equipment1 = null;
        }

        if (allEquips.ContainsKey(info.equip2Name))
        {
            info.equipment2 = allEquips[info.equip2Name];
        }
        else
        {
            info.equipment2 = null;
        }

        info.abilityName = saveToken.specialAbility;
        info.specialAbility = abilities[info.abilityName];
        info.skillName = saveToken.passiveSkill;
        info.passiveSkill = new PassiveSkill(this, skills[info.skillName]);
        //info.equippable1 = new EquippableItem();
        //info.equippable2 = new EquippableItem();
        //info.baseAttack1.attack = attacks[info.attack1Name];
        //info.baseAttack2.attack = attacks[info.attack2Name];

        if (attacks.ContainsKey(info.attack1Name))
        {
            info.baseAttack1 = new BaseAttack(attacks[info.attack1Name], this);
        }
        else
        {
            info.baseAttack1 = null;
        }

        if (attacks.ContainsKey(info.attack2Name))
        {
            info.baseAttack2 = new BaseAttack(attacks[info.attack2Name], this);
        }
        else
        {
            info.baseAttack2 = null;
        }

        SetExp();

 
    }

    //call this at the start of each game so the equipment items can be in effect during the games
    public void MonsterEquipment()
    {
       
        var equipment = GameManager.Instance.GetComponent<Items>().allEquipsDict;

        if (equipment.ContainsKey(info.equip1Name))
        {
            //info.equip1.equipment = equipment[info.equip1Name];
            //info.equip1.equipment.GetEquipInfo(this, 1);


            EquipmentScript equip1 = Instantiate(equipment[info.equip1Name]);
            info.equipment1 = equip1;
            //info.equipment2.trigger = new EventTrigger(info.equipment2.triggerType, info.equipment2);
            info.equipment1.GetEquipInfo(this, 1);


        }
        if (equipment.ContainsKey(info.equip2Name))
        {
            //info.equip2.equipment = equipment[info.equip2Name];
            //info.equip2.equipment.GetEquipInfo(this, 2);


            EquipmentScript equip2 = Instantiate(equipment[info.equip2Name]);
            info.equipment2 = equip2;
            //info.equipment2.trigger = new EventTrigger(info.equipment2.triggerType, info.equipment2);
            info.equipment2.GetEquipInfo(this, 2);

        }

    }

    //call this when a monster is summoned to change its stats based on the weather, if at all
    public IEnumerator WeatherCheck()
    {
        do
        {
            if (GameManager.Instance.activeMap.mapWeather == MapWeather.Sunny)
            {
                shadow.gameObject.SetActive(true);
            }
            else
            {
                shadow.gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(.05f);
        } while (true);
    }


    //use this to get a monster's potential stats without saving them
    public void CheckStats(MonsterSaveToken m)
    {
        MonsterStatsSet();

        StatsCalc stats = new StatsCalc(gameObject.GetComponent<Monster>());      
        info = stats.Monster.info;
        tempStats = stats.Monster.tempStats;
        
    }

    //call this from YourMonsters.cs to send a message to your monsters
    public void AcceptMessage()
    {

    }


    //use this to launch an attack from a menu screen
    public void TestAttack()
    {
        MonsterAttack attack = new MonsterAttack();
        BaseAttack baseAttack = new BaseAttack(null, this);

        if (tower.attackNumber == 1)
        {
            attack = info.attack1;
            baseAttack = info.baseAttack1;
        }
        else
        {
            attack = info.attack2;
            baseAttack = info.baseAttack2;
        }



        var attackSprite = Instantiate(GameManager.Instance.baseAttacks.attackDict[baseAttack.attack.name].attackAnimation, tower.attackPoint.transform.position, Quaternion.Euler(transform.eulerAngles));
        attackSprite.gameObject.name = baseAttack.attack.name;
        attackSprite.transform.localScale = attackSprite.transform.localScale * 2;
        attackSprite.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "PopMenu";
        //attackSprite.GetComponent<AttackEffects>().FromAttacker(attack, attack.name, attack.type, monster.attack, (int)attack.Power.Value, monster.info.level, attack.CritChance.Value, attack.CritMod.Value, gameObject.GetComponent<Monster>());
        attackSprite.GetComponent<AttackEffects>().FromAttacker(baseAttack.attack, baseAttack.attack.name, baseAttack.attack.type, tempStats.Attack.Value, (int)baseAttack.attack.Power.Value, info.level, baseAttack.attack.CritChance.Value, baseAttack.attack.CritMod.Value, gameObject.GetComponent<Monster>());
        attackSprite.GetComponent<AttackEffects>().AttackMotion(Vector2.right * 25);


    }

    //call this to refresh the monster's list of stat modifiers
    public void MonsterStatMods()
    {
        statMods.Clear();

        foreach(Stat stat in allStats)
        {
            foreach(StatModifier mod in stat.StatModifiers)
            {
                statMods.Add(mod);
                
            }
        }
 
    }


    //call this from other scripts to display the icon of the monster instead of its full body
    public void DisplayIcon()
    {
       

        if (enemy)
        {
            enemy.enemyCanvas.SetActive(false);
        }
        LoadMonsterToken(saveToken);

        monsterIcon.SetActive(true);
        
    }

    //use this to call the monster's passive skill
    public void PassiveSkill()
    {
       


        
        info.passiveSkill.ActivateSkill();
        
        
    }

    //use this to check the global stat mods in play and apply them to this monster
    public void GlobalStatMod(MapDetails map)
    {
        //checks all of the global stats and adds their effects to this monster if it hasn't been added yet
        for (int i = 0; i < map.activeGlobalStats.Count; i++)
        {
            if (!activeGlobalStats.Contains(map.activeGlobalStats[i]))
            {

                activeGlobalStats.Add(map.activeGlobalStats[i]);
                map.activeGlobalStats[i].AddStat(this);

            }
        }

        

        MonsterStatMods();
        

    }

    public void OnDestroy()
    {
        
    }
}
