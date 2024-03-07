using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Food Object", menuName = "Iventory System/Items/FoodObject")]
public class FoodObject : ItemObject
{

    [SerializeField] private FloatVariable _hungerPoints;

    public int RestoreHungerValue;

    public void Awake()
    {
         _ItemType = ItemType.Food;
    }


    public void Eat() 
    {
        _hungerPoints.ApplyChange(RestoreHungerValue);
    }
}
 