using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshBodyParts : MonoBehaviour
{
    public GameObject[] bodyMeshes;
    public Material[] starMeshes;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //call this to make the monster a star monster
    public void StarMonster()
    {
        for (int i = 0; i < starMeshes.Length; i++)
        {
            bodyMeshes[i].GetComponent<Renderer>().material = starMeshes[i];
            //bodyMeshes[i].GetComponent<Renderer>().materials[0].
            
        }
    }
}
