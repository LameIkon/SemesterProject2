
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Event", menuName = "AudioEvent/Simple Audio Event")]
public class SimpleAudioEvent : AudioEvent
{
    [Header("Clips"), Space(2f)]
    [SerializeField] private AudioClip[] _clips;

    [Header("Settings")]
    [SerializeField] private RangedFloat _volume;
    [SerializeField] private RangedFloat _pitch;

    public override void Play(AudioSource source)
    {
        source.clip = _clips[Random.Range(0, _clips.Length)];
        source.volume = Random.Range(_volume.MinValue, _volume.MaxValue);
        source.pitch = Random.Range(_pitch.MinValue, _pitch.MaxValue);

        source.Play();

    }

}