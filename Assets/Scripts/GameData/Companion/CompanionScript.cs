using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionScript : MonoBehaviour
{
    public CompanionData data = new CompanionData();

    // Start is called before the first frame update
    void Start()
    {
        data.miningRate = 2;
        data.miningInterval = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
