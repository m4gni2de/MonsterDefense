using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MapTileAnimations : MonoBehaviour
{
    public SpriteRenderer sp;
    public SpriteRenderer topSprite;
    public float animationTime;

    //the script in the Game Manager that contains all of the special tiles
    public Maps maps;

   

    public bool isRoad;


    private void Awake()
    {
        maps = GameManager.Instance.GetComponent<Maps>();
    }

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

        if (att == TileAttribute.None)
        {
            
        }

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
            float rand = Random.Range(.08f, .11f);

            animationTime = rand;
            StartCoroutine(MagicTile(animationTime));
        }


        if (att == TileAttribute.Nature)
        {
            float rand = Random.Range(.03f, .05f);
            animationTime = rand;
            StartCoroutine(NatureTile(animationTime));
        }


        if (att == TileAttribute.Poison)
        {
            float rand = Random.Range(.03f, .05f);
            animationTime = rand;
            PoisonTile();
        }

        if (att == TileAttribute.Electric)
        {
            float rand = Random.Range(.03f, .05f);
            animationTime = rand;
            ElectricTile(rand);
        }

        if (att == TileAttribute.Ice)
        {
            float rand = Random.Range(.01f, .20f);
            animationTime = rand;
            IceTile(rand);
        }
    }




    //the water tile animation
    public IEnumerator WaterTile(float time)
    {
        //loops the water motion sprites
        for (int i = 0; i < maps.waterTileSprites.Length; i++)
        {
            sp.sprite = maps.waterTileSprites[i];

            yield return new WaitForSecondsRealtime(time);

            if (i >= maps.waterTileSprites.Length - 1)
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
        fire.sprite = maps.fireTileTopSprites[0];
        fire.sortingLayerName = "MapTiles";
        fire.sortingOrder = topSprite.sortingOrder - 1;
        topSprite = fire;

        //int to keep track of how long the tile has been animating for. different things happen to different tiles at different ages
        int tileAge = 0;

        //keeps track of how many tile transitions this tile has gone through so it can identity the next state
        int tileNumber = 0;

        topSprite.color = new Color(1f, 1f, 1f, .75f);

        //loops the water motion sprites
        for (int i = 0; i < maps.fireTileSprites.Length; i++)
        {
            sp.sprite = maps.fireTileSprites[i];

            yield return new WaitForSecondsRealtime(time);

            if (i >= maps.fireTileSprites.Length - 1)
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


                        var debris = Instantiate(maps.fireDebris, spawnPoint, Quaternion.identity, GetComponentInParent<MapTile>().transform);
                        //var debris = Instantiate(fireDebris, spawnPoint, Quaternion.identity);
                        //debris.transform.SetParent(GetComponentInParent<MapTile>().transform, false);
                        debris.transform.localScale = new Vector3(scaleX / 60, scaleY / 60, debris.transform.localScale.z);
                        debris.GetComponent<Rigidbody2D>().AddForce(new Vector2(rand, randY), ForceMode2D.Impulse);
                        debris.GetComponent<Rigidbody2D>().angularVelocity = 200;
                        Destroy(debris, .8f);
                    }

                    tileNumber += 1;

                    if (tileNumber >= maps.fireTileTopSprites.Length)
                    {
                        tileNumber = maps.fireTileTopSprites.Length - 1;
                    }
                    fire.sprite = maps.fireTileTopSprites[tileNumber];
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
            float x2 = GetComponentInParent<MapTile>().transform.position.x + sp.bounds.size.x / 8;
            float y1 = GetComponentInParent<MapTile>().transform.position.y - sp.bounds.size.y / 4;
            float y2 = GetComponentInParent<MapTile>().transform.position.y + sp.bounds.size.y / 4;



            //float randX1 = Random.Range(x1, x2);

            //float randX2 = 1 - (randX1 / (GetComponentInParent<MapTile>().transform.position.x + sp.bounds.size.x / 2));


            //Debug.Log(randX1 + "  " + randX2);


            //float y1 = -randX2 * (GetComponentInParent<MapTile>().transform.position.y - sp.bounds.size.y / 2);
            //float y2 = randX2 * (GetComponentInParent<MapTile>().transform.position.y + sp.bounds.size.y / 2);


            //var spawnPoint = new Vector3(randX1, Random.Range(y1, y2), -5);
            var spawnPoint = new Vector3(Random.Range(x1, x2), Random.Range(y1, y2), -5);

            var nature = Instantiate(maps.natureTileBrush[0], spawnPoint, Quaternion.identity, GetComponentInParent<MapTile>().transform);
            
            nature.transform.localScale = new Vector3(.4f, .4f, nature.transform.localScale.z);
            nature.GetComponent<SpriteRenderer>().sortingOrder = -(int)spawnPoint.y;


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
        magic.sprite = maps.magicTileTopSprites[0];
        magic.sortingOrder = topSprite.sortingOrder - 1;
        topSprite = magic;

        ////creates a copy of the original tile to retain tile thickness
        //var sprite = Instantiate(sp, transform.position, Quaternion.identity);
        //sprite.transform.SetParent(GetComponentInParent<MapTile>().transform);
        //sprite.transform.localScale = new Vector3(1f, 1f, 1f);
        //sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.g, .75f);

        //turns the tile in to a smokey looking tile, and sets the smoke level to a random number to give the tiles a uniqueness
        sp.gameObject.AddComponent<SmokeFX>();
        float rand = Random.Range(.14f, .72f);
        sp.gameObject.GetComponent<SmokeFX>()._Value2 = rand;
        //sp.gameObject.GetComponent<SmokeFX>()._Alpha = sp.gameObject.GetComponent<SmokeFX>()._Value2;

       
        
        //creates the appearance of smoke within the magic tile
        for (int i = 0; i <=100; i++)
        {
            sp.gameObject.GetComponent<SmokeFX>()._Value2 += .03f;
            sp.gameObject.GetComponent<SmokeFX>()._Alpha += .03f;
            
            yield return new WaitForSecondsRealtime(time);

            if (i >= 99)
            {
                i = 0;
                var x = Instantiate(maps.magicTileBurst, transform.position, Quaternion.identity);
                x.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
                x.transform.SetParent(GetComponentInParent<MapTile>().transform, true);
                x.transform.localScale = new Vector3(25f, 25f, 1f);

                ParticleSystem ps = x.GetComponent<ParticleSystem>();
                Renderer rend = ps.GetComponent<Renderer>();
                rend.sortingOrder = 1000;
            }

            if (sp.gameObject.GetComponent<SmokeFX>()._Value2 >= .85f)
            {
                sp.gameObject.GetComponent<SmokeFX>()._Alpha = 1 - sp.gameObject.GetComponent<SmokeFX>()._Value2;
                sp.gameObject.GetComponent<SmokeFX>()._Value2 = 1 - sp.gameObject.GetComponent<SmokeFX>()._Value2;
               

            }

            //if (sp.gameObject.GetComponent<SmokeFX>()._Value2 >= .45f)
            //{

            //    sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.g, sprite.color.a + .02f);
            //}
            



            //if (sp.gameObject.GetComponent<SmokeFX>()._Value2 <= .13f || sp.gameObject.GetComponent<SmokeFX>()._Value2 >= .93f)
            //{

            //    goingDown = !goingDown;
            //}
        }


        //for (int i = 0; i < 2; i++)
        //{
        //    var x = Instantiate(magicTileBurst, transform.position, Quaternion.identity);
        //    x.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        //    x.transform.SetParent(GetComponentInParent<MapTile>().transform, true);
        //    x.transform.localScale = new Vector3(25f, 25f, 1f);

        //    ParticleSystem ps = x.GetComponent<ParticleSystem>();
        //    Renderer rend = ps.GetComponent<Renderer>();
        //    rend.sortingOrder = 1000;

        //    yield return new WaitForSecondsRealtime(time);
        //    time = Random.Range(10f, 20f);
        //    if (i >= 1)
        //    {
        //        i = 0;
        //    }
        //}
    }

   

    public void PoisonTile()
    {
        //var poison = Instantiate(topSprite, transform.position, Quaternion.identity, GetComponentInParent<MapTile>().transform);
        //poison.transform.localScale = new Vector3(1f, 1f, 1f);
        //poison.sprite = sp.sprite;
        //sp.sortingLayerName = sp.sortingLayerName;
        //sp.sortingOrder = sp.sortingOrder;
        //sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, .65f);
        //sp.sortingOrder = 0;
        //poison.gameObject.AddComponent<Ghost>();

        //sp.gameObject.AddComponent<SmokeFX>();
        //value 2 for Smoke is "Turn to Smoke"
        //sp.gameObject.GetComponent<SmokeFX>()._Value2 = .65f; 

        sp.gameObject.AddComponent<HSV_FX>();

    }


    public void ElectricTile(float time)
    {
       

        sp.gameObject.AddComponent<Lightning2D>();


        sp.gameObject.GetComponent<Lightning2D>()._Value1 = 47.7f;
        sp.gameObject.GetComponent<Lightning2D>()._Value2 = 2f;
    }


    public void IceTile(float time)
    {


        sp.gameObject.AddComponent<Frozen>();


        sp.gameObject.GetComponent<Frozen>()._Value1 = .80f - time;
        sp.gameObject.GetComponent<Frozen>()._Value2 = .976f - time;
    }

}
