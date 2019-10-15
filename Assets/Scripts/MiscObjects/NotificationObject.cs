using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotificationObject : MonoBehaviour
{
    public GameObject notifyImage;
    public SpriteRenderer notifyImageSp;
    public TMP_Text notifyText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Notification(Notification Notify)
    {
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
                item.transform.SetParent(transform, false);
                item.transform.position = notifyImage.transform.position;
            }

            if (consumables.ContainsKey(target))
            {
                notifyImageSp.sprite = consumables[target].sprite;
            }


            Debug.Log(itemAmount);
            notifyText.text = "You acquired " + Notify.targetQuantity + " " + target + "! You now have " + itemAmount + " of these!";

        }
    }
}
