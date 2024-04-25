using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visuals : MonoBehaviour
{
    // Objects
    private GameObject _volume;

    // Freeze
    [Header("Freeze")]
    [SerializeField] private FloatVariable _currentTemperature;
    [SerializeField] private float _temperatureThreshold = 20;
    private bool _isCold;
    
    
    void Awake()
    {
        // _volume = 
    }
    
    void Update()
    {
        if (_currentTemperature < _temperatureThreshold)
        {
            EnableFreezeVisual();
        }
        else
        {
            DisableFreezeVisual();
        }
    }
    void EnableFreezeVisual()
    {
        _isCold = true;
    }

    void DisableFreezeVisual()
    {
        _isCold = false;
    }
}
