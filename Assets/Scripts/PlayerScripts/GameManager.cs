
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Reflection;


public class GameManager : MonoBehaviour
{
    // create class as singleton
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    public void OnDestroy() { if (instance == this) instance = null; }

    //the script that holds all of the basic attacks for the monsters
    public Attacks baseAttacks;
    //blank tile used to create temporary tiles
    public MapTile blankTile;

    //the script that holds all of the base stats, level up movepool and everything else for the monster species
    public MonstersData monstersData;

    //the script that holds all of the data for the items in the game
    public Items items;

    //everything to do with the colors
    public Color electricAttackColor, waterAttackColor, natureAttackColor, shadowAttackColor, fireAttackColor, iceAttackColor, mechAttackColor, magicAttackColor, normalAttackColor, poisonAttackColor;
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
    //list for pending notifications to go when there are too many to show the player at once
    public List<Notification> pendingNotificationsList = new List<Notification>();


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
    //the symbol used to notify the player of a new notification
    public GameObject notifier;

    //the current active scene in the game
    public Scene activeScene;

    //gameobject that acts as a quick popmenu for displaying information to the player
    public GameObject popMenu;

    //object that exists to preempt a monster summon with an animation
    public GameObject summonAnimation;


    //sprites used for the pause/play button
    public Sprite pauseSprite, playSprite;

    //the game's audio manager
    public AudioManager AudioManager;

    //create the instance of the GameManager to be used throughout the game
    void Awake()
    {

        if (instance == null) instance = this;


        //PlayerPrefs.DeleteAll();

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);

        }


        

        typeColorDictionary.Add("Electric", electricAttackColor);
        typeColorDictionary.Add("Water", waterAttackColor);
        typeColorDictionary.Add("Nature", natureAttackColor);
        typeColorDictionary.Add("Shadow", shadowAttackColor);
        typeColorDictionary.Add("Fire", fireAttackColor);
        typeColorDictionary.Add("Ice", iceAttackColor);
        typeColorDictionary.Add("Mechanical", mechAttackColor);
        typeColorDictionary.Add("Magic", magicAttackColor);
        typeColorDictionary.Add("Normal", normalAttackColor);
        typeColorDictionary.Add("Poison", poisonAttackColor);


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
        SceneManager.activeSceneChanged += OnSceneChange;
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

        //Debug.Log(eventTriggers.Count);

        if (activeNotificationsDict.Count > 0)
        {
            notificationObject.SetActive(false);
        }
        else
        {
            notificationObject.SetActive(true);
        }

        
        //Monster[] monsters = FindObjectsOfType<Monster>();

        //Debug.Log(monsters.Length);

    }


    public void TouchIndicator()
    {
        //these are the same thing, but I am using GetMouse for Unity/WebGL and the Touch for mobile
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

                var x = Instantiate(touchIndicator, transform, false);
                x.transform.position = new Vector3(mousePos.x, mousePos.y, -2f);
            }
          
        }

        //these are the same thing, but I am using GetMouse for Unity/WebGL and the Touch for mobile
        //if (Input.GetMouseButtonDown(0))
        //{  
        //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        //    var x = Instantiate(touchIndicator, transform, false);
        //    x.transform.position = new Vector3(mousePos.x, mousePos.y, -2f);
        //}


    }

    //use this to load a notification for the player
    public void SendNotificationToPlayer(string target, int quantity, NotificationType type, string GotFrom)
    {
        Notification notify = new Notification();
        notify.CreateTrigger(type);

        int id = PlayerPrefs.GetInt("Notifications", 0);

        notify.id = id + 1;
        notify.type = type;
        notify.target = target;
        notify.targetQuantity = quantity;
        notify.gotFrom = GotFrom;

        
        PlayerPrefs.SetInt("Notifications", notify.id);

        

        //if there are less than 10 notifications to be seen, don't spawn them. instead, load them in to a List for pending notifications
        if (activeNotificationsDict.Count <= 9)
        {
            var n = Instantiate(notificationObject, notificationObject.transform.position, Quaternion.identity);


            n.GetComponent<NotificationObject>().Notification(notify);
            activeNotificationsDict.Add(notify, n);
            n.transform.SetParent(notificationContent.transform, false);
            n.transform.position = new Vector3(notificationObject.transform.position.x, (notificationObject.transform.position.y - ((n.GetComponent<RectTransform>().rect.height * (n.transform.localScale.y * overworldCanvas.transform.localScale.y)) * (activeNotificationsDict.Count - 1))), -2f);
        }
        else
        {
            pendingNotificationsList.Add(notify);
        }

        notifier.SetActive(true);

    }

    //this is called from the Notification Object script itself
    public void RemoveNotification(GameObject notifyObject, Notification notify)
    {
        int id = notify.id;
        
        foreach (KeyValuePair<Notification, GameObject> n in activeNotificationsDict)
        {
            //check all of the notifications in the active dictionary. if they have an id larger than the id of the notification being deleted, move them up
            if (n.Key.id > id)
            {
                n.Value.transform.position = new Vector3(n.Value.transform.position.x, (n.Value.transform.position.y + n.Value.GetComponent<RectTransform>().rect.height * (n.Value.transform.localScale.y * overworldCanvas.transform.localScale.y)), -2f);

               
            }
        }

        //remove notification from dictionary of active notifications
        activeNotificationsDict.Remove(notify);
        //destroy the object that was touched
        StartCoroutine(notifyObject.GetComponent<NotificationObject>().ClearObject());
        //Destroy(notifyObject);

        //checks for pending notifications, and if there are any, add the first one to the active notifications dictionary
        if (pendingNotificationsList.Count > 0)
        {
            var n = Instantiate(notificationObject, notificationObject.transform.position, Quaternion.identity);
            n.GetComponent<NotificationObject>().Notification(notify);
            activeNotificationsDict.Add(notify, n);
            n.transform.SetParent(notificationContent.transform, false);

            n.transform.position = new Vector3(notificationObject.transform.position.x, (notificationObject.transform.position.y - ((n.GetComponent<RectTransform>().rect.height * (n.transform.localScale.y * overworldCanvas.transform.localScale.y)) * (activeNotificationsDict.Count - 1))), -2f);

            //remove the added notification from the pending list to the active list
            pendingNotificationsList.Remove(pendingNotificationsList[0]);
            
        }

    }



    public void FreezeCameraMotion()
    {
        if (CameraMotion.Instance)
        {
            CameraMotion.Instance.isFree = false;
        }
    }

    //call this from other objects to add or remove a trigger from the list of triggers
    public void EventTriggerManager(Scene scene)
    {
        

    } 

    //call this from other objects and use it as a Global Event Trigger call
    public void TriggerEvent(TriggerType type)
    {

        foreach (KeyValuePair<int, Monster> monster in GetComponent<YourMonsters>().yourMonstersComplete)
        {
            MonsterInfo m = monster.Value.info;

            if (m.equipment1 != null && m.equipment1.triggerType == type)
            {
                m.equipment1.trigger.equipment = m.equipment1;
                m.equipment1.trigger.ActivateTrigger(m.equipment1.triggerType);
            }

            if (m.equipment2 != null && m.equipment2.triggerType == type)
            {
                m.equipment2.trigger.equipment = m.equipment2;
                m.equipment2.trigger.ActivateTrigger(m.equipment2.triggerType);
            }
        }

    }


    public void MonsterList()
    {
        Monster[] monsters = FindObjectsOfType<Monster>();

        Debug.Log(monsters.Length);
    }


    //this activates every time a scene is changed
    private void OnSceneChange(Scene arg0, Scene arg1)
    {
        //Debug.Log("Test:" + arg0.name + " -> " + arg1.name);

        activeTiles.Clear();

        GameObject camera = GameObject.Find("Main Camera");

        canvasCamera = camera.GetComponent<Camera>();
        Instance.overworldCanvas.worldCamera = GameManager.Instance.canvasCamera;
    }

    //accept an object and display it on the popmenu
    public void DisplayPopMenu(object obj)
    {
        Debug.Log(obj);
        if (obj.GetType() == typeof(EquipmentScript))
        {
            popMenu.SetActive(true);
            popMenu.GetComponent<PopMenuObject>().AcceptEquipment((EquipmentScript)obj);
        }

        if (obj.GetType() == typeof(GameObject))
        {
            GameObject name = (GameObject)obj;
            string n = name.name;
            popMenu.SetActive(true);
            popMenu.GetComponent<PopMenuObject>().AcceptAttackMode(n);
        }

        if (obj.GetType() == typeof(Status))
        {
            popMenu.SetActive(true);
            popMenu.GetComponent<PopMenuObject>().AcceptStatus((Status)obj);
        }

        if (obj.GetType() == typeof(TypeInfo))
        {
            popMenu.SetActive(true);
            popMenu.GetComponent<PopMenuObject>().AcceptType((TypeInfo)obj);
        }


    }

    

    //control the pause/play of the game
    public void PausePlayToggle(Image b)
    {
        if (Time.timeScale >= 1)
        {
            Time.timeScale = 0;
            b.sprite = playSprite;
        }
        else
        {
            Time.timeScale = 1;
            b.sprite = pauseSprite;
        }

        
    }

}




////PlayerPrefs/////
///Items: Int[Itemname, quantity]
///Monsters: String[Monster's Index as a String, monster info Json]////
///Account: String[account name, account info Json]////
///Notifications: Int[Notifications]
///Monster Sort Mode: String["SortMode", SortMode.Mode.ToString()]
///


//different types of notifications, so the notification popup object knows what to do
public enum NotificationType
{
    LevelUp,
    ItemGet,
    MonsterDrop,
    TileMine,
    AbilityReady,
    TowerSummon,

}
//a class for all notifications that the player will receieve while in game
public class Notification
{
    //ID for the notification so it can be logged
    public int id;
    public NotificationType type;
    public string target;
    public int targetQuantity;
    public string gotFrom;

    //used to create an event trigger when a notification happens
    public void CreateTrigger(NotificationType n)
    {
        if (n == NotificationType.ItemGet || n == NotificationType.MonsterDrop)
        {
            //EventTrigger trigger = new EventTrigger(TriggerType.ItemGet);
            GameManager.Instance.TriggerEvent(TriggerType.ItemGet);
        }

        if (n == NotificationType.LevelUp)
        {
            GameManager.Instance.TriggerEvent(TriggerType.LevelUp);
        }

        if (n == NotificationType.TowerSummon)
        {
            GameManager.Instance.TriggerEvent(TriggerType.TowerSummon);
        }


    }

}
