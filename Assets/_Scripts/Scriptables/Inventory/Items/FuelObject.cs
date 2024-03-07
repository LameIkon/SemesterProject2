using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Fuel Object", menuName = "Iventory System/Items/FuelObject")]

public class FuelObject : ItemObject
{

    public int FuelValue;

    public void Awake()
    {
       _ItemType = ItemType.Fuel;
    }
}
