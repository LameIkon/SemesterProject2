using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Script not finished
public class SceneLoadTileManager : MonoBehaviour
{

    public static event Action<Tilemap[]> OnSceneLoadedEvent;
    public static event Action<Tilemap[]> OnSceneUnloadedEvent;


    private void OnSceneLoad() 
    {
        OnSceneLoadedEvent?.Invoke(GetComponentsInChildren<Tilemap>());
    }

    private void OnSceneUnload() 
    {
        OnSceneUnloadedEvent?.Invoke(GetComponentsInChildren<Tilemap>());
    }

    private void OnEnable() 
    {
        AsyncSceneLoader.OnSceneLoadedEvent += OnSceneLoad;
        AsyncSceneLoader.OnSceneUnloadedEvent += OnSceneUnload;
        SceneSwapManager.OnSceneLoadedEvent += OnSceneLoad;
        SceneSwapManager.OnSceneUnloadedEvent += OnSceneUnload;
    }

    private void OnDisable() 
    {
        AsyncSceneLoader.OnSceneLoadedEvent -= OnSceneLoad;
        AsyncSceneLoader.OnSceneUnloadedEvent -= OnSceneUnload;
        SceneSwapManager.OnSceneLoadedEvent -= OnSceneLoad;
        SceneSwapManager.OnSceneUnloadedEvent -= OnSceneUnload;
    }

}
