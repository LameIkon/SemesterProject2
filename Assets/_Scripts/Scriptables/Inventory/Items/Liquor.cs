using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item Object", menuName = "Iventory System/Items/AlchoholItem")]

public class Liquor : ItemObject
{
    [SerializeField] private FloatVariable _SystemFloatHunger;
    [SerializeField] private FloatVariable _SystemFloatFreeze;

    public int RestoreHunger;
    public int RestoreFreeze;

    public void Awake()
    {
        _ItemType = ItemType.Food;
    }

    public override void Action()
    {
        _SystemFloatHunger.ApplyChange(RestoreHunger);
        _SystemFloatHunger.ApplyChange(RestoreFreeze);
    }

    public override void DisableAction()
    {

    }
}
