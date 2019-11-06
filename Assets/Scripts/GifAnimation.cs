using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GifAnimation : MonoBehaviour
{
    public float framesPerSecond = 10.0f;
    public Texture2D[] frames;
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        

        
    }


    // Update is called once per frame
    void Update()
    {
        index = (int)(Time.time * framesPerSecond);

        index = index % frames.Length; GetComponent<Renderer>().material.mainTexture = frames[index];
    }
}
