using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.VFX;

public class WeatherCondition : MonoBehaviour
{
    [Header("Weather Types")] // By default they start being disabled since in unity editor the effects love to run constantly
    [SerializeField] private GameObject _blizzard;
    [SerializeField] private GameObject _snow;
    [SerializeField] private GameObject _fog;

    [Header("Weather Effects")]
    [SerializeField] private ParticleSystem _blizzardEffect;
    [SerializeField] private VisualEffect _blizzardFogEffect;
    [SerializeField] private ParticleSystem _snowEffect;
    [SerializeField] private VisualEffect _fogEffect;

    [Header("Weather Checkers")] // Currently not being used by anything
    public static bool _IsBlizzard;
    public static bool _IsSnow;
    public static bool _IsFog;

    [Space(10f)]
    [Header("Temperatures")]
    [SerializeField] private float _defaultTemp = -15f; // Default temperature without any weather conditions
    [Space(10f)]
    [SerializeField] private float _blizzardTemp = -45f; // Temperature in a blizzard
    [SerializeField] private float _snowTemp = -35f; // temperature in a snow weather
    [SerializeField] private float _fogTemp = -25f; // temperature in a fog

    [SerializeField] private FloatVariable _CurrentOutsideTemperature; // This is the current temperature. Used to store the temperatures

    [Header("Movement Speed debuff")]
    [SerializeField] private FloatVariable _playerMaxSpeed;// Default speed without any weather conditions
    [SerializeField] private FloatVariable _playerMinSpeed;
    [Space(10f)]
    [SerializeField] private float _blizzardSpeedDebuff = .50f; // Speed% in a blizzard
    [SerializeField] private float _snowSpeedDebuff = .15f; // Speed% in a snow weather
    [SerializeField] private float _fogSpeedDebuff = .5f; // Speed% in a fog

    public static float _MovementSpeedDebuff = 0f; // Used in other scripts to impact a movement debuff. Consider this as percantage

    [Header("Stamina Regen")]
    [SerializeField] private float _defaultStaminaRegen = 5f; // Default stamina regen without any weather conditions
    [Space(10f)]
    [SerializeField] private float _blizzardStaminaRegen = 50f; // Regen in a blizzard
    [SerializeField] private float _snowStaminaRegen = 15f; // Regen in a snow weather
    [SerializeField] private float _fogStaminaRegen = 5f; // Regen in a fog

    private int _timeBetweenMin = 100;
    private int _timeBetweenMax = 200;
    private bool _isChoosingWeather;
    private bool _canChooseWeather; // Used to check for certain conditions, like being inside a house. 


    //private void SetPlayerSpeed(float debuff)
    //{
    //    _playerMaxSpeed.ApplyChange(_playerMaxSpeed.GetValue() * debuff);
    //    _playerMinSpeed.ApplyChange(_playerMinSpeed.GetValue() * debuff);
    //}


    void Awake()
    {
        _CurrentOutsideTemperature.SetValue(_defaultTemp); // Set current temperature to default
    }

    // Start is called before the first frame update
    void Start()
    {
        // Just to ensure in the future the effects arent starting at random. For example if you load into a new scene.
        // Or maybe this is just reduntant. We will see.
        if (_blizzardEffect != null)
        {
            _blizzardEffect.Stop();
        }
        if (_blizzardFogEffect != null)
        {
            _blizzardFogEffect.Stop();
        }
        if (_snowEffect != null)
        {
            _snowEffect.Stop();
        }
        if (_fogEffect != null)
        {
            _fogEffect.Stop();
        }
    }

    private void Update()
    {
        // For testing purpose
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Blizzard();
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Snow();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            Fog();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            ResetWeather();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //_counter--;

        //if (_counter <= 0)
        //{
        //    //Invoke("Blizzard", _counter);
        //    //_counter = Random.Range(_tikBetweenMin, _tikBetweenMax);
        //}
        //Timer();
    }


    void Blizzard()
    {
        if (!_blizzard.activeInHierarchy) // Checks if the GameObject is active in the scene
        {
            _blizzard.SetActive(true); // Make it active
        }

        if (!_IsSnow && !_IsFog)
        {
            _blizzardEffect.Play(); // Start Blizzard effect
            _blizzardFogEffect.Play(); // Start Fog effect
            _IsBlizzard = true;

            _CurrentOutsideTemperature.SetValue(_blizzardTemp); // Change current temperature

            //SetPlayerSpeed(_blizzardSpeedDebuff) ; // Give movement speed debuff
        }
    }

    void Fog()
    {
        if (!_fog.activeInHierarchy) // Checks if the GameObject is active in the scene
        {
            _fog.SetActive(true); // Make it active
        }

        if (!_IsBlizzard && !_IsSnow)
        {
            _fogEffect.Play();
            _IsFog = true;

            _CurrentOutsideTemperature.SetValue(_fogTemp); // Change current temperature

            //SetPlayerSpeed(_fogSpeedDebuff); // Give movement speed debuff
        }
    }

    void Snow()
    {
        if (!_snow.activeInHierarchy) // Checks if the GameObject is active in the scene
        {
            _snow.SetActive(true); // Make it active
        }

        if (!_IsBlizzard && !_IsFog)
        {
            _snowEffect.Play();
            _IsSnow = true;

            _CurrentOutsideTemperature.SetValue(_snowTemp); // Change current temperature

            //SetPlayerSpeed(_snowSpeedDebuff); // Give movement speed debuff
        }
    }

    void ResetWeather() // Reset everything back to default
    {
        // Stop all visual weather conditions
        _blizzardEffect.Stop();
        _blizzardFogEffect.Stop();
        _fogEffect.Stop();
        _snowEffect.Stop();

        //Reset bool checkers. Used for other scripts to detect.
        _IsBlizzard = false;
        _IsSnow = false;
        _IsFog = false;

        // Reset to default temperature
        _CurrentOutsideTemperature.SetValue(_defaultTemp);

        // Reset to default movement speed
       //SetPlayerSpeed(1f);
    }
}
