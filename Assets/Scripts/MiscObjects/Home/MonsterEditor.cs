using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterEditor : MonoBehaviour
{
    private Monster activeMonster;
    private int attackNumber;

    public TMP_Dropdown attackSelector;

    public TMP_Text newRange, newPower;

    public Button[] objectButtons;
    public GameObject monsterInfoPanel;

    // Start is called before the first frame update
    void Start()
    {
        Button[] buttons = GameObject.FindObjectsOfType<Button>();
        attackSelector.GetComponent<TMP_Dropdown>();
        
        

        for (int i = 0; i < buttons.Length; i++)
        {
           
            buttons[i].interactable = false;
        }

        for (int b = 0; b < objectButtons.Length; b++)
        {
            objectButtons[b].interactable = true;
        }

        
    }

    public void LoadMonster(Monster m, int atkNumber)
    {
        attackSelector.ClearOptions();


        activeMonster = m;
        attackNumber = atkNumber;
        string activeAttack = "";

        if (atkNumber == 1)
        {
            activeAttack = activeMonster.info.attack1Name;
        }
        else if (atkNumber == 2)
        {
            activeAttack = activeMonster.info.attack2Name;
        }

        

        var monsters = GameManager.Instance.monstersData.monstersAllDict;
        var attacks = GameManager.Instance.baseAttacks.attackDict;
        

        List<string> attackList = new List<string>();

        //cycle through the monster's attacks that it can learn, then add all of those attacks, except the ones it already knows, to the dropdown list
        foreach(string attackName in monsters[activeMonster.info.species].baseAttacks)
        {
            if (activeMonster.info.attack1Name != attackName && activeMonster.info.attack2Name != attackName)
            {
                attackList.Add(attackName);
            }
        }

        attackSelector.AddOptions(attackList);

        DisplayAttackStats();


    }

    public void ConfirmButton()
    {
        if (attackNumber == 1)
        {
            activeMonster.info.attack1Name = attackSelector.options[attackSelector.value].text;
        }
        else if (attackNumber == 2)
        {
            activeMonster.info.attack2Name = attackSelector.options[attackSelector.value].text;
        }

        activeMonster.AttackData();
        PlayerPrefs.SetString(activeMonster.info.index.ToString(), JsonUtility.ToJson(activeMonster.info));
        GameManager.Instance.GetComponent<YourMonsters>().GetYourMonsters();

        Button[] buttons = GameObject.FindObjectsOfType<Button>();

        for (int i = 0; i < buttons.Length; i++)
        {

            buttons[i].interactable = true;
        }

        
        //monsterInfoPanel.GetComponent<MonsterInfoPanel>().RefreshMonsterInfo(activeMonster);
        monsterInfoPanel.GetComponent<MonsterInfoPanel>().LoadInfo(activeMonster);
        //GetComponentInParent<YourHome>().LoadMonsters();
        gameObject.SetActive(false);

    }

    public void OnEnable()
    {
        Button[] buttons = GameObject.FindObjectsOfType<Button>();

        for (int i = 0; i < buttons.Length; i++)
        {

            buttons[i].interactable = false;
        }

        for (int b = 0; b < objectButtons.Length; b++)
        {
            objectButtons[b].interactable = true;
        }
    }

    public void OnDisable()
    {
        Button[] buttons = GameObject.FindObjectsOfType<Button>();

        for (int i = 0; i < buttons.Length; i++)
        {

            buttons[i].interactable = true;
        }
    }

    public void DisplayAttackStats()
    {
        var attacks = GameManager.Instance.baseAttacks.attackDict;

        if (attacks.ContainsKey(attackSelector.options[attackSelector.value].text)){
            var attack = attacks[attackSelector.options[attackSelector.value].text];

            newPower.text = attack.power.ToString();
            newRange.text = attack.range.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
