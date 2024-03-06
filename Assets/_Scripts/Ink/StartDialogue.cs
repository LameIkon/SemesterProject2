using Ink.Parsed;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogue : MonoBehaviour
{
    private bool _startDialogue;

    [Header("Default Dialogue")]
    [SerializeField] private TextAsset _defaultDialogue;

    [Header("Alternative Dialogue versions")]
    [SerializeField] private TextAsset _alt1Dialogue;
    [SerializeField] private TextAsset _alt2Dialogue;
    [SerializeField] private TextAsset _alt3Dialogue;
    [SerializeField] private TextAsset _alt4Dialogue;

    [Header ("Selected Dialogue")]
    [SerializeField] private TextAsset _chosenDialogue;



    // Update is called once per frame
    void Update()
    {
       if (_startDialogue) // Only run this if you are inside the area collider
       {
           IfDialogue();
           if (DialogueManager.instance._dialogueExited) // Only run this when _dialogueExited bool from the singleton is true. used to check when you exit the dialogue
           {
                UpdateDialogue();
           }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player") // Detect if the collision is the gameobject called Player
        {
            //Debug.Log("Enter");
            UpdateDialogue();
            _startDialogue = true; // Set to true allowing start dialogue (Warning be sure there arent overlapping triggers, might cause problems)
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player") // Detect if the collision is the gameobject called Player
        {
            //Debug.Log("Exit");
            _startDialogue = false; // Set to false to disable dialogue options
        }
    }

    void IfDialogue()
    {
        if (DialogueManager.instance._oneclick == false) // Ensures only 1 UI can be open at a time
        {
            DialogueManager.instance.OpenUI(); // Open UI
        }       
    }

    void UpdateDialogue()
    {
        ChooseDialogueVersion();
        DialogueManager.instance.DialogueData = _chosenDialogue; // Store the dialogue as data for the singleton
        DialogueManager.instance.InsertDialogue(); // Run a method that will showcase the dialogue in the UI
    }

    void ChooseDialogueVersion()
    {
        _chosenDialogue = _defaultDialogue; // If there is no tag to be found take this as default
        Dictionary<string, TextAsset> tagToDialogueMap = new Dictionary<string, TextAsset>() // Stores all tags and takes the last tag it finds as true.
        {
            // Chose the alternative dialogue options. IF there isnt an alternative take the choosen option whether its the default or an alternative.
            {"testTag1", _alt1Dialogue  != null ? _alt1Dialogue : _chosenDialogue}, 
            {"testTag2", _alt2Dialogue  != null ? _alt2Dialogue : _chosenDialogue},
            {"testTag3", _alt3Dialogue  != null ? _alt3Dialogue : _chosenDialogue},
            {"testTag4", _alt4Dialogue  != null ? _alt4Dialogue : _chosenDialogue}
        };

        foreach (var tag in DialogueManager.instance.savedTags) // Search singleton list for tags
        {
            if(tagToDialogueMap.TryGetValue(tag, out TextAsset dialogue)) // If tag matches the tags in dictionary 
            {
                _chosenDialogue = dialogue; // Assign tag to choosen dialogue
                //Debug.Log("you found" + tag);
            }
        }
    }
}
