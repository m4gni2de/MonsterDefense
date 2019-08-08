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
    }

    public void YourHomeButton()
    {
        SceneManager.LoadScene("YourHome");
    }

    public void ItemShop()
    {
        SceneManager.LoadScene("ItemShop");
    }
}
