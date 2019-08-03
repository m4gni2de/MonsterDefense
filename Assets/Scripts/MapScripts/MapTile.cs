using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapTile : MonoBehaviour
{
    public SpriteRenderer sp;
    private Sprite sprite;
    private Sprite startingSprite;

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

    //list of all of the towers that have this tile in their attack range
    public List<Monster> towersInRange = new List<Monster>();


    //the sprite of the tile
    public Sprite dirt, water, grass, road, blank;

    //the number at which the tile spawned
    public int tileNumber;

    //properties of the tile that affect the enemy moving on it
    public float waitSeconds = 0;
    public float speedOut = 0;

    //set a baseline for its natural color so it can be switched back easily
    public Color tileColor;


    private void Awake()
    {
        sp.GetComponent<SpriteRenderer>();
        sprite = GetComponent<Sprite>();


        sprite = sp.sprite;
    }

    // Start is called before the first frame update
    void Start()
    {
        startingSprite = sp.sprite;
        tileColor = sp.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (isShowingRange)
        {
            if (isCountingDown)
            {
                sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, sp.color.a - .01f);
            }
            else
            {
                sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, sp.color.a + .01f);
            }

            if (sp.color.a >= .85)
            {
                isCountingDown = true;
            }
            if (sp.color.a <= .35)
            {
                isCountingDown = false;
            }
        }
    }

    //invoke this method if the tile is going to be able to have a tower placed on it
    public void Build()
    {
        //sp.color = Color.black;
        sp.sprite = grass;
        isBuildable = true;
    }

    //invoke this method if the tile is going to be used as a pathway for monsters
    public void Dirt()
    {
        //sp.color = Color.green;
        sp.sprite = dirt;
    }

    //invoke this method if the tile is going to be used as a water tile
    public void Water()
    {
        //sp.color = Color.blue;
        sp.sprite = water;
    }

    public void Road()
    {
        //sp.color = Color.blue;
        sp.sprite = road;

        isRoad = true;
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
        if (!isShowingRange)
        {
            sp.sprite = blank;
            sp.color = color;
        }

        isShowingRange = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        //when something collides with this tile, it checks to see if an enemy leg entered. If it did, the towers in range attack the monster
        CheckEnemyOnTile(other);

    }


    public void CheckEnemyOnTile(Collision2D other)
    {
        var enemy = other.gameObject.GetComponentInParent<Monster>();

        //if the leg touching this tile is a tower, ignore it. If it's an enemy, invoke an attack from the Tower's 'Attack' method
        if (enemy.isEnemy)
        {
            //changes the current tile of the enemy to the tile that it enters
            enemy.gameObject.GetComponent<Enemy>().currentTile = tileNumber;


            var tag = other.gameObject.tag;
            if (tag == "Leg")
            {
                if (isAttackTarget)
                {
                    //send over the monster that is on the target tile so the attacking monster knows what enemy to target
                    foreach (Monster monster in towersInRange)
                    {
                        monster.GetComponent<Tower>().Attack(enemy);
                    }
                }
            }
        }
        else
        {
            //
        }
    }
}
