using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SummonAnimation : MonoBehaviour
{
    //animation for the summon
    public NewTeleportation teleport;
    public SpriteRenderer sp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSummon(Monster m)
    {
        var monsters = GameManager.Instance.monstersData.monstersAllDict;
        var types = GameManager.Instance.typeColorDictionary;

       
        gameObject.GetComponent<SpriteRenderer>().sprite = m.frontModel.GetComponent<SpriteRenderer>().sprite;
        

        Color c1 = types[m.info.type1];
        Color c2 = types[m.info.type1];

        if (m.info.type2 != "none")
        {
           c2 = types[m.info.type2];
        }

        teleport.TeleportationColor = c1;

        StartCoroutine(Fade(m, c1, c2));

        //gameObject.transform.localScale = new Vector3()
    }

    public IEnumerator Fade(Monster m, Color c1, Color c2)
    {
        for (int i = 100; 1 > 0; i--)
        {
            teleport._Fade -= .01f;

            if (teleport.TeleportationColor == c1)
            {
                teleport.TeleportationColor = c2;
            }
            else
            {
                teleport.TeleportationColor = c1;
            }

            gameObject.transform.localScale = new Vector2(transform.localScale.x - .17f, transform.localScale.y - .17f);

            yield return new WaitForSeconds(.005f);

            if (i < 1)
            {
                m.GetComponent<Tower>().summonAnimationComplete = true;
                Destroy(gameObject);
                
            }

        }
    }
}
