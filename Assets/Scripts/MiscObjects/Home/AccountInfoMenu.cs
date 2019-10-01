using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AccountInfoMenu : MonoBehaviour
{
    public TMP_Text nameText, idText, playTimeText, monstersCollectedText, levelText, expToLevelText, currentCoinsText, coinGenText;
    public Slider expSlider;

    public Button xBtn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadAccountInfo()
    {
        var accountInfo = GameManager.Instance.GetComponent<YourAccount>().account;

        //make all other buttons unable to be pushed
        Button[] buttons = FindObjectsOfType<Button>();

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }

        //make this menu's x button pushable
        xBtn.interactable = true;

        
        
        float hours = Mathf.Round(accountInfo.playTime / 3600);
        float minutes = Mathf.Round((accountInfo.playTime % 3600) / 60);
        float seconds = Mathf.Round((accountInfo.playTime % 3600) % 60);

        

        expSlider.GetComponent<Slider>();

        
        nameText.text = accountInfo.username;
        idText.text = "ID: " + accountInfo.userId;
        playTimeText.text = hours + ":" + minutes + ":" + seconds;
        monstersCollectedText.text = "Monsters Collected: " + accountInfo.totalMonstersCollected;
        levelText.text = "Level: " + accountInfo.playerLevel;
        expToLevelText.text = "EXP to Next Level: ";
        currentCoinsText.text = ((int)accountInfo.coins).ToString();
        coinGenText.text = "Generating: " + GameManager.Instance.coinGeneration + " /h";

    }


    public void CloseMenu()
    {
        //make all other buttons pushable before closing
        Button[] buttons = FindObjectsOfType<Button>();

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = true;
        }

        gameObject.SetActive(false);
    }
}
