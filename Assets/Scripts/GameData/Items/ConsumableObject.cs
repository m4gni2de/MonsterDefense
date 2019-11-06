using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableObject : MonoBehaviour
{
    public ConsumableItem consumableItem;

    public Image image;
    //public SpriteRenderer sp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //load this item's consumable item
    public void LoadItem()
    {
        image.sprite = consumableItem.sprite;
        gameObject.name = consumableItem.itemName;
        //image.material = sp.material;
        //sp.enabled = false;
    }
}
