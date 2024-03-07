using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    [SerializeField, Tooltip("Drag in the InputReader")] private InputReader _input;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _inventoryMenu;
    
    private void Start() 
    {
        // We set the Pause and Inventory Menus to false when we start
        _pauseMenu.SetActive(false);
        _inventoryMenu.SetActive(false);

        // Here we subscribe the events to the handlers
        _input.OnPauseEvent += HandlePause;
        _input.OnResumeEvent += HandleResume;
        _input.OnInventoryOpenEvent += HandleInventoryOpen;
        _input.OnInventoryCloseEvent += HandleInvertoryClose;
    }

    private void HandlePause() 
    {
        _pauseMenu.SetActive(true);
    }

    private void HandleResume() 
    {
        _pauseMenu.SetActive(false);
        _inventoryMenu.SetActive(false); // Important we close both the inventory and the pause menus here. This will mean if you have the inventory open and the OnResumeEvent fires it will close the inventory 
    }

    private void HandleInventoryOpen() 
    {
        _inventoryMenu.SetActive(true);
    }

    private void HandleInvertoryClose() 
    {
        _inventoryMenu.SetActive(false);
    }
}
