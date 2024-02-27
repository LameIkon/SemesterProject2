using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public abstract class ItemObject : ScriptableObject
{

     public GameObject ItemPrefab;
     public ItemType ItemType;

    [TextArea(10, 15)]
    public string ItemDescription;


}

public enum ItemType
{
    Food,
    Clothing,
    Fuel
}
