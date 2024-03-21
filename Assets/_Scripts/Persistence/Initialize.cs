using UnityEngine;
using UnityEngine.SceneManagement;

public class Initialize : MonoBehaviour
{
    
    public static Initialize instance;

    private GameObject _persistanceObject; // Store the value. used to activate and disable the GameObject
    private static bool _initialized = true; // Ensures only 1 instance of running some code once
    private bool _checkInstances; // Used to ensure that OnLevelWasLoaded only get run after it gets checked if there is more instances.

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
            _persistanceObject = Instantiate(Resources.Load("PersistObjects")) as GameObject; // Instantiate the persistant Objects
            DontDestroyOnLoad(_persistanceObject); // Ensure not being destroyed 
            _persistanceObject.SetActive(false); // Initialize it to be false to not effect anything in MainMenu
            _initialized = false; // This will never be able to run again

            if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MainMenu")) // If we start the game in another scene besides the MainMenu
            {
                Debug.Log("Awake");
                LoadGame();
            }

            _checkInstances = true; // Turns to true. This should give the script time to ensure only 1 instance
        }
    }

    private void OnLevelWasLoaded() //Called whenever a new scene is loaded
    {
        if (_checkInstances) // Retrict running before _checkInstances bool is checked.
        {
            Debug.Log("load level");
            switch (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenu")) // if MainMenu
            {
                case true: // Disable persistanceObjects when in MainMenu
                    LoadMenu();
                    break;
                default: // Activate persistanceObjects when loaded game
                    LoadGame();
                    break;
            }
        }
    }

    void LoadGame()
    {
        Debug.Log("Game loaded");
        _persistanceObject.SetActive(true);
        if (!LanternDisabler._LanternSTATIC)
        {
            LanternDisabler._LoadedSTATIC = true; // Something Jonas has
        }
    }

    void LoadMenu()
    {
        Debug.Log("Main Menu Loaded");
        _persistanceObject.SetActive(false);
    }


    
}
