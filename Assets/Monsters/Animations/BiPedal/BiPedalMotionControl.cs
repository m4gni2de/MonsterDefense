using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiPedalMotionControl : MonoBehaviour
{
    //how long this monster can stay in a singular idle state before another moving to another one
    public float maxIdleTime;
    //the script attached to the Idle Animation that controls the transition between idle states
    private IdleTime idleTime;

    private Animator monsterAnimator;

    
    // Start is called before the first frame update
    void Start()
    {
        monsterAnimator = gameObject.GetComponent<Animator>();
        idleTime = monsterAnimator.GetBehaviour<IdleTime>();
    }

    // Update is called once per frame
    void Update()
    {

        IdleState();
        
    }

    //this controls the Idle animator the monster is in. if it's in idle for too long, it changes what it does while idle
    public void IdleState()
    {
        //if (idleTime.isIdle && idleTime.idleTimer >= maxIdleTime)
        //{
        //    int rand = Random.Range(1, 3);
        //    monsterAnimator.SetInteger("idleState", rand);
            
        //}
    }

    public void MoveMonster()
    {
        gameObject.GetComponentInParent<Enemy>().Pause();
    }

    //**********************THE FOLLOWING ARE USED AS ANIMATION TRIGGERS ON ANIMATIONS ATTACHED TO THE MONSTER**//////////////////////////////
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
    }

}
