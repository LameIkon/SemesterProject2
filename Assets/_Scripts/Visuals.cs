using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Visuals : MonoBehaviour
{
    [Header("Objects")] 
    [SerializeField] private Volume _volume;
    [Space(5f)]

    // ------------------------------------------------------------------------------------------------ \\
    
    [Header("Health")] 
    [SerializeField] private FloatReference _currentHealth;
    [SerializeField] private float _thresholdHealth;
    [SerializeField] private float _changeRateHealth;
    [SerializeField] private float _maximumIntensityRange;
    [SerializeField] private float _minimumIntensityRange;
    [SerializeField] private float _currentIntensity;
    [SerializeField] private bool _isActive;
    private ClampedFloatParameter _cfpHealth;
    private Vignette _vignette;
    [Space(5f)]
    
    // ------------------------------------------------------------------------------------------------ \\
    
    [Header("Temperature")]
    [SerializeField] private FloatReference _currentTemperature;
    [SerializeField] private Color _colorAboveThreshold; 
    [SerializeField] private Color _colorBelowThreshold;
    [SerializeField] private float _thresholdTemperature;
    [SerializeField] private float _changeRateTemperature;
    [SerializeField] private float _volumeAboveThresholdTemperature;
    [SerializeField] private float _volumeBelowThresholdTemperature;
    [SerializeField] protected bool _isCold;
    private float _defaultTVAboveT;
    private float _defaultTVBelowT;
    private bool _defaulter;
    private Image _temperatureIcon;
    private ClampedFloatParameter _cfpTemperature;
    private WhiteBalance _whiteBalance;
    [Space(5f)]
    
    // ------------------------------------------------------------------------------------------------ \\
    
    [Header("Hunger")] 
    [SerializeField] private FloatReference _currentHunger;
    [SerializeField] private float _hungerThreshold;
    
    // ------------------------------------------------------------------------------------------------ \\
    
    #region Unity Methods
    
        private void Awake()
        {
            #region Health Variables

                _currentIntensity = _minimumIntensityRange;                                                                                          // Sets the current range to the minimum
                _cfpHealth = new ClampedFloatParameter(_currentIntensity, _minimumIntensityRange, _maximumIntensityRange, _isActive);   // Sets the Volume Vignette to the current 
                Vignette vig;                                                                                                                        // Used to initialize Volume components
                switch (_volume.profile.TryGet<Vignette>(out vig)) { case true: _vignette = vig; break; }                                            // Initializes Volume components
            
            #endregion
            
            #region Temperature Variables

                _temperatureIcon = GameObject.FindWithTag("TemperatureImage").GetComponent<Image>();                                               // Initializes the temperature icon
                _defaulter = true;                                                                                                                 // Assures only one use of the if-statement in DisableFreezeVisual() 
                _defaultTVAboveT = _volumeAboveThresholdTemperature;                                                                               // To reset _temperatureVolumeAboveThreshold after being changed
                _defaultTVBelowT = _volumeBelowThresholdTemperature;                                                                               // To reset _temperatureVolumeBelowThreshold after being changed
                _cfpTemperature = new ClampedFloatParameter(_defaultTVAboveT, _defaultTVBelowT, _defaultTVAboveT, true);   // Sets the Volume Temperature to the default 
                WhiteBalance wb;                                                                                                                   // Used to initialize Volume components
                switch (_volume.profile.TryGet<WhiteBalance>(out wb)) { case true: _whiteBalance = wb; break; }                                    // Initializes Volume components

            #endregion
        }

        private void Update()
        {
            #region Health Variables

                _vignette.intensity.SetValue(_cfpHealth);            // Avoids NullReferences
                switch (_currentHealth < _thresholdHealth)       
                {
                    case true: EnableLowHealthVisual(); break;
                    case false: DisableLowHealthVisual(); break;
                }

            #endregion
            
            #region Temperature Variables
            
                _whiteBalance.temperature.SetValue(_cfpTemperature);     // Avoids NullReferences
                switch (_currentTemperature < _thresholdTemperature) 
                { 
                    case true: EnableFreezeVisual(); break;
                    case false: DisableFreezeVisual(); break;
                }
                
            #endregion
        }
        
    #endregion
    
    #region Health Methods
    
        private void EnableLowHealthVisual()
        {
            _isActive = true;
            bool aboveThreshold = false;
            
            if (_currentIntensity > _maximumIntensityRange) { aboveThreshold = true; } // Omskrives på en anden måde, evt lave en metoder der gør boolen til true
            if (_currentIntensity < _minimumIntensityRange) { aboveThreshold = false; }

            switch (aboveThreshold)
            {
                case true: _currentIntensity -= _changeRateHealth; print("true"); break; 
                case false: _currentIntensity += _changeRateHealth; print("false"); break;
            }
        }
    
        private void DisableLowHealthVisual()
        {
            _isActive = false;
        }
        
    #endregion
    
    #region Temperature Methods
    
        protected virtual void EnableFreezeVisual()
        {
            _temperatureIcon.color = _colorBelowThreshold;
            
            _defaulter = false; // Assures only one use of the if-statement in DisableFreezeVisual()
            _isCold = true;
            
            _volumeBelowThresholdTemperature = _defaultTVBelowT; 
            _cfpTemperature = new ClampedFloatParameter(_volumeAboveThresholdTemperature, _defaultTVBelowT, _defaultTVAboveT, true);
            
            switch (_volumeAboveThresholdTemperature > _defaultTVBelowT)
            {
                case true: _volumeAboveThresholdTemperature -= _changeRateTemperature * Time.deltaTime; break;
            }
        }

        protected virtual void DisableFreezeVisual()
        {
            _temperatureIcon.color = _colorAboveThreshold;
            _isCold = false;

            switch (_defaulter) // Will only be accessed at the start of the game
            {
                case true: _cfpTemperature = new ClampedFloatParameter(_volumeAboveThresholdTemperature, _defaultTVBelowT, _defaultTVAboveT, true); 
                    return;
            }
            
            _volumeAboveThresholdTemperature = _defaultTVAboveT;
            _cfpTemperature = new ClampedFloatParameter(_volumeBelowThresholdTemperature, _defaultTVBelowT, _defaultTVAboveT, true);
            
            switch (_volumeBelowThresholdTemperature < _defaultTVAboveT) 
            {
                case true: _volumeBelowThresholdTemperature += _changeRateTemperature * Time.deltaTime; break;
            }
        }
        
    #endregion
    
    
    #region Hunger Methods
    
        private void EnableHungerVisual()
        {
        
        }

        private void DisableHungerVisual()
        {
        
        }
        
    #endregion
}

public sealed class HeatAbsorption : Visuals
{
    // [Header("Heat")] 
    [SerializeField] private FloatReference _currentTemperature;
    
    
    #region Unity Methods
    
        private void Awake()
        {
        
        }
    
        private void Update()
        {
            switch (_isCold)
            {
                case true: this.EnableFreezeVisual(); break;
                case false: this.DisableFreezeVisual(); break;
            }   
        }
        
    #endregion
    
    #region Heat Absorption Methods
    
        private void EnableAbsorptionVisual()
        {
            
        }
    
        private void DisableAbsorptionVisual()
        {
        
        }
    
    #endregion
    
    protected override void EnableFreezeVisual()
    {
        
    }

    protected override void DisableFreezeVisual() {
        
    }
}