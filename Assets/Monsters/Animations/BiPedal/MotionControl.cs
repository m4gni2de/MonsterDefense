using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionControl : MonoBehaviour
{
    //how long this monster can stay in a singular idle state before another moving to another one
    public float maxIdleTime;
    //the script attached to the Idle Animation that controls the transition between idle states
    private IdleTime idleTime;

    private Tower tower;
    private Enemy enemy;
    private Monster monster;
    private MapTile target;
    private int targetTile;
    

    public Animator monsterAnimator;

    //bool to check if an enemy was hit with an attack
    public bool isHit;
    public float hitAcumTime, hitTime;

    //used to transition to monster specific states within the animator
    public int dexId;
    public float animatorSpeed;

    public GameObject monsterBreath;


    
    
    
    // Start is called before the first frame update
    void Start()
    {
        tower = gameObject.GetComponentInParent<Tower>();
        monster = gameObject.GetComponentInParent<Monster>();
        monsterAnimator = gameObject.GetComponent<Animator>();
        idleTime = monsterAnimator.GetBehaviour<IdleTime>();
        


        monsterAnimator.speed = 1 * ((float)monster.info.speBase / 75);
        animatorSpeed = monsterAnimator.speed;
        dexId = monster.info.dexId;
        monsterAnimator.SetFloat("attackSpeed", animatorSpeed);
        monsterAnimator.SetInteger("dexID", dexId);


       



    }

    // Update is called once per frame
    void Update()
    {

        IdleState();
        
        //if a monster is hit with an attack, temporarily slow down it's movement
        if (isHit && monsterAnimator.GetBool("isDead") == false)
        {
            hitAcumTime += Time.deltaTime;
            monster.GetComponent<Enemy>().speed -= 1;

            //if the total time the monster's movement speed has been lowered is greater than the time that the attack slows the enemy for, reset the enemy's speed back to regular
            if (hitAcumTime >= hitTime)
            {
                isHit = false;
                hitTime = 0;
                monster.GetComponent<Enemy>().speed = 50 * ((float)monster.info.speBase / 100);
            }

        }
        
    }

    public void LateUpdate()
    {
        
    }

    //this controls the Idle animator the monster is in. if it's in idle for too long, it changes what it does while idle
    public void IdleState()
    {
        if (idleTime.isIdle && idleTime.idleTimer >= maxIdleTime)
        {
            int rand = Random.Range(1, 3);
            monsterAnimator.SetInteger("idleState", rand);
            monsterAnimator.GetBehaviour<IdleTime>().idleState = monsterAnimator.GetInteger("idleState");


        }
    }

    public void MoveMonster()
    {
        gameObject.GetComponentInParent<Enemy>().Pause();
    }


    //when a Tower readies an attack, the direction of the attack is taken so the tower knows which way to face...which is this monster's position
    public void AttackDirection(int TargetTile, Enemy Enemy)
    {
        //target = Target;
        targetTile = TargetTile;
        enemy = Enemy;
    }

    //get the type of attack from the attacker here so that the monster knows what type of attack animation to do
    public void AttackModeCheck(AttackMode mode)
    {
        monsterAnimator.SetBool("isProjectile", false);
        monsterAnimator.SetBool("isPunch", false);
        monsterAnimator.SetBool("isKick", false);

        //Debug.Log(mode);

        if (mode == AttackMode.Projectile)
        {
            monsterAnimator.SetBool("isProjectile", true);
            
        }
        if (mode == AttackMode.Punch)
        {
            monsterAnimator.SetBool("isPunch", true);
            
        }
        if (mode == AttackMode.Kick)
        {
            monsterAnimator.SetBool("isKick", true);
            
        }
    }
    

    //this is called from the enemy script when an emey is hit with an attack
    public void IsHit(MonsterAttack attack)
    {
        //hitTime = 
        isHit = true;
    }

    //**********************THE FOLLOWING ARE USED AS ANIMATION TRIGGERS ON ANIMATIONS ATTACHED TO THE MONSTER**//////////////////////////////

     //once the animation has officially started, actually fire the attack
    public void StartAttack()
    {

        //if the monster is in a game, launch attacks like normal. If not, use the more generalized attack
        if (GameManager.Instance.inGame == true)
        {
            if (enemy)
            {
                if (enemy.transform.position.x <= tower.attackPoint.transform.position.x)
                {
                    monster.puppet.flip = true;
                    

                }
                else
                {
                    monster.puppet.flip = false;
                   
                }



                tower.LaunchAttack(targetTile, enemy);
            }
        }
        else
        {
            monster.TestAttack();
        }
    }


    //when the monster is done with their "being hit by an attack" animation"
    public void EndHit()
    {
        monsterAnimator.SetBool("isHit", false);
        
       
    }

    public void EndDodge()
    {
        monsterAnimator.SetBool("isDodge", false);
        
    }

    public void EndAttack()
    {
        monsterAnimator.SetBool("isAttacking", false);
        monsterAnimator.SetBool("isProjectile", false);
        monsterAnimator.SetBool("isKick", false);
        monsterAnimator.SetBool("isPunch", false);
        tower.isAttacking = false;
        monster.isAttacking = false;



        
    }

    public void EndClickedAnimation()
    {
        monsterAnimator.SetBool("isClicked", false);
        
    }


    //set enemy up for being destroyed
    public void StartMonsterDie(Enemy Enemy)
    {
        //remove it's HP Slider
        Enemy.enemyHpSlider.gameObject.SetActive(false);
        //set its Animatoion State to dead
        monsterAnimator.SetBool("isDead", true);
        
        //stop the enemy from moving
        Enemy.speed = 0;
        //change its tag so it does not appear in any "Enemy" lists that use the "Enemy" tag
        Enemy.gameObject.tag = "Corpse";

        Enemy.mapDetails.LiveEnemyList();

    }


    //this is activated right at the start of the Death Animation. makes it so any other booleans are false, so another animation state won't override and kick in
    public void EndMonsterDeathState()
    {
        
        monsterAnimator.SetBool("isHit", false);
        monsterAnimator.SetBool("isDodge", false);

        
    }

    //the actual deal of the monster from the game
    public void MonsterDeath()
    {
        
        Destroy(monster.gameObject);
    }

    //used to signal the monster's breath
    public void TakeBreath()
    {
        

        ParticleSystem ps = monsterBreath.GetComponentInChildren<ParticleSystem>();
        Renderer rend = ps.GetComponent<Renderer>();
        rend.sortingLayerName = GetComponent<MeshBodyParts>().bodyMeshes[0].GetComponent<Renderer>().sortingLayerName;
        rend.sortingOrder = 1000;

        ps.Play();
    }

    //used to signal the monster to close their eyes
    public void Blink()
    {

    }


    public void ResetAttacks()
    {
        monsterAnimator.SetBool("isAttacking", false);
        monsterAnimator.SetBool("isProjectile", false);
        monsterAnimator.SetBool("isKick", false);
        monsterAnimator.SetBool("isPunch", false);
        tower.isAttacking = false;


        
    }

    
}
