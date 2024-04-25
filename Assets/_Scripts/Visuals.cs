using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visuals : MonoBehaviour
{
    [Header("Objects")] // -- Objects --
    [SerializeField] private GameObject _volume;
    [Space(5f)]

    [Header("Health")] // -- Health Variables --
    [SerializeField] private FloatReference _currentHealth;
    [Space(5f)]
    
    [Header("Freeze")] // -- Freeze Variables --
    [SerializeField] private FloatReference _currentTemperature;
    [SerializeField] private float _temperatureThreshold = 20;
    [SerializeField] private bool _isCold; //
    [Space(5f)]

    [Header("Hunger")] // -- Hunger Variables --
    [SerializeField] private FloatReference _currentHunger;
    //[Space(5f)]
    
    
    // [Header("Heat")] // -- Heat Variables --
    // [SerializeField] private FloatReference _current
    // [Space(5f)]
    
    #region Unity Methods
    private void Awake()
    {
        // _volume = GameObject.FindWithTag("Volume").GetComponent<Volume>();
    }
    
    private void Update()
    {
        switch (_currentTemperature < _temperatureThreshold) {
            case true: EnableFreezeVisual();
                break;
            case false: DisableFreezeVisual();
                break;

        }
    }
    #endregion
    
    #region Health Methods

    #endregion
    
    #region Freeze Methods
    private void EnableFreezeVisual()
    {
        _isCold = true;
    }

    private void DisableFreezeVisual()
    {
        _isCold = false;
    }
    #endregion
}