using Puppet2D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadow : MonoBehaviour
{
    public MeshBodyParts bodyParts;
    public List<GameObject> shadowParts;
    public GameObject bones;

    public Animator shadowAnimator;
    public MotionControl shadowMotion;

    public Material shadowMaterial;

    private GameObject body;

    public float yTilt;
    // Start is called before the first frame update
    void Start()
    {
        //create a copy of the monster's moving body parts and set them as a parents of the monster's shadow
        body = Instantiate(bones, transform.position, Quaternion.identity);
        body.transform.SetParent(transform, false);
        body.transform.localPosition = new Vector3(0f, 0f, 0f);
        //body.transform.SetParent(transform, true);
        //body.transform.localPosition = new Vector3(0f, 0f, 0f);
        //body.transform.position = new Vector3(0f, 0f, 0f);
        //body.transform.rotation = new Quaternion(0f, 0f, 180f, 0f);
        //body.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        //body.transform.position = new Vector2(body.transform.position.x, GetComponentInParent<Monster>().specs.legs[0].transform.position.y - gameObject.GetComponentInParent<Monster>().GetComponent<RectTransform>().rect.height);
        //body.transform.position = new Vector2(transform.position.x, transform.position.y - gameObject.GetComponentInParent<Monster>().GetComponent<RectTransform>().rect.height /2);
        //transform.position = new Vector2(transform.position.x + 2, transform.position.y - gameObject.GetComponentInParent<Monster>().GetComponent<RectTransform>().rect.height);
        //body.GetComponent<Puppet2D_GlobalControl>().flip = true;


        //give all of the shadow parts a material that looks like a shadow
        foreach (GameObject part in body.GetComponent<MeshBodyParts>().bodyMeshes)
        {
            shadowParts.Add(part);
            part.GetComponent<Renderer>().material = shadowMaterial;
            part.GetComponent<Renderer>().sortingOrder -= 5;

           
        }


        //set the shadow's animator to match the main body's animator
        shadowAnimator = GetComponentInParent<Monster>().monsterMotion;
        shadowMotion = bones.GetComponent<MotionControl>();
        body.GetComponent<MotionControl>().monsterAnimator = shadowAnimator;


        StartCoroutine(ShadowAnimate());



    }

   //make the shadow animator's parameters to match the main body animator's parameters so the motions match
    public IEnumerator ShadowAnimate()
    {
        do
        {
            for (int i = 0; i < shadowAnimator.parameterCount; i++)
            {
                AnimatorControllerParameter parameter = shadowAnimator.GetParameter(i);
                string name = parameter.name;
                if (shadowAnimator.GetParameter(i).type == AnimatorControllerParameterType.Bool)
                {
                    bool x = shadowAnimator.GetBool(name);
                    body.GetComponent<MotionControl>().monsterAnimator.SetBool(name, x);
                }

                if (shadowAnimator.GetParameter(i).type == AnimatorControllerParameterType.Float)
                {
                    float x = shadowAnimator.GetFloat(name);
                    body.GetComponent<MotionControl>().monsterAnimator.SetFloat(name, x);
                }

                if (shadowAnimator.GetParameter(i).type == AnimatorControllerParameterType.Int)
                {
                    int x = shadowAnimator.GetInteger(name);
                    body.GetComponent<MotionControl>().monsterAnimator.SetInteger(name, x);
                }

            }

            body.GetComponent<Puppet2D_GlobalControl>().flip = !bones.GetComponent<Puppet2D_GlobalControl>().flip;

            if (bones.GetComponent<Puppet2D_GlobalControl>().flip)
            {
                yTilt = 1;
                body.transform.localEulerAngles = new Vector3(body.transform.rotation.eulerAngles.x, body.transform.rotation.eulerAngles.y, -(GameManager.Instance.activeMap.weatherSystem.sun.transform.rotation.eulerAngles.z - 130));
            }
            else
            {
                yTilt = -1;
                body.transform.localEulerAngles = new Vector3(body.transform.rotation.eulerAngles.x, body.transform.rotation.eulerAngles.y, GameManager.Instance.activeMap.weatherSystem.sun.transform.rotation.eulerAngles.z + 130);
            }
            transform.localEulerAngles = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, GameManager.Instance.activeMap.weatherSystem.sun.transform.rotation.eulerAngles.z + 45 * yTilt);
            
            ///transform.position = new Vector2(transform.position.x + )



            //Debug.Log(Vector3.Angle(transform.position, transform.position - GameManager.Instance.activeMap.weatherSystem.sun.transform.position));

            //transform.rotation = GameManager.Instance.activeMap.weatherSystem.sun.transform.rotation;
            //transform.rotation = Quaternion.Euler(0f, 0f, GameManager.Instance.activeMap.weatherSystem.sun.transform.rotation.eulerAngles.z + 65);


            //body.transform.rotation = Quaternion.Euler(0f, 0f, GameManager.Instance.activeMap.weatherSystem.sun.transform.rotation.eulerAngles.z - 110);
            //body.transform.rotation = Quaternion.Euler(0f, 0f, GameManager.Instance.activeMap.weatherSystem.sun.transform.rotation.eulerAngles.z);



            //Debug.Log("Sun Angle: " + aimAngle + " Shadow Angle: " + (transform.rotation.z * Mathf.Rad2Deg) + " Bones Angle: " + (body.transform.rotation.z * Mathf.Rad2Deg));



            yield return new WaitForSeconds(GameManager.Instance.activeMap.weatherSystem.sunRotateTime);

        } while (true);
    }

    // Update is called once per frame
    void Update()
    {
        //shadowAnimator = GetComponentInParent<Monster>().monsterMotion;
        
        

        //foreach()
        

        //body.GetComponent<MotionControl>().monsterAnimator = shadowAnimator;
        //body.GetComponent<MotionControl>() = 
    }

    private void LateUpdate()
    {
        
    }
}
