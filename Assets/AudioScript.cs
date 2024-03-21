using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : PersistentSingleton<AudioScript>
{

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _clip;


    void Start() 
    {
        StartCoroutine(PlayBackground(_audioSource, _clip));
    }

    IEnumerator PlayBackground(AudioSource audioSource, AudioClip clip)
    {
        while (true)
        {
            audioSource.clip = clip;
            audioSource.Play();


            while (audioSource.isPlaying)
            {
                yield return null;
            }
        }
    }



}
