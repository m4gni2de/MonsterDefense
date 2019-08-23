
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Puppet2D;
using TMPro;



[System.Serializable]
public struct MonsterInfo
{
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
    public int staminaBase;
    public int staminaMax;
    public int level;
    public int totalExp;
    public int maxLevel;
    public float levelConst;
    public float energyGenBase;
    public float energyCost;
    


    public string attack1Name;
    //public BaseAttack attack1;
    public string attack2Name;
    //public BaseAttack attack2;


    //public GameObject attack1Animation;
    //public string chargeAttack1;
    //public string chargeAttack2;
    //public string specialAttack;

    public string equip1Name;
    public Equipment equip1;
    public string equip2Name;
    public Equipment equip2;
    public EquippableItem equippable;
    public EquippableItem equippable1;
    public EquippableItem equippable2;

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

    //new stats that need to be worked on
    
    public Stat Stamina;
    public Stat EnergyGeneration;
    public Stat EnergyCost;

    public int koCount;

    public bool isEquipped;
   

};

//these stats can change during a map without effecting it's permanent stats
[System.Serializable]
public struct TempStats
{
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
    public float evasionBase;
    public float critBase;

    
    public BaseAttack attack1;
    public BaseAttack attack2;
}

[System.Serializable]
public struct MonsterSpecs
{
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
    //public AllBaseAttacks allBaseAttacks = new AllBaseAttacks();
    //private BaseAttacks baseAttacks;

    private Tower tower;
    private Enemy enemy;

    //the number this monster is in the activeTowers Dictionary on the GameManager
    public int activeIndex;

    //checks if the monster being spawned is an enemy or not
    public bool isEnemy, isTower;

    //public GameObject expCanvas;
    //public Slider expSlider;

    //all of the values in regards to leveling up this monster
    public Dictionary<int, int> expToLevel = new Dictionary<int, int>();
    public Dictionary<int, int> totalExpForLevel = new Dictionary<int, int>();

    //use these as temporary variables to hold the monster's stats while it's on the field. these stats can be manipulated while on the field, but do not affect the monster's stats permanently.
    //public float attack, defense, speed, precision, hp, evasion, stamina, energyCost, energyGeneration;


    public Animator monsterMotion;

    //the puppet controller script that acts as the trigger for a monster's motions in the animator
    public Puppet2D_GlobalControl puppet;

    public GameObject monsterIcon, frontModel;

    //list to keep track of the tiles that are boosting this monster while it's on the map. this list is added to from the maptile script itself
    public List<MapTile> boostTiles = new List<MapTile>();

    //list to keep track of the monsters statuses afflictions and their timers
    //public Dictionary<Status, StatusTimer> statuses = new Dictionary<Status, StatusTimer>();
    public List<Status> statuses = new List<Status>();
    //use these icons to display a monster's current statuses
    public GameObject[] statusIcons;

    //script used to access the meshes that make up the monster
    public MeshBodyParts bodyParts;


    private void Awake()
    {


    }
    // Start is called before the first frame update
    void Start()
    {
        var attacksDict = GameManager.Instance.baseAttacks.baseAttackDict;


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


        //expSlider.GetComponent<Slider>();

        //make a new equipable for each buff/nerf that a monster will have. each ability, equippment, etc, will have their own equippable
        //info.equippable1 = new EquippableItem();
        //info.equippable2 = new EquippableItem();

        //checks for which items a monster has equipped and apply the appropriate effects
        EquipmentBoosts();

        //gives the monster temporary stats that can be changed in game, without affecting the monster's saved stats. these values are changed during a game, not anything in monster.info
        //attack = info.Attack.Value;
        //defense = info.Defense.Value;
        //speed = info.Speed.Value;
        //precision = info.Precision.Value;
        //hp = info.HP.Value;
        //evasion = info.evasionBase;
        //stamina = info.Stamina.Value;
        //energyCost = info.EnergyCost.Value;
        //energyGeneration = info.EnergyGeneration.Value;

        tempStats.HP.BaseValue = info.HP.Value;
        tempStats.Defense.BaseValue = info.Defense.Value;
        tempStats.Attack.BaseValue = info.Attack.Value;
        tempStats.Speed.BaseValue = info.Speed.Value;
        tempStats.Precision.BaseValue = info.Precision.Value;
        tempStats.Stamina.BaseValue = info.Stamina.Value;
        tempStats.EnergyCost.BaseValue = info.EnergyCost.Value;
        tempStats.EnergyGeneration.BaseValue = info.EnergyGeneration.Value;
        tempStats.evasionBase = info.evasionBase;
        tempStats.critBase = info.critBase;

        //AllStatuses status = new AllStatuses();
        //Status status2 = status.Burn;
        //Status status3 = status.Poison;

        //AddStatus(status2);
        //AddStatus(status3);

        //load in the attacks of the monster
        if (attacksDict.ContainsKey(info.attack1Name))
        {

            BaseAttack attack = attacksDict[info.attack1Name];

            tempStats.attack1 = attack;
            tempStats.attack1.Power.BaseValue = attack.power;
            tempStats.attack1.Range.BaseValue = attack.range;
            tempStats.attack1.CritChance.BaseValue = attack.critChance;
            tempStats.attack1.CritMod.BaseValue = attack.critMod;
            tempStats.attack1.EffectChance.BaseValue = attack.effectChance;
            tempStats.attack1.AttackTime.BaseValue = attack.attackTime;
            tempStats.attack1.AttackSpeed.BaseValue = attack.attackSpeed;


        }

        if (attacksDict.ContainsKey(info.attack2Name))
        {
            BaseAttack attack = attacksDict[info.attack2Name];

            tempStats.attack2 = attack;
            tempStats.attack2.Power.BaseValue = attack.power;
            tempStats.attack2.Range.BaseValue = attack.range;
            tempStats.attack2.CritChance.BaseValue = attack.critChance;
            tempStats.attack2.CritMod.BaseValue = attack.critMod;
            tempStats.attack2.EffectChance.BaseValue = attack.effectChance;
            tempStats.attack2.AttackTime.BaseValue = attack.attackTime;
            tempStats.attack2.AttackSpeed.BaseValue = attack.attackSpeed;
        }

       
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


    //Get the increased stats from the monster's equipment
    public void EquipmentBoosts()
    {

        //EquippableItem equippableItem;

        //equippableItem = new EquippableItem();
        //equippableItem.Equip(gameObject.GetComponent<Monster>());
        info.equippable1 = new EquippableItem();
        info.equippable2 = new EquippableItem();


        info.equippable1.Equip(gameObject.GetComponent<Monster>(), 1);
        info.equippable2.Equip(gameObject.GetComponent<Monster>(), 2);
        //info.equippable.Equip(gameObject.GetComponent<Monster>());


        //if (info.equip1Name != null || info.equip1Name != "none")
        //{
        //    info.equippable1.Equip(gameObject.GetComponent<Monster>(), info.equip1);
        //}

        //if (info.equip2Name != null || info.equip2Name != "none")
        //{
        //    info.equippable2.Equip(gameObject.GetComponent<Monster>(), info.equip2);
        //}


    }

    //Equip a new item to this monster
    public void EquipItem(Equipment equip, int slot)
    {

        if (slot == 1)
        {
            info.equip1Name = equip.name;
            info.equippable1.Equip(gameObject.GetComponent<Monster>(), 1);


            StatsCalc stats = new StatsCalc(gameObject.GetComponent<Monster>());
            GetStats(stats);
        }

        if (slot == 2)
        {
            info.equip2Name = equip.name;
            info.equippable2.Equip(gameObject.GetComponent<Monster>(), 2);


            StatsCalc stats = new StatsCalc(gameObject.GetComponent<Monster>());
            GetStats(stats);
        }
        //if (info.equip1Name == "none" || info.equip1Name == null)
        //{
        //    info.equip1Name = equip.name;

        //    EquipmentBoosts();

        //    bool isLevelUp = true;
        //    StatsCalc stats = new StatsCalc(gameObject.GetComponent<Monster>(), isLevelUp);
        //    GetStats(stats);
        //    return;
        //}


        //if (info.equip2Name == "none" || info.equip2Name == null)
        //{
        //    info.equip2Name = equip.name;
        //    EquipmentBoosts();

        //    bool isLevelUp = true;
        //    StatsCalc stats = new StatsCalc(gameObject.GetComponent<Monster>(), isLevelUp);
        //    GetStats(stats);

        //}



    }

    //unequip an item from this monster
    public void UnEquipItem(Equipment equip, int slot)
    {



        if (slot == 1)
        {
            info.equippable1.Unequip(gameObject.GetComponent<Monster>());
            info.equip1Name = "none";
            info.equip1 = new Equipment();

        }
        if (slot == 2)
        {
            info.equippable2.Unequip(gameObject.GetComponent<Monster>());
            info.equip2Name = "none";
            info.equip2 = new Equipment();

        }


        //if (slot == 1)
        //{

        //    info.equip1Name = "none";
        //    info.equip1 = new Equipment();

        //}
        //if (slot == 2)
        //{
        //    info.equip2Name = "none";
        //    info.equip2 = new Equipment();

        //}

        int itemCount = PlayerPrefs.GetInt(equip.name);
        PlayerPrefs.SetInt(equip.name, itemCount + 1);

        StatsCalc stats = new StatsCalc(gameObject.GetComponent<Monster>());
        GetStats(stats);


    }


    //public void


    //gets the monster's information from the Game Manager and creates the monster's stats
    public void SetMonsterStats()
    {

        var attacksDict = GameManager.Instance.baseAttacks.baseAttackDict;

        var equip = GameManager.Instance.items.allEquipmentDict;
        var dict = GameManager.Instance.monstersData.monstersAllDict;
        var name = info.species;


        //get the monster's species dependent stats from the Game Manager
        if (dict.ContainsKey(name))
        {
            info.type1 = dict[name].type1;
            info.type2 = dict[name].type2;
            info.dexId = dict[name].id;
            info.hpBase = dict[name].hpBase;
            info.atkBase = dict[name].atkBase;
            info.defBase = dict[name].defBase;
            info.speBase = dict[name].speBase;
            info.precBase = dict[name].precBase;
            info.staminaBase = dict[name].staminaBase;
            info.maxLevel = dict[name].maxLevel;
            info.levelConst = dict[name].levelConst;
            info.energyCost = dict[name].energyCost;
            info.energyGenBase = dict[name].energyGenBase;

            //for each of the attacks this monster has in its base attack array, choose 2 at random to give to this monster
            int rand = Random.Range(0, dict[name].baseAttacks.Length - 1);
            info.attack1Name = dict[name].baseAttacks[rand];
            int rand2 = Random.Range(0, dict[name].baseAttacks.Length - 1);

            //make sure the 2 attacks aren't the same
            if (rand2 == rand)
            {
                if (rand2 == 0)
                {
                    info.attack2Name = dict[name].baseAttacks[rand + 1];
                }
                if (rand2 == dict[name].baseAttacks.Length - 1)
                {
                    info.attack2Name = dict[name].baseAttacks[rand - 1];
                }
            }
            else
            {
                info.attack2Name = dict[name].baseAttacks[rand];
            }


        }




        //sets the monster's nickname to its name if there isn't a nickname
        if (info.name != null || info.name == "")
        {
            info.name = info.species;
        }

        //load in the attacks of the monster
        if (attacksDict.ContainsKey(info.attack1Name))
        {

            BaseAttack attack = attacksDict[info.attack1Name];


            tempStats.attack1.Power.BaseValue = attack.power;
            tempStats.attack1.Range.BaseValue = attack.range;
            tempStats.attack1.CritChance.BaseValue = attack.critChance;
            tempStats.attack1.CritMod.BaseValue = attack.critMod;
            tempStats.attack1.EffectChance.BaseValue = attack.effectChance;
            tempStats.attack1.AttackTime.BaseValue = attack.attackTime;
            tempStats.attack1.AttackSpeed.BaseValue = attack.attackSpeed;



        }

        if (attacksDict.ContainsKey(info.attack2Name))
        {
            BaseAttack attack = attacksDict[info.attack2Name];

            tempStats.attack2.Power.BaseValue = attack.power;
            tempStats.attack2.Range.BaseValue = attack.range;
            tempStats.attack2.CritChance.BaseValue = attack.critChance;
            tempStats.attack2.CritMod.BaseValue = attack.critMod;
            tempStats.attack2.EffectChance.BaseValue = attack.effectChance;
            tempStats.attack2.AttackTime.BaseValue = attack.attackTime;
            tempStats.attack2.AttackSpeed.BaseValue = attack.attackSpeed;
        }




        //if the monster has equipment attached to it, set their value to "none" to avoid Null Exceptions
        if (info.equip1Name == null)
        {
            info.equip1Name = "none";
        }
        if (info.equip2Name == null)
        {
            info.equip2Name = "none";
        }





        //if this monster is an enemy, then fill in the stats for the enemy with randomized values for Potential
        if (isEnemy)
        {

            //enemy.SetEnemyStats();

        }
        //if it is NOT an enemy, then it already has prefab stats, so get those
        else
        {


            StatsCalc stats = new StatsCalc(gameObject.GetComponent<Monster>());
            GetStats(stats);

        }


    }



    //when a new monster is being summoned, it's stats are randomized within the bounds of its species. Creates a new Playerpref for the amount of monsters, along with a playerpref string for the json object for the newly summoned object
    public void SummonNewMonster(string name)
    {
        bool haskey = PlayerPrefs.HasKey("MonsterCount");

        if (haskey)
        {
            int monsterCount = GameManager.Instance.monsterCount + 1;

            var attacksDict = GameManager.Instance.baseAttacks.baseAttackDict;

            var dict = GameManager.Instance.monstersData.monstersAllDict;



            //get the monster's species dependent stats from the Game Manager
            if (dict.ContainsKey(name))
            {
                info.type1 = dict[name].type1;
                info.type2 = dict[name].type2;
                info.dexId = dict[name].id;
                info.hpBase = dict[name].hpBase;
                info.atkBase = dict[name].atkBase;
                info.defBase = dict[name].defBase;
                info.speBase = dict[name].speBase;
                info.precBase = dict[name].precBase;
                info.staminaBase = dict[name].staminaBase;
                info.level = 1;
                info.totalExp = 0;
                info.AttackPotential.BaseValue = Random.Range(0, 26);
                info.DefensePotential.BaseValue = Random.Range(0, 26);
                info.SpeedPotential.BaseValue = Random.Range(0, 26);
                info.PrecisionPotential.BaseValue = Random.Range(0, 26);
                info.HPPotential.BaseValue = Random.Range(0, 26);
                info.index = monsterCount;
                info.maxLevel = dict[name].maxLevel;
                info.levelConst = dict[name].levelConst;




                PlayerPrefs.SetInt("MonsterCount", monsterCount);
                GameManager.Instance.monsterCount = monsterCount;

                //for each of the attacks this monster has in its base attack array, choose 2 at random to give to this monster
                int rand = Random.Range(0, dict[name].baseAttacks.Length - 1);
                info.attack1Name = dict[name].baseAttacks[rand];
                int rand2 = Random.Range(0, dict[name].baseAttacks.Length - 1);

                //make sure the 2 attacks aren't the same
                if (rand2 == rand)
                {
                    if (rand2 == 0)
                    {
                        info.attack2Name = dict[name].baseAttacks[rand + 1];
                    }
                    if (rand2 == dict[name].baseAttacks.Length - 1)
                    {
                        info.attack2Name = dict[name].baseAttacks[rand - 1];
                    }
                }
                else
                {
                    info.attack2Name = dict[name].baseAttacks[rand];
                }


            }



            //sets the monster's nickname to its name if there isn't a nickname
            if (info.name != null || info.name == "")
            {
                info.name = info.species;
            }

            //load in the attacks of the monster
            if (attacksDict.ContainsKey(info.attack1Name))
            {
                BaseAttack attack = attacksDict[info.attack1Name];
                tempStats.attack1 = attack;

            }

            if (attacksDict.ContainsKey(info.attack2Name))
            {
                BaseAttack attack = attacksDict[info.attack2Name];
                tempStats.attack2 = attack;
            }

            //StatsCalc stats = new StatsCalc(gameObject.GetComponent<Monster>());
            //GetStats(stats);

            SetMonsterStats();

        }

        //set the json info for the monster as a playerpref so it can be used against when the game turns off
        //PlayerPrefs.SetString(info.index.ToString(), JsonUtility.ToJson(info));




    }




    //****************The following methods are for calculating the stats of the monster***********//
    public void GetStats(StatsCalc stats)
    {


        PlayerPrefs.SetString(info.index.ToString(), JsonUtility.ToJson(info));

        info = stats.Monster.info;
        tempStats = stats.Monster.tempStats;


        //attack = info.Attack.Value;
        //defense = info.Defense.Value;
        //speed = info.Speed.Value;
        //precision = info.Precision.Value;
        //hp = info.HP.Value;
        //evasion = info.evasionBase;
        //stamina = info.Stamina.Value;
        //energyCost = info.EnergyCost.Value;
        //energyGeneration = info.EnergyGeneration.Value;

        GameManager.Instance.GetComponent<YourMonsters>().GetYourMonsters();
    }


    //statuses are added from monster attacks or from tiles on the map. they are added here and the statuses being calculating
    public void AddStatus(Status status)
    {
        StatusTimer timer = new StatusTimer(gameObject.GetComponent<Monster>(), status);
        StartCoroutine(timer.TriggerEffect());
        statuses.Add(status);
        if (GameManager.Instance.GetComponent<AllStatusEffects>().allStatusDict.ContainsKey(status.name))
        {
            statusIcons[statuses.Count - 1].GetComponent<SpriteRenderer>().sprite = GameManager.Instance.GetComponent<AllStatusEffects>().allStatusDict[status.name].statusSprite;
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
                    effect.HealStatus(this, status, true);
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
                    effect.HealStatus(this, status, false);
                }
            }
        }


        
    }

    public void CalculateStatus(StatusEffects effect)
    {

        tempStats = effect.Monster.tempStats;

        
        //attack = effect.Monster.attack;
        //defense = effect.Monster.defense;
        //evasion = effect.Monster.evasion;
        //hp = effect.Monster.hp;
        //speed = effect.Monster.speed;
        //energyGeneration = effect.Monster.energyGeneration;
        //precision = effect.Monster.precision;

        //if (hp <= 0)
        //{
        //    hp = 1;
        //};
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

            //expSlider.maxValue = (float)toNextLevel;
            //expSlider.value = toNextLevel - nextLevelDiff;
        }
    }

    //this is called from the defeated enemy
    public void GainEXP(int expGained)
    {

        info.totalExp += (int)Mathf.Round(expGained);

        info.koCount += 1;

        //Debug.Log("EXP Gained: " + expGained);
       

        if (expToLevel.ContainsKey(info.level))
        {
            int toNextLevel = expToLevel[info.level + 1];
            int totalNextLevel = totalExpForLevel[info.level + 1];
            int nextLevelDiff = totalNextLevel - info.totalExp;

            
            if (expGained >= nextLevelDiff)
            {
                info.level += 1;

                SetExp();


                //int defBefore = (int)info.defStat;
                //int hpBefore = (int)info.hpMax;
                //int atkBefore = (int)info.atkStat;
                //int speBefore = (int)info.speStat;
                //int precBefore = (int)info.precStat;



                StatsCalc stats = new StatsCalc(gameObject.GetComponent<Monster>());
                GetStats(stats);

                for (int b = 0; b < boostTiles.Count; b++)
                {
                    MapTileStatChange tile = new MapTileStatChange();
                    tile.ApplyTileChanges(gameObject.GetComponent<Monster>(), boostTiles[b]);
                }

                return;
                

                //int defChange = (int)info.defStat - defBefore;
                //int hpChange = (int)info.hpMax - hpBefore;
                //int atkChange = (int)info.atkStat - atkBefore;
                //int speChange = (int)info.speStat - speBefore;
                //int precChange = (int)info.precStat - precBefore;

                //Debug.Log("Previous Defense: " + defBefore + " New Defense: " + info.defStat + " Defense Change: +" + defChange);
            }

            
            PlayerPrefs.SetString(info.index.ToString(), JsonUtility.ToJson(info));
            
        }

        


    }
}
