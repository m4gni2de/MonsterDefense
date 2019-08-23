using UnityEngine;
using System.Collections;

public class FighterAIController : MonoBehaviour {

    public bool IsPlayer;
    public float Power = 1f;
    public Transform PowerBar ;

    private Animator _animator;
    public FighterAIController Enemy;
    public float speed =1.0f;
    public float HitForce = 3f;

	private string hor = "Horizontal";
	private string vert = "Vertical";

	public enum FighterStates {Idle,Walk, Attack, GetHit, Die, Dead, Thrown, Win, Block};
	public FighterStates FighterState = FighterStates.Walk;
    private float _timer = -1f;
    public Rigidbody2D _rigid;
	public float HitDistance = 11f;
    // Use this for initialization
    void Start () 
    {
        _animator = gameObject.GetComponent<Animator> ();
        //_globalControl = gameObject.GetComponent<Puppet2D_GlobalControl> ();
        //Enemy = FindObjectOfType<FighterAIController> ();
        _rigid = gameObject.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update () 
    {

		switch (FighterState) 
		{
			case FighterStates.Idle:
				Idle ();
				break;
			case FighterStates.Walk:
				
				Walk ();
				break;
            case FighterStates.Attack:
                Attack();
				break;
            case FighterStates.GetHit:
                GetHit();
                break;
			case FighterStates.Die:
                _animator.SetTrigger("Die");
				break;
			case FighterStates.Dead:
				break;
			case FighterStates.Thrown:
                Thrown();
				break;
			case FighterStates.Win:
				Win ();
				break;
			case FighterStates.Block:
				Block ();
                break;
		}
        if (Power<=0f)
		{
            FighterState = FighterStates.Die;
			Enemy.FighterState = FighterStates.Win;

		}
		if(!IsPlayer)
		{
			if (FighterState == FighterStates.Walk &&  InRange())
			{
	            _animator.SetFloat("Speed",0);

	            FighterState = FighterStates.Attack;

	        }
	        else if(FighterState== FighterStates.Attack)
	            FighterState = FighterStates.Walk;
		}

    }
	void Block()
	{
		if (IsPlayer)
			PlayerControl();
	}
    void EnemyControl()
    {
        _animator.SetFloat("Speed",1);
        transform.position += transform.right * speed* Time.deltaTime;



    }
    void PlayerControl () 
    {


        if (Input.GetAxis (hor) < 0)
        {
            //_globalControl.flip = true;
            _animator.SetFloat("Speed",1);
			transform.localScale = new Vector3(1, 1, -1);
			transform.localEulerAngles = new Vector3(0, 180, 0);
			transform.position += transform.right * speed* Time.deltaTime;
			

		} 
        else if (Input.GetAxis (hor) > 0)
        {
            _animator.SetFloat("Speed",1);
			// _globalControl.flip = false;
			transform.localScale = new Vector3(1,1, 1);
			transform.localEulerAngles = new Vector3(0, 0, 0);

			transform.position += transform.right * speed* Time.deltaTime;

        }
        else
            _animator.SetFloat("Speed",0);
		
		if (Input.GetAxis (vert) < 0)
			_animator.SetBool ("Crouch", true);
		else
			_animator.SetBool ("Crouch", false);
		
        if (Input.GetButtonDown("Fire1"))
        {
//			if (Input.GetAxis (vert) < 0)
//				_animator.SetTrigger("AttackLow");			
//			else
            	_animator.SetTrigger("Attack");
            Enemy.Hit();

        }
		if (Input.GetButtonDown("Fire3"))
		{
            _animator.SetBool("Block", true);
            FighterState = FighterStates.Block;
        }
		if (Input.GetButtonUp("Fire3"))
		{
			_animator.SetBool ("Block", false);
			FighterState = FighterStates.Idle;
		}

		if (Input.GetButtonDown("Fire2"))			
		{
			_animator.SetTrigger("Throw");
			if (InRange ())
			{
				Enemy.GetThrown ();
                Enemy.FighterState = FighterStates.Thrown;

			}
		}

    }
	public void GetThrown()
	{
		FighterState = FighterStates.Thrown;
		Debug.Log ("thrown");
        _timer = 0f;
		_animator.SetTrigger("Thrown");
		_rigid.isKinematic = true;
	}

    void Thrown()
    {
        _timer += Time.deltaTime;
         
        transform.position = Vector3.Lerp(transform.position, transform.position + .5f*transform.right,_timer) ;
         
        if (_timer > 1.5f)
        {
            _timer = 0f;
            FighterState = FighterStates.Walk;

			
			transform.localScale = new Vector3(1,  1, transform.localScale.z * -1);
			transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y+180, 0);

			Enemy.transform.localScale = new Vector3(1, 1, transform.localScale.z * -1);
			Enemy.transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + 180, 0);

			//Enemy._globalControl.flip = ! Enemy._globalControl.flip;


			_rigid.isKinematic = false;
            Damage();
            Damage();

        }
    }
	void Walk()
	{
        if (IsPlayer)
            PlayerControl();
        else
            EnemyControl();
		
	}
	void Idle()
	{
		if (IsPlayer)
			PlayerControl();
		else
			EnemyControl();

	}
	void GetHit()
	{
        _timer += Time.deltaTime;
        transform.position -= transform.right * speed* Time.deltaTime*Enemy.HitForce;



        if (_timer > 0.5f)
        {
            _timer = 0f;
            FighterState = FighterStates.Walk;

        }

	}
    void Attack()
    {
		if (IsPlayer)
			PlayerControl();

		_animator.SetTrigger("Attack");
        if ( InRange() && Enemy.FighterState != FighterStates.Block)
		{
            _timer += Time.deltaTime;
            if (_timer > 0.15f)
            {
                _timer = 0f;

                Enemy.Hit();
            }
        }
    }
	public bool InRange()
	{
		float distance = Vector3.Distance(Enemy.transform.position, transform.position);
		if (distance < HitDistance)
			return true;
		else
			return false;

	} 

    public void Damage()
    {
       
        Power -= .1f;
        if (Power >= 0f)
            PowerBar.localScale = new Vector3(Power, 1, 1);
        else
        {
            PowerBar.localScale = new Vector3(0, 1, 1);
        }
    }
	public void Win()
	{
		
		_animator.SetBool("Win",true);

	}
    public void Hit()
    {
		
		if ( InRange())
        {

            Damage();
            _timer = 0f;
            FighterState = FighterStates.GetHit;
            _animator.SetTrigger("GetHit");

        }
    }

}
