
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
    public Color electricAttackColor, waterAttackColor, natureAttackColor, shadowAttackColor, fireAttackColor;
    public Dictionary<string, Color> typeColorDictionary = new Dictionary<string, Color>();

    //dictionary that holds the values for the amount of EXP a tile needs to reach the next level
    public Dictionary<int, int> tileLevelUp = new Dictionary<int, int>();
    public int tileMaxLevel;
    public List<MapTile> tilesMining = new List<MapTile>();


    public List<int> expCurves = new List<int>();
    public Dictionary<int, int> expCurveDicts = new Dictionary<int, int>();

    //everything to do with the current Towers on the field
    public Dictionary<int, Monster> activeTowers = new Dictionary<int, Monster>();

    //everything to do with the current Tiles on the field
    public Dictionary<int, MapTile> activeTiles = new Dictionary<int, MapTile>();
    //Dictionary to keep track of notifications that the player has not yet seen/cleared, as well as their corresponding notification object
    public Dictionary<Notification, GameObject> activeNotificationsDict = new Dictionary<Notification, GameObject>();

    //public GameObject notificationObject;
    public GameObject touchIndicator;
    

    //these varibales will be used to store your monster data offline as playerprefs. monsterCount will be a 3 digit number that corresponds to the number of monsters a player has
    public int monsterCount;
    public string monsterJson;

    //the rate at which you generation coins
    public float coinGeneration;
    //public Dictionary<int, string> yourMonstersDict = new Dictionary<int, string>();

    //bool to tell if the player is in a match or not
    public bool inGame;
    //information about the active map
    public MapDetails activeMap;

    //the camera that renders the canvas
    public Camera canvasCamera;
    //canvas that displays the same menus, regardless of the screen
    public Canvas overworldCanvas;
    //an object to act as the template for notifications for the player
    public GameObject notificationObject;
    //the scrollbar that holds all of the notifications
    public GameObject notificationScroll;
    //the content window of the notifications Scroll
    public GameObject notificationContent;
    //create the instance of the GameManager to be used throughout the game
    void Awake()
    {

        if (instance == null) instance = this;


        //PlayerPrefs.DeleteAll();

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);

        }


        electricAttackColor = new Color(1.0f, 1.0f, 0.0f, 1.0f);
        waterAttackColor = new Color(0.22f, 0.22f, 1.0f, 1.0f);
        natureAttackColor = new Color(0f, 0.85f, .21f, 1.0f);
        shadowAttackColor = new Color(.36f, 0f, .37f, 1.0f);
        fireAttackColor = new Color(.98f, .20f, 0f, 1.0f);

        typeColorDictionary.Add("Electric", electricAttackColor);
        typeColorDictionary.Add("Water", waterAttackColor);
        typeColorDictionary.Add("Nature", natureAttackColor);
        typeColorDictionary.Add("Shadow", shadowAttackColor);
        typeColorDictionary.Add("Fire", fireAttackColor);


        if (PlayerPrefs.HasKey("MonsterCount"))
        {
            monsterCount = PlayerPrefs.GetInt("MonsterCount");
            PlayerPrefs.SetInt("MonsterCount", monsterCount);

            Debug.Log(monsterCount);
        }
        else
        {
            monsterCount = PlayerPrefs.GetInt("MonsterCount", 0);
            PlayerPrefs.SetInt("MonsterCount", monsterCount);
        }


        


        tileLevelUp.Add(1, 0);
        tileLevelUp.Add(2, 40);
        tileLevelUp.Add(3, 130);
        tileLevelUp.Add(4, 290);
        tileLevelUp.Add(5, 540);
        tileLevelUp.Add(6, 900);
        tileLevelUp.Add(7, 1390);
        tileLevelUp.Add(8, 2030);
        tileLevelUp.Add(9, 2840);
        tileLevelUp.Add(10, 3840);
        tileLevelUp.Add(11, 5050);

        //make the max level of a tile equal to the number of entries in the dictionary
        tileMaxLevel = tileLevelUp.Count;


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
        TouchIndicator();

        //foreach(KeyValuePair<int, MapTile> tile in activeTiles)
        //{
        //    if (tile.Value.info.attribute == "Ice")
        //    {
        //        Debug.Log(tile.Value.tileNumber);
        //    }
        //}

       
        
    }


    public void TouchIndicator()
    {
        //these are the same thing, but I am using GetMouse for Unity/WebGL and the Touch for mobile
        if (Input.GetMouseButtonDown(0) || Input.touchCount == 1)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            var x = Instantiate(touchIndicator, transform, false);
            x.transform.position = new Vector3(mousePos.x, mousePos.y, -2f);
          
        }
    }

    //use this to load a notification for the player
    public void SendNotificationToPlayer(string target, int quantity, NotificationType type)
    {
        Notification notify = new Notification();

        notify.id = 1;
        notify.type = type;
        notify.target = target;
        notify.targetQuantity = quantity;

        

        var n = Instantiate(notificationObject, notificationObject.transform.position, Quaternion.identity);
        
        




        n.GetComponent<NotificationObject>().Notification(notify);
        activeNotificationsDict.Add(notify, n);
        n.transform.SetParent(notificationContent.transform, false);
        n.transform.position = new Vector3(notificationObject.transform.position.x, (notificationObject.transform.position.y - ((n.GetComponent<RectTransform>().rect.height * notificationScroll.transform.localScale.y) * (activeNotificationsDict.Count - 1))), -2f);
        
    }



    public void FreezeCameraMotion()
    {
        CameraMotion.Instance.isFree = false;
    }
    
}




////PlayerPrefs/////
///Items: Int[Itemname, quantity]
///Monsters: String[Monster's Index as a String, monster info Json]////
///Account: String[account name, account info Json]////
///


//different types of notifications, so the notification popup object knows what to do
public enum NotificationType
{
    LevelUp,
    ItemGet,

}
//a class for all notifications that the player will receieve while in game
public class Notification
{
    //ID for the notification so it can be logged
    public int id;
    public NotificationType type;
    public string target;
    public int targetQuantity;

}
