using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//ATTACH THIS TO EVERY SCENE IN THE GAME SO THAT ON LOAD, IT MAKES THE GAME MANAGER'S CAMERA THE SAME CAMERA AS THE ONE IN THE SCENE
public class CameraGameManager : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {

        SetCamera();
        SetScene();
    }

    //set the Game Manager's main camera to the main camera of the scene
    public void SetCamera()
    {
        GameObject camera = GameObject.Find("Main Camera");

        GameManager.Instance.canvasCamera = camera.GetComponent<Camera>();
        GameManager.Instance.overworldCanvas.worldCamera = GameManager.Instance.canvasCamera;
    }


    //sets the Game Managers's active Scene to the current scene
    public void SetScene()
    {
        GameManager.Instance.activeScene = SceneManager.GetActiveScene();
        GameManager.Instance.EventTriggerManager(SceneManager.GetActiveScene());
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
