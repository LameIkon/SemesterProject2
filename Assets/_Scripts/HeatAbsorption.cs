/*using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class HeatAbsorption : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private Volume _volume;
    [SerializeField] private Visuals _visuals;
    [Space(5f)]
    
    // ------------------------------------------------------------------------------------------------ \\
    
    [Header("Heat")] 
    [SerializeField] private FloatReference _currentTemperature;
    [SerializeField] private Color _colorDefault;
    [SerializeField] private Color _colorWhenHeated;
    [SerializeField] private float _thresholdTemperature;   // 25     // --- MIGHT NOT BE USED - DELETE LATER IF NEEDED! ---
    [SerializeField] private float _changeRate;             // 100
    [SerializeField] private float _maximumVolume;          // 60
    [SerializeField] private float _minimumVolume;          // 0
    private Image _temperatureIcon;
    private ClampedFloatParameter _cfpTemperature;
    private WhiteBalance _whiteBalance;
    private float _defaultAbove;
    private float _defaultBelow;
    private bool _temperatureDefaulter;
    
    #region Unity Methods
    
        private void Start()
        {
            _volume = GetComponent<Volume>();                  // Initializes _volume
            _temperatureIcon = GameObject.FindWithTag("TemperatureImage").GetComponent<Image>();                // Initializes the temperature icon
            _defaultAbove = _maximumVolume;                                                                     // To reset _temperatureVolumeAboveThreshold after being changed
            _defaultBelow = _minimumVolume;                                                                     // To reset _temperatureVolumeBelowThreshold after being changed
            WhiteBalance wb;                                                                                    // Used to initialize Volume components
            switch (_volume.profile.TryGet<WhiteBalance>(out wb)) { case true: _whiteBalance = wb; break; }     // Initializes Volume components
            _colorDefault = _visuals._colorAboveThreshold;                                                                          // --- MIGHT NOT BE USED - DELETE LATER IF NEEDED! ---
        }

        private void OnTriggerStay2D(Collider2D col)
        {
            _cfpTemperature = new ClampedFloatParameter(_currentVolumeTemperature, _minimumVolume, _maximumVolume, true);     // Sets the Volume Temperature to the default
            _whiteBalance.temperature.SetValue(_cfpTemperature); // Avoids NullReferences
            EnableAbsorptionVisual();
            
            switch (_isCold)
            {
                case true: this.EnableFreezeVisual(); break;
                case false: this.DisableFreezeVisual(); break;
            }
        }

        private void OnTriggerExit(Collider col) { DisableAbsorptionVisual(); }
        
    #endregion
    
    #region Heat Absorption Methods
    
        private void EnableAbsorptionVisual()
        {
            _temperatureIcon.color = _colorWhenHeated;                                                                         // Changes the color of the icon when the method is called
            _currentVolumeTemperature += _changeRate * Time.deltaTime;
            _cfpTemperature = new ClampedFloatParameter(_currentVolumeTemperature, _minimumVolume, _maximumVolume, true);     // Sets the Volume Temperature to the default
        }
        
    
        private void DisableAbsorptionVisual()
        {
            _temperatureIcon.color = _colorAboveThreshold;      // Resets the color to default
        }
    
    #endregion

    protected override float EnableFreezeVisual() { return _currentVolumeTemperature; }

    protected override float DisableFreezeVisual() { return _currentVolumeTemperature; }
}*/