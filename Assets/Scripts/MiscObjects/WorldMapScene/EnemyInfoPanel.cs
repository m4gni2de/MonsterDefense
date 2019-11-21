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
    public GameObject[] statusSprites;

    //bool to determine if the camera is in a mode where it's following an enemy or not
    public bool isFollowing;

    //the main camera of the map
    public Camera mainCamera;

    //pop menu that is designed to display overlayed information
    public GameObject popMenuObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

   


    // Update is called once per frame
    void Update()
    {
        if (isFollowing == true)
        {
            mainCamera.transform.position = new Vector3(activeEnemy.transform.position.x, activeEnemy.transform.position.y, -10f);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            isFollowing = false;

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
                    //EnemyStatuses(activeEnemy);

                }
            }
        }

        if (activeEnemy)
        {
            
            hpSlider.maxValue = activeEnemy.monster.info.maxHP;
            hpSlider.value = activeEnemy.monster.info.currentHP;
            nameText.text = activeEnemy.monster.info.name;
            typeText.text = activeEnemy.monster.info.type1 + "/" + activeEnemy.monster.info.type2;
            levelText.text = "Level: " + activeEnemy.monster.info.level;
            hpText.text = System.Math.Round(activeEnemy.monster.info.currentHP, 0) + "/" + activeEnemy.monster.info.maxHP;
            defText.text = "Defense: " + System.Math.Round(activeEnemy.monster.info.Defense.Value, 0);
            //defText.text = "Defense: " + System.Math.Round(activeEnemy.monster.info.def, 0);
            //speText.text = "Speed: " + System.Math.Round(activeEnemy.monster.info.speed, 0);
            speText.text = "Speed: " + System.Math.Round(activeEnemy.monster.info.Speed.Value, 0);
            evaText.text = "Evasion: " + System.Math.Round(activeEnemy.monster.info.evasionBase, 2) + "%";


                //for (int i = 0; i < activeEnemy.GetComponent<Monster>().statuses.Count; i++)
                for (int i = 0; i < statusSprites.Length; i++)
                {
                    statusSprites[i].GetComponent<SpriteRenderer>().sprite = activeEnemy.GetComponent<Monster>().statusIcons[i].GetComponent<SpriteRenderer>().sprite;
                    statusSprites[i].transform.localScale = new Vector3(7, 3.5f, statusSprites[i].transform.localScale.z);
                }

            




        }
    }


    ////use this to set the enemy status icons without having to loop every second. Call this to change the status icons as well
    //public void EnemyStatuses(Enemy enemy)
    //{
    //    for (int i = 0; i < activeEnemy.GetComponent<Monster>().statuses.Count; i++)
    //    {
    //        if (GameManager.Instance.GetComponent<AllStatusEffects>().allStatusDict.ContainsKey(activeEnemy.GetComponent<Monster>().statuses[i].name))
    //        {
    //            statusSprites[i].GetComponent<SpriteRenderer>().sprite = GameManager.Instance.GetComponent<AllStatusEffects>().allStatusDict[activeEnemy.GetComponent<Monster>().statuses[i].name].statusSprite;
    //            statusSprites[i].transform.localScale = new Vector3(8, 6, statusSprites[i].transform.localScale.z);
    //        }


    //    }
    //}

    public void FollowEnemyBtn()
    {
        isFollowing = true;
        mainCamera.orthographicSize = 35;
        mainCamera.transform.position = new Vector3(activeEnemy.transform.position.x, activeEnemy.transform.position.y, -10f);
    }
}
