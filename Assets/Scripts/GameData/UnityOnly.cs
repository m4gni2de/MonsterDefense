using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityOnly : MonoBehaviour
{
    //objects with this script are not active, but become active if in Unity. These are used as Unity only tools
    void Start()
    {
#if UNITY_EDITOR
        ShowOnUnity();
#endif

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowOnUnity()
    {

        gameObject.SetActive(true);
    }
}
