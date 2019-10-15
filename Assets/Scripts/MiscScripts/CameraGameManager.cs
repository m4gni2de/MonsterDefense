using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//ATTACH THIS TO EVERY SCENE IN THE GAME SO THAT ON LOAD, IT MAKES THE GAME MANAGER'S CAMERA THE SAME CAMERA AS THE ONE IN THE SCENE
public class CameraGameManager : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {

        SetCamera();
    }

    //set the Game Manager's main camera to the main camera of the scene
    public void SetCamera()
    {
        GameObject camera = GameObject.Find("Main Camera");

        GameManager.Instance.canvasCamera = camera.GetComponent<Camera>();
        GameManager.Instance.overworldCanvas.worldCamera = GameManager.Instance.canvasCamera;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
