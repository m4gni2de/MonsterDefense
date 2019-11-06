using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackEffects : MonoBehaviour
{
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

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

        

        if (attackEmission)
        {
            var x = Instantiate(attackEmission, transform.position, Quaternion.identity);
            x.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingLayerName = gameObject.GetComponent<Renderer>().sortingLayerName;
        }
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            
            transform.Translate(direction / (150 /Attack.attackSpeed), Space.World);
            
            
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

        animator.speed = animator.speed + (animator.speed / Attack.attackTime);

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

           

            transform.rotation = Quaternion.Euler(0f, 0f, aimAngle);

            
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

        

        if (tag == "Enemy")
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
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }


}
