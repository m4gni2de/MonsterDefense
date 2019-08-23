using UnityEngine;
using System.Collections;

public class PlatformerSimpleController : MonoBehaviour 
{
	// UNCOMMENT THIS WHEN USING PUPPET2D RIG
	//private Puppet2D_GlobalControl _globalControl;


	private Animator _animator;

	private Vector3 Scaler;

	public float speed =1.0f;
    public float jumpHeight =1.0f;

    private float walkSpeed =1.0f;

	private string _hor = "Horizontal";
    private string _ver = "Vertical";

    private float _currentSpeed;
	private float _timer;
	private float _RandomTrigger;

	// Use this for initialization
	void Start () 
	{
		_animator = gameObject.GetComponent<Animator> ();
        _currentSpeed = 0;
		_timer = 0;
		_RandomTrigger = Random.Range(2f, 8f);
		Scaler = transform.localScale;
	}

	// Update is called once per frame
	void Update () 
	{


        _currentSpeed = Mathf.Clamp01(_currentSpeed);

        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftCommand) )
        {
            _currentSpeed = 1f;
            _animator.SetFloat("Speed",_currentSpeed);
            _animator.speed=1.5f;
            walkSpeed = speed*2f;
        }
        else
        {

            _animator.SetFloat("Speed",_currentSpeed);
            _animator.speed=1f+ _currentSpeed*.5f;
            walkSpeed = speed*(1f+ _currentSpeed);

        }
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.LeftCommand) )
		{
            _currentSpeed = 0f;
			_timer =0;
			_animator.SetBool("Wait", false);
		}

        _currentSpeed-= 2f*Time.deltaTime;
		if (Input.GetAxis (_hor) < 0)
		{
            _currentSpeed+= 2.25f*Time.deltaTime;

			// UNCOMMENT THIS WHEN USING PUPPET2D RIG
			//_globalControl.flip = true;
			//transform.position += transform.right * walkSpeed* Time.deltaTime;

			// COMMENT THIS WHEN USING PUPPET2D RIG
			transform.localScale= new Vector3(-1f*Scaler.x, 1f*Scaler.y, 1f*Scaler.z);
            transform.position -= transform.right * walkSpeed* Time.deltaTime;


			_animator.SetBool("Wait", false);
			_timer=0;
			_RandomTrigger = Random.Range(2f, 8f);

		} 
		else if (Input.GetAxis (_hor) > 0)
		{
            _currentSpeed+= 2.25f*Time.deltaTime;

			// UNCOMMENT THIS WHEN USING PUPPET2D RIG
			//_globalControl.flip = false;

			// COMMENT THIS WHEN USING PUPPET2D RIG
			transform.localScale= Scaler;

			transform.position += transform.right * walkSpeed* Time.deltaTime;


			_animator.SetBool("Wait", false);
			_timer=0;
			_RandomTrigger = Random.Range(2f, 8f);

		}
		else
		{
			_timer+=Time.deltaTime;
			if(_timer>_RandomTrigger)
			{
				_animator.SetTrigger("Breakout");
				_RandomTrigger =1000f;
			}
			if(_timer>10f)
				_animator.SetBool("Wait", true);
		}
        if (Input.GetAxis (_ver) < 0)
        {
            _animator.SetBool("Down", true);
			_animator.SetBool("Up", false);

        }
		else if (Input.GetAxis (_ver) > 0)
		{
			_animator.SetBool("Up", true);
			_animator.SetBool("Down", false);

		}
        else
        {
            _animator.SetBool("Down", false);
			_animator.SetBool("Up", false);
        }
		if (Input.GetKeyDown ("space")) 
		{

			_animator.SetBool ("Jump", true);
		}
        else
        {
            _animator.SetBool ("Jump", false);
        }
        if (Input.GetMouseButtonDown(0))
        {
            _animator.SetTrigger ("Shoot");

        }
		if (Input.GetMouseButtonDown(1))
		{
			_animator.SetTrigger ("GetHit");
			_animator.SetInteger ("Damage", _animator.GetInteger ("Damage")+1);
			
		}

        _currentSpeed = Mathf.Clamp01(_currentSpeed);



	}


}
