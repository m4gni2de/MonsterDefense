using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleWeather : MonoBehaviour
{
    public ParticleSystem ps;
    public List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();

    // Start is called before the first frame update
    void Start()
    {
       
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void OnParticleTrigger()
    {

        Debug.Log("here");

        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = enter[i];
            //take damage
            enter[i] = p;
        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        Debug.Log(numEnter);
    }


}
