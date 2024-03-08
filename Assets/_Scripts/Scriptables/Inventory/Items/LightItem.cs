using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[CreateAssetMenu(fileName = "New light Object", menuName = "Iventory System/Items/LightItem")]
public class LightItem : ItemObject
{
    private GameObject _light;

    public void Awake()
    {
        _ItemType = ItemType.Light;        
    }

    public void OnEnable()
    {
        if (_light != null)
        {
            _light = GameObject.FindGameObjectWithTag("PlayerLight");
        }        
    }


    public override void Action()
    {
       _light.SetActive(true);        
    }
}