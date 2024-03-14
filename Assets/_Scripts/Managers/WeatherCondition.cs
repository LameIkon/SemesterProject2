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


    [Header(" Temperatures")]
    [SerializeField] private float _defaultTemp = -15f; // defauly temperature without any weather conditions
    [Space (10f)]        
    [SerializeField] private float _blizzardTemp = -45f; // Temperature in a blizzard
    [SerializeField] private float _snowTemp = -35f; // temperature in a snow weather
    [SerializeField] private float _fogTemp = -25f; // temperature in a fog

    public static float _CurrentOutsideTemperature; // This is the current temperature. Used to store the temperatures




    [Header("Weather Checkers")] // Currently not being used by anything
    public bool _IsBlizzard;
    public bool _IsSnow;
    public bool _IsFog; 

    private int _timeBetweenMin = 100;
    private int _timeBetweenMax = 200;
    private bool _isChoosingWeather;
    private bool _canChooseWeather; // Used to check for certain conditions, like being inside a house. 

    //[SerializeField] private FloatReferencer _playerSpeed;


    void Awake()
    {
        _CurrentOutsideTemperature = _defaultTemp; // Set current temperature to default

        //_playerSpeed.SetValue(10);
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

    //void Timer()
    //{
    //    if (!_isChoosingWeather)
    //    {
    //        _isChoosingWeather = true;
    //        //_counter = Random.Range(_timeBetweenMin, _timeBetweenMax);
    //        StartCoroutine(ChangeWeather());
    //    }
    //}

    //IEnumerator ChangeWeather()
    //{
    //    Blizzard();
    //    yield return new WaitForSeconds(_counter); // Amount of time for the ongoing weather
    //    ResetWeather(); // Reset
    //    _isChoosingWeather = false;
    //}

    void Blizzard()
    {
        if(!_blizzard.activeInHierarchy) // Checks if the GameObject is active in the scene
        {
            _blizzard.SetActive(true); // Make it active
        }

        if(!_IsSnow && !_IsFog)
        {
            _blizzardEffect.Play(); // Start Blizzard effect
            _blizzardFogEffect.Play(); // Start Fog effect
            _IsBlizzard = true;

            _CurrentOutsideTemperature = _blizzardTemp;
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

            _CurrentOutsideTemperature = _fogTemp;
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

            _CurrentOutsideTemperature = _snowTemp;
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
        _CurrentOutsideTemperature = _defaultTemp;
    }
}
