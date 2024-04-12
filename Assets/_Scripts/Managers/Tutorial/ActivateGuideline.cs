using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateGuideline : MonoBehaviour
{
    [SerializeField] private bool _activateMovement;
    [SerializeField] private bool _activateRunning;
    [SerializeField] private bool _activateInventory;
    [SerializeField] private bool _activateChest;
    [SerializeField] private bool _activateCampFire;

    private void Awake()
    {
        _activateMovement = false;
        _activateRunning = false;
        _activateInventory = false;
        _activateChest = false;
        _activateCampFire = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_activateMovement)
        {
            GuidelineManager.instance.ShowMovement();
            _activateMovement = false;
        }
        if (_activateRunning)
        {
            GuidelineManager.instance.ShowRunning();
            _activateRunning = false;
        }
        if (_activateInventory)
        {
            GuidelineManager.instance.ShowInventory();
            _activateInventory = false;
        }
        if (_activateChest)
        {
            GuidelineManager.instance.ShowChest();
            _activateChest = false;
        }
        if (_activateCampFire)
        {
            GuidelineManager.instance.ShowCampfire();
            _activateCampFire = false;
        }
    }
}
