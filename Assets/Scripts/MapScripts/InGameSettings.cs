using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameSettings : MonoBehaviour
{
    //the main control button for the settings during the game
    public Button mainButton;
    public Sprite plusButton, minusButton;

    //an object containing the buttons that stem from pushing the main button
    public GameObject subButtons;

    // Start is called before the first frame update
    void Start()
    {
        mainButton.image.sprite = plusButton;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //change the look of the options button every time it's pushed
    public void ChangeOptionsBtn()
    {
        //Debug.Log(mainButton.image.sprite);


        if (mainButton.image.sprite == plusButton)
        {
            subButtons.SetActive(true);
            mainButton.image.sprite = minusButton;

            return;
        }

        if (mainButton.image.sprite == minusButton)
        {
            
            subButtons.SetActive(false);
            mainButton.image.sprite = plusButton;

            return;
        }
    }
}
