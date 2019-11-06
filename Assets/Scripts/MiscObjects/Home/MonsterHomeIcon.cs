using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MonsterHomeIcon : MonoBehaviour
{
    public TMP_Text nameText, levelText, rankText;

    // Start is called before the first frame update
    void Start()
    {
        nameText.GetComponent<TMP_Text>();
        levelText.GetComponent<TMP_Text>();
        rankText.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
