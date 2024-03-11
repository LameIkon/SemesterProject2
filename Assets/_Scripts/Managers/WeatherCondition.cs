using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.VFX;

public class WeatherCondition : MonoBehaviour
{
    SurvivalManager _SurvivalManager; // get temperature an increase the rate of depletion
    PlayerController _playerController; // Get movement speed and reduce it
    // Get light source and reduce the light amount emitted


    [Header("Weather Types")]
    [SerializeField] private GameObject _blizzard; // Only used to disable, since its annoying in Unity Editor to have constant effects running
    [SerializeField] private GameObject _snow; // Only used to disable, since its annoying in Unity Editor to have constant effects running
    [SerializeField] private GameObject _fog; // Only used to disable, since its annoying in Unity Editor to have constant effects running

    [Header("Weather Data")]
    [SerializeField] private ParticleSystem _blizzardEffect; 
    [SerializeField] private VisualEffect _blizzardFogEffect;
    [SerializeField] private ParticleSystem _snowEffect;
    [SerializeField] private VisualEffect _fogEffect;

    [Header("Data")]
    [SerializeField] private int _counter = 0;
    private int _timeBetweenMin = 100;
    private int _timeBetweenMax = 200;
    private bool _isChoosingWeather;
    private bool _canChooseWeather; // Used to check for certain conditions, like being inside a house. 

    void Awake()
    {

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

        if (_counter <= 0)
        {
            //Invoke("Blizzard", _counter);
            //_counter = Random.Range(_tikBetweenMin, _tikBetweenMax);
        }
        //Timer();
    }

    void Timer()
    {
        if (!_isChoosingWeather)
        {
            _isChoosingWeather = true;
            _counter = Random.Range(_timeBetweenMin, _timeBetweenMax);
            StartCoroutine(ChangeWeather());
        }
    }

    IEnumerator ChangeWeather()
    {
        Blizzard();
        yield return new WaitForSeconds(_counter); // Amount of time for the ongoing weather
        ResetWeather(); // Reset
        _isChoosingWeather = false;
    }

    void Blizzard()
    {
        if(!_blizzard.activeInHierarchy) // Checks if the GameObject is active in the scene
        {
            _blizzard.SetActive(true); // Make it active
        }

        _blizzardEffect.Play(); // Start Blizzard effect
        _blizzardFogEffect.Play(); // Start Fog effect

        //Invoke("ResetWeather", 20f);
    }

    void Fog()
    {
        if (!_fog.activeInHierarchy) // Checks if the GameObject is active in the scene
        {
            _fog.SetActive(true); // Make it active
        }

        _fogEffect.Play();
    }

    void Snow()
    {
        if (!_snow.activeInHierarchy) // Checks if the GameObject is active in the scene
        {
            _snow.SetActive(true); // Make it active
        }

        _snowEffect.Play();
    }

    void ResetWeather() // Reset everything back to default
    {
        // Stop all visual weather conditions
        _blizzardEffect.Stop();
        _blizzardFogEffect.Stop();
        _fogEffect.Stop();
        _snowEffect.Stop();
    }
}
