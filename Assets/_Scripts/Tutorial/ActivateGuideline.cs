using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivateGuideline : MonoBehaviour
{
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


    private bool _finishedMovement;
    private bool _finishedRunning;
    private bool _finishedInventory;
    private bool _finishedChest;
    private bool _finishedCampFire;

    private bool _finishedTemperature;    
    private bool _finishedFood;
    private bool _finishedStamina;

    private void Awake()
    {

    }

    private void Start()
    {
        //Invoke("ShowMovement", 0.5f);
    }

    private void OnValidate()
    {
        //StartGuideline(); // Only used in the editor to test
    }

    private void OnEnable()
    {
        InputReader.OnInventoryEvent += ShowHunger;

        _finishedMovement = false;
        
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
        InputReader.OnInventoryEvent -= ShowHunger;
    }

   


    // Update is called once per frame
    void Update()
    {
        ShowTemperature();
        ShowInventory();

        

    }


    public void ShowMovement() // Activated automatically at start of game
    {
        if (!_finishedMovement)
        {
            //StartCoroutine(GuidelineManager.instance.ShowMovement());
            _finishedMovement = true;
        }
    }

    void ShowHunger() // Activated when opening inventory
    {
        if (!_finishedFood)
        {
            StartCoroutine(GuidelineManager.instance.ShowFood());
            _finishedFood = true;
        }        
    }

    void ShowTemperature() // Activated when going outside ship
    {
        if (EnvironmentManager.instance._outside && !_finishedTemperature)
        {
            GuidelineManager.instance.ShowTemperature();
            _finishedTemperature = true;
        }
    }

    void ShowCampFire()
    {
        if (!_finishedCampFire)
        {
            GuidelineManager.instance.ShowCampfire();
            _finishedCampFire = true;
        }
    }

    void ShowStamina()
    {
        if (!_finishedStamina)
        {
            GuidelineManager.instance.ShowStamina();
            _finishedStamina = true;
        }
    }

    void ShowInventory() // Activated when exiting dialogue
    {
        if (DialogueManager.instance._DialogueExited && !_finishedInventory)
        {
            //StartCoroutine(GuidelineManager.instance.ShowInventory());
            _finishedInventory = true;
        }
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
