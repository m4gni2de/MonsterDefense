using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

[System.Serializable]
public struct SpriteEffect
{
    public string materialName;
    public Shader shader;
    public Material material;
    public MonoBehaviour effect;
    
};

[System.Serializable]
public class SpriteEffects 
{
    public SpriteEffect Hologram = new SpriteEffect
    {
        materialName = "hologram",
    };
}




public class SpriteMaterials : MonoBehaviour
{
    public MonoBehaviour effect;

    public Dictionary<string, SpriteEffect> spriteEffectsList = new Dictionary<string, SpriteEffect>();
    public List<Type> effectsList = new List<Type>();
    

    //private void Awake()
    //{
    //    Type[] types = Assembly.GetExecutingAssembly().GetTypes();
    //    List<Type> myTypes = new List<Type>();
    //    foreach (Type t in types)
    //    {
    //        if (t.Namespace == "Effects")
    //        {
    //            myTypes.Add(t);
    //            effectsList.Add(t);
    //            Debug.Log(t);
    //        }

    //    }
    //}



    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
