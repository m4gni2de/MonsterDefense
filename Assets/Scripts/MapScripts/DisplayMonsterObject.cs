using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayMonsterObject : MonoBehaviour
{
    public Monster Monster;
    public GameObject monsterIcon, equip1, equip2, atk1Outline, atk1Bg, atk2Outline, atk2Bg;
    public GameObject[] rankSprites;
    public SpriteRenderer[] statusSprites;
    public TMP_Text levelText, attackText, koText, nameText, atk1Text, atk2Text, abilityAmmoText;

    public TMP_Text atkLabel, koLabel;

    public Image staminaProgress;
    public ColorFX stamFX;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //when this object is created, load a monster in to it and then display all of the information
    public void DisplayMonster(Monster m)
    {
        Monster = m;

        var data = GameManager.Instance.monstersData.monstersAllDict;
        var equips = GameManager.Instance.items.allEquipsDict;
        var statuses = GameManager.Instance.GetComponent<AllStatusEffects>().allStatusDict;
        var colors = GameManager.Instance.typeColorDictionary;

        Color type1Color = colors[m.info.type1];

        Color atk1Color = colors[m.info.attack1.type];
        Color atk2Color = colors[m.info.attack2.type];

        GetComponent<SpriteRenderer>().color = type1Color;

        levelText.text = m.info.level.ToString();
        attackText.text = Mathf.Round(m.info.Attack.Value).ToString();
        koText.text = m.currentMapKOs.ToString();
        nameText.text = m.info.name;

        monsterIcon.GetComponent<SpriteRenderer>().sprite = m.frontModel.GetComponent<SpriteRenderer>().sprite;

        //show the equipment if this monster has any
        if (equips.ContainsKey(m.info.equip1Name))
        {
            EquipmentScript e = equips[m.info.equip1Name];
            equip1.GetComponent<SpriteRenderer>().sprite = e.sprite;
            e.ActivateItem(e, equip1);
        }
        else
        {
            equip1.GetComponent<SpriteRenderer>().sprite = null;
        }

        if (equips.ContainsKey(m.info.equip2Name))
        {
            EquipmentScript e = equips[m.info.equip2Name];
            equip2.GetComponent<SpriteRenderer>().sprite = e.sprite;
            e.ActivateItem(e, equip2);

        }
        else
        {
            equip2.GetComponent<SpriteRenderer>().sprite = null;
        }

        //if there are no statuses, don't show anything in the status sprites
        if (m.statuses.Count == 0)
        {
            for (int s = 0; s < statusSprites.Length; s++)
            {
                statusSprites[s].sprite = null;
            }
        }
        else
        {

            for (int i = 0; i < m.statuses.Count; i++)
            {
                statusSprites[i].sprite = m.statuses[0].statusSprite;
            }
        }

        //show the rank this monster is with rank stars equal to its rank
        for(int r = 0; r < m.info.monsterRank; r++)
        {
            rankSprites[r].SetActive(true);
        }

        //set the attack graphics

        atk1Text.text = m.info.attack1Name;
        atk1Bg.GetComponent<SpriteRenderer>().color = atk1Color;
        atk2Bg.GetComponent<SpriteRenderer>().color = atk2Color;
        atk2Text.text = m.info.attack2Name;

        if (m.GetComponent<Tower>().attackNumber == 1)
        {
            atk1Outline.GetComponent<GoldFX>().enabled = true;
            atk2Outline.GetComponent<GoldFX>().enabled = false;
        }
        else
        {
            atk1Outline.GetComponent<GoldFX>().enabled = false;
            atk2Outline.GetComponent<GoldFX>().enabled = true;
        }


        //set the ability graphics
        abilityAmmoText.text = (m.info.specialAbility.castingAmmo - m.info.specialAbility.castingCount).ToString();
        //lowerbound value for this fill is .23 while the upperbound is .86, thus these numbers
        staminaProgress.fillAmount = .23f + (.63f * m.GetComponent<Tower>().staminaBar.BarProgress);

        if (staminaProgress.fillAmount >= .86f)
        {
            stamFX._Color = Color.green;
        }
        else if (staminaProgress.fillAmount >= .73f)
        {
            stamFX._Color = Color.yellow;
        }
        else if (staminaProgress.fillAmount >= .63f)
        {
            stamFX._Color = Color.magenta;
        }
        else if (staminaProgress.fillAmount >= .49f)
        {
            stamFX._Color = Color.red;
        }
        else if (staminaProgress.fillAmount >= .39f)
        {
            stamFX._Color = Color.cyan;
        }
        else if (staminaProgress.fillAmount >= .28f)
        {
            stamFX._Color = Color.blue;
        }

    }


    //when displaying enemies, use this method
    public void EnemyDisplay(Monster m)
    {
        if (m)
        {
            Monster = m;
            Enemy enemy = m.GetComponent<Enemy>();

            var data = GameManager.Instance.monstersData.monstersAllDict;
            var equips = GameManager.Instance.items.allEquipsDict;
            var statuses = GameManager.Instance.GetComponent<AllStatusEffects>().allStatusDict;
            var colors = GameManager.Instance.typeColorDictionary;

            Color type1Color = colors[enemy.monster.info.type1];
            Color type2Color = colors[enemy.monster.info.type2];

            levelText.text = enemy.monster.info.level.ToString();

            atkLabel.text = "Def";
            attackText.text = Mathf.Round(enemy.monster.info.Defense.Value).ToString();
            koLabel.text = "Spe";
            koText.text = Mathf.Round(enemy.monster.info.Speed.Value).ToString();

            nameText.text = enemy.monster.info.species;

            monsterIcon.GetComponent<SpriteRenderer>().sprite = m.frontModel.GetComponent<SpriteRenderer>().sprite;

            GetComponent<SpriteRenderer>().color = type1Color;


            //if there are no statuses, don't show anything in the status sprites
            if (m.statuses.Count == 0)
            {
                for (int s = 0; s < statusSprites.Length; s++)
                {
                    statusSprites[s].sprite = null;
                }
            }
            else
            {

                for (int i = 0; i < m.statuses.Count; i++)
                {
                    statusSprites[i].sprite = m.statuses[0].statusSprite;
                }
            }

            atk1Text.text = "";
            atk1Bg.GetComponent<SpriteRenderer>().color = Color.clear;
            atk2Bg.GetComponent<SpriteRenderer>().color = Color.clear;
            atk2Text.text = "";
            
            atk1Outline.GetComponent<GoldFX>().enabled = false;
            atk2Outline.GetComponent<GoldFX>().enabled = false;
            atk1Outline.SetActive(false);
            atk2Outline.SetActive(false);

            abilityAmmoText.text = Mathf.Round(enemy.monster.info.currentHP).ToString();

            staminaProgress.fillAmount = .23f + (.63f * (enemy.monster.info.currentHP / enemy.monster.info.maxHP));

            if (staminaProgress.fillAmount >= .63f)
            {
                stamFX._Color = Color.green;
            }
            else if (staminaProgress.fillAmount >= .49f)
            {
                stamFX._Color = Color.yellow;
            }
            else
            {
                stamFX._Color = Color.red;
            }
           
            

            equip1.GetComponent<SpriteRenderer>().sprite = null;
            equip2.GetComponent<SpriteRenderer>().sprite = null;
        }
    }

    //update this information every second
    public IEnumerator UpdateMonster(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            DisplayMonster(Monster);
        }
    }

    public IEnumerator UpdateEnemy(float time)
    {
        while (true)
        {
            yield return new WaitForSeconds(time);
            EnemyDisplay(Monster);
        }
    }


}
