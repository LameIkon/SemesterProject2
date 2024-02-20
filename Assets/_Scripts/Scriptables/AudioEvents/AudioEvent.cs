using UnityEngine;
using Random = UnityEngine.Random;

public abstract class AudioEvent : ScriptableObject
{
    public abstract void Play(AudioSource source);
}