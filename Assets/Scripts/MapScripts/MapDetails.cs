using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDetails : MonoBehaviour
{
    public string levelCode;
    public string pathCode;
    public MapTile[] path;
    public List<string> pathOrder = new List<string>();

    private Map Map;
    // Start is called before the first frame update
    void Start()
    {
        Map = GetComponent<Map>();
        path = Map.path;


       

    }

    public void DisplayMap()
    {

        var width = Map.width;
        var height = Map.height;
        var columns = Map.columns;
        var rows = Map.rows;
        var tileNumber = Map.tileNumber;
        var mapTile = Map.mapTile;
        var mapCanvas = Map.mapCanvas;

        string[] chars = new string[levelCode.Length];

        for (int i = 0; i < levelCode.Length; i++)
        {
            chars[i] = levelCode[i].ToString();
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


        //GameObject[] paths = GameObject.FindGameObjectsWithTag("MapTile");

        //break up the path code in to sections of 3, since each tile is a 3 digit number
        string[] pathChars = new string[pathCode.Length];
        int h = 2;
        for (int i = 0; i < pathCode.Length / 3; i++)
        {
            chars[i] = pathCode[h - 2].ToString() + pathCode[h - 1].ToString() + pathCode[h].ToString();
            h += 3;
            int tileCheck = int.Parse(chars[i]);
            path[i] = GameObject.Find(tileCheck.ToString()).GetComponent<MapTile>();
            //path[i] = paths[tileCheck].GetComponent<MapTile>();
            pathOrder.Add(chars[i]);

            Map.path[i] = GameObject.Find(tileCheck.ToString()).GetComponent<MapTile>();
        }






    }

    // Update is called once per frame
    void Update()
    {

    }
}
