using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AccountManagement : MonoBehaviour
{
    //input texts used to create an account
    public TMP_InputField userText, pwText;
    public Canvas createCanvas, selectAccountCanvas;

    public TMP_Dropdown accountSelector;

    

    // Start is called before the first frame update
    void Start()
    {
        


        //string savesDirectory = Application.persistentDataPath + "/Saves/saves.txt";
        

        //try
        //{
        //    if (!File.Exists(savesDirectory))
        //    {
        //        createCanvas.gameObject.SetActive(true);
        //        userText.GetComponent<TMP_InputField>();
        //        pwText.GetComponent<TMP_InputField>();
        //    }
        //    else
        //    {
        //        selectAccountCanvas.gameObject.SetActive(true);
        //        accountSelector.GetComponent<TMP_Dropdown>();
        //        AccountSelection();
        //    }
        //}
        //catch (Exception e)
        //{
        //    Debug.Log(e);
        //}


        if (PlayerPrefs.HasKey("Account"))
        {
            selectAccountCanvas.gameObject.SetActive(true);
            AccountSelection(PlayerPrefs.GetString("Account"));
        }
        else
        {
            createCanvas.gameObject.SetActive(true);
        }


    }

    //this is invoked when the create button is pressed
    public void AccountCreateBtn()
    {
        string player = userText.text.Normalize();
        string password = pwText.text;
        CreateSavesFile(player, password);
    }

    //creates the directory where the player data will be saved if it does not exist already
    public void CreateSavesFile(string player, string password)
    {
        
    

        AccountInfo info = new AccountInfo();

        info.username = player;
        info.emailAddress = "none";
        info.password = password;
        info.userId = "none";
        info.playerLevel = 0;
        info.playerLevel = 0;
        info.playTime = 0;
        info.coins = 0;
        info.defenderDataString = "none";

        GameManager.Instance.GetComponent<YourAccount>().account = info;

        PlayerPrefs.SetString("Account", JsonUtility.ToJson(info));

        SceneManager.LoadScene("MainMenu");

        //StreamWriter file, savesFile, monsterFile, itemsFile;

        ////creates the directory for that particular player account
        //string playerDirectory = Application.persistentDataPath + "/Saves/" + player;
        //string accountText = playerDirectory + "/player.txt";

        ////directory used to indicate an account has been created, so there is at least one selectable account on startup
        //string savesDirectory = Application.persistentDataPath + "/Saves";



        //try
        //{
        //    //creates a player account based on the username and password inputs, as well as files for your inventory and monsters
        //    if (!File.Exists(savesDirectory))
        //    {
        //        Directory.CreateDirectory(savesDirectory);
        //        savesFile = File.CreateText(savesDirectory + "/saves.txt");
        //        savesFile.WriteLine(player);
        //        savesFile.Close();

        //        if (!File.Exists(accountText))
        //        {
        //            Directory.CreateDirectory(playerDirectory);
        //            savesFile = File.AppendText(savesDirectory + "/saves.txt");
        //            savesFile.WriteLine(player);
        //            savesFile.Close();

        //            file = File.CreateText(playerDirectory + "/player.txt");

        //            AccountInfo info = new AccountInfo();

        //            info.username = player;
        //            info.emailAddress = "none";
        //            info.password = password;
        //            info.userId = "none";
        //            info.playerLevel = 0;
        //            info.playerLevel = 0;
        //            info.coins = 0;


        //            file.WriteLine(JsonUtility.ToJson(info));
        //            file.Close();

        //            monsterFile = File.CreateText(playerDirectory + "/monsters.txt");
        //            monsterFile.Close();
        //            itemsFile = File.CreateText(playerDirectory + "/inventory.txt");
        //            itemsFile.Close();

        //            GameManager.Instance.GetComponent<YourAccount>().account = info;

        //            SceneManager.LoadScene("MainMenu");
        //        }


        //    }
        //    else
        //    {
        //        if (!File.Exists(accountText))
        //        {
        //            Directory.CreateDirectory(playerDirectory);
        //            savesFile = File.AppendText(savesDirectory + "/saves.txt");
        //            savesFile.WriteLine(player);
        //            savesFile.Close();

        //            file = File.CreateText(playerDirectory + "/player.txt");

        //            AccountInfo info = new AccountInfo();

        //            info.username = player;
        //            info.emailAddress = "none";
        //            info.password = password;
        //            info.userId = "none";
        //            info.playerLevel = 0;
        //            info.playerLevel = 0;
        //            info.coins = 0;


        //            file.WriteLine(JsonUtility.ToJson(info));
        //            file.Close();

        //            monsterFile = File.CreateText(playerDirectory + "/monsters.txt");
        //            monsterFile.Close();
        //            itemsFile = File.CreateText(playerDirectory + "/inventory.txt");
        //            itemsFile.Close();

        //            GameManager.Instance.GetComponent<YourAccount>().account = info;

        //            SceneManager.LoadScene("YourHome");
        //        }
        //    }
        //}
        //catch (Exception e)
        //{
        //    Debug.Log(e);
        //}
    }

    //fill the dropdown with the account names for the player to select by the names on each line of the "saves" text file. 
    public void AccountSelection(string acctInfo)
    {
        //string savesFile = Application.persistentDataPath + "/Saves/saves.txt";
        //List<string> names = new List<string>();


        //string[] users = File.ReadAllLines(savesFile);

        //for (int i = 0; i < users.Length; i++)
        //{
        //    names.Add(users[i]);
        //}

        //accountSelector.AddOptions(names);


        var player = JsonUtility.FromJson<AccountInfo>(acctInfo);
        var info = GameManager.Instance.GetComponent<YourAccount>().account;

        info.username = player.username;
        info.password = player.password;
        info.totalMonstersCollected = player.totalMonstersCollected;
        info.emailAddress = player.emailAddress;
        info.playerLevel = player.playerLevel;
        info.playerExp = player.playerExp;
        info.playTime = player.playTime;
        info.coins = player.coins;
        info.defenderDataString = player.defenderDataString;

        GameManager.Instance.GetComponent<YourAccount>().account = info;

        //GameManager.Instance.GetComponent<YourMonsters>().GetYourMonsters();
        SceneManager.LoadScene("MainMenu");
    }

    //load the account that is on the dropdown menu for the users
    public void AccountSelectionBtn(string acctInfo)
    {
        //int menuIndex = accountSelector.value;
        //List<TMP_Dropdown.OptionData> accounts = accountSelector.options;
        //string account = accounts[menuIndex].text;

        //string playerDirectory = Application.persistentDataPath + "/Saves/" + account;
        //string accountText = playerDirectory + "/player.txt";

        ////gets the account information from the json string in the textfile and loads in to the player information
        //if (File.Exists(accountText))
        //{
        //    string[] lines = File.ReadAllLines(accountText);

        //    string acctInfo = lines[0];


        //    var player = JsonUtility.FromJson<AccountInfo>(acctInfo);
        //    var info = GameManager.Instance.GetComponent<YourAccount>().account;

        //    info.username = player.username;
        //    info.password = player.password;
        //    info.emailAddress = player.emailAddress;
        //    info.playerLevel = player.playerLevel;
        //    info.playerExp = player.playerExp;
        //    info.coins = player.coins;

        //    GameManager.Instance.GetComponent<YourAccount>().account = info;

        //    GameManager.Instance.GetComponent<YourMonsters>().GetYourMonsters();
        //    SceneManager.LoadScene("YourHome");
        //}

        //var player = JsonUtility.FromJson<AccountInfo>(acctInfo);
        //var info = GameManager.Instance.GetComponent<YourAccount>().account;

        //info.username = player.username;
        //info.password = player.password;
        //info.emailAddress = player.emailAddress;
        //info.playerLevel = player.playerLevel;
        //info.playerExp = player.playerExp;
        //info.coins = player.coins;

        //GameManager.Instance.GetComponent<YourAccount>().account = info;

        //GameManager.Instance.GetComponent<YourMonsters>().GetYourMonsters();
        //SceneManager.LoadScene("MainMenu");


    }

    //if you're on the account selection screen, press this to go back and create a new account
    public void OpenCreateAccountBtn()
    {
        createCanvas.gameObject.SetActive(true);
        selectAccountCanvas.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}


//*********THIS IS TO CHECK FOR DIRECTORIES IN A PARTICULAR FILE LOCATION***********//
//DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath + "/Saves");
//DirectoryInfo[] info = dir.GetDirectories("*.*");
//int count = dir.GetDirectories().Length;
//for (int i = 0; i < count; i++)
//{
//    Debug.Log("Found Directory: " + info[i]);
//}

