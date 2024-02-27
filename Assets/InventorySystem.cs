using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : ScriptableObject
{

    [SerializeField, Header("The Inventory")] private InventoryFiller _filler; 
    private List<GameItem> _inventory;


    void Awake()
    {
        if (_filler != null)
        {
            InitializeInventory(_filler.ItemFiller());
        }
    }
    private void InitializeInventory(GameItem[] gameItems)
    {
        foreach (GameItem item in gameItems) 
        {
            AddItem(item);
        }
    }

    public void AddItem(GameItem item) 
    {
        _inventory.Add(item);
    }

    public void RemoveItem(GameItem item) 
    {
        _inventory.Remove(item);
    }
    

    public void RemoveAllItems() 
    {
        _inventory.Clear();
    }

    public List<GameItem> OpenInventory() 
    {
        return _inventory;
    }

}
