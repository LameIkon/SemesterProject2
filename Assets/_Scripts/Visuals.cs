using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class Visuals : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Volume _volume;
    private CinemachineImpulseSource _impulseSource;
    [Space(5f)]

    // ------------------------------------------------------------------------------------------------ \\

    [Header("Health")]
    [SerializeField] private FloatReference _currentHealth;
    [SerializeField] private ColorParameter _vignetteColor;
    [SerializeField] private float _highThresholdHealth;    // 25 
    [SerializeField] private float _lowThresholdHealth;     // 10
    [SerializeField] private float _changeRateHealth;       // 0.24
    [SerializeField] private float _changeRateMultiplier;   // 1.5
    [SerializeField] private float _maximumIntensityRange;  // 0.4
    [SerializeField] private float _minimumIntensityRange;  // 0.1
    private ClampedFloatParameter _cfpHealth;
    private Vignette _vignette;
    private float _currentIntensity;
    private float _defaultIntensity;
    private bool _healthDefaulter;
    [Space(5f)]

    // ------------------------------------------------------------------------------------------------ \\

    [Header("Temperature")]
    [SerializeField] private FloatReference _currentTemperature;
    [SerializeField] private ColorReference _colorAboveThreshold;
    [SerializeField] private ColorReference _colorBelowThreshold;
    [SerializeField] private ColorReference _colorDuringHeat;
    [SerializeField] private float _thresholdTemperature;               // 25
    [SerializeField] private float _changeRateTemperature;              // 100
    [SerializeField] private float _volumeAboveThresholdTemperature;    // -40
    [SerializeField] private float _volumeBelowThresholdTemperature;    // -100
    [SerializeField] private float _volumeForHeat;                      // 60
    private Image _temperatureIcon;
    private ClampedFloatParameter _cfpTemperature;
    private WhiteBalance _whiteBalance;
    private float _currentVolumeTemperature;
    private float _defaultAbove;
    private float _defaultBelow;
    private bool _temperatureDefaulter;
    private bool _aboveThreshold;
    [Space(5f)]

    // ------------------------------------------------------------------------------------------------ \\

    [Header("Hunger")]
    [SerializeField] private FloatReference _currentHunger;
    [SerializeField] private float _hungerThreshold;

    #region Unity Methods
    
        private void Awake()
        {
            #region Health Variables

                _currentIntensity = _minimumIntensityRange;                                                     // Sets the current range to the minimum
                _defaultIntensity = _currentIntensity;                                                          // To reset _currentIntensity after being changed
                _healthDefaulter = true;                                                                        // Ensures only one use of the if-statement in DisableLowHealthVisual()
                Vignette vig;                                                                                   // Used to initialize Volume components
                switch (_volume.profile.TryGet<Vignette>(out vig)) { case true: _vignette = vig; break; }       // Initializes Volume components
                _vignette.color = _vignetteColor;                                                               // Initializes vignette color
                _vignette.active = false;                                                                       // Deactivates the vignette effect by default
            
            #endregion
            
            #region Temperature Variables

                _temperatureIcon = GameObject.FindWithTag("TemperatureImage").GetComponent<Image>();                // Initializes the temperature icon
                _temperatureDefaulter = true;                                                                       // Ensures only one use of the if-statement in DisableFreezeVisual() 
                _defaultAbove = _volumeAboveThresholdTemperature;                                                   // To reset _temperatureVolumeAboveThreshold after being changed
                _defaultBelow = _volumeBelowThresholdTemperature;                                                   // To reset _temperatureVolumeBelowThreshold after being changed
                _cfpTemperature = new ClampedFloatParameter(_defaultAbove, _defaultBelow, _defaultAbove, true);     // Sets the Volume Temperature to the default 
                WhiteBalance wb;                                                                                    // Used to initialize Volume components
                switch (_volume.profile.TryGet<WhiteBalance>(out wb)) { case true: _whiteBalance = wb; break; }     // Initializes Volume components

            #endregion
            
            #region Hunger Variables

                _impulseSource = GameObject.FindWithTag("CameraShaker").GetComponent<CinemachineImpulseSource>();

            #endregion
        }

        private void Update()
        {
            #region Health Variables

                _cfpHealth = new ClampedFloatParameter(_currentIntensity, _minimumIntensityRange, _maximumIntensityRange, true);        // Sets the Volume Vignette to the current intensity
                _vignette.intensity.SetValue(_cfpHealth);                                                                               // Avoids NullReferences
                switch (_currentHealth < _highThresholdHealth)                                                                          // Calls EnableLowHealthVisual() or DisableLowHealthVisual() depending on the Player HP
                {
                    case true: EnableLowHealthVisual(); break;
                    case false: DisableLowHealthVisual(); break;
                }

            #endregion
            
            #region Temperature Variables
            
                _whiteBalance.temperature.SetValue(_cfpTemperature);     // Avoids NullReferences
                switch (_currentTemperature < _thresholdTemperature)     // Calls EnableFreezeVisual() or DisableFreezeVisual depending on the temperature level
                { 
                    case true: EnableFreezeVisual(); break;
                    case false: DisableFreezeVisual(); break;
                }
                
            #endregion

            #region Hunger Variables

                switch (_currentHunger < _hungerThreshold)
                {
                    case true: EnableHungerVisual(); break;
                    case false: DisableHungerVisual(); break;
                }
            
            #endregion
        }
        
    #endregion
    
    #region Health Methods
    
        private void AboveThresholdIsTrue() => _aboveThreshold = true;  

        private void AboveThresholdIsFalse() => _aboveThreshold = false; 
    
        private void EnableLowHealthVisual()
        {
            _vignette.active = true;        // Activates the vignette effect
            _healthDefaulter = false;       // Ensures only one use of the if-statement in DisableLowHealthVisual()
            
            if (_currentIntensity > _maximumIntensityRange) { AboveThresholdIsTrue(); }     // Sets _aboveThreshold to true when the intensity exceeds the maximum range 
            if (_currentIntensity < _minimumIntensityRange) { AboveThresholdIsFalse(); }    // Sets _aboveThreshold to false when the intensity is below the minimum range

            switch (_aboveThreshold)        // Increments or decrements the intensity depending on whether _aboveThreshold is true or false
            {
                case true:
                {
                    _currentIntensity -= _changeRateHealth * Time.deltaTime;
                    if (_currentHealth < _lowThresholdHealth)
                    {
                        _currentIntensity -= _changeRateHealth * _changeRateMultiplier * Time.deltaTime; 
                    }
                    break;   
                }

                case false:
                {
                    _currentIntensity += _changeRateHealth * Time.deltaTime;
                    if (_currentHealth < _lowThresholdHealth)
                    {
                        _currentIntensity += _changeRateHealth * _changeRateMultiplier * Time.deltaTime; 
                    }
                    break; 
                }
            }
            
        }

        private void DisableLowHealthVisual()
        {
            if (_healthDefaulter) { return; }                           // Ensures that the rest of the method won't be called at the start of the game                 
            
            _currentIntensity -= _changeRateHealth * Time.deltaTime;    // Fades the vignette effect away
            switch (_currentIntensity < 0)
            {
                case true: _vignette.active = false; break;             // Deactivates the vignette effect
            }
        }
        
    #endregion
    
    #region Temperature Methods
    
        private void EnableFreezeVisual()
        {
            if (Bonfire._IsHeatingUp) { ApplyHeatAbsorption(); return; }
            if (!Bonfire._IsHeatingUp) { DeapplyHeatAbsorption(); }

            _temperatureDefaulter = false;                              // Ensures only one use of the if-statement in DisableFreezeVisual()

            ///////////////////////////////////_temperatureIcon.;///////////
            
            _temperatureIcon.color = _colorBelowThreshold;              // Changes the color of the icon when the method is called
            _currentVolumeTemperature = _defaultBelow;                  // Resets _currentVolumeTemperature to -100
            _volumeBelowThresholdTemperature = _defaultBelow;           // Resets _volumeBelowThresholdTemperature to default 
            
            _cfpTemperature = new ClampedFloatParameter(_volumeAboveThresholdTemperature, _defaultBelow, _defaultAbove, true);

            if (_volumeAboveThresholdTemperature > _defaultBelow)
            {
                _volumeAboveThresholdTemperature -= _changeRateTemperature * Time.deltaTime;
            }
        }

        private void DisableFreezeVisual()
        {
            if (Bonfire._IsHeatingUp) { ApplyHeatAbsorption(); return; } 
            if (!Bonfire._IsHeatingUp) { DeapplyHeatAbsorption(); }

            _temperatureIcon.color = _colorAboveThreshold;      // Changes the color of the icon when the method is called
            _currentVolumeTemperature = _defaultAbove;          // Defaults _currentVolumeTemperature to -40
            
            if (_temperatureDefaulter)                          // Will only be accessed at the start of the game
            {
                _cfpTemperature = new ClampedFloatParameter(_volumeAboveThresholdTemperature, _defaultBelow, _defaultAbove, true);
                return;
            }
            
            _volumeAboveThresholdTemperature = _defaultAbove;   // Resets _volumeAboveThresholdTemperature to default 
            _cfpTemperature = new ClampedFloatParameter(_volumeBelowThresholdTemperature, _defaultBelow, _defaultAbove, true);

            if (_volumeBelowThresholdTemperature < _defaultAbove)
            {
                _volumeBelowThresholdTemperature += _changeRateTemperature * Time.deltaTime;
            }
        }

        private void ApplyHeatAbsorption()
        {
            _temperatureIcon.color = _colorDuringHeat;
            switch (_currentTemperature < _thresholdTemperature)     
            {
                case true:         // For EnableFreezeVisual() 
                {
                    _cfpTemperature = new ClampedFloatParameter(_currentVolumeTemperature, _defaultBelow, _volumeForHeat, true);

                    if (_currentVolumeTemperature < _volumeForHeat)
                    {
                        _currentVolumeTemperature += _changeRateTemperature * Time.deltaTime;
                    }
                    break;
                }
                
                case false:         // For DisableFreezeVisual() 
                {
                   _cfpTemperature = new ClampedFloatParameter(_currentVolumeTemperature, _defaultAbove, _volumeForHeat, true);

                    if (_currentVolumeTemperature < _volumeForHeat)
                    {
                        _currentVolumeTemperature += _changeRateTemperature * Time.deltaTime;
                    }
                    break;
                }
            }
        }

        private void DeapplyHeatAbsorption()
        {
            
        }
        
    #endregion
    
    
    #region Hunger Methods

        private void EnableHungerVisual() // CameraShake(); on Icon
        {
        
        } 

        private void DisableHungerVisual()
        {
            
        }
        
    #endregion

    private void CameraShake()
    {
        CameraShakeManager._Instance.CameraShake(_impulseSource);
    }
}
