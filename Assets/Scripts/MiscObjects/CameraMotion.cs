using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class CameraMotion : MonoBehaviour
{
    private static CameraMotion instance;
    public static CameraMotion Instance { get { return instance; } }
    public void OnDestroy() { if (instance == this) instance = null; }


    private Camera mainCamera;

    public float cameraMoveSpeed;
    private Vector2 facingDirection;
    private float xDiff, yDiff, acumTime;

    //************VARIBLES FOR TOUCH CAMERA
    public float cameraMaxSize = 225;
    //private float cameraMaxSize = 160;
    public float cameraMinSize = 50;

   

    public Vector2?[] oldTouchPositions = {
        null,
        null
    };
    Vector2 oldTouchVector;
    float oldTouchDistance;
    //*******************

    //bool to determine if the camera is in a state of "being moved"
    public bool isClicked;

    public Vector3 clickPosition;
    // Start is called before the first frame update

    //bool that other scripts can manipulate to turn off/on the ability to move the camera
    public bool isFree;




    private void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        isFree = true;
        cameraMoveSpeed = 10;
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //else
        //{
        //    oldTouchPositions[0] = null;
        //    oldTouchPositions[1] = null;
        //}

        //if there is nothing touching the screen, the camera defaults back to free
        if (Input.touchCount == 0 || Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            oldTouchPositions[0] = null;
            oldTouchPositions[1] = null;
            isFree = true;

        }

    }

    private void LateUpdate()
    {
        if (isFree)
        {
            TouchCamera();
            MouseCamera();
        }
    }

    public void MouseCamera()
    {
        var worldMousePosition =
                    Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var x = worldMousePosition.x;
        var y = worldMousePosition.y;
        if (Input.GetMouseButton(0))
        {
            if (isClicked == false)
            {
                isClicked = true;
                clickPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
            }
            if (isClicked)
            {
                //Debug.Log(worldMousePosition);

                if (worldMousePosition != clickPosition)
                {
                    facingDirection = clickPosition - worldMousePosition;

                    xDiff = clickPosition.x - worldMousePosition.x;
                    yDiff = clickPosition.y - worldMousePosition.y;

                    if (facingDirection.x < 0)
                    {
                        mainCamera.transform.Translate(new Vector3(xDiff / cameraMoveSpeed, 0), Space.Self);

                    }
                    if (facingDirection.x > 0)
                    {
                        mainCamera.transform.Translate(new Vector3(xDiff / cameraMoveSpeed, 0), Space.Self);

                    }
                    if (facingDirection.y < 0)
                    {
                        mainCamera.transform.Translate(new Vector3(0, yDiff) / cameraMoveSpeed, Space.Self);

                    }
                    if (facingDirection.y > 0)
                    {
                        mainCamera.transform.Translate(new Vector3(0, yDiff / cameraMoveSpeed), Space.Self);
                    }
                }
            }

        }
        else
        {

            if (isClicked == true)
            {
                acumTime += 3;
                mainCamera.transform.Translate(facingDirection / (cameraMoveSpeed + acumTime), Space.Self);

                if (acumTime >= 21 || Input.GetMouseButton(0))
                {
                    mainCamera.transform.position = mainCamera.transform.position;
                    isClicked = false;
                    acumTime = 0;
                }
            }
            //isClicked = false;
        }



        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            mainCamera.orthographicSize += (1 * cameraMoveSpeed);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            mainCamera.orthographicSize -= (1 * cameraMoveSpeed);
        }

        if (mainCamera.orthographicSize >= cameraMaxSize)
        {
            mainCamera.orthographicSize = cameraMaxSize;
        }

        if (mainCamera.orthographicSize <= cameraMinSize)
        {
            mainCamera.orthographicSize = cameraMinSize;
        }
    }


    //************************TOUCH CONTROLS TO MOVE CAMERA*****************************//
    public void TouchCamera()
    {
        if (Input.touchCount == 0)
        {
            oldTouchPositions[0] = null;
            oldTouchPositions[1] = null;
            isFree = true;
            
        }
        else if (Input.touchCount == 1)
        {
            

            
                if (oldTouchPositions[0] == null || oldTouchPositions[1] != null)
                {
                    oldTouchPositions[0] = Input.GetTouch(0).position;
                    oldTouchPositions[1] = null;
                }
                else
                {
                    Vector2 newTouchPosition = Input.GetTouch(0).position;

                    transform.position += transform.TransformDirection((Vector3)((oldTouchPositions[0] - newTouchPosition) * GetComponent<Camera>().orthographicSize / GetComponent<Camera>().pixelHeight * 2f));

                    oldTouchPositions[0] = newTouchPosition;
                }
            }
            else
            {
                if (oldTouchPositions[1] == null)
                {
                    oldTouchPositions[0] = Input.GetTouch(0).position;
                    oldTouchPositions[1] = Input.GetTouch(1).position;
                    oldTouchVector = (Vector2)(oldTouchPositions[0] - oldTouchPositions[1]);
                    oldTouchDistance = oldTouchVector.magnitude;
                }
                else
                {
                    Vector2 screen = new Vector2(GetComponent<Camera>().pixelWidth, GetComponent<Camera>().pixelHeight);

                    Vector2[] newTouchPositions = {
                    Input.GetTouch(0).position,
                    Input.GetTouch(1).position
                };
                    Vector2 newTouchVector = newTouchPositions[0] - newTouchPositions[1];
                    float newTouchDistance = newTouchVector.magnitude;

                    transform.position += transform.TransformDirection((Vector3)((oldTouchPositions[0] + oldTouchPositions[1] - screen) * GetComponent<Camera>().orthographicSize / screen.y));
                    //transform.localRotation *= Quaternion.Euler(new Vector3(0, 0, Mathf.Asin(Mathf.Clamp((oldTouchVector.y * newTouchVector.x - oldTouchVector.x * newTouchVector.y) / oldTouchDistance / newTouchDistance, -1f, 1f)) / 0.0174532924f));
                    GetComponent<Camera>().orthographicSize *= oldTouchDistance / newTouchDistance;
                    transform.position -= transform.TransformDirection((newTouchPositions[0] + newTouchPositions[1] - screen) * GetComponent<Camera>().orthographicSize / screen.y);

                    oldTouchPositions[0] = newTouchPositions[0];
                    oldTouchPositions[1] = newTouchPositions[1];
                    oldTouchVector = newTouchVector;
                    oldTouchDistance = newTouchDistance;
                }
            }

            if (GetComponent<Camera>().orthographicSize > cameraMaxSize)
            {
                GetComponent<Camera>().orthographicSize = cameraMaxSize;
            }

            if (GetComponent<Camera>().orthographicSize < cameraMinSize)
            {
                GetComponent<Camera>().orthographicSize = cameraMinSize;
            }



        //for (var i = 0; i < Input.touchCount; ++i)
        //{

        //    Input.GetTouch(0).
        //}
    }



    
}