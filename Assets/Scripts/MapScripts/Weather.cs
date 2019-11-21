using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
//class to house different variables for different weather effects
public class WeatherEffects
{
    public MapWeather MapWeather;
    public Sprite sprite;
    public string name;

}
[System.Serializable]
public class AllWeather
{
    public WeatherEffects Snow = new WeatherEffects
    {
        MapWeather = MapWeather.Snow,
        name = "Snow",
    };

    public WeatherEffects Rain = new WeatherEffects
    {
        MapWeather = MapWeather.Rain,
        name = "Rain",
    };

    public WeatherEffects Sun = new WeatherEffects
    {
        MapWeather = MapWeather.Sunny,
        name = "Sun",
    };
}


//class that changes stats of some monsters based on the weather
public class Weather: MonoBehaviour
{
    public Dictionary<string, WeatherEffects> allWeatherDict = new Dictionary<string, WeatherEffects>();

    public AllWeather allWeather;
   


    private void Awake()
    {
        allWeatherDict.Add(allWeather.Snow.name, allWeather.Snow);
        allWeatherDict.Add(allWeather.Rain.name, allWeather.Rain);
        allWeatherDict.Add(allWeather.Sun.name, allWeather.Sun);
    }
}
