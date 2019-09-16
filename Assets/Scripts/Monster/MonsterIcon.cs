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
    // Start is called before the first frame update
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        image = GetComponent<Image>();

        nameText.GetComponent<TMP_Text>();
        levelText.GetComponent<TMP_Text>();
        //sp.GetComponent<SpriteRenderer>();
        canvas.sortingLayerName = "GameUI";
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            
            nameText.text = GetComponentInParent<Monster>().info.name;
            levelText.text = GetComponentInParent<Monster>().info.level.ToString();  
        }
    }

    


    //these methods are called from other scripts to hide or show the icon in different menus
    public void IconVisibility(string layerName)
    {
        canvas.overrideSorting = true;
        canvas.sortingLayerName = layerName;
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = layerName;
    }

    

    
   
}
