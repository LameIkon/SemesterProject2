using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New map Object", menuName = "Iventory System/Items/MapItem")]
public class MapItem : ItemObject
{
    public void Awake()
    {
        _ItemType = ItemType.Default;
    }

    public override void Action()
    {
       
    }

    public override void DisableAction()
    {
        
    }
}
