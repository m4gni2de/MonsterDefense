using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum TileAttribute
{
    None,
    Water,
    Fire,
    Nature,
    Magic,
    Electric,
    Poison,
    Ice,
    Shadow,
    Mechanical,
    Normal,
};

[System.Serializable]
public enum TileType
{
    Build,
    Path,
};

[System.Serializable]
public enum PathDirection
{
    NE,
    NW,
    SE, 
    SW,
};

[System.Serializable]
public enum BoostType
{
    HPBonus, 
    AtkBonus, 
    DefBonus, 
    SpeedBonus, 
    PrecBonus, 
    AtkPowerBonus, 
    AtkTimeBonus, 
    AtkRangeBonus, 
    CritModBonus, 
    CritChanceBonus, 
    HpPercentBonus,
    AtkPercentBonus,
    DefPercentBonus,
    SpePercentBonus,
    AtkPowerPercentBonus,
    AtkTimePercentBonus,
    EvasionPercentBonus,

    
};

[System.Serializable]
public struct TileInfo
{
    
    public string type;
    public string attribute;
    public int level;
    public float totalExp;
    public int maxLevel;
    public string statBoost;
    public float boostPercentage;
    public string description;

    public int expToLevel;

    public int row;
    public int column;

    public int hpBonus;
    public int atkBonus;
    public int defBonus;
    public int speedBonus;
    public int precBonus;
    public int atkPowerBonus;
    public int atkTimeBonus;
    public int atkRangeBonus;
    public int critModBonus;
    public int critChanceBonus;

    public float hpPercentBonus;
    public float atkPercentBonus;
    public float defPercentBonus;
    public float spePercentBonus;
    public float precPercentBonus;
    public float atkPowerPercentBonus;
    public float atkTimePercentBonus;
    public float evasionPercentBonus;

};
public class MapTile : MonoBehaviour
{
    public SpriteRenderer sp;
    private Sprite sprite;
    private Sprite startingSprite;

    //road sprite on top of the tile sprite, as well as the array of possible road sprites
    public SpriteRenderer roadSprite;
    public Sprite[] roadSprites;

    [Header("Tile Booleans")]
    //makes the tile a tile that your monsters can be placed on
    public bool isBuildable;

    //makes the tile eligible for enemy monsters to walk on
    public bool isRoad;

    //bool to tell if the tile is in the attack range of an tower
    public bool isAttackTarget;

    //bool to tell if the tile is in a state of showing it's range on the map
    public bool isShowingRange;

    //bool to tell if the tile's color transition is counting up from 0 or down from 1
    public bool isCountingDown;

    public bool isActiveTile;
    private MapTile activeTileIndicator;

    public Monster monsterOn;
    public bool hasMonster;


    //list of all of the towers that have this tile in their attack range
    public List<Monster> towersInRange = new List<Monster>();


    //the sprite of the tile
    public Sprite road, blank;

    //the number at which the tile spawned
    public int tileNumber;


    [Header("Tile Path Values")]
    //properties of the tile that affect the enemy moving on it
    public float waitSeconds = 0;
    public float speedOut = 0;

    public string pathDirection;
    //this is used to check for paths that are on different paths, but overlap. if this happens, use the 4 way intersection sprite
    public int pathCount;

    public List<int> pathDirections = new List<int>();

    //set a baseline for its natural color so it can be switched back easily
    public Color tileColor;

    //give the tile a certain element or special type
    public TileAttribute tileAtt = new TileAttribute();
    public int tileAttInt;

    //give the tile a certain element or special type
    public TileType tileType = new TileType();
    public int tileTypeInt;

    public TileInfo info = new TileInfo();

    

    //the tile animator for different elements and their idle/movement animations
    public MapTileAnimations tileAnimations;

    

    [Header("Tile Level And Mining")]
    public AnimationCurve curve;

    public bool isMining;
    //rate at which tile EXP is mined
    public float mineRate;
    public float acumTime;
    //interval between each "mine"
    public float mineInterval;
    //total time the tile has been mined for
    public float mineTotalTime;
    //float to track the time between mining for item checks
    public float mineAcumTime;
    
    //the mining object for this tile
    public MapTileMining miner;

    //public GameObject reflector;
    //public GameObject reflectorCamera;
    //private Camera camera;
    //public Renderer Renderer;

    public static int cameraDepth;

    //this is set from the Map Details script when the tile is created
    public MapDetails mapDetails;
    public GameObject snowTile;
    
    private void Awake()
    {
        sp.GetComponent<SpriteRenderer>();
        roadSprite.GetComponent<SpriteRenderer>();
        sprite = GetComponent<Sprite>();
        tileAnimations.GetComponent<MapTileAnimations>();

        sprite = sp.sprite;
        
        
        
    }

    // Start is called before the first frame update
    void Start()
    {
        startingSprite = sp.sprite;
        tileColor = sp.color;

        //info.level = 1;
        //info.totalExp = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (isShowingRange)
        //{
        //    if (isCountingDown)
        //    {
        //        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, sp.color.a - .01f);
        //    }
        //    else
        //    {
        //        sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, sp.color.a + .01f);
        //    }

        //    if (sp.color.a >= .85)
        //    {
        //        isCountingDown = true;
        //    }
        //    if (sp.color.a <= .35)
        //    {
        //        isCountingDown = false;
        //    }
        //}

        //if the tile is mining, allow the MineTile method to activate every frame
        if (isMining)
        {
            MineTile();
        }

    }

    //this is called from the Tile Info Menu Script to trigger the start of the tile mining
    public void StartMining()
    {
        isMining = true;
        mineRate = GameManager.Instance.GetComponent<YourAccount>().companion.data.miningRate;
        mineInterval = GameManager.Instance.GetComponent<YourAccount>().companion.data.miningInterval;
        GameManager.Instance.tilesMining.Add(this);

        //create a new miner
        miner = new MapTileMining(GameManager.Instance.activeMap, this);
    }

    //use this to stop mining the tile
    public void StopMining()
    {
        isMining = false;
        GameManager.Instance.tilesMining.Remove(this);
    }

    //this is the method that is active when a tile is being mined
    public void MineTile()
    {
        acumTime += Time.deltaTime;
        mineAcumTime += Time.deltaTime;

        if (acumTime <= mineInterval)
        {
            //
        }
        else
        {
            GetExp((int)mineRate);
            acumTime = 0;
        }

        //attempt to mine an item every interval. the interval goes up depending on the level of the tile. higher level = higher rewards
        if (mineAcumTime >= 10 + (info.level * 2))
        {
            miner.MineCheck();
            mineAcumTime = 0;
        }


        mineTotalTime += Time.deltaTime;
    }

    //use this when a tile gets EXP 
    public void GetExp(int exp)
    {
        info.totalExp += exp;
        info.expToLevel = GameManager.Instance.tileLevelUp[info.level + 1] - (int)info.totalExp;

        //if the exp the tile gains is enough to level it up, then do so
        if (info.expToLevel <= 0)
        {
            //if the tile isn't at the max level, level it up
            if (info.level < GameManager.Instance.tileMaxLevel)
            {
                info.level += 1;

                //checks if the new level is the max tile level or not
                if (info.level < GameManager.Instance.tileMaxLevel)
                {
                    info.expToLevel = GameManager.Instance.tileLevelUp[info.level + 1] - (int)info.totalExp;
                }
                else
                {
                    info.expToLevel = 0;
                }
            }
        }
    }

    

    //when the maps are being loaded, call this method from MAPDETAILS to set the level and EXP of the tile, within the random bounds of the map level
    public void SetLevel(int mapLevel)
    {
        float r = UnityEngine.Random.Range(0f, 1f);
        


        //with the level of the map, figure out the maximum level of the tiles, and the chances of each tile being each level
        MapTileLevelCalc calc = new MapTileLevelCalc(mapLevel, curve);

        curve = calc.Curve;

        if (calc.value < .1f)
        {
            calc.value = .1f;
        }

        //set the level of the tile, and based on its level, give it exp
        info.level = Mathf.FloorToInt(calc.value * 10);
        info.totalExp = GameManager.Instance.tileLevelUp[info.level];

        info.expToLevel = GameManager.Instance.tileLevelUp[info.level + 1] - (int)info.totalExp;

        

    }


    //invoke this method if the tile is going to be able to have a tower placed on it
    public void Build()
    {
        //sp.color = Color.black;
        //sp.sprite = dirt;
        //sp.sprite = blank;
        isBuildable = true;
        
    }


    public void Road()
    {
        //sp.color = Color.blue;
        //sp.sprite = road;
        isBuildable = false;
        isRoad = true;
        pathCount += 1;



        ///////LATER ON, GET A VARIABLE FOR A MAP "THEME" AND THEME THE ROADS WITH A DIFFERENT SHADER//////////
        roadSprite.gameObject.AddComponent<StoneFX>();
    }

   

    
   

    public void GetAttribute(int tileInt)
    {
        tileAttInt = tileInt;

        var sprites = GameManager.Instance.GetComponent<Maps>().allTileSpritesDict;


        tileAtt = (TileAttribute)Enum.ToObject(typeof(TileAttribute), tileInt);
        //Debug.Log(tileAtt);

        info.attribute = tileAtt.ToString();


        if (sprites.ContainsKey(tileInt))
        {
            sp.sprite = sprites[tileInt];
            tileAnimations.TileAnimation(tileAtt, isRoad);
        }
        

        

        info.atkBonus = 100;

        //adds this tile to the Map Details' AllTiles, if it isn't already
        if (!mapDetails.allTiles.Contains(this))
        {
            mapDetails.allTiles.Add(this);
        }
        else
        {
            //
        }
    }

    
    //use this to clear a tile's attributes before applying a new one
    public void ClearAttribute()
    {
        
        tileAnimations.ClearTile();
        GameManager.Instance.TriggerEvent(TriggerType.TileChange);
    }


    //this method is invoked by a Tower to turn the tile in to an attack range target
    public void AttackRange(Monster monster)
    {
        isAttackTarget = true;
        towersInRange.Add(monster);
    }

    //this method is invoked by a Tower to remove the tile as a "tile in range" of a monster's attack
    public void RemoveAttackRange(Monster monster)
    {
        isAttackTarget = false;
        towersInRange.Remove(monster);
    }

    //this method is invoked by a Tower if an attack's range is being displayed on the UI
    public void ShowRange(Color color)
    {
        //if (!isShowingRange)
        //{
        //    sp.sprite = blank;
        //    sp.color = color;
        //}

        isShowingRange = !isShowingRange;


        var range = Instantiate(GameManager.Instance.blankTile, transform.position, Quaternion.identity);
        range.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
        range.gameObject.tag = "RangeTile";
        range.gameObject.name = "ActiveOverlay";
        range.transform.SetParent(gameObject.transform, true);
        range.transform.position = transform.position;
        //range.GetComponent<SpriteRenderer>().sprite 
        range.GetComponent<SpriteRenderer>().color = color;
        range.GetComponent<Image>().raycastTarget = false;
        range.GetComponent<MapTile>().isShowingRange = true;

        if (!isShowingRange)
        {

            Destroy(range);
        }
        else 
        {
            
            //activeTileIndicator = range;
        }

       
        //GetComponent<PolygonCollider2D>().enabled = false;
    }


    //tells what monster is on the current tile, and applies any boosts or nerfs to the monster
    public void MonsterOnTile(Monster monster)
    {
        monsterOn = monster;
        hasMonster = true;
        MapTileStatChange change = new MapTileStatChange();

        if (monster.info.type1 == info.attribute || monster.info.type2 == info.attribute)
        {
            //apply the tile changes to the monster
            change.ApplyTileChanges(monster, gameObject.GetComponent<MapTile>());
            //add the tile to the monster's list of boosted tiles
            monster.boostTiles.Add(gameObject.GetComponent<MapTile>());
        }
    }

    //sets the tile's "Active Status" in the MapTileMenu script
    public void ActiveTile()
    {
        isActiveTile = !isActiveTile;

        

        if (isActiveTile)
        {
            var range = Instantiate(GameManager.Instance.blankTile, transform.position, Quaternion.identity);
            range.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder +1;
            range.gameObject.tag = "Untagged";
            range.gameObject.name = "ActiveOverlay";
            range.transform.SetParent(gameObject.transform, true);
            range.transform.position = transform.position;
            range.gameObject.AddComponent<GoldFX>();
            range.GetComponent<SpriteRenderer>().color = new Color(.54f, .54f, .54f, 1f);
            range.GetComponent<Image>().raycastTarget = false;

            activeTileIndicator = range;
        }
        else
        {
           if (activeTileIndicator != null)
            {
                Destroy(activeTileIndicator.gameObject);
                activeTileIndicator = null;
            }
        }
    }






    private void OnCollisionEnter2D(Collision2D other)
    {
        //when something collides with this tile, it checks to see if an enemy leg entered. If it did, the towers in range attack the monster
        CheckEnemyOnTile(other);

    }


    public void CheckEnemyOnTile(Collision2D other)
    {
        var enemy = other.gameObject.GetComponentInParent<Monster>();

        if (enemy)
        {
            //if the leg touching this tile is a tower, ignore it. If it's an enemy, invoke an attack from the Tower's 'Attack' method
            if (enemy.isEnemy)
            {


                var tag = other.gameObject.tag;

                if (tag == "Leg")
                {
                    //changes the current tile of the enemy to the tile that it enters
                    enemy.gameObject.GetComponent<Enemy>().currentTile = tileNumber;
                    if (isAttackTarget)
                    {
                        //send over the monster that is on the target tile so the attacking monster knows what enemy to target
                        foreach (Monster monster in towersInRange)
                        {
                            //monster.GetComponent<Tower>().Attack(enemy, gameObject.GetComponent<MapTile>());

                            //if the enemy moving over this tile is already in range of the targeted tower, don't re-add it
                            //if (!monster.GetComponent<Tower>().enemiesInRange.Contains(enemy.gameObject.GetComponent<Enemy>()))
                            //{
                            //    monster.GetComponent<Tower>().enemiesInRange.Add(enemy.gameObject.GetComponent<Enemy>());
                            //}

                        }
                    }
                    //else
                    //{
                    //    foreach (Monster monster in towersInRange)
                    //    {
                    //        //monster.GetComponent<Tower>().Attack(enemy, gameObject.GetComponent<MapTile>());

                    //        //if the enemy moving over this tile is already in range of the targeted tower, don't re-add it
                    //        if (monster.GetComponent<Tower>().enemiesInRange.Contains(enemy.gameObject.GetComponent<Enemy>()))
                    //        {
                    //            monster.GetComponent<Tower>().enemiesInRange.Remove(enemy.gameObject.GetComponent<Enemy>());
                    //        }

                    //    }

                    //}
                }
            }
            else
            {
                //
            }
        }
    }


    //use this to control a snowy tile. Called from the Map Details script
    public void StartSnow(int intensity)
    {
        var snow = snowTile.GetComponent<DesintegrationFX>();

        float rand = UnityEngine.Random.Range(0f, 1f);
        snow.Desintegration = .8f;
        snow.Seed = rand;
        snowTile.SetActive(true);
        snowTile.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 10;
        snowTile.GetComponent<SpriteRenderer>().sortingLayerName = "TileTop";
        StartCoroutine(TileSnow(intensity));
    }

    public IEnumerator TileSnow(int intensity)
    {
        var snow = snowTile.GetComponent<DesintegrationFX>();

        do
        {
            snow.Desintegration -= .01f;

            yield return new WaitForSeconds((10f - snow.Seed) / (1 + intensity));

        } while (true);
    }
   
}
