using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Clothing Object", menuName = "Iventory System/Items/ClothingObject")]
public class ClothingObject : ItemObject
{

    public int ColdResistance;

    public void Awake()
    {
        ItemType = ItemType.Clothing;
    }
}
