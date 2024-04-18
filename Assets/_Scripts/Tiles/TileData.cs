using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Audio/Tile Sounds")]
public class TileData : ScriptableObject
{

    [SerializeField] private RuleTile[] _tiles;

    [SerializeField] private AudioClip[] _sounds;

    public AudioClip GetClip() 
    {
        return _sounds[Random.Range(0, _sounds.Length)];
    }

    public TileBase[] GetTileBases() 
    {
        return _tiles;
    }
}
