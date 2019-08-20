using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public struct AccountInfo
{
    public string username;
    public string emailAddress;
    public string password;
    public string userId;

    public int playerLevel;
    public int playerExp;
    public int coins;


};
public class YourAccount : MonoBehaviour
{

    public AccountInfo account = new AccountInfo();
    


    private void Awake()
    {
        

    }




    // Start is called before the first frame update
    void Start()
    {
       

    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
