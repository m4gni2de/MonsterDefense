using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class NotificationObject : MonoBehaviour, IPointerDownHandler
{
    public GameObject notifyImage;
    public SpriteRenderer notifyImageSp;
    public TMP_Text notifyText;
    public Notification notification;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //create the notification image and load it on the player's notification bar
    public void Notification(Notification Notify)
    {
        gameObject.SetActive(true);
        gameObject.tag = "Notification";

        notification = Notify;
        string target = Notify.target;

        bool hasKey = PlayerPrefs.HasKey(target);

        if (hasKey || !hasKey)
        {
            int itemAmount = PlayerPrefs.GetInt(target, 0);
            var equips = GameManager.Instance.items.allEquipmentDict;
            var consumables = GameManager.Instance.items.allConsumablesDict;
            var inventory = GameManager.Instance.GetComponent<YourItems>().yourInventory;

            

            if (equips.ContainsKey(target))
            {
                var item = Instantiate(equips[target].equipPrefab, transform.position, Quaternion.identity);
                item.transform.SetParent(transform, true);
                item.transform.position = notifyImage.transform.position;
                item.GetComponent<Image>().raycastTarget = false;
            }

            if (consumables.ContainsKey(target))
            {
                notifyImageSp.sprite = consumables[target].sprite;
            }


            
            notifyText.text = "You acquired " + Notify.targetQuantity + " " + target + "! You now have " + itemAmount + " of these!";

        }
    }

    
    //use this to clear a notification from the player
    public void ClearNotification()
    {

    }


    public void OnPointerDown(PointerEventData eventData)
    {


        if (eventData.pointerEnter)
        {

            
            var tag = eventData.pointerEnter.gameObject.tag;
            var hit = eventData.pointerEnter.gameObject;


            
            if (tag == "Notification")
            {
                
                GameManager.Instance.RemoveNotification(gameObject, notification);
                
                
            }

        }


    }
}
