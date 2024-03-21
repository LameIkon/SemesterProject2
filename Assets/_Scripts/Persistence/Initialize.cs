using UnityEngine;
using UnityEngine.SceneManagement;

public class Initialize : MonoBehaviour
{
    
    public static Initialize instance;

    private GameObject _persistanceObject; // Store the value. used to activate and disable the GameObject
    private bool _initialized = true;
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
    }

    private void OnLevelWasLoaded() //Called whenever a new scene is loaded
    {
        if (_initialized)
        {
            _persistanceObject = Instantiate(Resources.Load("PersistObjects")) as GameObject; // Instantiate the persistant Objects
            DontDestroyOnLoad(_persistanceObject); // Ensure not being destroyed 
            _persistanceObject.SetActive(false); // Initialize it to be false to not effect anything in MainMenu
            _initialized = false;
        }

        switch (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenu")) // if MainMenu
        {
            case true: // Disable persistanceObjects when in MainMenu
                Debug.Log("Main Menu Loaded");
                //LanternDisabler._LoadedSTATIC = false; // Something Jonas has
                _persistanceObject.SetActive(false); 

                break;
            default: // Activate persistanceObjects when loaded game
                Debug.Log("Game loaded");
                _persistanceObject.SetActive(true);
                if (!LanternDisabler._LanternSTATIC)
                {
                    LanternDisabler._LoadedSTATIC = true; // Something Jonas has
                }
                break;
        }
    }


    
}
