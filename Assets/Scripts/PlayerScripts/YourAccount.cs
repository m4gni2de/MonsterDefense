using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct AccountInfo
{
    public string username;
    public string emailAddress;
    public string password;
    public string userId;

    public int playerLevel;
    public int playerExp;
    public int coins;


};
    public class YourAccount : MonoBehaviour
{

    //public AccountInfo account = new AccountInfo();

    // Start is called before the first frame update
    void Start()
    {
        // PlayerPrefs.SetString(username, JsonUtility.ToJson(account));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
