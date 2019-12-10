using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapInfoMenu : MonoBehaviour
{
    public MapDetails mapDetails;
    public Slider energySlider, mapHPSlider;
    public TMP_Text energyText, energyGenText, mapHPText;
    public SpriteRenderer weatherSprite;

    

    // Start is called before the first frame update
    void Start()
    {
       


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMap()
    {
        



        energySlider.maxValue = mapDetails.mapInformation.playerEnergyMax;
        energySlider.value = mapDetails.mapInformation.playerEnergy;

        mapHPSlider.maxValue = mapDetails.mapInformation.mapHealthMax;
        mapHPSlider.value = mapDetails.mapInformation.mapHealthCurrent;
    }

    private void LateUpdate()
    {
       

        energySlider.value = mapDetails.mapInformation.playerEnergy;
        energySlider.maxValue = mapDetails.mapInformation.playerEnergyMax;

        mapHPSlider.value = mapDetails.mapInformation.mapHealthCurrent;
        mapHPSlider.maxValue = mapDetails.mapInformation.mapHealthMax;


       
        mapHPText.text = mapDetails.mapInformation.mapHealthCurrent.ToString();
        energyText.text = mapDetails.mapInformation.playerEnergy + " / " + mapDetails.mapInformation.playerEnergyMax;
        energyGenText.text = mapDetails.mapInformation.energyRate + " E/s";

        weatherSprite.sprite = mapDetails.weatherSystem.weatherSprite;
    }
}
