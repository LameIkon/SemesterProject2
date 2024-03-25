using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class AudioChangeTester : MonoBehaviour
{

    public static event Action<AudioClip> _OnAudioChangeEvent;

    [SerializeField] private AudioClip _clip;


    private void ChangeAudio() 
    {
        _OnAudioChangeEvent?.Invoke(_clip);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            ChangeAudio();
        }

    }


}
