using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ConsumableType
{
    Shard,
}


[CreateAssetMenu]
public class ConsumableItem : ScriptableObject
{
    public string itemName;
    //public int id;
    public string description;
    public float cost;
    public Sprite sprite;
    public ConsumableType itemType;
}
