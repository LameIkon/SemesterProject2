using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item Object", menuName = "Iventory System/Items/RestoreItem")]
public class RestoreItem : ItemObject
{

    [SerializeField] private FloatVariable _SystemFloat;

    public int RestoreValue;

    public void Awake()
    {
         _ItemType = ItemType.Food;
    }

    public override void Action() 
    {
        _SystemFloat.ApplyChange(RestoreValue);
        Debug.Log(_SystemFloat + " Value restored: " + RestoreValue);
    }
}
 