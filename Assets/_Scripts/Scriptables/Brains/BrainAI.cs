using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BrainAI : ScriptableObject
{
    public virtual void Initialize(AIThinker brain) { }

    public abstract void Think(AIThinker brain);

}

public enum Directions 
{
    N,
    S,
    E,
    W,
    NE,
    NW,
    SE,
    SW
}