using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(AudioSource))]
public class TileMapManager : PersistentSingleton<TileMapManager>
{
    [SerializeField] private Tilemap _tilemap;

    [SerializeField] TileData[] _tileDatas;

    private AudioSource _audioSource;

    private Dictionary<TileBase, TileData> _tileDictionary;

    

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
        Vector3Int gridposition = _tilemap.WorldToCell(worldPostion);

        TileBase tile = _tilemap.GetTile(gridposition);

        if (tile == null) 
        {
            return;
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


}
