using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public float def;
    public int level;
    public int expGiven;

    public Stat HP;
    public Stat HPPotential;
    public Stat Defense;
    public Stat DefensePotential;
};


public class Enemy : MonoBehaviour
{
    public MapTile[] path;
    public List<MapTile> pathList = new List<MapTile>();
    public float speed;

    public bool isCircular;
    // Always true at the beginning because the moving object will always move towards the first waypoint
    public bool inReverse = true;

    public LayerMask mapTileLayer;

    private MapTile currentPath;
    //the current tile that the enemy is on
    public int currentTile;
    private int currentIndex = 0;
    private bool isWaiting = false;
    private float speedStorage = 0;

    private int count;

    //list of the paths that the enemy has already traveled over
    private List<int> roadsHit = new List<int>();
    public Dictionary<MapTile, float> roadTiles = new Dictionary<MapTile, float>();


    private Monster monster;
    public EnemyStats stats = new EnemyStats();
    //the enemy's canvas for HP and effects
    public GameObject enemyCanvas;
    //the enemy's HP slider
    private Slider enemyHpSlider;
    //the box that appears when an enemy takes damage
    public GameObject damageText;

    

    private void Awake()
    {
        monster = GetComponent<Monster>();
        //enemyCanvas = monster.enemyCanvas;
        enemyHpSlider = enemyCanvas.GetComponentInChildren<Slider>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //clears the list of roads to prevent index overflows
        roadsHit.Clear();

        GameObject map = GameObject.FindGameObjectWithTag("Map");

        enemyCanvas.GetComponent<Canvas>().sortingLayerName = "Monster";
        for (int i = 0; i < map.GetComponent<Map>().path.Length; i++)
        {
            path[i] = map.GetComponent<Map>().path[i];
        }

        if (path.Length > 0)
        {
            currentPath = path[0];
        }
    }

    //this is called from the Monster script. information of this enemy is taken from the Monster Script, but new data is added since it's an "Enemy", so it will very likely be its own unique monster
    public void SetEnemyStats()
    {
        int hpRand = Random.Range(24, 26);
        int defRand = Random.Range(24, 26);
        //int levelRand = Random.Range(1, 101);
        int levelRand = Random.Range(1, 3);

        stats.id = monster.info.dexId;
        monster.info.dexId = stats.id;
        stats.species = monster.info.species;
        stats.name = monster.info.name;
        stats.type1 = monster.info.type1;
        stats.type2 = monster.info.type2;
        stats.hpPot = hpRand;
        stats.hpBase = monster.info.hpBase;
        stats.defPot = defRand;
        stats.defBase = monster.info.defBase;
        stats.level = levelRand;
        monster.info.level = stats.level;

        //Debug.Log(stats.level);


        
        StatsCalc StatsCalc = new StatsCalc(gameObject.GetComponent<Monster>());
        GetEnemyStats(StatsCalc);

       
    }

   

    public void GetEnemyStats(StatsCalc StatsCalc)
    {
        stats.def = (int)StatsCalc.Monster.info.Defense.Value;
        stats.Defense.BaseValue = stats.def;
        stats.hpMax = (int)StatsCalc.Monster.info.HP.Value;
        stats.HP.BaseValue = stats.hpMax;
        stats.currentHp = stats.hpMax;
        enemyHpSlider.maxValue = stats.hpMax;
        enemyHpSlider.value = stats.hpMax;
    }





    //gets the attacker information from the attack sprite that hits the enemy, and then calculate the damage. method invoked from the Attack Effects script
    public void OutputDamage(string atkName, string atkType, int atkPower, float atkStat, int attackerLevel, float critChance, float criMod, Monster attacker)
    {
        if (stats.type2 == "none" || stats.type2 == null || stats.type2 == "")
        {
            if (GameManager.Instance.monstersData.typeChartDict.ContainsKey(atkType) && GameManager.Instance.monstersData.typeChartDict.ContainsKey(stats.type1))
            {
                float force = (((attackerLevel * 2) / 5) + 2) * atkPower * (atkStat / stats.def);
                float resistance = 38 * (stats.def / atkStat);

                TypeInfo attacking = GameManager.Instance.monstersData.typeChartDict[atkType];
                TypeInfo defending = GameManager.Instance.monstersData.typeChartDict[stats.type1];
                TypeChart attack = new TypeChart(attacking, defending, force, resistance);

                OutputDamage(attack, attack.typeModifier, attacker);

            }
        }
        else
        {
            if (GameManager.Instance.monstersData.typeChartDict.ContainsKey(atkType) && GameManager.Instance.monstersData.typeChartDict.ContainsKey(stats.type1) && GameManager.Instance.monstersData.typeChartDict.ContainsKey(stats.type2))
            {
                float force = (((attackerLevel * 2) / 5) + 2) * atkPower * (atkStat / stats.def);
                float resistance = 38 * (stats.def / atkStat);
                float damageMod = new float();


                for (int i = 0; i < 2; i++)
                {
                    if (i == 0)
                    {
                        TypeInfo attacking = GameManager.Instance.monstersData.typeChartDict[atkType];
                        TypeInfo defending = GameManager.Instance.monstersData.typeChartDict[stats.type1];
                        TypeChart attack = new TypeChart(attacking, defending, force, resistance);
                        damageMod = attack.typeModifier;
                    }
                    else
                    {
                        TypeInfo attacking = GameManager.Instance.monstersData.typeChartDict[atkType];
                        TypeInfo defending = GameManager.Instance.monstersData.typeChartDict[stats.type2];
                        TypeChart attack = new TypeChart(attacking, defending, force, resistance);
                        damageMod *= attack.typeModifier;

                        OutputDamage(attack, damageMod, attacker);
                    }
                }




            }
        }


    }

    //when the attack animation hits the enemy, deal the damage. this method is invoked from the AttackEffects script. Also gives information about the attacking monster, so if an enemy is destroyed, it can tell which monster destroyed it
    public void OutputDamage(TypeChart atk, float damageMod, Monster attacker)
    {

        //Debug.Log(damageMod);
        float damageTaken = Mathf.Round(atk.totalDamage * damageMod);
        //spawn the box to display damage done and change the properties
        var damage = Instantiate(damageText, transform.position, Quaternion.identity);
        damage.transform.SetParent(enemyCanvas.transform, false);
        damage.GetComponentInChildren<TMP_Text>().text = "-" + damageTaken.ToString();
        Destroy(damage, damage.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);


        stats.currentHp -= damageTaken;
        enemyHpSlider.value = stats.currentHp;

        if (stats.currentHp <= 0)
        {
            float expGained = (stats.level + 1 * monster.info.levelConst) / (attacker.info.level + 1 - stats.level);

            if (expGained < 1)
            {
                expGained = 1;
            }
            attacker.GainEXP((int)expGained);
            Debug.Log(expGained);
            Destroy(gameObject);
        }
        
        Debug.Log("Current HP: " + stats.currentHp + "  & Damage Taken: " + damageTaken);
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

        if (currentPath != null && !isWaiting)
        {
            MoveTowardsPath();
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


    void CheckForPathTile()
    {
        var aimAngle = Mathf.Atan2(transform.position.x, transform.position.y);
        if (aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + (Time.time * 10);
        }
        var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.up;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, aimDirection * 25);
        Debug.DrawRay(transform.position, aimDirection * 25, Color.green);


        if (Physics2D.Raycast(transform.position, aimDirection * 25, ~1 << 9))
        {
            Debug.Log("okay");
        }
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.name == "mapTile(Clone)")
            {
                var tile = hit.collider.gameObject;

                if (tile.GetComponent<MapTile>().isRoad == true)
                {
                    //checks to see if the path hit by the ray was already traveled by the enemy
                    if (roadsHit.Contains(tile.GetComponent<MapTile>().tileNumber))
                    {
                        //
                    }
                    else
                    {
                        //adds the next visable road to the enemy's path
                        pathList.Add(tile.GetComponent<MapTile>());
                        path[roadsHit.Count] = tile.GetComponent<MapTile>();
                        currentPath = path[roadsHit.Count];
                    }
                    //Debug.Log(tile.GetComponent<MapTile>().tileNumber);

                }
                else
                {
                    //Debug.Log(tile.GetComponent<MapTile>().);
                }

                //Debug.Log(tile.GetComponent<MapTile>().tileNumber);
            }
            Debug.Log(hit.collider.gameObject.name);
        }

        //if (currentPath != null && !isWaiting)
        //{
        //    MoveTowardsPath();
        //}
    }

    void Pause()
    {
        isWaiting = !isWaiting;
    }

    private void MoveTowardsPath()
    {
        //moves the monster according to the position of his legs as opposed to the center of his object
        Vector3 averageLegPos = new Vector3();

        for (int i = 0; i < monster.specs.legs.Length; i++)
        {
            monster.specs.legPos[i] = monster.specs.legs[i].transform.position;
            averageLegPos.x += monster.specs.legPos[i].x;
            averageLegPos.y += monster.specs.legPos[i].y - (monster.specs.legs[i].GetComponent<RectTransform>().rect.height * 2);

        }
        averageLegPos = new Vector3(averageLegPos.x / monster.specs.legPos.Length, averageLegPos.y / monster.specs.legPos.Length, averageLegPos.z);


        //**** Get the moving objects current position LEGACY****
        //Vector3 currentPosition = this.transform.position;

        // Get the moving objects current position of its legs
        Vector3 currentPosition = averageLegPos;

        // Get the target waypoints position
        Vector3 targetPosition = currentPath.transform.position;


        //This code moves the 'mover'
        Vector3 moveDirection = currentPosition - targetPosition;
        if (moveDirection != Vector3.zero)
        {

            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }


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

            NextPath();
        }
    }



    private void NextPath()
    {
        //if (isCircular)
        //{

        //    if (!inReverse)
        //    {
        //        currentIndex = (currentIndex + 1 >= path.Length) ? 0 : currentIndex + 1;
        //    }
        //    else
        //    {
        //        currentIndex = (currentIndex == 0) ? path.Length - 1 : currentIndex - 1;
        //    }

        //}
        //else
        //{

        //    // If at the start or the end then reverse
        //    if ((!inReverse && currentIndex + 1 >= path.Length) || (inReverse && currentIndex == 0))
        //    {
        //        inReverse = !inReverse;
        //    }
        //    currentIndex = (!inReverse) ? currentIndex + 1 : currentIndex - 1;

        //}

        currentIndex += 1;
        currentPath = path[currentIndex];
        //currentPath = path[roadsHit.Count];

        //currentPath = path[currentIndex];
    }
}
