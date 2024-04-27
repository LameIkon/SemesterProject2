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
    [SerializeField] private SceneField _shipIn;
    [SerializeField] private GameObject _guideline;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _playerMovePoint;
    public static GameObject _inventoryMenuSTATIC;

   


    public bool _mainSceneBool;
    public static bool _shipInBool;
    private bool _isInventoryOpen = false;
    private bool _isPaused = false;

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
        _isPaused = false;
        _isInventoryOpen = false;

    }
    private void OnEnable()
    {
        // Here we subscribe the events to the handlers
        InputReader.OnPauseEvent += HandlePause;
        InputReader.OnInventoryEvent += HandleInventory;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() 
    {
        InputReader.OnPauseEvent -= HandlePause;
        InputReader.OnInventoryEvent -= HandleInventory;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Vector3 spawnPoint = new Vector3(107, -1, 0);

        CheckScene();
        if (!_mainSceneBool && _shipInBool)
        {
            _player.transform.position = spawnPoint;
            _playerMovePoint.transform.position = spawnPoint;

            if (SkipGuide._skipGuide && SkipGuide._showGuide)
            {
                _guideline.SetActive(true);
            }
        }
    }
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

        if (currentScene.name == _shipIn)
        {
            _shipInBool = true;
        }
        else
        {
            _shipInBool = false;
        }
    }


    public void HandlePause() 
    {
        if (!_mainSceneBool)
        {
            if (_isPaused) 
            {
                _pauseMenu.SetActive(false);
                _inventoryMenu.SetActive(false); // Important we close both the inventory and the pause menus here. This will mean if you have the inventory open and the OnResumeEvent fires it will close the inventory 
                Time.timeScale = 1.0f;
                _isPaused = false;
            }
            else 
            {
                _isPaused = true;
                _pauseMenu.SetActive(true);
                Time.timeScale = 0f;
            }

            
        }
    }


    private void HandleInventory() 
    {

        if (_isInventoryOpen)
        {
            _inventoryMenu.SetActive(false);
            _hideEInteractables = false; // interactables can be seen again
            _isInventoryOpen = false;
        }
        else
        {
            _inventoryMenu.SetActive(true);
            _hideEInteractables = true; // Hide E interactables
            _isInventoryOpen=true;
        }
    }


}
