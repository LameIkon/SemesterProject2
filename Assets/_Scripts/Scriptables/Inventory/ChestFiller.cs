using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New chest Filler", menuName = "Iventory System/ChestFiller")]

public class ChestFiller : ScriptableObject
{
    public InventorySlot[] _ChestSlots = new InventorySlot[6];


    public InventorySlot[] ReturnSlots ()
    {
        return _ChestSlots;
    }

}
