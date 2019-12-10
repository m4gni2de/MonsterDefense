using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

//each spwned enemy will have unique stats that stem from their base stats as a species, in addition to whatever modifiers are affecting them on the map or from tower effects
//this script acts as the control panel for a monster spawned as an Enemy. 
[System.Serializable]
public struct EnemyStats
{
    public string species;
    public string name;
    public string type1;
    public string type2;
    public int id;
    public int hpPot;
    public int hpBase;
    public float hpMax;
    public float currentHp;
    public int defPot;
    public int defBase;
    public int speBase;
    public int spePot;
    //public float def;
    //public float speed;
    public int level;
    public int expGiven;
    public float evasion;

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

    public MonsterClass Class;
};


public class Enemy : MonoBehaviour
{
    //public MapTile[] path;

    //a list of the tiles in the pathway
    public List<MapTile> pathList = new List<MapTile>();
    public float speed;
    public float zeroSpeed = 0;

    public bool isCircular;
    // Always true at the beginning because the moving object will always move towards the first waypoint
    public bool inReverse = true;

    public LayerMask mapTileLayer;

    public MapTile currentPath;
    //the current tile that the enemy is on
    public int currentTile;
    public int currentIndex = 0;
    private bool isWaiting = false;
    private float speedStorage = 0;
    private bool isPaused = false;

    private int count;

    //list of the paths that the enemy has already traveled over
    private List<int> roadsHit = new List<int>();
    public Dictionary<MapTile, float> roadTiles = new Dictionary<MapTile, float>();


    public Monster monster;

    //public EnemyStats stats = new EnemyStats();

    //public MonsterInfo info;
    
    //the enemy's canvas for HP and effects
    public GameObject enemyCanvas;
    //the enemy's HP slider
    public Slider enemyHpSlider;
    //the box that appears when an enemy takes damage
    public GameObject damageText;

    public GameObject map;

    //used to determine if this enemy is the active enemy on the enemy menu
    public bool isActiveEnemy;

    //directions that the enemy is travelling so that the sprite knows to flip or not
    public bool isLeft, isRight;

    public MapDetails mapDetails;
    private void Awake()
    {
        monster = GetComponent<Monster>();
        //info = monster.info;
        //enemyCanvas = monster.enemyCanvas;
        enemyHpSlider = enemyCanvas.GetComponentInChildren<Slider>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //clears the list of roads to prevent index overflows
        roadsHit.Clear();
        
        map = GameObject.FindGameObjectWithTag("Map");
        mapDetails = map.GetComponent<WorldMap>().mapDetails;
        enemyCanvas.GetComponent<Canvas>().sortingLayerName = "Monster";

        //monster = GetComponent<Monster>();
        //for (int i = 0; i < map.GetComponent<Map>().path.Length; i++)
        //{
        //    path[i] = map.GetComponent<Map>().path[i];
        //}

        //if (path.Length > 0)
        //{
        //    currentPath = path[0];
        //}
    }

    //this is called from the Monster script. information of this enemy is taken from the Monster Script, but new data is added since it's an "Enemy", so it will very likely be its own unique monster
    public void SetEnemyStats(int level)
    {
        var monsters = GameManager.Instance.monstersData.monstersAllDict;
        var attacks = GameManager.Instance.baseAttacks.attackDict;
        var abilities = GameManager.Instance.GetComponent<MonsterAbilities>().allAbilitiesDict;
        var skills = GameManager.Instance.GetComponent<AllSkills>().allSkillsDict;
        var allEquips = GameManager.Instance.items.allEquipsDict;



        monster.info.name = monster.info.species;
        monster.info.maxLevel = monsters[monster.info.species].maxLevel;
        monster.info.levelConst = monsters[monster.info.species].levelConst;
        monster.info.energyCost = monsters[monster.info.species].energyCost;
        monster.info.energyGenBase = monsters[monster.info.species].energyGenBase;
        monster.info.type1 = monsters[monster.info.species].type1;
        monster.info.type2 = monsters[monster.info.species].type2;
        monster.info.dexId = monsters[monster.info.species].id;
        monster.info.hpBase = monsters[monster.info.species].hpBase;
        monster.info.atkBase = monsters[monster.info.species].atkBase;
        monster.info.defBase = monsters[monster.info.species].defBase;
        monster.info.speBase = monsters[monster.info.species].speBase;
        monster.info.precBase = monsters[monster.info.species].precBase;
        monster.info.staminaBase = monsters[monster.info.species].staminaBase;
        monster.info.Class = monsters[monster.info.species].Class;
        monster.info.coinGenBase = monsters[monster.info.species].coinGenBase;
        monster.info.level = level;


        int starChance = monsters[monster.info.species].starChance;
        int randStar = Random.Range(0, 500);

        if (randStar <= starChance)
        {
            monster.info.isStar = true;
            monster.bodyParts.StarMonster();
        }

        if (allEquips.ContainsKey(monster.info.equip1Name))
        {
            EquipmentScript eq = Instantiate(allEquips[monster.info.equip1Name]);
            monster.info.equip1 = new Equipment(eq);
        }
        else
        {
            monster.info.equip1 = null;
        }

        if (allEquips.ContainsKey(monster.info.equip2Name))
        {
            EquipmentScript eq = Instantiate(allEquips[monster.info.equip2Name]);
            monster.info.equip2 = new Equipment(eq);
        }
        else
        {
            monster.info.equip2 = null;
        }

        //set the monster's ability'
        if (monsters[monster.info.species].abilities.Length > 1)
        {
            int ab = Random.Range(0, monsters[monster.info.species].abilities.Length - 1);
            monster.info.abilityName = monsters[monster.info.species].abilities[ab];
        }
        else
        {
            monster.info.abilityName = monsters[monster.info.species].abilities[0];
        }

        //set the monster's skill
        if (monsters[monster.info.species].skills.Length > 1)
        {
            int sk = Random.Range(0, monsters[monster.info.species].skills.Length - 1);
            monster.info.skillName = monsters[monster.info.species].skills[sk];
        }
        else
        {
            monster.info.skillName = monsters[monster.info.species].skills[0];
        }

        //set the monster's first 2 base attacks
        int rand = Random.Range(0, monsters[monster.info.species].baseAttacks.Length - 1);
        monster.info.attack1Name = monsters[monster.info.species].baseAttacks[rand];
        int rand2 = Random.Range(0, monsters[monster.info.species].baseAttacks.Length - 1);
        monster.info.attack2Name = monsters[monster.info.species].baseAttacks[rand2];

        monster.info.specialAbility = new MonsterAbility(abilities[monster.info.abilityName], monster);
        monster.info.passiveSkill = new PassiveSkill(monster, skills[monster.info.skillName]);
        StartCoroutine(monster.WeatherCheck());


        int hpRand = Random.Range(0, 26);
        int defRand = Random.Range(0, 26);
        int speRand = Random.Range(0, 26);
        



        StatsCalc StatsCalc = new StatsCalc(monster);
        GetEnemyStats(StatsCalc);

        
        monster.info.currentHP = monster.info.maxHP;
        enemyHpSlider.maxValue = monster.info.maxHP;
        enemyHpSlider.value = monster.info.currentHP;
        speed = 40 * ((float)monster.info.speBase / 100);
    }

   

    public void GetEnemyStats(StatsCalc stats)
    {
        monster.info = stats.Monster.info;
        monster.tempStats = stats.Monster.tempStats;

    }


    public void CalculateStatus(StatusEffects effect)
    {

        monster.info = effect.Monster.info;
        monster.tempStats = effect.Monster.tempStats;

        enemyHpSlider.value = monster.info.currentHP;

        if (monster.info.currentHP <= 0)
        {
            monster.info.currentHP = 1;
        };
    }





    //gets the attacker information from the attack sprite that hits the enemy, and then calculate the damage. method invoked from the Attack Effects script
    public void OutputDamage(string atkName, string atkType, int atkPower, float atkStat, int attackerLevel, float critChance, float criMod, Monster attacker, MonsterAttack monsterAttack)
    {
        if (monster.info.type2 == "none" || monster.info.type2 == null || monster.info.type2 == "")
        {
            if (GameManager.Instance.monstersData.typeChartDict.ContainsKey(atkType) && GameManager.Instance.monstersData.typeChartDict.ContainsKey(monster.info.type1))
            {
                float force = (((attackerLevel * 2) / 2) + 2) * atkPower * (atkStat / monster.info.Defense.Value);
                //float force = (((attackerLevel * 2) / 5) + 2) * atkPower * (atkStat / stats.def);
                //float resistance = 38 * (stats.def / atkStat);
                float resistance = 38 * (monster.info.Defense.Value / atkStat);

                TypeInfo attacking = GameManager.Instance.monstersData.typeChartDict[atkType];
                TypeInfo defending = GameManager.Instance.monstersData.typeChartDict[monster.info.type1];
                TypeChart attack = new TypeChart(attacking, defending, force, resistance);

                DealDamage(attack, attack.typeModifier, attacker, critChance, criMod, monsterAttack);

            }
        }
        else
        {
            if (GameManager.Instance.monstersData.typeChartDict.ContainsKey(atkType) && GameManager.Instance.monstersData.typeChartDict.ContainsKey(monster.info.type1) && GameManager.Instance.monstersData.typeChartDict.ContainsKey(monster.info.type2))
            {
                float force = (((attackerLevel * 2) / 2) + 2) * atkPower * (atkStat / monster.info.Defense.Value);
                //float force = (((attackerLevel * 2) / 5) + 2) * atkPower * (atkStat / stats.def);
                //float resistance = 38 * (stats.def / atkStat);
                float resistance = 38 * (monster.info.Defense.Value / atkStat);
                float damageMod = new float();


                for (int i = 0; i < 2; i++)
                {
                    if (i == 0)
                    {
                        TypeInfo attacking = GameManager.Instance.monstersData.typeChartDict[atkType];
                        TypeInfo defending = GameManager.Instance.monstersData.typeChartDict[monster.info.type1];
                        TypeChart attack = new TypeChart(attacking, defending, force, resistance);
                        damageMod = attack.typeModifier;
                    }
                    else
                    {
                        TypeInfo attacking = GameManager.Instance.monstersData.typeChartDict[atkType];
                        TypeInfo defending = GameManager.Instance.monstersData.typeChartDict[monster.info.type2];
                        TypeChart attack = new TypeChart(attacking, defending, force, resistance);
                        damageMod *= attack.typeModifier;
                        DealDamage(attack, damageMod, attacker, critChance, criMod, monsterAttack);

                    }

                }


               


            }
        }


    }

    //when the attack animation hits the enemy, deal the damage. this method is invoked from the AttackEffects script. Also gives information about the attacking monster, so if an enemy is destroyed, it can tell which monster destroyed it
    public void DealDamage(TypeChart atk, float damageMod, Monster attacker, float critChance, float critMod, MonsterAttack attack)
    {
        var statuses = GameManager.Instance.GetComponent<AllStatusEffects>().allStatusDict;
        
        float damageTaken = Mathf.Round(atk.totalDamage * damageMod);

        if (damageTaken <= 0)
        {
            damageTaken = 1;
        }

        float critRand = Random.Range(0f, 100f);
        float rand = Random.Range(0f, 100f);
        float statusRand = Random.Range(0f, 100f);

        
        //check to see if the attack misses by comparing the enemies' dodge stat with a number from 1-100. if the enemy dodges, deal 0 damage and spawn the word DODGE instead of a damage value
        if (rand >= monster.info.evasionBase)
        {
            monster.monsterMotion.SetBool("isHit", true);
            monster.monsterMotion.GetComponent<MotionControl>().IsHit(attack);

            

            //check and see if the attack is a critical hit, and if so, change the damage output and color of the font to indicate a crit
            if (critRand <= critChance)
            {
                damageTaken = damageTaken * (1 + critMod);
                //spawn the box to display damage done and change the properties
                var damage = Instantiate(damageText, transform.position, Quaternion.identity);
                damage.transform.SetParent(enemyCanvas.transform, false);
                damage.transform.position = new Vector3(transform.position.x, transform.position.y, 1f);
                damage.GetComponentInChildren<TMP_Text>().text = "-" + damageTaken + "!";
                damage.GetComponentInChildren<TMP_Text>().color = Color.yellow;
                Destroy(damage, damage.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            }
            else
            {
                //spawn the box to display damage done and change the properties to signify a critical hit
                var damage = Instantiate(damageText, transform.position, Quaternion.identity);
                damage.transform.SetParent(enemyCanvas.transform, false);
                damage.transform.position = new Vector3(transform.position.x, transform.position.y, 1f);
                damage.GetComponentInChildren<TMP_Text>().text = "-" + damageTaken.ToString();
                Destroy(damage, damage.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            }

            
            //if the attack hits and it has a chance to inflict a secondary status, that is calculated here
            if (attack.effectName != "none")
            {
                if (statusRand <= attack.effectChance * 100)
                {
                    //checks if the monster is already inflicted with this status. if they are not, then the monster is now inflicted. 
                    if (statuses.ContainsKey(attack.effectName)){

                        if (monster.statuses.Contains(statuses[attack.effectName]))
                        {
                            //
                        }
                        else
                        {

                            monster.AddStatus(statuses[attack.effectName]);
                        }
                    }
                }
            }
            
        }
        else
        {
            monster.monsterMotion.SetBool("isDodge", true);
            
            //spawn the box to display damage done and change the properties to "DODGE" if the enemy succesfully evades
            var damage = Instantiate(damageText, transform.position, Quaternion.identity);
            damage.transform.SetParent(enemyCanvas.transform, false);
            damage.transform.position = new Vector3(transform.position.x, transform.position.y, 1f);
            damage.GetComponentInChildren<TMP_Text>().text = "Dodge!";
            Destroy(damage, damage.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            damageTaken = 0;
        }

        

        //call this to actually deal the damage afer its been calculated
        TakeDamage(damageTaken, attacker);
        

       
    }

    //use this to actually deal the damage after it's been calculated
    public void TakeDamage(float damageTaken, Monster attacker)
    {
        monster.info.currentHP -= damageTaken;
        enemyHpSlider.value = monster.info.currentHP;

        //if the enemy's HP falls below 0, it is destroyed and the monster that destroyed it gains EXP, and all other towers on the field gain 10% of that EXP.
        if (monster.info.currentHP <= 0)
        {
            if (isActiveEnemy)
            {
                map.GetComponentInChildren<EnemyInfoPanel>().enemyInfoMenu.SetActive(false);
            }
            float expGained = (monster.info.level + 1 * monster.info.levelConst) / (attacker.info.level + (1 / monster.info.levelConst) - monster.info.level);
            float expShared = expGained / 10;

            if (expGained < 1)
            {
                expGained = 1;
            }

            if (expShared < 1)
            {
                expShared = 1;
            }

            attacker.GainEXP((int)Mathf.Round(expGained));
            attacker.currentMapKOs += 1;

            //give the other active towers EXP as well
            foreach (KeyValuePair<int, Monster> towers in GameManager.Instance.activeTowers)
            {
                if (attacker.activeIndex != towers.Value.activeIndex)
                {
                    Monster m = towers.Value;
                    m.GainEXP((int)Mathf.Round(expShared));
                }
            }

            MonsterItemDrop itemDrop = new MonsterItemDrop(this, attacker);
            GetComponentInChildren<MotionControl>().StartMonsterDie(this);

            GameManager.Instance.TriggerEvent(TriggerType.EnemyKO);
            //Destroy(gameObject);
        }
    }

    //called from the EnemyInfoPanel script
    public void ToggleActiveStatus()
    {
        isActiveEnemy = !isActiveEnemy;
    }



    public void StartMotion()
    {
        //GameObject[] roads = GameObject.FindGameObjectsWithTag("MapTile");

        //for (int i = 0; i < roads.Length; i++)
        //{
        //    if (roads[i].GetComponent<MapTile>().isStartRoad == true)
        //    {
        //        path[0] = roads[i].GetComponent<MapTile>();
        //        pathList.Add(roads[i].GetComponent<MapTile>());
        //        isWaiting = false;
        //        currentIndex = 0;
        //        currentPath = path[currentIndex];
        //        //roadsHit.Add(path[currentIndex].GetComponent<MapTile>().tileNumber);
        //        return;
        //    }
        //}
        //isWaiting = false;
        //currentPath = path[0];

        //GameObject map = GameObject.FindGameObjectWithTag("Map");

        //for (int i = 0; i < map.GetComponent<MapTemplate>().path.Length; i++)
        //{
        //    path[i] = map.GetComponent<MapTemplate>().path[i];
        //}



        //if (path.Length > 0)
        //{
        //    currentPath = path[0];
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //CheckForStart();
        //CheckForPathTile();

        //StartMotion();

        //if (currentPath == null)
        //{
        //    gameObject.transform.localScale = new Vector2(.001f, .001f);
        //}
        //else
        //{
        //    gameObject.transform.localScale = new Vector2(1.8f, 1.8f);
        //}


        //if the path has a value, move the monster towards it
        if (currentPath != null && !isWaiting)
        {
            MoveTowardsPath();
        }


        //if the path does NOT have a value, and the monster has finished it's run, have it move towards the End Path Object
        if (currentPath == null && currentIndex >= pathList.Count - 1)
        {
            MoveTowardsPath();
        }

        //if (isPaused)
        //{
        //    speed = 0;
        //}
        //else
        //{
        //    speed = 40 * ((float)monster.info.speBase / 100);
        //}
        //if (currentIndex == 0)
        //{
        //    gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, -1);
        //}
        //else
        //{
        //    gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, gameObject.transform.localScale.y, 1);
        //}
       



    }


    private void OnDestroy()
    {
        if (map)
        {
            mapDetails.LiveEnemyList();
        }
    }





















    //************ THESE METHODS ARE USED FOR THE MOTION OF THE ENEMY***********//

    //method used to check for the starting pathway on a map
    private void CheckForStart()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Enemy")
                {
                    StartMotion();
                }
            }
        }
    }


    //void CheckForPathTile()
    //{
    //    var aimAngle = Mathf.Atan2(transform.position.x, transform.position.y);
    //    if (aimAngle < 0f)
    //    {
    //        aimAngle = Mathf.PI * 2 + (Time.time * 10);
    //    }
    //    var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.up;

    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, aimDirection * 25);
    //    Debug.DrawRay(transform.position, aimDirection * 25, Color.green);


    //    if (Physics2D.Raycast(transform.position, aimDirection * 25, ~1 << 9))
    //    {
    //        Debug.Log("okay");
    //    }
    //    if (hit.collider != null)
    //    {
    //        if (hit.collider.gameObject.name == "mapTile(Clone)")
    //        {
    //            var tile = hit.collider.gameObject;

    //            if (tile.GetComponent<MapTile>().isRoad == true)
    //            {
    //                //checks to see if the path hit by the ray was already traveled by the enemy
    //                if (roadsHit.Contains(tile.GetComponent<MapTile>().tileNumber))
    //                {
    //                    //
    //                }
    //                else
    //                {
    //                    //adds the next visable road to the enemy's path
    //                    pathList.Add(tile.GetComponent<MapTile>());
    //                    path[roadsHit.Count] = tile.GetComponent<MapTile>();
    //                    currentPath = path[roadsHit.Count];
    //                }
    //                //Debug.Log(tile.GetComponent<MapTile>().tileNumber);

    //            }
    //            else
    //            {
    //                //Debug.Log(tile.GetComponent<MapTile>().);
    //            }

    //            //Debug.Log(tile.GetComponent<MapTile>().tileNumber);
    //        }
    //        Debug.Log(hit.collider.gameObject.name);
    //    }

    //    //if (currentPath != null && !isWaiting)
    //    //{
    //    //    MoveTowardsPath();
    //    //}
    //}

    public void Pause()
    {
        //isWaiting = !isWaiting;

        isPaused = !isPaused;
        //Debug.Log(isPaused);
    }

    private void MoveTowardsPath()
    {

        
        //moves the monster according to the position of his legs as opposed to the center of his object
        Vector3 averageLegPos = new Vector3();

        for (int i = 0; i < monster.specs.legs.Length; i++)
        {
            monster.specs.legPos[i] = monster.specs.legs[i].transform.position;
            averageLegPos.x += monster.specs.legPos[i].x;
            //averageLegPos.y += monster.specs.legPos[i].y - (monster.specs.legs[i].GetComponent<RectTransform>().rect.height * 2);
            averageLegPos.y += monster.specs.legPos[i].y - (monster.GetComponent<RectTransform>().rect.height);

        }

        averageLegPos = new Vector3(averageLegPos.x / monster.specs.legPos.Length, averageLegPos.y / monster.specs.legPos.Length, averageLegPos.z);
        


        //**** Get the moving objects current position LEGACY****
        //Vector3 currentPosition = this.transform.position;

        // Get the moving objects current position of its legs
        Vector3 currentPosition = averageLegPos;
        //Vector3 currentPosition = new Vector3(monster.transform.position.x, monster.transform.position.y + gameObject.GetComponent<RectTransform>().rect.height, 1f);

        //gets the next target for the monster.
        Vector3 targetPosition = new Vector3();

        //if the monster is on the final path, make it's last destination the position of the "end path" object
        if (currentPath == null)
        {
            targetPosition = mapDetails.pathEnd.transform.position;
        }
        else
        {
            // Get the target waypoints position
            targetPosition = currentPath.transform.position;
        }


        //This code moves the 'mover'
        Vector3 moveDirection = currentPosition - targetPosition;
        if (moveDirection != Vector3.zero)
        {

            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
        }

        
            if (targetPosition.x <= currentPosition.x)
            {
                isRight = true;
                isLeft = false;
                monster.puppet.flip = true;
                

            }
            else
            {
                isRight = false;
                isLeft = true;
                monster.puppet.flip = false;
               
        }




        if (!isPaused)
        {

            //This code rotates the 'mover' based on how close it is to the waypoint
            // If the moving object isn't that close to the waypoint
            if (Vector3.Distance(currentPosition, targetPosition) > 2f)
            {

                // Get the direction and normalize
                Vector3 directionOfTravel = targetPosition - currentPosition;
                directionOfTravel.Normalize();

                //scale the movement on each axis by the directionOfTravel vector components
                this.transform.Translate(
                    directionOfTravel.x * speed * Time.deltaTime,
                    directionOfTravel.y * speed * Time.deltaTime,
                    directionOfTravel.z * speed * Time.deltaTime,
                    Space.World

                );
            }
            else
            {
                if (currentPath)
                {
                    // If the waypoint has a pause amount then wait a bit
                    if (currentPath.waitSeconds > 0)
                    {
                        Pause();
                        Invoke("Pause", currentPath.waitSeconds);
                    }

                    // If the current waypoint has a speed change then change to it
                    if (currentPath.speedOut > 0)
                    {
                        speedStorage = speed;
                        speed = currentPath.speedOut;
                    }
                    else if (speedStorage != 0)
                    {
                        speed = speedStorage;
                        speedStorage = 0;
                    }
                }

                NextPath();
            }
        }
    }

   



    private void NextPath()
    {
        

        currentIndex += 1;

        //if the index becomes larger than the number of tiles in the path, set the path to null
        if (currentIndex < pathList.Count)
        {
            currentPath = pathList[currentIndex];
        }
        else
        {
            currentPath = null;
        }
        //currentPath = path[currentIndex];
        
       
    }


}
