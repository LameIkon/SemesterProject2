using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class AIThinker : MonoBehaviour
{
    [SerializeField] private BrainAI _brain;

    private Dictionary<string, object> _memory;


    private bool inDialogue = false;

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

        StartDialogue.OnDialogueStartedEvent += HandleDialogueStart; // Handles events for dialogue to make NPC stop walking
        DialogueManager.OnDialogueEndedEvent += HandleDialogueEnd;

        if (!_brain)
        {
            enabled = false;
            return;
        }

        _memory = new Dictionary<string, object>();
        _brain.Initialize(this);
    }

    private void OnDisable()
    {
        StartDialogue.OnDialogueStartedEvent -= HandleDialogueStart;
        DialogueManager.OnDialogueEndedEvent -= HandleDialogueEnd;
    }

    void Update()
    {
        if (!inDialogue)
        {
            _brain.Think(this);
        }


        // Testing
        bool isRememberedTrue = Remember<bool>("idle");

        if (isRememberedTrue)
        {
            Debug.Log("The remembered bool is true!");

            // reset
            Remember("isTrue", false);
        }

    }

    public void SetBrain(BrainAI brain) 
    {
        _brain = brain; 
    }


    void HandleDialogueStart() 
    {
        inDialogue = true;
    }

    void HandleDialogueEnd() 
    {
        inDialogue = false;
    }

}
