using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaceMotion : MonoBehaviour
{

    public Animator faceAnimator;


    // Start is called before the first frame update
    void Start()
    {
        //set a random blink interval for the monster
        float rand = Random.Range(6f, 11f);

        InvokeRepeating("StartBlink", rand, rand);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartBlink()
    {
        faceAnimator.SetBool("isBlinking", true);
    }

    public void EndBlink()
    {
        faceAnimator.SetBool("isBlinking", false);
    }
}
