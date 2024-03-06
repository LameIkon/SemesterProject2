using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class ItemObject : ScriptableObject
{

     public Sprite ItemDisplayed;
     public bool Stackable;
     public ItemType ItemType;
     public Item data = new Item();
    

    [TextArea(10, 15)]
    public string ItemDescription;


}

public enum ItemType
{
    Default,
    Food,    
    Fuel,
    Head,
    Body,
    Legs,
    Shoes,
    MainHand,
    OffHand
}

[System.Serializable]
public class Item
{
    public string ItemName;
    public int ID = -1;
    
    public Item()
    {
        ID = -1;
        ItemName = "";
    }

    public Item (ItemObject item)
    {
        ItemName = item.name;
        
        
    }
}
