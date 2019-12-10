using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //mark the player as not in a game every time the main menu opens
        GameManager.Instance.inGame = false;

        GameManager.Instance.GetComponent<YourMonsters>().GetYourMonsters();

        //GameManager.Instance.eventTriggers.Clear();
        //GameManager.Instance.eventTriggerCount = 0;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EditorButton()
    {
        SceneManager.LoadScene("MapEditor");
    }

    public void WorldMapButton()
    {
        SceneManager.LoadScene("WorldMap");
        GameManager.Instance.gameMode = GameMode.NormalMode;
    }

    public void YourHomeButton()
    {
        SceneManager.LoadScene("YourHome");
    }

    public void ItemShop()
    {
        SceneManager.LoadScene("ItemShop");
    }


    public void YourDefenses()
    {
        SceneManager.LoadScene("Defenders");
        GameManager.Instance.gameMode = GameMode.DefenderMode;
    }
}
