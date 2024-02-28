using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Item Database", menuName = "Iventory System/Items/Item Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] ItemsArray;

    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();


    public void OnAfterDeserialize()
    {    
       
        for (int i = 0; i < ItemsArray.Length; i++)
        {
            ItemsArray[i].ID = i;
            GetItem.Add(i, ItemsArray[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        GetItem = new Dictionary<int, ItemObject>();
    }
}