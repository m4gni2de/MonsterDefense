using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Map : MonoBehaviour
{
    public float width, height;
    public int columns, rows;
    //string to represent the code that represents the level
    public string levelCode;
    //the number of the tile as it's spawned
    public int tileNumber;

    //canvas for which the map tiles become a child of
    public GameObject mapCanvas;

    public GameObject mapTile;

    //a string to represent the pathway that the monsters take
    public string pathCode;

    public TMP_InputField codeInput;

    public List<int> pathList = new List<int>();
    public MapTile[] path;


    public bool pathDesign, buildDesign, pathDirection, editorMode;
    //Dictionary that will hold the codes for each tile type
    public Dictionary<string, string> TileValues = new Dictionary<string, string>();


    void Awake()
    {

        TileValues.Add("0", "Build");
        TileValues.Add("1", "Dirt");
        TileValues.Add("2", "Water");
        TileValues.Add("3", "Path");

    }

    // Start is called before the first frame update
    void Start()
    {
        width = gameObject.GetComponent<RectTransform>().rect.width;
        height = gameObject.GetComponent<RectTransform>().rect.height;

        codeInput.GetComponent<TMP_InputField>();

        //Debug.Log(height);

        columns = int.Parse(width.ToString()) / 50;
        rows = int.Parse(height.ToString()) / 50;

        if (editorMode == true)
        {
            RandomMap();
        }
        else
        {

        }
    }


    public void RandomMap()
    {
        for (int i = 1; i < rows * 2; i++)
        {
            //Debug.Log(i);

            for (int c = 1; c <= columns; c++)
            {



                int rand = Random.Range(0, 3);
                var tile = Instantiate(mapTile, transform.position, Quaternion.identity);
                var tile2 = Instantiate(mapTile, transform.position, Quaternion.identity);
                if (rand == 0)
                {
                    tile.GetComponent<MapTile>().Build();
                    levelCode += "0";
                }
                if (rand == 1)
                {
                    tile.GetComponent<MapTile>().Dirt();
                    levelCode += "1";
                }
                if (rand == 2)
                {
                    tile.GetComponent<MapTile>().Water();
                    levelCode += "2";
                }
                if (rand == 3)
                {
                    tile.GetComponent<MapTile>().Road();
                    levelCode += "3";
                }

                tile.GetComponent<MapTile>().tileNumber = tileNumber;
                tile.name = tileNumber.ToString();
                tileNumber += 1;

                int rand2 = Random.Range(0, 3);
                if (rand2 == 0)
                {
                    tile2.GetComponent<MapTile>().Build();
                    levelCode += "0";
                }
                if (rand2 == 1)
                {
                    tile2.GetComponent<MapTile>().Dirt();
                    levelCode += "1";
                }
                if (rand2 == 2)
                {
                    tile2.GetComponent<MapTile>().Water();
                    levelCode += "2";
                }
                if (rand2 == 3)
                {
                    tile2.GetComponent<MapTile>().Road();
                    levelCode += "3";
                }

                tile2.GetComponent<MapTile>().tileNumber = tileNumber;
                tile2.name = tileNumber.ToString();
                tileNumber += 1;


                tile.transform.position = new Vector2((-width/2) + (i * 50), (height/2) - (c * 25));
                tile.GetComponent<SpriteRenderer>().sortingOrder = (int)-tile.transform.position.y;
                tile2.transform.position = new Vector2((-width/2) + (i * 50) + 25, (height/2) - (c * 25) + 12.5f);
                tile2.GetComponent<SpriteRenderer>().sortingOrder = (int)-tile2.transform.position.y;

                tile.transform.SetParent(mapCanvas.transform, false);
                tile2.transform.SetParent(mapCanvas.transform, false);




            }





        }
    }


    public void ImportMap()
    {

        GameObject[] maps = GameObject.FindGameObjectsWithTag("MapTile");
        tileNumber = 0;

        for (int m = 0; m < maps.Length; m++)
        {
            Destroy(maps[m]);
        }



        string code = codeInput.text;
        levelCode = code;

        string[] chars = new string[code.Length];

        for (int i = 0; i < code.Length; i++)
        {
            chars[i] = code[i].ToString();
        }

        int charCount = 0;

        for (int i = 1; i < rows * 2; i++)
        {
            //Debug.Log(i);

            for (int c = 1; c <= columns; c++)
            {

                var tile = Instantiate(mapTile, transform.position, Quaternion.identity);
                var tile2 = Instantiate(mapTile, transform.position, Quaternion.identity);
                if (chars[charCount] == "0")
                {
                    tile.GetComponent<MapTile>().Build();
                }
                if (chars[charCount] == "1")
                {
                    tile.GetComponent<MapTile>().Dirt();
                }
                if (chars[charCount] == "2")
                {
                    tile.GetComponent<MapTile>().Water();
                }
                if (chars[charCount] == "3")
                {
                    tile.GetComponent<MapTile>().Road();
                }

                tile.GetComponent<MapTile>().tileNumber = tileNumber;
                tile.name = tileNumber.ToString();
                tileNumber += 1;

                charCount += 1;

                if (chars[charCount] == "0")
                {
                    tile2.GetComponent<MapTile>().Build();
                }
                if (chars[charCount] == "1")
                {
                    tile2.GetComponent<MapTile>().Dirt();
                }
                if (chars[charCount] == "2")
                {
                    tile2.GetComponent<MapTile>().Water();
                }
                if (chars[charCount] == "3")
                {
                    tile2.GetComponent<MapTile>().Road();
                }


                tile2.GetComponent<MapTile>().tileNumber = tileNumber;
                tile2.name = tileNumber.ToString();
                tileNumber += 1;

            
                //tile.transform.position = new Vector2(((-height / 4) + (c * 50)), (width / 4) - 6.5f - (i * 27));
                tile.transform.position = new Vector2((-width / 2) + (i * 50), (height / 2) - (c * 25));
                tile.GetComponent<SpriteRenderer>().sortingOrder = (int)-tile.transform.position.y;
                //tile2.transform.position = new Vector2(((-height / 4) + 25 + (c * 50)), (width / 4) + 7f - (i * 27));
                tile2.transform.position = new Vector2((-width / 2) + (i * 50) + 25, (height / 2) - (c * 25) + 12.5f);
                tile2.GetComponent<SpriteRenderer>().sortingOrder = (int)-tile2.transform.position.y;

                tile.transform.SetParent(mapCanvas.transform, false);
                tile2.transform.SetParent(mapCanvas.transform, false);

                charCount += 1;



            }





        }

        
    }


    //get information on the map tiles that were clicked
    public void CheckTile()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.tag == "MapTile")
                {
                    var tile = hit.collider.gameObject;

                    //change the clicked tile to a pathway for enemies
                    if (pathDesign == true)
                    {
                        //replace tile with a road tile
                        tile.GetComponent<MapTile>().Road();
                        levelCode = levelCode.Remove(tile.GetComponent<MapTile>().tileNumber, 1);
                        levelCode = levelCode.Insert(tile.GetComponent<MapTile>().tileNumber, "3");

                    }

                    //change the clicked tile to accept a tower for players
                    if (buildDesign == true)
                    {
                        //replace tile with a buildable tile
                        tile.GetComponent<MapTile>().Build();
                        levelCode = levelCode.Remove(tile.GetComponent<MapTile>().tileNumber, 1);
                        levelCode = levelCode.Insert(tile.GetComponent<MapTile>().tileNumber, "0");
                    }

                    //click the paths in order to make a path. if pathDesign and pathDirection are both true, you can make a path and set it next in the list of pathways at the same time
                    if (pathDirection == true)
                    {
                        if (tile.GetComponent<MapTile>().isRoad == true)
                        {
                            pathList.Add(tile.GetComponent<MapTile>().tileNumber);
                            path[pathList.Count - 1] = tile.GetComponent<MapTile>();

                            if (tile.GetComponent<MapTile>().tileNumber < 10)
                            {
                                pathCode += "00" + tile.GetComponent<MapTile>().tileNumber;
                            }
                            if (tile.GetComponent<MapTile>().tileNumber >= 10 && tile.GetComponent<MapTile>().tileNumber < 100)
                            {
                                pathCode += "0" + tile.GetComponent<MapTile>().tileNumber;
                            }
                            if (tile.GetComponent<MapTile>().tileNumber > 100)
                            {
                                pathCode += tile.GetComponent<MapTile>().tileNumber;
                            }
                        }
                    }


                }
            }
        }


        //if (Input.GetMouseButtonDown(1))
        //{
        //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        //    RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        //    if (hit.collider != null)
        //    {
        //        if (hit.collider.gameObject.name == "mapTile(Clone)")
        //        {
        //            var tile = hit.collider.gameObject;

        //            if (tile.GetComponent<MapTile>().isRoad == true)
        //            {
        //                tile.GetComponent<MapTile>().Road(true);
        //                levelCode = levelCode.Remove(tile.GetComponent<MapTile>().tileNumber, 1);
        //                levelCode = levelCode.Insert(tile.GetComponent<MapTile>().tileNumber, "4");
        //            }
        //            else
        //            {
        //                //replace tile with a buildable tile
        //                tile.GetComponent<MapTile>().Build();
        //                levelCode = levelCode.Remove(tile.GetComponent<MapTile>().tileNumber, 1);
        //                levelCode = levelCode.Insert(tile.GetComponent<MapTile>().tileNumber, "0");
        //            }
        //            Debug.Log(levelCode);

        //        }
        //    }
        //}
    }



    // Update is called once per frame
    void Update()
    {
        CheckTile();

        //var p = JsonUtility.ToJson(
    }
}
