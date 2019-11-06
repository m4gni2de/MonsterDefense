using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//this script is used to display pop menu boxes 
public class PopMenuObject : MonoBehaviour
{
    public TMP_Text menuText, titleText, secondaryText;

    //set an active monster to this object so that it can display information about that monster
    public Monster activeMonster;
    

    // Start is called before the first frame update
    void Start()
    {
        menuText.GetComponent<TMP_Text>();
        titleText.GetComponent<TMP_Text>();
        secondaryText.GetComponent<TMP_Text>();

       

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void FillBox(string desc)
    {
        
        menuText.text = desc;
    }

    //use this to display equipment information
    public void AcceptEquipment(EquipmentScript equip)
    {
        titleText.text = equip.itemName;
        menuText.text = equip.description;
        secondaryText.text = "";
    }

    //use this to display status information
    public void AcceptStatus(Status status)
    {
        titleText.text = status.name;
        menuText.text = status.description;
        secondaryText.text = "";
    }

    //use this to display status information
    public void AcceptType(TypeInfo type)
    {
        titleText.text = "Type";
        menuText.text = type.name;
        secondaryText.text = "";
    }

    //use this to display status information
    public void AcceptAttackMode(string mode)
    {
        titleText.text = "Attack Mode";
        menuText.text = mode;
        secondaryText.text = "";
    }




    


}
