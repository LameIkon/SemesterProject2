using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class Visuals : MonoBehaviour
{
    [Header("Objects")] 
    [SerializeField] private Volume _volume;
    [Space(5f)]
    
    [Header("Freeze")]
    [SerializeField] private FloatReference _currentTemperature;
    [SerializeField] private float _threshold;
    [SerializeField] private float _changeRate;
    [SerializeField] private float _temperatureVolumeAboveThreshold;
    [SerializeField] private float _temperatureVolumeBelowThreshold;
    [SerializeField] protected bool _isCold;
    [CanBeNull] private ClampedFloatParameter _cfp;
    private float _defaultTVAboveT;
    private float _defaultTVBelowT;
    private bool _defaulter;
    private WhiteBalance _whiteBalance;
    private Vignette _vignette;
    [Space(5f)]

    [Header("Health")] 
    [SerializeField] private FloatReference _currentHealth;
    [SerializeField] private float _healthThreshold;
    [Space(5f)]
    
    [Header("Hunger")] 
    [SerializeField] private FloatReference _currentHunger;
    [SerializeField] private float _hungerThreshold;
    
    #region Unity Methods
    
        private void Awake()
        {
            #region Freeze Variables
            
                _defaulter = true; // Assures that 

                _defaultTVAboveT = _temperatureVolumeAboveThreshold; // For resetting _temperatureVolumeAboveThreshold
                _defaultTVBelowT = _temperatureVolumeBelowThreshold; // For resetting _temperatureVolumeBelowThreshold
                
                _cfp = new ClampedFloatParameter(_defaultTVAboveT, _defaultTVBelowT, _defaultTVAboveT, true);
                
                WhiteBalance wb; 
                Vignette vig;

                switch (_volume.profile.TryGet<WhiteBalance>(out wb)) { case true: _whiteBalance = wb; break; }
                switch (_volume.profile.TryGet<Vignette>(out vig)) { case true: _vignette = vig; break; }
                
            #endregion
        }

        private void Update()
        {
            // print(_currentTemperature.Value);
            _whiteBalance.temperature.SetValue(_cfp);
            
            switch (_currentTemperature < _threshold) {
                case true: EnableFreezeVisual(); break;
                case false: DisableFreezeVisual(); break;
            }
        }
        
    #endregion
    
    #region Freeze Methods
    
        protected virtual void EnableFreezeVisual()
        {
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