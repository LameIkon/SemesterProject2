using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InventoryFiller : ScriptableObject
{
    public abstract GameItem[] ItemFiller();

}
