using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using JetBrains.Annotations;
using System.Runtime.Serialization;
using UnityEngine.UIElements;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Iventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public ItemDatabaseObject _ItemsDataBase;
    public string _SavePath;
    public Inventory _Container;
    public InventorySlot[] GetSlots { get { return _Container._Slots; } }
    // public InventorySlot[] SetSlots { set; private get; }

    // public ChestFiller _InventoryFiller;

    public void InitializeInventory(InventorySlot[] slots, ItemDatabaseObject database, Inventory inventory)
    {
        _ItemsDataBase = database;

        for (int i = 0; i < slots.Length; i++)
        {
            AddItem(slots[i]._Item, slots[i]._Amount);
        }
    }

    //private void OnEnable()
    //{
    //    if(_InventoryFiller != null)
    //    {
    //        InitializeInventory(_InventoryFiller.ReturnSlots());
    //    }
    //}

    public bool AddItem (Item item, int amount)
    {
        try
        {
            InventorySlot slot = FindItemOnInventory(item);

            if (EmptySlotCount <= 0)
            {
                return false;
            }


            if (!_ItemsDataBase._ItemObjects[item._ID]._Stackable || slot == null)
            {
                SetEmptySlot(item, amount);
                return true;
            }
            slot.AddAmount(amount);
            return true;
        }
        catch (System.IndexOutOfRangeException)
        {
            return false;
        }
    }

    public int EmptySlotCount
    {
        get 
        {
           int counter = 0;
            for (int i = 0; i < GetSlots.Length; i++)
            {
                if (GetSlots[i]._Item._ID <= -1)
                {                
                  counter++;
                }
            }
            return counter;
        }
    }

    public InventorySlot FindItemOnInventory(Item item) 
    { 
        for (int i = 0;i < GetSlots.Length;i++)
        {
            if (GetSlots[i]._Item._ID == item._ID)
            {
                return GetSlots[i];  
            }
        }
        return null;
     
    }


    public InventorySlot SetEmptySlot(Item item, int amount)
    {
        for(int i = 0; i < GetSlots.Length; i++)
        {
            if (GetSlots[i]._Item._ID <= -1)
            {
                GetSlots[i].UpdateSlot(item, amount);
                return GetSlots[i];
            }
        }

        //SETUP FOR WHEN INVENTORY IS FULL
        return null;
    }

    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        if (item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject))
        {
            InventorySlot temp = new InventorySlot(item2._Item, item2._Amount);
            item2.UpdateSlot(item1._Item, item1._Amount);
            item1.UpdateSlot(temp._Item, temp._Amount);
        }

    }


    public void RemoveItem(Item item)
    {
        for (int i = 0; i < GetSlots.Length;i++)
        {
            if (GetSlots[i]._Item == item)
            {
                GetSlots[i].UpdateSlot(null, 0);
            }
        }
    }

    [ContextMenu("Save")]
    public void Save()
    {
      
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, _SavePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, _Container);
        stream.Close();

    }

    [ContextMenu("Load")]
    public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, _SavePath))) 
        {
       
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, _SavePath), FileMode.Open, FileAccess.Read);
            Inventory newContainer = (Inventory)formatter.Deserialize(stream);

            for (int i = 0; i < GetSlots.Length;i++)
            {
                GetSlots[i].UpdateSlot(newContainer._Slots[i]._Item, newContainer._Slots[i]._Amount);
            }

            stream.Close();
        }
        
    }


    [ContextMenu("Clear")]
    public void Clear()
    {
        _Container.Clear();
    }    
}

[System.Serializable]
public class Inventory
{
    public InventorySlot[] _Slots;  
        //new InventorySlot[24];

    public Inventory(InventorySlot[] slots)
    {
        _Slots = slots;
    }

    public Inventory() 
    {
        _Slots = new InventorySlot[4];
    }
    public void Clear()
    {
        for (int i = 0; i < _Slots.Length; i++)
        {
            if (_Slots[i] != null)
            {
                _Slots[i].RemoveItem();
            }
            
        }

    }
}

public delegate void SlotUpdated(InventorySlot slot);

[System.Serializable]
public class InventorySlot
{
    [System.NonSerialized]
    public UserInterface _Parent;
    [System.NonSerialized]
    public GameObject _SlotDisplay;
    [System.NonSerialized]
    public SlotUpdated _OnAfterUpdate;
    [System.NonSerialized]
    public SlotUpdated _OnBeforeUpdate;


    public ItemType[] _AllowedItems = new ItemType[0];    
    public Item _Item = new Item ();
    public int _Amount;
   
    public ItemObject ItemObject
    {
        get
        { 
            if(_Item._ID >= 0)
            {
                return _Parent._Inventory._ItemsDataBase._ItemObjects[_Item._ID];
            }
           return null;
        } 
    }
    public InventorySlot()
    {

        UpdateSlot(new Item(), 0);
      
    }

    public InventorySlot(Item item, int amount)
    {
       
       UpdateSlot(item, amount);
    }

    public void UpdateSlot (Item item, int amount)
    { 
        if(_OnBeforeUpdate != null)
        {
            _OnBeforeUpdate.Invoke(this);
        }
       _Item = item;
       _Amount = amount; 

        if(_OnAfterUpdate != null)
        {
            _OnAfterUpdate.Invoke(this);
        }
    }

    public void RemoveItem()
    {
        UpdateSlot (new Item(), 0);
    }

    public void AddAmount(int value)
    {
        UpdateSlot(_Item, _Amount += value);
    }

    public bool CanPlaceInSlot(ItemObject itemObject)
    {
        if (_AllowedItems.Length <= 0 || itemObject == null || itemObject._Data._ID < 0) 
        {
            return true;
        }

        for (int i = 0; i < _AllowedItems.Length; i++)
        {
            if (itemObject._ItemType == _AllowedItems[i])
                return true;
        }
        return false;
    }
}