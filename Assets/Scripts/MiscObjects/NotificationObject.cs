using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class NotificationObject : MonoBehaviour, IPointerDownHandler
{
    public GameObject notifyImageObject;
    public SpriteRenderer notifyImageSp;
    public Image notifyImage;
    public TMP_Text notifyText;
    public Notification notification;
    private GameObject item;
    

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
                var equips = GameManager.Instance.items.allEquipsDict;
                var consumables = GameManager.Instance.items.allConsumablesDict;
                var inventory = GameManager.Instance.GetComponent<YourItems>().yourInventory;
                var monsters = GameManager.Instance.monstersData.monstersAllDict;
                var weather = GameManager.Instance.GetComponent<Weather>().allWeatherDict;


            if (equips.ContainsKey(target))
                {
                    notifyImageSp.enabled = false;
                    notifyImage.color = Color.white;
                    notifyImage.sprite = equips[target].sprite;
                }

                if (consumables.ContainsKey(target))
                {
                    notifyImageSp.enabled = false;
                    notifyImage.color = Color.white;
                    notifyImage.sprite = consumables[target].sprite;
                }


            if (Notify.type == NotificationType.MonsterDrop)
            {
                notifyText.text = "Enemy " +  Notify.gotFrom + " dropped " +  Notify.targetQuantity + " " + target + "!";
                
            }

            if (Notify.type == NotificationType.TileMine)
            {
                notifyText.text = "You mined " + Notify.targetQuantity + " " + target + " from " + Notify.gotFrom + "!";
            }

            if (Notify.type == NotificationType.AbilityReady)
            {
                notifyText.text = Notify.gotFrom + "'s Ability " + target + " is ready!";
                notifyImageSp.enabled = false;
                notifyImage.color = Color.white;
                notifyImage.sprite = monsters[Notify.gotFrom].frontIcon;
            }

            if (Notify.type == NotificationType.LevelUp)
            {
                notifyText.text = Notify.target + " has Leveled Up to Level " + Notify.targetQuantity;
                notifyImageSp.enabled = false;
                notifyImage.color = Color.white;
                notifyImage.sprite = monsters[Notify.target].frontIcon;
            }

            if (Notify.type == NotificationType.TowerSummon)
            {
                notifyText.text = Notify.target + " has been summoned as an ally on Tile " + Notify.gotFrom + "!";
                notifyImageSp.enabled = false;
                notifyImage.color = Color.white;
                notifyImage.sprite = monsters[Notify.target].frontIcon;
            }

            if (Notify.type == NotificationType.WeatherChange)
            {

                notifyText.text = Notify.target + Notify.gotFrom;
                notifyImageSp.enabled = false;
                notifyImage.color = Color.white;
                notifyImage.sprite = weather[Notify.target].sprite;

            }
        }
    }

    //use this to remove the object with an animation
    public IEnumerator ClearObject()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 999;
        gameObject.AddComponent<NewTeleportation2>();
        GetComponent<Image>().color = Color.clear;
        notifyText.text = "";
        notifyImage.sprite = null;
        gameObject.GetComponent<Image>().raycastTarget = false;

        if (item != null)
        {
            Destroy(item.gameObject);
        }
        var t = GetComponent<NewTeleportation2>();

        transform.SetParent(GetComponentInParent<Canvas>().transform, true);
        


        t._Fade = 0f;
        t._Distortion = 1;
        t._Alpha = 1f;

        for (int i = 0; i < 50; i++)
        {
            t._Fade += (1f / 30f);
            t._Distortion += (1f / 50f);
            t._Alpha -= (1f / 50f);

            //transform.localScale = transform.localScale / 1.05f;
           
            transform.Translate(Vector3.right * 2, Space.World);
            

            if (t._Fade >= .95)
            {
                
                Destroy(gameObject);

                break;
            }

            yield return new WaitForSeconds(.0005f);
        }
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
