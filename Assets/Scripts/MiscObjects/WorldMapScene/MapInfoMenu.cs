using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapInfoMenu : MonoBehaviour
{
    private MapDetails mapDetails;
    public Slider energySlider;
    public TMP_Text energyText, energyGenText;

    // Start is called before the first frame update
    void Start()
    {
        mapDetails = GetComponentInParent<MapDetails>();
        energySlider.GetComponent<Slider>();


        energySlider.maxValue = mapDetails.mapInformation.playerEnergyMax;
        energySlider.value = mapDetails.mapInformation.playerEnergy;


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        energySlider.value = mapDetails.mapInformation.playerEnergy;
        energyText.text = mapDetails.mapInformation.playerEnergy + " / " + mapDetails.mapInformation.playerEnergyMax;
        energyGenText.text = mapDetails.mapInformation.energyRate + " E/s";
    }
}
