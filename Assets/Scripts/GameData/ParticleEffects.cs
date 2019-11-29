using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticleEffects : MonoBehaviour
{
    public GameObject magicRing, explosiveHit, lineParticle, powerCleave, spray, energyBullet;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //call this to summon a magic ring around the called Tower
    public void MagicRing(Tower t)
    {
        var x = Instantiate(magicRing, t.gameObject.transform.position, Quaternion.Euler(-90f, 0f, 0f));
        t.abilityAuraActive = true;
        t.abilityAura = x;
    }
}
