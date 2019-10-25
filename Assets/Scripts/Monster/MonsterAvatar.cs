using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MonsterAvatar : MonoBehaviour
{
    public Monster activeMonster;

    public SpriteRenderer outline, monsterSprite;
    public SpriteMask mask;

    public TMP_Text nameText, levelText;

    public Sprite rankSprite;
    public GameObject[] rankObjects;


    //load a monster in to the icon and display the information
    public void LoadMonster(Monster monster)
    {
        activeMonster = monster;

        nameText.text = activeMonster.name;
        levelText.text = activeMonster.info.level.ToString();
        monsterSprite.sprite = GameManager.Instance.monstersData.monstersAllDict[monster.info.species].frontIcon;

        mask.isCustomRangeActive = true;
        mask.frontSortingLayerID = monsterSprite.sortingLayerID;
        mask.backSortingLayerID = mask.frontSortingLayerID;

        mask.frontSortingOrder = monsterSprite.sortingOrder;
        mask.backSortingOrder = monsterSprite.sortingOrder - 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
