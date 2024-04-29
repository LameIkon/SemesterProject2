using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class ItemObject : ScriptableObject
{

    public Sprite _ItemDisplayed;
    public bool _Stackable;
    public ItemType _ItemType;
    public Item _Data = new Item();
    public bool _Consumable;

    
    

    [TextArea(10, 15)]
    public string ItemDescription;

    public abstract void Action ();
    public abstract void DisableAction();
  
}



public enum ItemType
{
    Default,
    Light,
    Food,    
    Fuel,
    Journal
}

[System.Serializable]
public class Item
{
    public string _ItemName;
    public int _ID = -1;
    
    public Item()
    {
        _ID = -1;
        _ItemName = "";
    }

    public Item (ItemObject item)
    {
        _ItemName = item.name;
        _ID = item._Data._ID;      
    }
}
