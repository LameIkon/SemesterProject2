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
    private GameObject _Player;
    [SerializeField] GameObject _lanternPrefab;
    private GameObject _light;

    public void Awake()
    {
        _ItemType = ItemType.Light;
    }

    public void OnEnable()
    {
        _Player = GameObject.FindGameObjectWithTag("Player");
        if (_light == null)
        {
            _light = Instantiate(_lanternPrefab, _Player.transform);
        }
       
    }


    public override void Action()
    {
       _light.SetActive(true);
    }

    public override void DisableAction()
    {
        _light.SetActive(false);
    }
}