using JetBrains.Annotations;
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

    [Header("Health")] 
    [SerializeField] private FloatReference _currentHealth;
    [SerializeField] private float _healthThreshold;
    private Vignette _vignette;
    [Space(5f)]
    
    [Header("Temperature")]
    [SerializeField] private FloatReference _currentTemperature;
    [SerializeField] private Color _colorAboveThreshold; 
    [SerializeField] private Color _colorBelowThreshold;
    [SerializeField] private float _temperatureThreshold;
    [SerializeField] private float _changeRate;
    [SerializeField] private float _temperatureVolumeAboveThreshold;
    [SerializeField] private float _temperatureVolumeBelowThreshold;
    [SerializeField] protected bool _isCold;
    private float _defaultTVAboveT;
    private float _defaultTVBelowT;
    private bool _defaulter;
    private Image _temperatureIcon;
    private ClampedFloatParameter _cfp;
    private WhiteBalance _whiteBalance;
    [Space(5f)]
    
    [Header("Hunger")] 
    [SerializeField] private FloatReference _currentHunger;
    [SerializeField] private float _hungerThreshold;
    
    #region Unity Methods
    
        private void Awake()
        {
            #region Health Variables
            
                Vignette vig;   // Used to initialize Volume components
            
                switch (_volume.profile.TryGet<Vignette>(out vig)) { case true: _vignette = vig; break; }   // Initializes Volume components
            
            #endregion
            
            #region Temperature Variables

                _temperatureIcon = GameObject.FindWithTag("TemperatureImage").GetComponent<Image>();
            
                _defaulter = true;  // Assures only one use of the if-statement in DisableFreezeVisual() 

                _defaultTVAboveT = _temperatureVolumeAboveThreshold; // To reset _temperatureVolumeAboveThreshold after being changed
                _defaultTVBelowT = _temperatureVolumeBelowThreshold; // To reset _temperatureVolumeBelowThreshold after being changed
                
                _cfp = new ClampedFloatParameter(_defaultTVAboveT, _defaultTVBelowT, _defaultTVAboveT, true);   // Sets the Volume Temperature to the default 
                
                WhiteBalance wb;    // Used to initialize Volume components

                switch (_volume.profile.TryGet<WhiteBalance>(out wb)) { case true: _whiteBalance = wb; break; } // Initializes Volume components

            #endregion
        }

        private void Update()
        {
            
            _whiteBalance.temperature.SetValue(_cfp); // Avoids a NullReference
            
            switch (_currentTemperature < _temperatureThreshold) {
                case true: EnableFreezeVisual(); break;
                case false: DisableFreezeVisual(); break;
            }
        }
        
    #endregion
    
    #region Temperature Methods
    
        protected virtual void EnableFreezeVisual()
        {
            _temperatureIcon.color = _colorBelowThreshold;
            
            _defaulter = false; // Assures only one use of the if-statement in DisableFreezeVisual()
            _isCold = true;
            
            _temperatureVolumeBelowThreshold = _defaultTVBelowT; 
            _cfp = new ClampedFloatParameter(_temperatureVolumeAboveThreshold, _defaultTVBelowT, _defaultTVAboveT, true);
            
            switch (_temperatureVolumeAboveThreshold > _defaultTVBelowT)
            {
                case true: 
                    _temperatureVolumeAboveThreshold -= _changeRate; 
                    break;
            }
        }

        protected virtual void DisableFreezeVisual()
        {
            _temperatureIcon.color = _colorAboveThreshold;
            _isCold = false;
            
            if (_defaulter) // Will only be accessed at the start of the game
            {
                _cfp = new ClampedFloatParameter(_temperatureVolumeAboveThreshold, _defaultTVBelowT, _defaultTVAboveT, true);
                return;
            }
            
            _temperatureVolumeAboveThreshold = _defaultTVAboveT;
            _cfp = new ClampedFloatParameter(_temperatureVolumeBelowThreshold, _defaultTVBelowT, _defaultTVAboveT, true);
            
            switch (_temperatureVolumeBelowThreshold < _defaultTVAboveT) 
            {
                case true: 
                    _temperatureVolumeBelowThreshold += _changeRate; 
                    break;
            }
        }
        
    #endregion
    
    #region Health Methods
    
        private void EnableLowHealthVisual()
        {
        
        }
    
        private void DisableLowHealthVisual()
        {
        
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