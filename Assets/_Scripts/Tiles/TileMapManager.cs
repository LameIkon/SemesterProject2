using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Script not finished
[RequireComponent(typeof(AudioSource))]
public class TileMapManager : PersistentSingleton<TileMapManager>
{
    [SerializeField] private List<Tilemap> _tilemaps; // This is a list because it will change during runtime

    [SerializeField] TileData[] _tileDatas;

    private AudioSource _audioSource;

    private Dictionary<TileBase, TileData> _tileDictionary; // Dictionary to easily get data out to the AudioSource

    

    protected override void Awake()
    {
        base.Awake();

        _tileDictionary = new Dictionary<TileBase, TileData>();

        foreach (var tileData in _tileDatas) 
        {
            foreach (var tile in tileData.GetTileBases()) 
            {
                _tileDictionary.Add(tile, tileData);
            }
        }

        _audioSource = GetComponent<AudioSource>();

    }

    private void OnEnable()
    {
        PlayerController.OnMovePositionEvent += PlayWalkingAudio;
    }

    private void OnDisable()
    {
        PlayerController.OnMovePositionEvent -= PlayWalkingAudio;
    }



    public void PlayWalkingAudio(Vector2 worldPostion) 
    {
        Vector3Int gridposition;
        TileBase tile = null;

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

        if (_audioSource.isPlaying)
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
        _tilemaps.Add(tilemap);
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
            _tilemaps.Add(map);
        }
    }

    public void RemoveTilemap(List<Tilemap> tilemaps) 
    {
        foreach (var map in tilemaps) 
        {
            _tilemaps.Remove(map);
        }
    }

    public void AddTilemap(Tilemap[] tilemaps)
    {
        foreach (var map in tilemaps)
        {
            _tilemaps.Add(map);
        }
    }

    public void RemoveTilemap(Tilemap[] tilemaps)
    {
        foreach (var map in tilemaps)
        {
            _tilemaps.Remove(map);
        }
    }

}
