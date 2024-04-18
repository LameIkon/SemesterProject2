using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Script not finished
public class SceneLoadTileManager : MonoBehaviour
{

    public static event Action<Tilemap[]> OnSceneLoadedEvent;
    public static event Action<Tilemap[]> OnSceneUnloadedEvent;
    public static event Action OnSceneSwapEvent;


    private void OnSceneLoad() 
    {
        OnSceneLoadedEvent?.Invoke(GetComponentsInChildren<Tilemap>());
    }

    private void OnSceneUnload() 
    {
        OnSceneUnloadedEvent?.Invoke(GetComponentsInChildren<Tilemap>());
    }

    private void OnSceneUnloadSwap() 
    {
        OnSceneSwapEvent?.Invoke();
    }

    private void OnEnable() 
    {
        AsyncSceneLoader.OnSceneLoadedEvent += OnSceneLoad;
        AsyncSceneLoader.OnSceneUnloadedEvent += OnSceneUnload;
        SceneSwapManager.OnSceneLoadedEvent += OnSceneLoad;
        SceneSwapManager.OnSceneUnloadedEvent += OnSceneUnloadSwap;
    }

    private void OnDisable() 
    {
        AsyncSceneLoader.OnSceneLoadedEvent -= OnSceneLoad;
        AsyncSceneLoader.OnSceneUnloadedEvent -= OnSceneUnload;
        SceneSwapManager.OnSceneLoadedEvent -= OnSceneLoad;
        SceneSwapManager.OnSceneUnloadedEvent -= OnSceneUnloadSwap;
    }

}
