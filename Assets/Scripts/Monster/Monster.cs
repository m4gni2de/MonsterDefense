
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
    public int equip1Exp;
    public string equip2;
    public int equip2Exp;
    public int hpPot;
    public int atkPot;
    public int defPot;
    public int spePot;
    public int precPot;
    public int koCount;
    public int maxLevel;
    public string specialAbility;
    public string passiveSkill;
    public int equip1Slot;
    public int equip2Slot;
    public bool isStar;
    public int defenderIndex;

}

[System.Serializable]
public struct MonsterInfo
{
    [Header("Monster Stats")]

    public int index;
    public int defenderIndex;
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
    public MonsterAbility specialAbility;

    [Header("Monster Skill")]
    public string skillName;
    public PassiveSkill passiveSkill;

    [Header("Monster Equips")]
    public string equip1Name;
    public string equip2Name;

    //public EquipmentScript equipment1;
    //public EquipmentScript equipment2;

    public Equipment equip1;
    public int equip1Slot;
    public int equip1Exp;
    public Equipment equip2;
    public int equip2Slot;
    public int equip2Exp;


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

    //bool to tell if the monster is a Star Form or not. Star Forms have different sprites
    public bool isStar;

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

    //the animator that controls the monster's movement
    public Animator monsterMotion;

    //the puppet controller script that acts as the trigger for a monster's motions in the animator
    public Puppet2D_GlobalControl puppet;

    //variables related to the visual models of the monster
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
    //the aura that glows when the monster's ability is ready
    public GameObject abilityAura;

    
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
        var yourEquips = GameManager.Instance.Inventory.EquipmentPocket.items;

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
        info.sortIndex = info.index; 
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
    public void EquipItem(Equipment equip, int slot)
    {


        if (slot == 1)
        {
            info.equip1Name = equip.itemName;
            info.equip1 = equip;
            info.equip1Slot = equip.inventorySlot.slotIndex;

        }

        if (slot == 2)
        {
            

            info.equip2Name = equip.itemName;
            info.equip2 = equip;
            info.equip2Slot = equip.inventorySlot.slotIndex;



        }

        MonsterStatMods();
        SaveMonsterToken();
        GameManager.Instance.GetComponent<YourMonsters>().yourMonstersDict.Remove(info.index);
        GameManager.Instance.GetComponent<YourMonsters>().yourMonstersDict.Add(info.index, PlayerPrefs.GetString(info.index.ToString()));

    }

    //unequip an item from this monster
    public void UnEquipItem(Equipment equip, int slot)
    {
        if (slot == 1)
        {
            info.equip1Name = "none";
            info.equip1 = null;
            info.equip1Slot = 0;
        }
        if (slot == 2)
        {
            info.equip2Name = "none";
            info.equip2 = null;
            info.equip1Slot = 0;          
        }

        MonsterStatMods();
        SaveMonsterToken();
        GameManager.Instance.GetComponent<YourMonsters>().yourMonstersDict.Remove(info.index);
        GameManager.Instance.GetComponent<YourMonsters>().yourMonstersDict.Add(info.index, PlayerPrefs.GetString(info.index.ToString()));
        
    }

    //****************The following methods are for calculating the stats of the monster***********//
    public void GetStats(StatsCalc stats)
    {    
        info = stats.Monster.info;
        tempStats = stats.Monster.tempStats;
        SaveMonsterToken();
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


    public void UseItem()
    {
        SaveMonsterToken();

        StatsCalc stats = new StatsCalc(gameObject.GetComponent<Monster>());
        GetStats(stats);
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

                int starChance = monsters[name].starChance;
                int randStar = Random.Range(0, 500);

                if (randStar <= starChance)
                {
                    info.isStar = true;
                }

                

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
        var yourEquips = GameManager.Instance.Inventory.EquipmentPocket.items;

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
        info.defenderIndex = saveToken.defenderIndex;
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
        info.equip1Exp = saveToken.equip1Exp;
        info.equip2Name = saveToken.equip2;
        info.equip2Exp = saveToken.equip2Exp;
        info.maxLevel = saveToken.maxLevel;
        info.equip1Slot = saveToken.equip1Slot;
        info.equip2Slot = saveToken.equip2Slot;
        info.isStar = saveToken.isStar;



        if (allEquips.ContainsKey(info.equip1Name))
        {
            //info.equipment1 = allEquips[info.equip1Name];
            EquipmentScript eq = Instantiate(allEquips[info.equip1Name]);
            info.equip1 = new Equipment(eq);

            for (int i = 0; i < yourEquips.Count; i++)
            {
                if (yourEquips[i].slotIndex == info.equip1Slot)
                {
                    //info.equip1.monster = this;
                    info.equip1.SetInventorySlot(yourEquips[i]);
                    info.equip1Exp = info.equip1.inventorySlot.itemExp;
                }
            }
            //EquipmentScript e = Instantiate(allEquips[info.equip1Name]);
            //info.equip1.Equip(this, 1);
        }
        else
        {
            //info.equipment1 = null;
            info.equip1 = null;
            info.equip1Exp = 0;
        }

        if (allEquips.ContainsKey(info.equip2Name))
        {
            //info.equipment2 = allEquips[info.equip2Name];
            EquipmentScript eq = Instantiate(allEquips[info.equip2Name]);
            info.equip2 = new Equipment(eq);
            //EquipmentScript e = Instantiate(allEquips[info.equip2Name]);
            //info.equip2.Equip(this, 2);

            for (int i = 0; i < yourEquips.Count; i++)
            {
                if (yourEquips[i].slotIndex == info.equip2Slot)
                {
                    //info.equip2.monster = this;
                    info.equip2.SetInventorySlot(yourEquips[i]);
                    info.equip2Exp = info.equip2.inventorySlot.itemExp;
                }
            }
            //info.equip2.Equip(this, 2);
        }
        else
        {
            //info.equipment2 = null;
            info.equip2 = null;
            info.equip2Exp = 0;
        }

        info.abilityName = saveToken.specialAbility;
        info.specialAbility = new MonsterAbility(abilities[info.abilityName], this);
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


        if (info.isStar)
        {
            bodyParts.StarMonster();
        }
        
        
        SetExp();

 
    }

    //call this at the start of each game so the equipment items can be in effect during the games
    public void MonsterEquipment()
    {
       
        var equipment = GameManager.Instance.GetComponent<Items>().allEquipsDict;

        

        if (equipment.ContainsKey(info.equip1Name))
        {
            info.equip1.SetInventorySlot(info.equip1.inventorySlot);
            info.equip1.GetStats();
            info.equip1.Equip(this, 1);



        }
        if (equipment.ContainsKey(info.equip2Name))
        {

            info.equip2.SetInventorySlot(info.equip2.inventorySlot);
            info.equip2.GetStats();
            info.equip2.Equip(this, 2);

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
        
        

        if (tower.attackNumber == 1)
        {
            var attackSprite = Instantiate(GameManager.Instance.baseAttacks.attackDict[info.baseAttack1.attack.name].attackAnimation, tower.attackPoint.transform.position, Quaternion.Euler(transform.eulerAngles));
            attackSprite.gameObject.name = info.baseAttack1.attack.name;
            attackSprite.transform.localScale = attackSprite.transform.localScale * 2;
            attackSprite.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "PopMenu";
            //attackSprite.GetComponent<AttackEffects>().FromAttacker(attack, attack.name, attack.type, monster.attack, (int)attack.Power.Value, monster.info.level, attack.CritChance.Value, attack.CritMod.Value, gameObject.GetComponent<Monster>());
            attackSprite.GetComponent<AttackEffects>().FromAttacker(info.baseAttack1.attack, info.baseAttack1.attack.name, info.baseAttack1.type, tempStats.Attack.Value, (int)info.baseAttack1.Power.Value, info.level, info.baseAttack1.CritChance.Value, info.baseAttack1.CritMod.Value, gameObject.GetComponent<Monster>());
            attackSprite.GetComponent<AttackEffects>().AttackMotion(Vector2.right * 25);
        }
        else
        {
            var attackSprite = Instantiate(GameManager.Instance.baseAttacks.attackDict[info.baseAttack2.attack.name].attackAnimation, tower.attackPoint.transform.position, Quaternion.Euler(transform.eulerAngles));
            attackSprite.gameObject.name = info.baseAttack2.attack.name;
            attackSprite.transform.localScale = attackSprite.transform.localScale * 2;
            attackSprite.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "PopMenu";
            //attackSprite.GetComponent<AttackEffects>().FromAttacker(attack, attack.name, attack.type, monster.attack, (int)attack.Power.Value, monster.info.level, attack.CritChance.Value, attack.CritMod.Value, gameObject.GetComponent<Monster>());
            attackSprite.GetComponent<AttackEffects>().FromAttacker(info.baseAttack2.attack, info.baseAttack2.attack.name, info.baseAttack2.type, tempStats.Attack.Value, (int)info.baseAttack2.Power.Value, info.level, info.baseAttack2.CritChance.Value, info.baseAttack2.CritMod.Value, gameObject.GetComponent<Monster>());
            attackSprite.GetComponent<AttackEffects>().AttackMotion(Vector2.right * 25);
        }



        


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
