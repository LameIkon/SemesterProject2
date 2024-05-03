using UnityEngine;
using UnityEngine.SceneManagement;

public class Initialize : MonoBehaviour
{
    
    public static Initialize instance;

    private GameObject _persistanceObject; // Store the value. used to activate and disable the GameObject
    private static bool _initialized = true; // Ensures only 1 instance of running some code once
    private bool _checkInstances; // Used to ensure that OnLevelWasLoaded only get run after it gets checked if there is more instances.

    private const string _mainMenu = "MainMenu";
    private const string _introScene = "Intro";
    private const string _ending1 = "Ending1";
    private const string _ending2 = "Ending2";

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_checkInstances) // Retrict running before _checkInstances bool is checked.
        {

            switch (scene.name)
            {
                case _mainMenu:
                case _introScene:
                case _ending1:
                case _ending2:
                    LoadMenu();
                    break;

                default:
                    LoadGame();
                    break;
            }

            //switch (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenu")) // if MainMenu
            //{
            //    case true: // Disable persistanceObjects when in MainMenu
            //        LoadMenu();
            //        break;
            //    default: // Activate persistanceObjects when loaded game
            //        LoadGame();
            //        break;
            //}
        }
    }


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject); // This script should never be destroyed

        if (_initialized) // Just does this once
        {
            _persistanceObject = Instantiate(Resources.Load("PersistObjects")) as UnityEngine.GameObject; // Instantiate the persistant Objects
            DontDestroyOnLoad(_persistanceObject); // Ensure not being destroyed 
            _persistanceObject.SetActive(false); // Initialize it to be false to not effect anything in MainMenu
            _initialized = false; // This will never be able to run again

            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MainMenu")) // If we start the game in another scene besides the MainMenu
            {
                LoadGame();
            }

            _checkInstances = true; // Turns to true. This should give the script time to ensure only 1 instance
        }
    }

    //private void OnLevelWasLoaded() //Called whenever a new scene is loaded
    //{
    //    if (_checkInstances) // Retrict running before _checkInstances bool is checked.
    //    {
    //        switch (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenu")) // if MainMenu
    //        {
    //            case true: // Disable persistanceObjects when in MainMenu
    //                LoadMenu();
    //                break;
    //            default: // Activate persistanceObjects when loaded game
    //                LoadGame();
    //                break;
    //        }
    //    }
    //}

    void LoadGame()
    {
        _persistanceObject.SetActive(true);
        if (!LanternDisabler._LanternSTATIC)
        {
            LanternDisabler._LoadedSTATIC = true; // Something Jonas has
        }
    }

    void LoadMenu()
    {
        _persistanceObject.SetActive(false);
    }


    
}
