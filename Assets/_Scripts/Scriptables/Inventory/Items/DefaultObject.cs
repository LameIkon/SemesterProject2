using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Default Object", menuName = "Iventory System/Items/DefaultObject")]

public class DefaultObject : ItemObject
{
    public ItemObject Item;
    

    public void Awake()
    {
        ItemType = ItemType.Default;
    }
}
