using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemPopMenu : MonoBehaviour
{
    public GameObject itemSprite;

    public TMP_Text nameText, typeText, typeReqText, boostsText, yourQuantityText, buyCostText, sellCostText;

    public Button buyButton, sellButton;

    public string activeItemName;
    public GameObject activeItem;

    //public Items item;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayEquipment(EquipmentItem equip, GameObject obj)
    {
        GameObject[] r = GameObject.FindGameObjectsWithTag("Respawn");
        
        for (int i = 0; i < r.Length; i++)
        {
            Destroy(r[i]);
        }


        GameObject item = Instantiate(equip.equip.equipPrefab, transform.position, Quaternion.identity);
        item.transform.position = itemSprite.transform.position;
        item.transform.localScale = new Vector3(item.transform.localScale.x * 2f, item.transform.localScale.y * 2f, 1f);
        item.GetComponent<SpriteRenderer>().sortingLayerName = "PopMenu";
        item.tag = "Respawn";

        //itemSprite.GetComponent<SpriteRenderer>().sprite = equip.GetComponent<Image>().sprite;
        nameText.text = equip.equip.name;
        typeText.text = equip.equip.equipType.ToString();
        typeReqText.text = "Required Type: " + equip.equip.typeMonsterReq;
        boostsText.text = equip.equip.description;
        yourQuantityText.text = "On hand: " + PlayerPrefs.GetInt(equip.equip.name).ToString();
        buyCostText.text = "Buy For: " + equip.equip.cost.ToString();

        float sellValue = equip.equip.cost * .8f;
        sellCostText.text = "Sell For: " + sellValue;

        activeItemName = equip.equip.name;
        activeItem = obj;
        
    }

    public void BuyItemBtn()
    {

        int itemCount =  PlayerPrefs.GetInt(activeItemName, 0);
        PlayerPrefs.SetInt(activeItemName, itemCount + 1);

        GameManager.Instance.GetComponent<YourItems>().GetYourItems();


        //GetComponentInParent<ItemShop>().DisplayYourItems();
        GetComponentInParent<ItemShop>().UpdateItem();
        gameObject.SetActive(false);

    }

    public void SellItemBtn()
    {
        int itemCount = PlayerPrefs.GetInt(activeItemName, 0);
        PlayerPrefs.SetInt(activeItemName, itemCount - 1);

        GameManager.Instance.GetComponent<YourItems>().GetYourItems();

        //GetComponentInParent<ItemShop>().DisplayYourItems();
        GetComponentInParent<ItemShop>().UpdateItem();
        gameObject.SetActive(false);
    }

    public void OnDisable()
    {
        GameObject[] r = GameObject.FindGameObjectsWithTag("Respawn");

        for (int i = 0; i < r.Length; i++)
        {
            Destroy(r[i]);
        }
    }
}
