using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Iventory System/Items/Equipment")]
public class EquipmentObject : ItemObject
{

    public int ColdResistance;

    public void Awake()
    {
        ItemType = ItemType.Body;
    }
}
