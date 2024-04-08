using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Script not finished
public class SceneLoadTileManager : PersistentSingleton<SceneLoadTileManager>
{

    public static event Action<Tilemap[]> OnSceneLoadedEvent;


    private Tilemap[] OnSceneLoad() 
    {
        return GetComponentsInChildren<Tilemap>();
    }


}
