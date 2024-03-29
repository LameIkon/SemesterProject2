using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherTrigger : MonoBehaviour
{
    [Header("choose 1 weather type")] // Choose weather type in the inspector 
    [SerializeField] private bool _noWeather; // probably not needed
    [SerializeField] private bool _blizzard;
    [SerializeField] private bool _snow;
    [SerializeField] private bool _fog;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            ChooseWeather();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            EnvironmentManager.instance.ResetWeather(); // stop the weather when you exit
        }
    }

    // Start the weather that was choosen in the inspector to run. Calls to the EnvionmentManager for the weather.
    private void ChooseWeather()
    {
        if (_noWeather) // If no weather was choosen run this. Atm its redundant 
        {
            EnvironmentManager.instance.ResetWeather();
        }
        else if (_blizzard)
        {
            EnvironmentManager.instance.Blizzard();
        }
        else if (_snow)
        {
            EnvironmentManager.instance.Snow();
        }
        else if (_fog)
        {
            EnvironmentManager.instance.Fog();
        }
        else // Gives warning if a weather type wasnt picked. 
        {
            Debug.LogError("No weather condition choosen!");
        }
    }
}
