using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackEffects : MonoBehaviour
{
    private Vector2 direction;
    private bool isMoving;
    private float delay;

    //the target of the attack and the information about the attack to happen
    public TypeChart attackInfo;
    public Monster enemy;

    public float force, AtkStat, CritChance, CritMod;
    public int AtkPower, AttackerLevel;
    public string AttackName, AttackType;
    public Monster attacker;
    // Start is called before the first frame update

    

    void Start()
    {
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            transform.Translate(direction / 10, Space.World);
        }
    }

    //gotten from the Enemy script
    public void Damage(TypeChart attack, Monster target)
    {
        enemy = target;
        attackInfo = attack;

    }

    //recieve attacker information from the Tower Template script. holds data about the attack and attacker
    public void FromAttacker(string atkName, string atkType, float atkStat, int attackPower, int attackerLevel, float critChance, float critMod, Monster attackingMonster)
    {
        Debug.Log(attackPower);
        AtkPower = attackPower;
        AtkStat = atkStat;
        AttackerLevel = attackerLevel;
        AttackName = atkName;
        AttackType = atkType;
        CritChance = critChance;
        CritMod = critMod;
        attacker = attackingMonster;


    }

    //gotten from the TowerTemplate script
    public void AttackMotion(Vector2 dir)
    {
        direction = dir;
        isMoving = true;

    }

    public void OnTriggerStay2D(Collider2D other)
    {
        var tag = other.gameObject.tag;

        

        if (tag == "Enemy")
        {

            enemy = other.gameObject.GetComponent<Monster>();
            enemy.GetComponent<Enemy>().OutputDamage(AttackName, AttackType, AtkPower, AtkStat, AttackerLevel, CritChance, CritMod, attacker);
            gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            Destroy(gameObject, .5f);
        }
    }


}
