using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Item Database", menuName = "Iventory System/Items/Item Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] _ItemObjects;

    // public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();


    [ContextMenu("Update ID's")]
    public void UpdateID()
    {
        for (int i = 0; i < _ItemObjects.Length; i++)
        {
            if (_ItemObjects[i]._Data._ID != i)
            {
                _ItemObjects[i]._Data._ID = i;
            }          
        }
    }

    public void OnAfterDeserialize()
    {    
        UpdateID();
    }

    public void OnBeforeSerialize()
    {
       
    }
}