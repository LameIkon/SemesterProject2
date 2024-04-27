using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivateGuideline : MonoBehaviour
{
    // Pragma used to ignore the warnings
    #pragma warning disable 0414
    // This script is used as testing for the GuidelineManager only
    [Header("Check activation")]
    [SerializeField] private bool _activateMovement;
    [SerializeField] private bool _activateRunning;
    [SerializeField] private bool _activateInventory;
    [SerializeField] private bool _activateChest;
    [SerializeField] private bool _activateCampFire;

    [SerializeField, Space(5)] private bool _activateTemperature;
    [SerializeField] private bool _activateFood;
    [SerializeField] private bool _activateStamina;

    #pragma warning restore 0414

    private void OnValidate()
    {
        //StartGuideline(); // Only used in the editor to test
    }

    private void OnDisable()
    {
        _activateMovement = false;
        _activateRunning = false;
        _activateInventory = false;
        _activateChest = false;
        _activateCampFire = false;

        _activateFood = false;
        _activateStamina = false;
        _activateTemperature = false;
    }




    //void StartGuideline()
    //{
    //    if (_activateMovement)
    //    {
    //        GuidelineManager.instance.ShowMovement();
    //        _activateMovement = false;
    //    }
    //    if (_activateRunning)
    //    {
    //        StartCoroutine(GuidelineManager.instance.ShowRunning());
    //        _activateRunning = false;
    //    }
    //    if (_activateInventory)
    //    {
    //        GuidelineManager.instance.ShowInventory();
    //        _activateInventory = false;
    //    }
    //    if (_activateChest)
    //    {
    //        GuidelineManager.instance.ShowChest();
    //        _activateChest = false;
    //    }
    //    if (_activateCampFire)
    //    {
    //        GuidelineManager.instance.ShowCampfire();
    //        _activateCampFire = false;
    //    }

    //    if (_activateFood)
    //    {
    //        GuidelineManager.instance.ShowFood();
    //        _activateFood = false;
    //    }
    //    if (_activateStamina)
    //    {
    //        GuidelineManager.instance.ShowStamina();
    //        _activateStamina = false;
    //    }
    //    if (_activateTemperature)
    //    {
    //        GuidelineManager.instance.ShowTemperature();
    //        _activateTemperature = false;
    //    }
    //}
}
