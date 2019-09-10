
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // create class as singleton
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    public void OnDestroy() { if (instance == this) instance = null; }

    //the script that holds all of the basic attacks for the monsters
    public Attacks baseAttacks;

    //the script that holds all of the base stats, level up movepool and everything else for the monster species
    public MonstersData monstersData;

    //the script that holds all of the data for the items in the game
    public Items items;

    //everything to do with the colors
    public Color electricAttackColor, waterAttackColor;
    public Dictionary<string, Color> typeColorDictionary = new Dictionary<string, Color>();

    //everything to do with the current Towers on the field
    public Dictionary<int, Monster> activeTowers = new Dictionary<int, Monster>();


    

    //these varibales will be used to store your monster data offline as playerprefs. monsterCount will be a 3 digit number that corresponds to the number of monsters a player has
    public int monsterCount;
    public string monsterJson;
    //public Dictionary<int, string> yourMonstersDict = new Dictionary<int, string>();

    //create the instance of the GameManager to be used throughout the game
    void Awake()
    {

        if (instance == null) instance = this;

       

        electricAttackColor = new Color(1.0f, 1.0f, 0.0f, 1.0f);
        waterAttackColor = new Color(0.22f, 0.22f, 1.0f, 1.0f);

        typeColorDictionary.Add("Electric", electricAttackColor);
        typeColorDictionary.Add("Water", waterAttackColor);


        if (PlayerPrefs.HasKey("MonsterCount"))
        {
            monsterCount = PlayerPrefs.GetInt("MonsterCount");
            PlayerPrefs.SetInt("MonsterCount", monsterCount);
        }
        else
        {
            monsterCount = PlayerPrefs.GetInt("MonsterCount", 0);
            PlayerPrefs.SetInt("MonsterCount", monsterCount);
        }


        //PlayerPrefs.DeleteAll();

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);

        }


    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }



    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this);

        

    }

    // Update is called once per frame
    void Update()
    {


    }



    
}




////PlayerPrefs/////
///Items: Int[Itemname, quantity]
///Monsters: String[Monster's Index as a String, monster info Json]////
///Account: String[account name, account info Json]////
