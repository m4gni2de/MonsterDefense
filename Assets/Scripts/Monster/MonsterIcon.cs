using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonsterIcon : MonoBehaviour
{
    public TMP_Text nameText, levelText;
    public Canvas canvas;
    

    // Start is called before the first frame update
    void Start()
    {
        nameText.GetComponent<TMP_Text>();
        levelText.GetComponent<TMP_Text>();
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
