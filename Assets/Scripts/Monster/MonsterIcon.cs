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
        if (gameObject.activeSelf)
        {
            
            nameText.text = GetComponentInParent<Monster>().info.name;
            levelText.text = GetComponentInParent<Monster>().info.level.ToString();

            gameObject.tag = GetComponentInParent<Monster>().gameObject.tag;

            monsterSprite.sprite = GameManager.Instance.monstersData.monstersAllDict[GetComponentInParent<Monster>().info.species].frontIcon;

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


    public void OnEnable()
    {
        if (gameObject.activeSelf)
        {

            for (int i = 0; i < GetComponentInParent<Monster>().saveToken.rank; i++)
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
    }


}
