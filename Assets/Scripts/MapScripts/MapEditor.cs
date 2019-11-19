using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//use this as the map Editor script
public class MapEditor : MonoBehaviour
{
    public float width, height;
    public int columns, rows;
    

    
    //the number of the tile as it's spawned
    public int tileNumber;

    //canvas for which the map tiles become a child of
    private GameObject mapCanvas;

    private GameObject mapTile;

    //a string to represent the pathway that the monsters take
    public string pathCode;

    public TMP_InputField codeInput;

    public List<int> pathList = new List<int>();
    public MapTile[] path;

    private MapDetails mapDetails;


    public bool pathDesign, buildDesign, pathDirection, editorMode;
    //Dictionary that will hold the codes for each tile type
    public Dictionary<string, string> TileValues = new Dictionary<string, string>();
    public Dictionary<string, string> TileAttributes = new Dictionary<string, string>();


    public GameObject tileEditorMenu;

   


    void Awake()
    {

        //TileValues.Add("0", "Build");
        //TileValues.Add("1", "Dirt");
        //TileValues.Add("2", "Water");
        //TileValues.Add("3", "Path");


        



        TileAttributes.Add("00", "None");
        TileAttributes.Add("01", "Water");
        TileAttributes.Add("02", "Fire");
        TileAttributes.Add("03", "Nature");
        TileAttributes.Add("04", "Magic");
        TileAttributes.Add("05", "Electric");
        TileAttributes.Add("06", "Poison");
        TileAttributes.Add("07", "Ice");
        TileAttributes.Add("08", "Shadow");
        TileAttributes.Add("09", "Mechanical");
        TileAttributes.Add("10", "Normal");


    }

    // Start is called before the first frame update
    void Start()
    {
        mapDetails = GetComponent<MapDetails>();

        mapCanvas = mapDetails.mapCanvas;
        mapTile = mapDetails.mapTile;

        width = gameObject.GetComponent<RectTransform>().rect.width;
        height = gameObject.GetComponent<RectTransform>().rect.height;



        //Debug.Log(height);

        columns = int.Parse(width.ToString()) / 50;
        rows = int.Parse(height.ToString()) / 50;

        if (editorMode == true)
        {
            //RandomMap();
            codeInput.GetComponent<TMP_InputField>();
        }
        else
        {

        }
    }


    //public void RandomMap()
    //{
    //    for (int i = 1; i < rows * 2; i++)
    //    {
    //        //Debug.Log(i);

    //        for (int c = 1; c <= columns; c++)
    //        {



    //            int rand = Random.Range(0, TileValues.Count -1);
    //            int randType = Random.Range(0, TileAttributes.Count);
    //            string type = "";


    //            if (randType < 10)
    //            {
    //                type = "0" + randType;
    //            }
    //            else
    //            {
    //               type = randType.ToString();
    //            }

    //            var tile = Instantiate(mapTile, transform.position, Quaternion.identity);
    //            var tile2 = Instantiate(mapTile, transform.position, Quaternion.identity);



    //            ////picks a random tile type for the tile to have
    //            //if (TileValues.ContainsKey(rand.ToString()))
    //            //{

    //            //    tile.GetComponent<MapTile>().GetType(rand);
    //            //    levelCode += rand.ToString();
    //            //}



    //            //picks a random tile attribute for the tile to have
    //            if (TileAttributes.ContainsKey(type))
    //            {

    //                tile.GetComponent<MapTile>().GetAttribute(randType);
    //                tileAttCode += type;
    //            }

    //            tile.GetComponent<MapTile>().tileNumber = tileNumber;
    //            tile.name = tileNumber.ToString();
    //            tileNumber += 1;

    //            //if (rand == 0)
    //            //{
    //            //    tile.GetComponent<MapTile>().Build();
    //            //    levelCode += "0";
    //            //}
    //            //if (rand == 1)
    //            //{
    //            //    tile.GetComponent<MapTile>().Dirt();
    //            //    levelCode += "1";
    //            //}
    //            //if (rand == 2)
    //            //{
    //            //    tile.GetComponent<MapTile>().Water();
    //            //    levelCode += "2";
    //            //}
    //            //if (rand == 3)
    //            //{
    //            //    tile.GetComponent<MapTile>().Road();
    //            //    levelCode += "3";
    //            //}



    //            //*******************************************************************TILE 1 AND 2 SEPARATOR**********************8


    //            int randType2 = Random.Range(0, TileAttributes.Count);
    //            string type2 = "";
    //            if (randType2 < 10)
    //            {
    //                type2 = "0" + randType2;
    //            }
    //            else
    //            {
    //                type2 = randType.ToString();
    //            }



    //            int rand2 = Random.Range(0, TileValues.Count - 1);

    //            ////picks a random tile type for the tile to have
    //            //if (TileValues.ContainsKey(rand2.ToString()))
    //            //{

    //            //    tile2.GetComponent<MapTile>().GetType(rand2);
    //            //    levelCode += rand2.ToString();
    //            //}

    //            //picks a random tile attribute for the tile to have
    //            if (TileAttributes.ContainsKey(type))
    //            {
    //                tile2.GetComponent<MapTile>().GetAttribute(randType2);
    //                tileAttCode += type;

    //            }

    //            tile2.GetComponent<MapTile>().tileNumber = tileNumber;
    //            tile2.name = tileNumber.ToString();
    //            tileNumber += 1;

    //            //if (rand2 == 0)
    //            //{
    //            //    tile2.GetComponent<MapTile>().Build();
    //            //    levelCode += "0";
    //            //}
    //            //if (rand2 == 1)
    //            //{
    //            //    tile2.GetComponent<MapTile>().Dirt();
    //            //    levelCode += "1";
    //            //}
    //            //if (rand2 == 2)
    //            //{
    //            //    tile2.GetComponent<MapTile>().Water();
    //            //    levelCode += "2";
    //            //}
    //            //if (rand2 == 3)
    //            //{
    //            //    tile2.GetComponent<MapTile>().Road();
    //            //    levelCode += "3";
    //            //}




    //            tile.transform.position = new Vector2((-width/2) + (i * 50), (height/2) - (c * 25));
    //            tile.GetComponent<SpriteRenderer>().sortingOrder = (int)-tile.transform.position.y;
    //            tile2.transform.position = new Vector2((-width/2) + (i * 50) + 25, (height/2) - (c * 25) + 12.5f);
    //            tile2.GetComponent<SpriteRenderer>().sortingOrder = (int)-tile2.transform.position.y;

    //            tile.transform.SetParent(mapCanvas.transform, false);
    //            tile2.transform.SetParent(mapCanvas.transform, false);




    //        }





    //    }
    //}

    public void RandomMap()
    {
        for (int i = 1; i < rows * 2; i++)
        {
            //Debug.Log(i);

            for (int c = 1; c <= columns; c++)
            {



                
                //int randType = Random.Range(0, TileAttributes.Count);
                int randType = Random.Range(0, 2);
                string type = "";


                if (randType < 10)
                {
                    type = "0" + randType;
                }
                else
                {
                    type = randType.ToString();
                }

                var tile = Instantiate(mapTile, transform.position, Quaternion.identity);
                var tile2 = Instantiate(mapTile, transform.position, Quaternion.identity);

                tile.GetComponent<MapTile>().mapDetails = GetComponent<MapDetails>();
                tile2.GetComponent<MapTile>().mapDetails = GetComponent<MapDetails>();

                //picks a random tile attribute for the tile to have
                if (TileAttributes.ContainsKey(type))
                {

                    tile.GetComponent<MapTile>().GetAttribute(randType);
                    mapDetails.mapCode += type;
                    
                }

                tile.GetComponent<MapTile>().tileNumber = tileNumber;
                tile.name = tileNumber.ToString();
                tileNumber += 1;

                



                //*******************************************************************TILE 1 AND 2 SEPARATOR**********************8


                int randType2 = Random.Range(0, TileAttributes.Count - 1);
                //int randType2 = Random.Range(0, 4);
                string type2 = "";
                if (randType2 < 10)
                {
                    type2 = "0" + randType2;
                }
                else
                {
                    type2 = randType.ToString();
                }



                int rand2 = Random.Range(0, TileValues.Count - 1);

                

                //picks a random tile attribute for the tile to have
                if (TileAttributes.ContainsKey(type2))
                {
                    tile2.GetComponent<MapTile>().GetAttribute(randType2);
                    mapDetails.mapCode += type2;

                }

                tile2.GetComponent<MapTile>().tileNumber = tileNumber;
                tile2.name = tileNumber.ToString();
                tileNumber += 1;

                tile.transform.position = new Vector2((-width / 2) + (i * 50), (height / 2) - (c * 25));
                tile.GetComponent<SpriteRenderer>().sortingOrder = (int)-tile.transform.position.y;
                tile2.transform.position = new Vector2((-width / 2) + (i * 50) + 25, (height / 2) - (c * 25) + 12.5f);
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

        mapDetails.LoadMap(code);


        return;
        
        

        
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
                    tileEditorMenu.SetActive(true);
                    tileEditorMenu.GetComponent<TileEditorMenu>().SetActiveTile(tile.GetComponent<MapTile>());

                    //change the clicked tile to a pathway for enemies
                    if (pathDesign == true)
                    {

                        //replace tile with a road tile
                        tile.GetComponent<MapTile>().Road();
                        tile.GetComponent<MapTile>().roadSprite.sprite = tile.GetComponent<MapTile>().road;
                    }


                    //click the paths in order to make a path. if pathDesign and pathDirection are both true, you can make a path and set it next in the list of pathways at the same time
                    if (pathDirection == true)
                    {
                        if (tile.GetComponent<MapTile>().isRoad == true)
                        {
                            //pathList.Add(tile.GetComponent<MapTile>().tileNumber);
                            //path[pathList.Count - 1] = tile.GetComponent<MapTile>();

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

    public void TileEditor()
    {

    }

    public void TileEditorBtn()
    {
        tileEditorMenu.SetActive(!tileEditorMenu.activeSelf);
    }


    // Update is called once per frame
    void Update()
    {
        CheckTile();

        //var p = JsonUtility.ToJson(
    }
}
