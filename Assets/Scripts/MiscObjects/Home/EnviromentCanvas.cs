using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnviromentCanvas : MonoBehaviour
{
    public GameObject[] background;

    public GameObject backgroundSpawn;
    public GameObject[] bgFrames;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void GetEnviroment(string type)
    {
        GameObject bg = GameObject.Find("Background");

        Destroy(bg);

        if (GetComponentInParent<YourHome>().activeMonster.info.isStar)
        {
            for (int i = 0; i < bgFrames.Length; i++)
            {
                bgFrames[i].GetComponent<PlasmaRainbow>().enabled = true;
            }
        }
        else
        {
            for (int i = 0; i < bgFrames.Length; i++)
            {
                bgFrames[i].GetComponent<PlasmaRainbow>().enabled = false;
            }
        }

        if (type == "Ice")
        {
            IceEnviroment();
        }

        if (type == "Nature")
        {
            NatureEnviroment();
        }

        if (type == "Mechanical")
        {
            MechEnviroment();
        }
    }


    public void IceEnviroment()
    {
        var x = Instantiate(background[0], transform, true);
        x.transform.position = new Vector3(backgroundSpawn.transform.position.x, backgroundSpawn.transform.position.y, -2);
        x.transform.localScale = backgroundSpawn.transform.localScale;
        x.name = "Background";
    }

    public void NatureEnviroment()
    {
        var x = Instantiate(background[1], transform, true);
        x.transform.position = new Vector3(backgroundSpawn.transform.position.x, backgroundSpawn.transform.position.y, -2);
        x.transform.localScale = backgroundSpawn.transform.localScale;
        x.name = "Background";
    }

    public void MechEnviroment()
    {
        var x = Instantiate(background[2], transform, true);
        x.transform.position = new Vector3(backgroundSpawn.transform.position.x, backgroundSpawn.transform.position.y, -2);
        x.transform.localScale = backgroundSpawn.transform.localScale;
        x.name = "Background";
    }



    //destroys the previous background and adds the new one
    public void OnEnable()
    {
        GameObject bg = GameObject.Find("Background");

            Destroy(bg);
    }
}
