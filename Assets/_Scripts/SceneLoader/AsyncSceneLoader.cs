using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;


[RequireComponent(typeof(BoxCollider2D))]
public class AsyncSceneLoader : MonoBehaviour
{


    private BoxCollider2D _sceneTrigger;

    [SerializeField] private SceneField[] _loadScenes; // This is the array of scenes that get loaded
    [SerializeField] private SceneField[] _unloadScenes; // This is the array of scenes that get unloaded

    public static event Action OnSceneLoadedEvent; 
    public static event Action OnSceneUnloadedEvent;


    private void Awake()
    {
        _sceneTrigger = GetComponent<BoxCollider2D>(); // Gets the collider on the gameObject
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") && _unloadScenes.Length != 0) // We get the Player and ensure that the _unloadScenes array is not null
        {
            StartCoroutine(UnloadScene(_unloadScenes)); // The Coroutine to unload the scenes is started
   
        }

        if (collision.CompareTag("Player") && _loadScenes.Length != 0) // We get the Player and ensure that  the _loadScenes array is not null
        {
            StartCoroutine(LoadScene(_loadScenes)); // The Coroutine to load the scenes is started
        }

    }

    // The Coroutine for loading the scenes 
    private IEnumerator LoadScene(SceneField[] scenes) 
    {
        foreach (var scene in scenes) // looping over all the scenes in the array
        {
            if (!SceneManager.GetSceneByName(scene).isLoaded) // We check that the scene is not loaded such that it does not load already loaded scenes
            {
                SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive); // This is how we load multiple scenes together
                OnSceneLoadedEvent?.Invoke();
            }
            while (!SceneManager.GetSceneByName(scene).isLoaded)
            {
                yield return null; // This will yield untill the scene has been loaded, if this was not here the game lags when to many scenes are loaded at once
            }
        }

    }

    private IEnumerator UnloadScene(SceneField[] scenes) 
    {

        foreach (var scene in scenes)
        {
            if (SceneManager.GetSceneByName(scene).isLoaded)
            {
                OnSceneUnloadedEvent?.Invoke();
                SceneManager.UnloadSceneAsync(scene, UnloadSceneOptions.None);
            }
            while (SceneManager.GetSceneByName(scene).isLoaded) 
            { 
                yield return null; 
            }
            
        }

    }

    private void Reset()
    {
        _sceneTrigger.isTrigger = true;
    }

}
