using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class EnvironmentManager : MonoBehaviour
{
    public static EnvironmentManager instance { get; private set; }

    [Header("Weather Types")] // By default they start being disabled since in unity editor the effects love to run constantly
    [SerializeField] private UnityEngine.GameObject _blizzard;
    [SerializeField] private UnityEngine.GameObject _snow;
    [SerializeField] private UnityEngine.GameObject _fog;
    public UnityEngine.GameObject _barrierBlizzard;

    [Header("Weather Effects")]
    [SerializeField] private ParticleSystem _blizzardEffect;
    [SerializeField] private VisualEffect _blizzardFogEffect;
    [SerializeField] private ParticleSystem _snowEffect;
    [SerializeField] private VisualEffect _fogEffect;
    public ParticleSystem _barrierBlizzardEffect;
    public VisualEffect _barrierBlizzardFogEffect;

    [Header("Weather Checkers")] // Currently not being used by anything
    public static bool _IsBlizzard;
    public static bool _IsSnow;
    public static bool _IsFog;

    [Space(10f)]
    [Header("Temperatures")]
    [SerializeField] private float _defaultTemp = -15f; // Default temperature without any weather conditions
    [Space(10f)]
    [SerializeField] private float _blizzardTemp = -450f; // Temperature in a blizzard
    [SerializeField] private float _snowTemp = -35f; // temperature in a snow weather
    [SerializeField] private float _fogTemp = -25f; // temperature in a fog
    [SerializeField] private float _barrierBlizzardTemp = -60f; // Temperature in an area you shouldnt be

    [SerializeField] private FloatVariable _CurrentOutsideTemperature; // This is the current temperature. Used to store the temperatures

    [Header("Movement Speed debuff")]
    [SerializeField] private FloatVariable _playerMaxSpeed;// Default speed without any weather conditions
    [SerializeField] private FloatVariable _playerMinSpeed;
    [Space(10f)]
    [SerializeField] private float _blizzardSpeedDebuff = .50f; // Speed% in a blizzard
    [SerializeField] private float _snowSpeedDebuff = .15f; // Speed% in a snow weather
    [SerializeField] private float _fogSpeedDebuff = .5f; // Speed% in a fog

    public static float _MovementSpeedDebuff = 0f; // Used in other scripts to impact a movement debuff. Consider this as percantage

    [Header("Stamina Regen")] // Weather effects will affect the amount of stamina you have and how much you gain
    [SerializeField] private float _defaultStaminaUse = 0.2f; // Max stamina at default
    [SerializeField] private float _defaultStaminaRegen = 0.05f; // Default stamina regen without any weather conditions
    [Space(10f)]
    [SerializeField] private float _blizzardStaminaUse = 0.3f; // Max stamina in a blizzard
    [SerializeField] private float _blizzardStaminaRegen = 0.002f; // Regen in a blizzard
    [SerializeField] private float _snowStaminaUse = 0.22f; // Max stamina in a snow weather
    [SerializeField] private float _snowStaminaRegen = 0.04f; // Regen in a snow weather
    [SerializeField] private float _fogStaminaUse = 0.24f; // Max stamina in a fog
    [SerializeField] private float _fogStaminaRegen = 0.045f; // Regen in a fog

    [SerializeField] private FloatVariable _CurrentStaminaUse; // This is the current max stamina
    [SerializeField] private FloatVariable _CurrentStaminaRegen; // This is the current stamina regen

    [Header("Scenes")]
    public bool _inside;
    public bool _outside;

    //private int _timeBetweenMin = 100;
    //private int _timeBetweenMax = 200;
    //private bool _isChoosingWeather;
    //private bool _canChooseWeather; // Used to check for certain conditions, like being inside a house. 


    //private void SetPlayerSpeed(float debuff)
    //{
    //    _playerMaxSpeed.ApplyChange(_playerMaxSpeed.GetValue() * debuff);
    //    _playerMinSpeed.ApplyChange(_playerMinSpeed.GetValue() * debuff);
    //}


    void Awake()
    {
        // Ensure only 1 singleton of this script
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        _CurrentOutsideTemperature.SetValue(_defaultTemp); // Set current temperature to default
        _CurrentStaminaUse.SetValue(_defaultStaminaUse); // Set current stamina use on run to default
        _CurrentStaminaRegen.SetValue(_defaultStaminaRegen); // Set current stamina regen to default
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

    private void OnLevelWasLoaded(int level)
    {
        CheckScene();
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


    public void Blizzard()
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
            _CurrentStaminaUse.SetValue(_blizzardStaminaUse); // change current max stamina
            _CurrentStaminaRegen.SetValue(_blizzardStaminaRegen); // change current stamina regen 

            //SetPlayerSpeed(_blizzardSpeedDebuff) ; // Give movement speed debuff
        }
    }

    public void Fog()
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
            _CurrentStaminaUse.SetValue(_fogStaminaUse); // change current max stamina
            _CurrentStaminaRegen.SetValue(_fogStaminaRegen); // change current stamina regen 

            //SetPlayerSpeed(_fogSpeedDebuff); // Give movement speed debuff
        }
    }

    public void Snow()
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
            _CurrentStaminaUse.SetValue(_snowStaminaUse); // change current max stamina
            _CurrentStaminaRegen.SetValue(_snowStaminaRegen); // change current stamina regen 

            //SetPlayerSpeed(_snowSpeedDebuff); // Give movement speed debuff
        }
    }

    public void BarrierBlizzard()
    {
        if (!_barrierBlizzard.activeInHierarchy) // Checks if the GameObject is active in the scene
        {
            _barrierBlizzard.SetActive(true); // Make it active
        }

        _barrierBlizzardEffect.Play();
        _barrierBlizzardFogEffect.Play();

        _CurrentOutsideTemperature.SetValue(_barrierBlizzardTemp); // Change current temperature

    }

    public void ResetWeather() // Reset everything back to default
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

        // Reset to default conditions
        _CurrentOutsideTemperature.SetValue(_defaultTemp);
        _CurrentStaminaUse.SetValue(_defaultStaminaUse);
        _CurrentStaminaRegen.SetValue(_defaultStaminaRegen);

        // Reset to default movement speed
        //SetPlayerSpeed(1f);
    }

    public void ExitBlizzardBarrier()
    {
        _CurrentOutsideTemperature.SetValue(_defaultTemp);
    }

    public void RemoveBarrier() // works exactly like resetWeather but needs to be independent
    {
        _barrierBlizzardEffect.Stop();
        _barrierBlizzardFogEffect.Stop();

        if (!_blizzard && !_IsSnow && !_IsFog) // only change if no other weather effects is ongoing
        {
            _CurrentOutsideTemperature.SetValue(_defaultTemp);
        }
    }

    void CheckScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();


        switch (currentScene.name)
        {
            case "ShipInside": // Change this to be equal to the ship interior scene
                _inside = true;
                _outside = false;
                break;

            default:
                _outside = true;
                _inside = false;
                break;
        }
    }
}
