using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//this script is used to display pop menu boxes 
public class PopMenuObject : MonoBehaviour
{
    public TMP_Text menuText, titleText;

    

    // Start is called before the first frame update
    void Start()
    {
        menuText.GetComponent<TMP_Text>();
        titleText.GetComponent<TMP_Text>();

       

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void FillBox(string desc)
    {
        
        menuText.text = desc;
    }

    //accepts an object and then displays the information about that object based on the object type
    public void AcceptObject(string name, object obj)
    {
        var items = GameManager.Instance.items.allItemsDict;
        var types = GameManager.Instance.monstersData.typeChartDict;
        var effects = GameManager.Instance.GetComponent<AllStatusEffects>().allStatusDict;
        var atkModes = GameManager.Instance.GetComponent<Attacks>().atkModeDict;

        //checks to see if the object brought over was an item
        if (items.ContainsKey(name))
        {
            Item(name);
        }

        //checks to see if the object brought over was a type
        if (types.ContainsKey(name))
        {
            Type(name);
        }

        if (effects.ContainsKey(name))
        {
            StatusEffect(name);
        }

        if (atkModes.ContainsKey(name))
        {
            AttackMode(name);
        }


    }

    //use this method for an item
    public void Item(string name)
    {
        var items = GameManager.Instance.items.allItemsDict;
        var equips = GameManager.Instance.items.allEquipmentDict;


        if (items[name].itemType == ItemType.Equipment)
        {
            if (equips.ContainsKey(name))
            {
                titleText.text = equips[name].name;
                menuText.text = equips[name].description;
            }
        }
    }

    //use this method for an item
    public void Type(string name)
    {
        var types = GameManager.Instance.monstersData.typeChartDict;

        titleText.text = "Type";
        menuText.text = types[name].name;

    }

    public void StatusEffect(string name)
    {
        var effects = GameManager.Instance.GetComponent<AllStatusEffects>().allStatusDict;

        titleText.text = name;
        menuText.text = effects[name].description;
    }

    public void AttackMode(string name)
    {
        var atkModes = GameManager.Instance.GetComponent<Attacks>().atkModeDict;

        titleText.text = "Attack Mode";
        menuText.text = name;
    }


}
