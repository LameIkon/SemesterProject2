using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class AIThinker : MonoBehaviour
{
    [SerializeField] private BrainAI _brain;

    private Dictionary<string, object> _memory;

    public T Remember<T>(string key)
    {
        object result;
        if (!_memory.TryGetValue(key, out result))
            return default(T);
        return (T)result;
    }

    public void Remember<T>(string key, T value)
    {
        _memory[key] = value;
    }


    void OnEnable()
    {
        if (!_brain)
        {
            enabled = false;
            return;
        }

        _memory = new Dictionary<string, object>();
        _brain.Initialize(this);
    }

    void Update()
    {
        _brain.Think(this);
    }

    public void SetBrain(BrainAI brain) 
    {
        _brain = brain; 
    }

}
