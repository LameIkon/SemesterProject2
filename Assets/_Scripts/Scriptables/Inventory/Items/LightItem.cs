using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New light Object", menuName = "Iventory System/Items/LightItem")]
public class LightItem : ItemObject
{
    public void Awake()
    {
        _ItemType = ItemType.Light;
    }

    public override void Action()
    {
       LanternDisabler._LanternSTATIC.SetActive(true);
    }

    public override void DisableAction()
    {
        LanternDisabler._LanternSTATIC.SetActive(false);
    }
}