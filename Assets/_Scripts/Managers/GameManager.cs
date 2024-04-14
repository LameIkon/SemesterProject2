using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : PersistentSingleton<GameManager>
{
    private InputReader _inputs; 

    [SerializeField] private UnityEngine.GameObject _pauseMenu;
    [SerializeField] private UnityEngine.GameObject _inventoryMenu;
    [SerializeField] private Scene _mainMenu;

    private bool _mainSceneBool;

    protected override void Awake()
    {
        base.Awake();
        if (_inputs == null) 
        {
            _inputs = ScriptableObject.CreateInstance<InputReader>();
        }
    }

    private void Start() 
    {       
   
        if (_pauseMenu != null)
        {
            // We set the Pause and Inventory Menus to false when we start
            _pauseMenu.SetActive(false);
        }
        if (_inventoryMenu != null)
        {
            _inventoryMenu.SetActive(false);
        }
        CheckScene();

        // Here we subscribe the events to the handlers
        InputReader.OnPauseEvent += HandlePause;
        InputReader.OnResumeEvent += HandleResume;
        InputReader.OnInventoryOpenEvent += HandleInventoryOpen;
        InputReader.OnInventoryCloseEvent += HandleInvertoryClose;
    }

    private void OnDisable() 
    {
        InputReader.OnPauseEvent -= HandlePause;
        InputReader.OnResumeEvent -= HandleResume;
        InputReader.OnInventoryOpenEvent -= HandleInventoryOpen;
        InputReader.OnInventoryCloseEvent -= HandleInvertoryClose;
    }

    private void OnLevelWasLoaded(int level)
    {
        CheckScene();
    }

    void CheckScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == _mainMenu.name)
        {
            _mainSceneBool = true;
        }
        else
        {
            _mainSceneBool = false;
        }
    }

    private void HandlePause() 
    {
        if (_mainSceneBool)
        {
            _pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void HandleResume() 
    {
        if (_mainSceneBool)
        {
            _pauseMenu.SetActive(false);
            _inventoryMenu.SetActive(false); // Important we close both the inventory and the pause menus here. This will mean if you have the inventory open and the OnResumeEvent fires it will close the inventory 
            Time.timeScale = 1.0f;
        }
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
