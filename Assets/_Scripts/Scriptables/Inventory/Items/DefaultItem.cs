using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New default Object", menuName = "Iventory System/Items/DefaultItem")]
public class DefaultItem : ItemObject
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
