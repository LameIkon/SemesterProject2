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

    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _inventoryMenu;
    [SerializeField] private SceneField _mainMenu;
    public static GameObject _inventoryMenuSTATIC;


    private bool _mainSceneBool;

    public static bool _hideEInteractables; // used for scripts disable interactables such as chest and campfire

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
        _inventoryMenuSTATIC = _inventoryMenu;
   
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
        _hideEInteractables = false;

        // Here we subscribe the events to the handlers
        InputReader.OnPauseEvent += HandlePause;
        InputReader.OnResumeEvent += HandleResume;
        InputReader.OnInventoryOpenEvent += HandleInventoryOpen;
        InputReader.OnInventoryCloseEvent += HandleInvertoryClose;
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() 
    {
        InputReader.OnPauseEvent -= HandlePause;
        InputReader.OnResumeEvent -= HandleResume;
        InputReader.OnInventoryOpenEvent -= HandleInventoryOpen;
        InputReader.OnInventoryCloseEvent -= HandleInvertoryClose;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckScene();
    }

    //private void OnLevelWasLoaded(int level)
    //{
    //    CheckScene();
    //}

    void CheckScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == _mainMenu)
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
        if (!_mainSceneBool)
        {
            _pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void HandleResume() 
    {
        if (!_mainSceneBool)
        {
            _pauseMenu.SetActive(false);
            _inventoryMenu.SetActive(false); // Important we close both the inventory and the pause menus here. This will mean if you have the inventory open and the OnResumeEvent fires it will close the inventory 
            Time.timeScale = 1.0f;
        }
    }

    private void HandleInventoryOpen() 
    {
        _inventoryMenu.SetActive(true);
        _hideEInteractables = true; // Hide E interactables
    }

    private void HandleInvertoryClose() 
    {
        _inventoryMenu.SetActive(false);
        _hideEInteractables = false; // interactables can be seen again
    }

}
