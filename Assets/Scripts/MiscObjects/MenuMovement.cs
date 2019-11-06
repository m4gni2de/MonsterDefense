using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float acumTime;
    public bool isHolding;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isHolding)
        {
            var position = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
            //Debug.Log(position);
            var x = position.x;
            var y = position.y;
            transform.position = new Vector3(x, y, transform.position.z);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

        if (eventData.pointerEnter)
        {
            isHolding = true;
        }



    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //if a player holds their finger on an item, equip the item. If they just tap the item, display the item details
        if (isHolding)
        {
            isHolding = false;
        }




    }
}
