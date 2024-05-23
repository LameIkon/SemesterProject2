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
    
    
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _playerMovePoint;
    public static GameObject _inventoryMenuSTATIC;

    [Header("chests")]
    [SerializeField] private List<GameObject> _chestCanvases;
    [SerializeField] private List<GameObject> _chests;

    [Header("Fire")]
    [SerializeField] private List<GameObject> _fireCanvases;
    [SerializeField] private List<GameObject> _firePlaces;

    

    [Header("Lost Expedition Intro")]
    [SerializeField] private SceneField _lostExpeditionIntro;
    public static bool _LostExpeditionBool;
    public GameObject _blackSceen;
    private bool _lostExpeditionOnceBool;

    [Header("Tutorial")]
    [SerializeField] private GameObject _guideline;
    [SerializeField] private GameObject _skipTutorial;
    [SerializeField] private GameObject _SkipTutorialButtonFromControls;


    [Header("Other stuff")]
    public bool _mainSceneBool;
    public static bool _shipInBool;  
    public bool _isInventoryOpen = false;
    public bool _isChestOpen;
    private bool _isPaused = false;     
    private Vector3 _spawnPosition;
    

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
        _isPaused = false;
        _isInventoryOpen = false;

        // Set to false if they are active
        //_guideline.SetActive(false);
        //_skipTutorial.SetActive(false);
        //_SkipTutorialButtonFromControls.SetActive(false);

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
        CheckScene();
        if (!_mainSceneBool && _shipInBool)
        {
            _spawnPosition = new Vector3(107, -1, 0);
            _player.transform.position = _spawnPosition;
            _playerMovePoint.transform.position = _spawnPosition;

            _lostExpeditionOnceBool = true;
            InputReader.OnInventoryEvent += HandleInventory; // you can open inventory
        }
        else if (_LostExpeditionBool && !_mainSceneBool && !_lostExpeditionOnceBool)
        {
            _spawnPosition = new Vector3(-36, 351, 0);
            _player.transform.position = _spawnPosition;
            _playerMovePoint.transform.position = _spawnPosition;
             InputReader.OnInventoryEvent -= HandleInventory; // you cannot open inventory
            Debug.Log("expedition cutscene");
        }
    }
    void CheckScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == _mainMenu)
        {
            Cursor.visible = true; // By chance when you load into the scene and cursor wasnt active
            _mainSceneBool = true;
        }
        else
        {
            _mainSceneBool = false;
        }

        if (currentScene.name == _shipIn)
        {
            Cursor.visible = true; // By chance when you load into the scene and cursor wasnt active
            _shipInBool = true;
        }
        else
        {
            _shipInBool = false;
        }

        if (currentScene.name == _lostExpeditionIntro)
        {
            _LostExpeditionBool = true;
        }
        else
        {
            _blackSceen.SetActive(false); // need for when changing away from this scene, because it gives a frame where you see everything
            _LostExpeditionBool= false;
        }
    }


    public void HandlePause() 
    {
        if (!_mainSceneBool && !_inventoryMenu.activeInHierarchy)
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
            return;
        }
        // handle inventory instead
        else
        {
            HandleInventory();
        }
    }


    private void HandleInventory() 
    {
        // Inventory
        if (_inventoryMenu.activeInHierarchy)
        {
            _inventoryMenu.SetActive(false);
            _isInventoryOpen = false;

            // Check if any chests are active
            foreach (var chest in _chestCanvases)
            {
                if (chest.activeInHierarchy)
                {
                    //chest.SetActive(false);

                    Debug.Log("found active chest");

                    // not the most effecient way but we will close all chests
                    foreach (var chestManager in _chests)
                    {
                        chestManager.GetComponent<ChestManager>().CloseChest();
                    }
                }
            }

            // Check fire places
            foreach (var firePlace in _fireCanvases)
            {
                if (firePlace.activeInHierarchy)
                {
                    Debug.Log("found active fire");

                    // not the most effecient way but we will close all fires
                    foreach (var fireManager in _firePlaces)
                    {
                        fireManager.GetComponent<FurnaceManager>().CloseFire();
                    }
                }
            }
        }

        else if (!_inventoryMenu.activeInHierarchy)
        {
            _inventoryMenu.SetActive(true);
            _isInventoryOpen=true;
        }

       
    }
 


}
