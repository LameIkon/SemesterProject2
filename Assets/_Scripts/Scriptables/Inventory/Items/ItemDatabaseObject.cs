using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Item Database", menuName = "Iventory System/Items/Item Database")]
public class ItemDatabaseObject : ScriptableObject, ISerializationCallbackReceiver
{
    public ItemObject[] ItemsArray;
    public Dictionary<ItemObject, int> GetId = new Dictionary<ItemObject, int>();
    public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();


    public void OnAfterDeserialize()
    {
        GetId = new Dictionary<ItemObject, int>();
        GetItem = new Dictionary<int, ItemObject>();

        for (int i = 0; i < ItemsArray.Length; i++)
        {
            GetId.Add(ItemsArray[i], i);
            GetItem.Add(i, ItemsArray[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        
    }
}
