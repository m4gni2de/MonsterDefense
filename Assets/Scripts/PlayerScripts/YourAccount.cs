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

    public string defenderDataString;

};
public class YourAccount : MonoBehaviour
{

    public AccountInfo account = new AccountInfo();
    public YourItems yourItems;

    //the player's companion
    public CompanionScript companion;

    //variable used to track the current play time, so events can happen at certain intervals
    public float sessionPlayTime;

    //used to hold the value for the amount of coins able to be acquired
    public double acumCoins;

    public float acumTime;

    private void Awake()
    {
        

    }




    // Start is called before the first frame update
    void Start()
    {
        yourItems = GetComponent<YourItems>();

    }

    

    // Update is called once per frame
    void Update()
    {
        //need to save this to the account playerpref or else it'll reset every time the game is turned off
        account.playTime += Time.deltaTime;


        acumTime += Time.deltaTime;

        //get an amount of coins that are able to be aquired by the player as long as they active the GetCoins method
        if (acumTime >= 1)
        {
            acumTime = 0;
            acumCoins += GameManager.Instance.coinGeneration / 3600;
        }
        
        
    }


    //the coins generate,but can only be gotten using this method
    public void GetCoins()
    {
        account.coins += acumCoins;
        acumCoins = 0;
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
