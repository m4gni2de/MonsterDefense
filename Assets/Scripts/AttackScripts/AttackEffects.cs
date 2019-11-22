using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AttackEffects : MonoBehaviour
{

    public bool hasAnimation;
    public bool hasRotation;
    public float rotationRate;


    private Vector2 direction;
    private bool isMoving;
    public float delay;

    //the target of the attack and the information about the attack to happen
    public TypeChart attackInfo;
    public Monster enemy;

    public float force, AtkStat, CritChance, CritMod;
    public int AtkPower, AttackerLevel;
    public string AttackName, AttackType;
    public Monster attacker;
    public MonsterAttack Attack;

    //if an attack has a particle emission sprite for when it's summoned and for when it's destroyed
    public GameObject attackEmission;
    public GameObject attackDemission;

    private Animator animator;

    //fill this with the sprites of this attack that are to be animated. this is to prevent using animations for everything
    public Sprite[] attackSprites;

    
    private void Awake()
    {
        
        if (hasAnimation)
        {
            animator = GetComponent<Animator>();
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {

        

        if (attackEmission)
        {
            var x = Instantiate(attackEmission, transform.position, Quaternion.identity);
            x.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = gameObject.GetComponent<Renderer>().sortingLayerName;
        }


        //if an attack doesn't need an animation and only uses a sprite loop, don't attach an animation to it
        if (hasAnimation)
        {
            Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
        }
        else
        {
            Destroy(gameObject, delay);
            StartCoroutine(SpriteAnimation());
        }

       
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            transform.Translate(direction / (150 /Attack.attackSpeed), Space.World);
        }

        if (hasRotation)
        {
            transform.Rotate(transform.rotation.x, transform.rotation.y, transform.rotation.z + rotationRate);
        }
    }

   public IEnumerator SpriteAnimation()
    {
        int i = 0;

        if (attackSprites.Length > 0)
        {
            do
            {
                GetComponent<SpriteRenderer>().sprite = attackSprites[i];
                yield return new WaitForSeconds(.05f);

                i += 1;

                if (i >= attackSprites.Length)
                {
                    i = 0;
                }
            } while (true);
        }
    }

    //recieve attacker information from the Tower Template script. holds data about the attack and attacker
    public void FromAttacker(MonsterAttack attack, string atkName, string atkType, float atkStat, int attackPower, int attackerLevel, float critChance, float critMod, Monster attackingMonster)
    {

        AtkPower = attackPower;
        AtkStat = atkStat;
        AttackerLevel = attackerLevel;
        AttackName = atkName;
        AttackType = atkType;
        CritChance = critChance;
        CritMod = critMod;
        attacker = attackingMonster;
        Attack = attack;

        //animator.speed = animator.speed + (animator.speed / Attack.attackTime);

    }

    //gotten from the Enemy script
    public void Damage(TypeChart attack, Monster target)
    {
        enemy = target;
        attackInfo = attack;

    }

    //gotten from the TowerTemplate script
    public void AttackMotion(Vector2 dir)
    {
        direction = dir;

        //if the attack type is a projectile, shoot the projectile. If it's not, spawn the physical animation on the enemy itself
        if (Attack.attackMode == AttackMode.Projectile)
        {
            isMoving = true;

            float aimAngle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            if (aimAngle < 0f)
            {
                aimAngle = Mathf.PI * 2 + aimAngle;
            }


            
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + aimAngle);
            //transform.localEulerAngles = new Vector3(transform.rotation.x, transform.rotation.y, transform.localEulerAngles.z + aimAngle);

            
        }
        else
        {
            //transform.position = attacker.GetComponent<Tower>().attackPoint.transform.position;
            //transform.SetParent(attacker.transform);

            isMoving = true;

            //float aimAngle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            //if (aimAngle < 0f)
            //{
            //    aimAngle = Mathf.PI * 2 + aimAngle;
            //}

            //if (aimAngle > 180)
            //{
            //    aimAngle = 180 - aimAngle;
            //}

            ////Debug.Log(aimAngle);
            if (attacker)
            {
                transform.rotation = attacker.monsterMotion.transform.rotation;
            }
            else
            {

            }
        }


    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        var tag = other.gameObject.tag;

        if (other.GetType() != typeof(PolygonCollider2D))
        {
            return;
        }
        
        if (tag == "Enemy" && other.GetType() == typeof(PolygonCollider2D))
        {

            enemy = other.gameObject.GetComponent<Monster>();
            enemy.GetComponent<Enemy>().OutputDamage(AttackName, AttackType, AtkPower, AtkStat, AttackerLevel, CritChance, CritMod, attacker, Attack);
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;

            if (attackDemission)
            {
                var x = Instantiate(attackDemission, other.transform.position, Quaternion.identity);
            }

            //if the attack is NOT a projectile, let the attack animation run it's full course. if it is a projectile, destroy the animation immediately
            if (Attack.attackMode == AttackMode.Projectile)
            {
                StartCoroutine(AttackOnHit());
            }
            else
            {
                StartCoroutine(AttackOnHit());
            }
        }
    }

    //when an attack lands, start its destroy animating
    public IEnumerator AttackOnHit()
    {
        SpriteRenderer sp = gameObject.GetComponent<SpriteRenderer>();
        for (int i = 0; i < 30; i++)
        {
            sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, sp.color.a - .09f);

            yield return new WaitForSeconds(.03f);

            if (i >= 29)
            {
                Destroy(gameObject);
            }
        }
    }


}
