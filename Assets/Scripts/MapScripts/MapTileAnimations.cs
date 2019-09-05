using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MapTileAnimations : MonoBehaviour
{
    public SpriteRenderer sp;
    public SpriteRenderer topSprite;
    public float animationTime;

    //variables and objects related to the water tiles
    public Sprite[] waterTileSprites;


    //variables and objects related to the fire tiles
    public Sprite[] fireTileSprites;
    //lava sprites that will glow
    public Sprite[] fireTileTopSprites;
    public GameObject fireDebris;

    public GameObject[] natureTileBrush;

    public Sprite[] magicTileTopSprites;
    public GameObject magicTileBurst;

    public bool isRoad;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TileAnimation(TileAttribute att, bool IsRoad)
    {
        isRoad = IsRoad;
        
        if (att == TileAttribute.Water)
        {
            float rand = Random.Range(.12f, .14f);
            animationTime = rand;
            StartCoroutine(WaterTile(animationTime));
        }

        if (att == TileAttribute.Fire)
        {
            float rand = Random.Range(.04f, .10f);
            animationTime = rand;
            StartCoroutine(FireTile(animationTime));
        }

        if (att == TileAttribute.Magic)
        {
            float rand = Random.Range(10f, 20f);
            animationTime = rand;
            StartCoroutine(MagicTile(animationTime));
        }


        if (att == TileAttribute.Nature)
        {
            float rand = Random.Range(.03f, .05f);
            animationTime = rand;
            StartCoroutine(NatureTile(animationTime));
        }
    }




    //the water tile animation
    public IEnumerator WaterTile(float time)
    {
        //loops the water motion sprites
        for (int i = 0; i < waterTileSprites.Length; i++)
        {
            sp.sprite = waterTileSprites[i];

            yield return new WaitForSecondsRealtime(time);

            if (i >= waterTileSprites.Length - 1)
            {
                i = 0;
                
            }
        }
    }

   


    //the water tile animation
    public IEnumerator FireTile(float time)
    {
        //create a copy of the first first sprite and set it above the fire tile
        var fire = Instantiate(topSprite, transform.position, Quaternion.identity);
        fire.transform.SetParent(GetComponentInParent<MapTile>().transform, true);
        fire.transform.localScale = new Vector3(1f, 1f, 1f);
        fire.sprite = fireTileTopSprites[0];
        fire.sortingOrder = topSprite.sortingOrder - 1;
        topSprite = fire;

        //int to keep track of how long the tile has been animating for. different things happen to different tiles at different ages
        int tileAge = 0;

        //keeps track of how many tile transitions this tile has gone through so it can identity the next state
        int tileNumber = 0;

        topSprite.color = new Color(1f, 1f, 1f, .75f);

        //loops the water motion sprites
        for (int i = 0; i < fireTileSprites.Length; i++)
        {
            sp.sprite = fireTileSprites[i];

            yield return new WaitForSecondsRealtime(time);

            if (i >= fireTileSprites.Length - 1)
            {
                i = 0;
                tileAge += 1;

                if (tileAge > 3)
                {
                    for (int d = 0; d < 20; d++)
                    {
                        float rand = Random.Range(-100f, 100f);
                        float randY = Random.Range(10f, 80f);
                        float scaleX = Random.Range(10f, 25f);
                        float scaleY = Random.Range(10f, 25f);

                        ///Location of the enemies that spawn
                        float x1 = GetComponentInParent<MapTile>().transform.position.x - sp.bounds.size.x / 2;
                        float x2 = GetComponentInParent<MapTile>().transform.position.x + sp.bounds.size.x / 2;
                        float y1 = GetComponentInParent<MapTile>().transform.position.y - sp.bounds.size.y / 2;
                        float y2 = GetComponentInParent<MapTile>().transform.position.y + sp.bounds.size.y / 2;

                        var spawnPoint = new Vector2(Random.Range(x1, x2), Random.Range(y1, y2));


                        var debris = Instantiate(fireDebris, spawnPoint, Quaternion.identity, GetComponentInParent<MapTile>().transform);
                        //var debris = Instantiate(fireDebris, spawnPoint, Quaternion.identity);
                        //debris.transform.SetParent(GetComponentInParent<MapTile>().transform, false);
                        debris.transform.localScale = new Vector3(scaleX / 60, scaleY / 60, debris.transform.localScale.z);
                        debris.GetComponent<Rigidbody2D>().AddForce(new Vector2(rand, randY), ForceMode2D.Impulse);
                        debris.GetComponent<Rigidbody2D>().angularVelocity = 200;
                        Destroy(debris, .8f);
                    }

                    tileNumber += 1;

                    if (tileNumber >= fireTileTopSprites.Length)
                    {
                        tileNumber = fireTileTopSprites.Length - 1;
                    }
                    fire.sprite = fireTileTopSprites[tileNumber];
                    tileAge = 0;
                }
            }
        }

        
    
    }

    //the water tile animation
    public IEnumerator NatureTile(float time)
    {
        
        for (int i = 0; i < 3; i++)
        {
           
            ///Location of the enemies that spawn
            float x1 = GetComponentInParent<MapTile>().transform.position.x - sp.bounds.size.x / 8;
            float x2 = GetComponentInParent<MapTile>().transform.position.x + sp.bounds.size.x / 4;
            float y1 = GetComponentInParent<MapTile>().transform.position.y - sp.bounds.size.y / 8;
            float y2 = GetComponentInParent<MapTile>().transform.position.y + sp.bounds.size.y / 4;

            var spawnPoint = new Vector3(Random.Range(x1, x2), Random.Range(y1, y2), -5f);

            var nature = Instantiate(natureTileBrush[0], spawnPoint, Quaternion.identity, GetComponentInParent<MapTile>().transform);
            
            nature.transform.localScale = new Vector3(.4f, .4f, nature.transform.localScale.z);
            nature.GetComponent<SpriteRenderer>().sortingOrder = sp.sortingOrder + 1;


            yield return new WaitForSecondsRealtime(time);

            //if (i >= waterTileSprites.Length - 1)
            //{
            //    i = 0;

            //}
        }
    }

    public IEnumerator MagicTile(float time)
    {

        //create a copy of the first star sprite and set it above the magic tile
        var magic = Instantiate(topSprite, transform.position, Quaternion.identity);
        magic.transform.SetParent(GetComponentInParent<MapTile>().transform);
        magic.transform.localScale = new Vector3(1f, 1f, 1f);
        magic.sprite = magicTileTopSprites[0];
        magic.sortingOrder = topSprite.sortingOrder - 1;
        topSprite = magic;


        for (int i = 0; i < 2; i++)
        {
            var x = Instantiate(magicTileBurst, transform.position, Quaternion.identity);
            x.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
            x.transform.SetParent(GetComponentInParent<MapTile>().transform, true);
            x.transform.localScale = new Vector3(25f, 25f, 1f);
            ParticleSystem ps = x.GetComponent<ParticleSystem>();
            Renderer rend = ps.GetComponent<Renderer>();
            rend.sortingOrder = 1000;

            yield return new WaitForSecondsRealtime(time);
            time = Random.Range(10f, 20f);
            if (i >= 1)
            {
                i = 0;
            }
        }





    }


}
