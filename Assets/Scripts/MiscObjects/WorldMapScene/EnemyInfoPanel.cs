using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class EnemyInfoPanel : MonoBehaviour
{
    public TMP_Text nameText, typeText, levelText, hpText, defText, speText, evaText;
    public Enemy activeEnemy;
    public GameObject enemyInfoMenu, monsterInfoMenu;
    public Slider hpSlider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

   


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);


            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "Enemy")
                {

                    if (activeEnemy)
                    {
                        activeEnemy.ToggleActiveStatus();
                        activeEnemy = hit.collider.gameObject.GetComponent<Enemy>();
                        activeEnemy.ToggleActiveStatus();
                    }
                    else
                    {
                        activeEnemy = hit.collider.gameObject.GetComponent<Enemy>();
                        activeEnemy.ToggleActiveStatus();
                    }
                    
                    

                    
                    enemyInfoMenu.SetActive(true);
                    monsterInfoMenu.SetActive(false);

                }
            }
        }

        if (activeEnemy)
        {
            hpSlider.maxValue = activeEnemy.stats.hpMax;
            hpSlider.value = activeEnemy.stats.currentHp;

            nameText.text = activeEnemy.stats.name;
            typeText.text = activeEnemy.stats.type1 + "/" + activeEnemy.stats.type2;
            levelText.text = "Level: " + activeEnemy.stats.level;
            hpText.text = activeEnemy.stats.currentHp + "/" + activeEnemy.stats.hpMax;
            defText.text = "Defense: " + activeEnemy.stats.def;
            speText.text = "Speed: " + activeEnemy.stats.speed;
            evaText.text = "Evasion: " + activeEnemy.stats.evasion;
        }
    }
}
