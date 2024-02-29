using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class ItemObject : ScriptableObject
{

     public Sprite ItemDisplay;
     public ItemType ItemType;
     public int ID;

    [TextArea(10, 15)]
    public string ItemDescription;


}

public enum ItemType
{
    Food,
    Clothing,
    Fuel
}

[System.Serializable]
public class Item
{
    public string ItemName;
    public int ID;
    

    public Item (ItemObject item)
    {
        ItemName = item.name;
        ID = item.ID;
        
    }
}
