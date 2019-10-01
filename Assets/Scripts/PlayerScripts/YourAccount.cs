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
    public double coins;
    public int totalMonstersCollected;

    public float playTime;


};
public class YourAccount : MonoBehaviour
{

    public AccountInfo account = new AccountInfo();

    //variable used to track the current play time, so events can happen at certain intervals
    public float sessionPlayTime;


    public float acumTime;
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
        //need to save this to the account playerpref or else it'll reset every time the game is turned off
        account.playTime += Time.deltaTime;

        acumTime += Time.deltaTime;
        
        if (acumTime >= 1)
        {
            acumTime = 0;
            account.coins += GameManager.Instance.coinGeneration / 3600;
        }
        
        
    }


    private void OnDestroy()
    {
        string user = PlayerPrefs.GetString("Account");
        var userJson = JsonUtility.FromJson<AccountInfo>(user);

        //if your current account matches the saved account, overwrite the saved account with the current account
        if (account.username == userJson.username){
            PlayerPrefs.SetString("Account", JsonUtility.ToJson(account));
        }
    }
}
