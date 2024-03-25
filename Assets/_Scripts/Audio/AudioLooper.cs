using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script has a needs an Audio Source to work, it inherits from the PersistentSingleton class
[RequireComponent(typeof(AudioSource))]
public class AudioLooper : PersistentSingleton<AudioLooper>
{

    [SerializeField] private AudioClip _clip;
    private AudioSource _audioSource;


    protected override void Awake()
    {
        base.Awake();
        _audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        _audioSource.loop = true;
        if (_clip != null) {
            PlayBackground(_audioSource, _clip); // This will play the clip, that is inserted on the AudioLooper if there is one
        }
    }
    private void OnEnable()
    {
        AudioChangeTester._OnAudioChangeEvent += HandleBackgroundSound; // This is the event subscriptitons when these are called it will change the current audio
    }

    private void OnDisable()
    {
        AudioChangeTester._OnAudioChangeEvent -= HandleBackgroundSound;
    }

    // This will play an audio clip on the Audio Source
    void PlayBackground(AudioSource audioSource, AudioClip clip) 
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    // This is currently not in use, but it could come in use at a later stage
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

    // The handler for seting new background sounds
    public void HandleBackgroundSound(AudioClip clip)
    {
        // This will check that the clip comming is not the same, if it were missing it would restart the clip every time it is called
        if (_audioSource.clip.length != clip.length) 
        {
            _audioSource.Stop();
            PlayBackground(_audioSource, clip);
        }
    }

}
