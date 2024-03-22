using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Object", menuName = "Iventory System/Items/WoodItem")]
public class Wood : ItemObject
{
   

    public void Awake()
    {
        _ItemType = ItemType.Fuel;
    }

    public override void Action()
    {
        
    }

    public override void DisableAction()
    {

    }
}
