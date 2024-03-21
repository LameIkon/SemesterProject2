using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioLooper : PersistentSingleton<AudioLooper>
{

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _clip;

    protected override void Awake()
    {
        base.Awake();
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        StartCoroutine(PlayBackground(_audioSource, _clip));
    }

    IEnumerator PlayBackground(AudioSource audioSource, AudioClip clip)
    {
        if (audioSource == null) 
        {
           
        }

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

    IEnumerator PlayerBackgroundArray(AudioSource audioSource, AudioClip[] clips)
    {
        while (true)
        {
            audioSource.clip = clips[Random.Range(0, clips.Length)];
            audioSource.Play();

            while (audioSource.isPlaying)
            {
                yield return null;
            }
        }
    }


    public void SetBackgroundSound(AudioClip clip)
    {
        StopAllCoroutines();
        StartCoroutine(PlayBackground(_audioSource, clip));
    }

    public void SetBackgroundSoundArray(AudioClip[] clips) 
    {
        StopAllCoroutines();
        StartCoroutine(PlayerBackgroundArray(_audioSource, clips));
    }


}
