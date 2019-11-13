using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherSystem : MonoBehaviour
{
    public MapWeather weather;
    public MapDetails mapDetails;
    public GameObject mapCanvas, rainDrop;

    //the audio source clips for both weather and wind. they are objects on this script as opposed to objects on each different weather
    public AudioSource weatherClip, windClip;
    //intensity is first set from the MapDetails script from a random number. Eventually will change
    public int intensity;

    public WindZone wind;

    public Sprite weatherSprite;
    public Sprite rainSprite, snowSprite, fogSprite, sunSprite;

    public GameObject rain, snow, fog, sun;

    //lists of the audio clips for the different weather
    public List<AudioClip> rainAudioClips;
    public List<AudioClip> windAudioClips;


    

    public void StartWeather(MapDetails map)
    {
        mapDetails = map;
        weather = map.mapWeather;
        

        if (weather == MapWeather.Snow)
        {
            Snow();
        }

        if (weather == MapWeather.Rain)
        {
            Rain();
        }

        if (weather == MapWeather.Sunny)
        {
            Sun();
        }

        wind.windMain = 1 * (1 +intensity);
        wind.windPulseMagnitude = 3 * (1 + intensity);
        weatherClip.Play();
        windClip.Play();
    }



    public void Snow()
    {
        ParticleSystem ps = snow.GetComponent<ParticleSystem>();
        var emission = ps.emission;
        emission.rateOverTime = (200 + (100 * intensity));

        


        snow.SetActive(true);
        weatherSprite = snowSprite;
        windClip.clip = windAudioClips[intensity];
        windClip.clip = windAudioClips[0];

        foreach (MapTile tile in mapDetails.allTiles)
        {
            if (tile.tileAtt != TileAttribute.Fire)
            {
                tile.StartSnow(intensity);

                if (tile.isRoad)
                {
                    tile.snowTile.GetComponent<SpriteRenderer>().sortingLayerName = "Pathways";
                }
            }
        }
    }


    public void Rain()
    {
        rain.SetActive(true);


        weatherClip.clip = rainAudioClips[intensity];
        windClip.clip = windAudioClips[intensity];
        weatherSprite = rainSprite;

        StartCoroutine(RainDrops());
    }

    public void Sun()
    {
        sun.SetActive(true);
        weatherSprite = sunSprite;
    }


    private void Update()
    {
        
    }


    public IEnumerator RainDrops()
    {
        

        do
        {
            var location1 = mapDetails.allTiles[0].GetComponent<Renderer>();
            var location2 = mapDetails.allTiles[mapDetails.allTiles.Count - 1].GetComponent<Renderer>();

            var spawnLocation = GetComponent<Renderer>();

            float x1 = location1.transform.position.x;
            float x2 = location2.transform.position.x;
            float y1 = location1.transform.position.y;
            float y2 = location2.transform.position.y;


            var spawnPoint = new Vector2(Random.Range(x1, x2), Random.Range(y1, y2));

            var x = Instantiate(rainDrop, spawnPoint, Quaternion.identity);
            x.transform.SetParent(mapCanvas.transform, true);
            x.transform.position = spawnPoint;
            x.GetComponent<RainDrop>().StartCoroutine("RainDropAnimation");
            yield return new WaitForSeconds(.0015f / (1 + intensity));
        } while (true);
    }
}


//class to house different variables for different weather effects
public class Weather
{
    public MapWeather MapWeather;
    public int intensity;
    public Sprite sprite;
    public string direction;

}


//class that changes stats of some monsters based on the weather
public class WeatherBoosts
{

}
