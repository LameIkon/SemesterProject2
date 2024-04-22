using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// This script holds the tilemaps in the scenes. It gets them from events from other scripts, and updates which tilemaps are currently loaded.
/// This is also were the sounds for the walking is played from.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class TileMapManager : PersistentSingleton<TileMapManager>
{
    [SerializeField] private List<Tilemap> _tilemaps; // This is a list because it will change during runtime

    [SerializeField] private TileData[] _tileDatas; // An array of all the walking sounds that have been will bee used, this is constant

    private AudioSource _audioSource;

    private Dictionary<TileBase, TileData> _tileDictionary; // Dictionary to easily get data out to the AudioSource

    

    protected override void Awake()
    {
        base.Awake();

        _tileDictionary = new Dictionary<TileBase, TileData>();

        foreach (var tileData in _tileDatas) // Here we loop over the array of the TileData put into _tileDatas
        {
            foreach (var tile in tileData.GetTileBases())  // foreach TileData in _tileDatas we ad the data to the Dictionary such that we easily can get the AudioClips to the AudioSource
            {
                _tileDictionary.Add(tile, tileData);
            }
        }

        _audioSource = GetComponent<AudioSource>();

    }

    private void OnEnable()
    {
        PlayerController.OnMovePositionEvent += PlayWalkingAudio; // this event is sent from the PlayerController and plays the Walking audio
        SceneLoadTileManager.OnSceneLoadedEvent += AddTilemap;
        SceneLoadTileManager.OnSceneUnloadedEvent += RemoveTilemap;
        SceneLoadTileManager.OnSceneSwapEvent += ClearTilemap;
    }

    private void OnDisable()
    {
        PlayerController.OnMovePositionEvent -= PlayWalkingAudio;
        SceneLoadTileManager.OnSceneUnloadedEvent -= AddTilemap;
        SceneLoadTileManager.OnSceneUnloadedEvent -= RemoveTilemap;
        SceneLoadTileManager.OnSceneSwapEvent -= ClearTilemap;
    }



    public void PlayWalkingAudio(Vector2 worldPostion) // This method plays the sounds when walking
    {
        Vector3Int gridposition; // Get a grid position only as and it as the tiles are 1x1 big in the grid
        TileBase tile = null; // We assume that there is no tile underneath the player.

        foreach (var map in _tilemaps) 
        {
            gridposition = map.WorldToCell(worldPostion);
            tile = map.GetTile(gridposition);

            if (tile != null)
            {
                break;
            }
            else 
            {
                return;
            }
        }

        AudioClip clip =  _tileDictionary[tile].GetClip();

        if (_audioSource.isPlaying || clip == null)
        {
            return;
        }
        else 
        {
            _audioSource.clip = clip;
            _audioSource.Play();
        }

    }

    public void AddTilemap(Tilemap tilemap) 
    {
        if (!_tilemaps.Contains(tilemap))
        {
            _tilemaps.Add(tilemap);
        }
    }

    public void RemoveTilemap(Tilemap tilemap) 
    {
         _tilemaps.Remove(tilemap);
    }

    public void ClearTilemap() 
    {
        _tilemaps.Clear();
    }

    public void AddTilemap(List<Tilemap> tilemaps) 
    {
        foreach (var map in tilemaps) 
        {
            AddTilemap(map);
        }
    }

    public void RemoveTilemap(List<Tilemap> tilemaps)
    {
        foreach (var map in tilemaps)
        {
            RemoveTilemap(map);
        }
    }

    public void AddTilemap(Tilemap[] tilemaps)
    {
        foreach (var map in tilemaps)
        {
            AddTilemap(map);
        }
    }

    public void RemoveTilemap(Tilemap[] tilemaps)
    {
        foreach (var map in tilemaps)
        {
            RemoveTilemap(map);
        }  
    }

    private void OnApplicationQuit()
    {
        ClearTilemap();
    }

}
