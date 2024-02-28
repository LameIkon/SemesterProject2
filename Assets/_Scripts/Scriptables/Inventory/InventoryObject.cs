using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEditor;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Iventory System/Inventory")]
public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
{
    private ItemDatabaseObject _dataBase;
    public List<InventorySlot> Container = new List<InventorySlot>();
    public string savePath;


    private void OnEnable()
    {
#if UNITY_EDITOR
        _dataBase = (ItemDatabaseObject)AssetDatabase.LoadAssetAtPath("Assets/Resources/ItemDatabase.asset", typeof(ItemDatabaseObject));
#else
        _dataBase = Resources.Load<ItemDatabaseObject>("ItemDatabase");
#endif         
    }


    public void AddItem (ItemObject item, int amount)
    {
       

        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].Item == item)
            {
                Container[i].AddAmount(amount);
               
                return;
            }
        }        
            Container.Add(new InventorySlot(_dataBase.GetId[item], item, amount));
        
    }

    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        bf.Serialize(file, saveData);
        file.Close();

    }

    public void Load()
    {
        if(File.Exists(string.Concat(Application.persistentDataPath, savePath))) 
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);

            JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
            file.Close();
        }
        
    }

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < Container.Count;i++)
        {
            Container[i].Item = _dataBase.GetItem[Container[i].ID];
        }
    }

    public void OnBeforeSerialize()
    {
        
    }
}


[System.Serializable]
public class InventorySlot
{
    public ItemObject Item;
    public int Amount;

    public int ID;


    public InventorySlot(int id, ItemObject item, int amount)
    {
        ID = id;
        Item = item;
        Amount = amount;
      
    }


    public void AddAmount(int value)
    {
        Amount += value;
    }
}