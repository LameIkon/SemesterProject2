using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;
using JetBrains.Annotations;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Iventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public ItemDatabaseObject _dataBase;    
    public string SavePath;
    public Inventory Container;




    public void AddItem (Item item, int amount)
    {
       

        for (int i = 0; i < Container.Items.Count; i++)
        {
            if (Container.Items[i].Item.ID == item.ID)
            {
                Container.Items[i].AddAmount(amount);
               
                return;
            }
        }        
            Container.Items.Add(new InventorySlot(item, amount));
        
    }


    [ContextMenu("Save")]
    public void Save()
    {
        /*
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, SavePath));
        bf.Serialize(file, saveData);
        file.Close();
        */

        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, SavePath), FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, Container);
        stream.Close();

    }

    [ContextMenu("Load")]
    public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, SavePath))) 
        {
            /*
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, SavePath), FileMode.Open);

            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
            */
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(string.Concat(Application.persistentDataPath, SavePath), FileMode.Open, FileAccess.Read);
            Container = (Inventory)formatter.Deserialize(stream);
            stream.Close();

        }
        
    }


    [ContextMenu("Clear")]
    public void Clear()
    {
        Container = new Inventory();
    }

    
}

[System.Serializable]
public class Inventory
{
    public List<InventorySlot> Items = new List<InventorySlot>();
}


[System.Serializable]
public class InventorySlot
{
    public Item Item;
    public int Amount;

    


    public InventorySlot(Item item, int amount)
    {
       
        Item = item;
        Amount = amount;
      
    }


    public void AddAmount(int value)
    {
        Amount += value;
    }
}