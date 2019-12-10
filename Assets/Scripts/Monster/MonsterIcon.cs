using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MonsterIcon : MonoBehaviour
{
    public TMP_Text nameText, levelText;
    //public SpriteRenderer sp;
    public Canvas canvas;

    public SpriteRenderer sp;
    public Image image;

    public GameObject[] rankSprite;
    public Sprite rankIconSprite;


    public SpriteRenderer monsterSprite;


    public SpriteRenderer[] renderers;




    public GameObject outline, background, levelOutline;

    private Monster Monster;



    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        image = GetComponent<Image>();

        nameText.GetComponent<TMP_Text>();
        levelText.GetComponent<TMP_Text>();
        //sp.GetComponent<SpriteRenderer>();
        //canvas.sortingLayerName = "GameUI";
    }

    // Update is called once per frame
    void Update()
    {
        //if (gameObject.activeSelf)
        //{
            
        //    nameText.text = GetComponentInParent<Monster>().info.name;
        //    levelText.text = GetComponentInParent<Monster>().info.level.ToString();

        //    gameObject.tag = GetComponentInParent<Monster>().gameObject.tag;

        //    monsterSprite.sprite = GameManager.Instance.monstersData.monstersAllDict[GetComponentInParent<Monster>().info.species].frontIcon;

        //}
    }

    //call this when making the icon visible
    public void DisplayMonster(Monster monster)
    {
        var colors = GameManager.Instance.typeColorDictionary;

        Monster = monster;

        nameText.text = monster.info.name;
        levelText.text = monster.info.level.ToString();

        gameObject.tag = monster.gameObject.tag;

        monsterSprite.sprite = GameManager.Instance.monstersData.monstersAllDict[monster.info.species].frontIcon;


       

        if (colors.ContainsKey(monster.info.type1))
        {
            Color type1Color = colors[monster.info.type1];

            outline.GetComponent<SpriteRenderer>().color = type1Color;
            background.GetComponent<SpriteRenderer>().color = type1Color;
            levelOutline.GetComponent<SpriteRenderer>().color = type1Color;

            if (monster.info.isStar)
            {
                outline.GetComponent<SpriteRenderer>().color = Color.white;
                outline.GetComponent<PlasmaRainbow>().enabled = true;
            }
            //else
            //{
            //    outline.GetComponent<PlasmaRainbow>().enabled = false;
            //}
        }

        //if (colors.ContainsKey(monster.info.type2))
        //{
           
        //}
        

        


        for (int i = 0; i < monster.saveToken.rank; i++)
        {
            //rankSprite[i].GetComponent<Image>().sprite = rankIconSprite;
            //rankSprite[i].GetComponent<Image>().color = Color.white;

            rankSprite[i].GetComponent<SpriteRenderer>().sprite = rankIconSprite;
        }

        foreach (SpriteRenderer s in renderers)
        {
            s.sortingLayerName = canvas.sortingLayerName;
        }
    }

    //these methods are called from other scripts to hide or show the icon in different menus
    public void IconVisibility(string layerName)
    {
        canvas.overrideSorting = true;
        canvas.sortingLayerName = layerName;


        foreach (SpriteRenderer s in renderers)
        {
            s.sortingLayerName = canvas.sortingLayerName;
            //s.sortingOrder += 20;
        }
        //gameObject.GetComponent<SpriteRenderer>().sortingLayerName = layerName;
    }


    //call this from scripts to display the stat that is being sorted 
    public void DisplayCorrectText(SortMode s)
    {
        if (s == SortMode.Attack)
        {
            nameText.text = Mathf.Round(Monster.info.Attack.Value).ToString();
        }

        if (s == SortMode.Defense)
        {
            nameText.text = Mathf.Round(Monster.info.Defense.Value).ToString();
        }

        if (s == SortMode.Evasion)
        {
            nameText.text = Monster.info.evasionBase + "%";
        }

        if (s == SortMode.CoinGen)
        {
            nameText.text = System.Math.Round(Monster.info.CoinGeneration.Value, 2) + "/m";
        }

        if (s == SortMode.Precision)
        {
            nameText.text = Mathf.Round(Monster.info.Precision.Value).ToString();
        }

        if (s == SortMode.EnGen)
        {
            nameText.text = System.Math.Round(Monster.info.EnergyGeneration.Value / 60f, 2) + " /s";
        }

        if (s == SortMode.Speed)
        {
            nameText.text = Mathf.Round(Monster.info.Speed.Value).ToString();
        }

        if (s == SortMode.Cost)
        {
            nameText.text = Mathf.Round(Monster.info.EnergyCost.Value).ToString();
        }

        if (s == SortMode.KOCount)
        {
            nameText.text = Mathf.Round(Monster.info.koCount).ToString();
        }

        if (s == SortMode.Stamina)
        {
            nameText.text = Mathf.Round(Monster.info.Stamina.Value).ToString();
        }
        if (s == SortMode.Critical)
        {
            nameText.text = System.Math.Round(Monster.info.critBase, 2) + "%";
        }

        //foreach(StatModifier mod in Monster.st)
    }


    public void OnEnable()
    {
        //if (gameObject.activeSelf)
        //{

        //    for (int i = 0; i < GetComponentInParent<Monster>().saveToken.rank; i++)
        //    {
        //        //rankSprite[i].GetComponent<Image>().sprite = rankIconSprite;
        //        //rankSprite[i].GetComponent<Image>().color = Color.white;

        //        rankSprite[i].GetComponent<SpriteRenderer>().sprite = rankIconSprite;
        //    }

        //    foreach (SpriteRenderer s in renderers)
        //    {
        //        s.sortingLayerName = canvas.sortingLayerName;
        //    }
        //}
    }


}
