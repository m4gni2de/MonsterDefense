using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MapTileAnimations : MonoBehaviour
{
    public SpriteRenderer sp;
    public SpriteRenderer topSprite;
    public float animationTime;

    //the script in the Game Manager that contains all of the special tiles
    public Maps maps;

    //keeps track of all of the extra components added to this tiles
    public List<GameObject> attList = new List<GameObject>();
    public List<Component> tileEffects = new List<Component>();

    public bool isRoad;

    public SpriteMaterials effects;  

    private void Awake()
    {
        maps = GameManager.Instance.GetComponent<Maps>();
    }

    // Start is called before the first frame update
    void Start()
    {
        effects = GameManager.Instance.GetComponent<SpriteMaterials>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //use this to clear all of the tile's added extras and reset it back to a basic tile
    public void ClearTile()
    {
        sp.sprite = null;
        //sp.color = Color.white;
        StopAllCoroutines();


        if (!GetComponentInParent<MapTile>().isRoad)
        {
            topSprite.sprite = null;
            //topSprite.color = Color.white;
        }


        //destroy each extra tile component, and then clear the list of components
        for (int i = 0; i < attList.Count; i++)
        {
            Destroy(attList[i]);
        }

        attList.Clear();

        //destroy the added effect components on a tile
        for (int e = 0; e < tileEffects.Count; e++)
        {

            Destroy(sp.gameObject.GetComponent(tileEffects[e].GetType().ToString()));

        }

        tileEffects.Clear();
        


    }

    public void TileAnimation(TileAttribute att, bool IsRoad)
    {
        


        isRoad = IsRoad;

        if (att == TileAttribute.None)
        {
            
        }

        if (att == TileAttribute.Water)
        {
            float rand = UnityEngine.Random.Range(.12f, .14f);
            animationTime = rand;
            StartCoroutine(WaterTile(animationTime));
        }

        if (att == TileAttribute.Fire)
        {
            float rand = UnityEngine.Random.Range(.04f, .10f);
            animationTime = rand;
            StartCoroutine(FireTile(animationTime));
        }

        if (att == TileAttribute.Magic)
        {
            float rand = UnityEngine.Random.Range(.08f, .11f);

            animationTime = rand;
            StartCoroutine(MagicTile(animationTime));
        }


        if (att == TileAttribute.Nature)
        {
            float rand = UnityEngine.Random.Range(.03f, .05f);
            animationTime = rand;
            StartCoroutine(NatureTile(animationTime));
        }


        if (att == TileAttribute.Poison)
        {
            float rand = UnityEngine.Random.Range(.01f, .20f);
            animationTime = rand;
            PoisonTile(rand);
        }

        if (att == TileAttribute.Electric)
        {
            float rand = UnityEngine.Random.Range(.03f, .05f);
            animationTime = rand;
            ElectricTile(rand);
        }

        if (att == TileAttribute.Ice)
        {
            float rand = UnityEngine.Random.Range(.01f, .20f);
            animationTime = rand;
            IceTile(rand);
        }

        if (att == TileAttribute.Shadow)
        {
            float rand = UnityEngine.Random.Range(-.07f, .07f);
            animationTime = rand;
            ShadowTile(rand);
        }

        if (att == TileAttribute.Mechanical)
        {
            float rand = UnityEngine.Random.Range(.01f, .1f);
            animationTime = rand;
            MechanicalTile(rand);
        }

        if (att == TileAttribute.Normal)
        {
            float rand = UnityEngine.Random.Range(.03f, .12f);
            animationTime = rand;
            StartCoroutine(NormalTile(animationTime));
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
        attList.Add(fire.gameObject);
        fire.sortingOrder = topSprite.sortingOrder + 1;
        fire.sortingLayerName = "MapTiles";

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

            yield return new WaitForSeconds(time);

            if (i >= maps.fireTileSprites.Length - 1)
            {
                i = 0;
                tileAge += 1;

                if (tileAge > 12)
                {
                    for (int d = 0; d < 20; d++)
                    {
                        float rand = UnityEngine.Random.Range(-100f, 100f);
                        float randY = UnityEngine.Random.Range(10f, 80f);
                        float scaleX = UnityEngine.Random.Range(10f, 25f);
                        float scaleY = UnityEngine.Random.Range(10f, 25f);

                        ///Location of the enemies that spawn
                        float x1 = GetComponentInParent<MapTile>().transform.position.x - sp.bounds.size.x / 2;
                        float x2 = GetComponentInParent<MapTile>().transform.position.x + sp.bounds.size.x / 2;
                        float y1 = GetComponentInParent<MapTile>().transform.position.y - sp.bounds.size.y / 2;
                        float y2 = GetComponentInParent<MapTile>().transform.position.y + sp.bounds.size.y / 2;

                        var spawnPoint = new Vector2(UnityEngine.Random.Range(x1, x2), UnityEngine.Random.Range(y1, y2));


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



            //float randX1 = UnityEngine.Random.Range(x1, x2);

            //float randX2 = 1 - (randX1 / (GetComponentInParent<MapTile>().transform.position.x + sp.bounds.size.x / 2));


            //Debug.Log(randX1 + "  " + randX2);


            //float y1 = -randX2 * (GetComponentInParent<MapTile>().transform.position.y - sp.bounds.size.y / 2);
            //float y2 = randX2 * (GetComponentInParent<MapTile>().transform.position.y + sp.bounds.size.y / 2);


            //var spawnPoint = new Vector3(randX1, UnityEngine.Random.Range(y1, y2), -5);
            var spawnPoint = new Vector3(UnityEngine.Random.Range(x1, x2), UnityEngine.Random.Range(y1, y2), -5);

            var nature = Instantiate(maps.natureTileBrush[0], spawnPoint, Quaternion.identity, GetComponentInParent<MapTile>().transform);

            attList.Add(nature.gameObject);
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
        //magic.sortingLayerName = "MapTiles";

        //add this component to the list of the tile's extra components, so it can be deleted if the tile is changed
        attList.Add(magic.gameObject);
        topSprite = magic;

        

        //turns the tile in to a smokey looking tile, and sets the smoke level to a random number to give the tiles a uniqueness
        sp.gameObject.AddComponent<SmokeFX>();
        float rand = UnityEngine.Random.Range(.14f, .72f);
        sp.gameObject.GetComponent<SmokeFX>()._Value2 = rand;
        //sp.gameObject.GetComponent<SmokeFX>()._Alpha = sp.gameObject.GetComponent<SmokeFX>()._Value2;

        tileEffects.Add(sp.gameObject.GetComponent<SmokeFX>());

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

                attList.Add(x.gameObject);

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
        //    time = UnityEngine.Random.Range(10f, 20f);
        //    if (i >= 1)
        //    {
        //        i = 0;
        //    }
        //}
    }

   

    public void PoisonTile(float rand)
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

        sp.gameObject.AddComponent<Liquid>();


        sp.gameObject.GetComponent<Liquid>().Heat = .07f;
        sp.gameObject.GetComponent<Liquid>().Speed = .07f;
        sp.gameObject.GetComponent<Liquid>().Light = -2f;
        sp.gameObject.GetComponent<Liquid>().EValue = 1f;

        tileEffects.Add(sp.gameObject.GetComponent<Liquid>());

    }


    public void ElectricTile(float time)
    {
       

        sp.gameObject.AddComponent<Lightning2D>();


        sp.gameObject.GetComponent<Lightning2D>()._Value1 = 47.7f;
        sp.gameObject.GetComponent<Lightning2D>()._Value2 = 2f;

        tileEffects.Add(sp.gameObject.GetComponent<Lightning2D>());
    }


    public void IceTile(float time)
    {


        sp.gameObject.AddComponent<Frozen>();


        sp.gameObject.GetComponent<Frozen>()._Value1 = .80f - time;
        sp.gameObject.GetComponent<Frozen>()._Value2 = .976f - time;

        tileEffects.Add(sp.gameObject.GetComponent<Frozen>());
    }

    public void ShadowTile(float time)
    {
        if (time == 0)
        {
            time = .01f;
        }

        sp.gameObject.AddComponent<SkycloudFX>();


        sp.gameObject.GetComponent<SkycloudFX>()._AutoScrollX = true;
        sp.gameObject.GetComponent<SkycloudFX>()._AutoScrollY = true;
        sp.gameObject.GetComponent<SkycloudFX>()._AutoScrollSpeedX = 0 + time;
        sp.gameObject.GetComponent<SkycloudFX>()._AutoScrollSpeedY = 0 + time;
        sp.gameObject.GetComponent<SkycloudFX>()._Intensity = 1f;
        sp.gameObject.GetComponent<SkycloudFX>()._Zoom = .1f;

        tileEffects.Add(sp.gameObject.GetComponent<SkycloudFX>());

    }

    public void MechanicalTile(float time)
    {
        //if (time == 0)
        //{
        //    time = .01f;
        //}

        sp.gameObject.AddComponent<NewTeleportation2>();


        sp.gameObject.GetComponent<NewTeleportation2>()._Fade = .1f + time;
        sp.gameObject.GetComponent<NewTeleportation2>()._Distortion = .6f + time;

        tileEffects.Add(sp.gameObject.GetComponent<NewTeleportation2>());

    }

    public IEnumerator NormalTile(float time)
    {
        //if (time == 0)
        //{
        //    time = .01f;
        //}

        sp.gameObject.AddComponent<ColorChange>();
        tileEffects.Add(sp.gameObject.GetComponent<ColorChange>());

        for (int i = 0; i < 360; i++) {

            sp.gameObject.GetComponent<ColorChange>()._HueShift = i;
            yield return new WaitForSeconds(time);
                if (i >= 359)
                {
                    i = 0;
                }
        }

    }

}
