using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathEnd : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
    
    

    //if an enemy hits this object, send that information to the map
    public void OnTriggerEnter2D(Collider2D other)
    {
        var tag = other.gameObject.tag;

        if (tag == "Enemy")
        {
            GetComponentInParent<MapDetails>().MapHealth(other.gameObject.GetComponent<Enemy>());

        }
    }
}
