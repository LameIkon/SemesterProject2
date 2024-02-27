using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Food Object", menuName = "Iventory System/Items/FoodObject")]
public class FoodObject : ItemObject
{

    public int RestoreHungerValue;

    public void Awake()
    {
         ItemType = ItemType.Food;
    }

}
