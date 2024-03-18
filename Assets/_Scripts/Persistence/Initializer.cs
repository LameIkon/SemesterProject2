using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Initializer
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    
    public static void Execute()
    {
        switch (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("MainMenu"))
        {
            case true:
                Debug.Log("Main Menu Loaded");
                break;
            default:
                Debug.Log("Loaded by the Persist Objects from the Initializer script" + " WE ARE NOT IN MAIN MENU" ); 
                Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("PersistObjects")));
                break;
        }
    }
}

