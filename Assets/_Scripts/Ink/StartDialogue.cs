using Ink.Parsed;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StartDialogue : MonoBehaviour
{
    private bool _startDialogue;
    //private GameObject _highlight; // Used to get the GameObject named highligh
    //private GameObject _textbox; // Used to get the GameObject named textbox
    //private GameObject _showInteraction; // Used to get the GameObject named showInteraction
    private bool _interactionChecker;
    [SerializeField] private Highlight _highlightScript;

    [Header("NPC Name")]
    public string _NPCName; // Insert the name of the NPC in the inspector

    [Header("Default Dialogue")]
    [SerializeField] private TextAsset _defaultDialogue;

    [Header("Alternative Dialogue versions")]
    [SerializeField] private TextAsset _alt1Dialogue;
    [SerializeField] private TextAsset _alt2Dialogue;
    [SerializeField] private TextAsset _alt3Dialogue;
    [SerializeField] private TextAsset _alt4Dialogue;

    [Header ("Selected Dialogue")]
    [SerializeField] private TextAsset _chosenDialogue;

    public static event Action OnDialogueStartedEvent;

    private void Awake()
    {
        // Fist child is canvas and the next is the child of the canvas
        //_highlight = transform.GetChild(0).GetChild(0).gameObject; // Get the child of child attached to the parent.
        //_textbox = transform.GetChild(0).GetChild(1).gameObject; // Get the child of child attached to the parent.
        //_showInteraction = transform.GetChild(0).GetChild(2).gameObject; // Get the child of child attached to the parent.
        _highlightScript = GetComponentInChildren<Highlight>();

    }


    // Update is called once per frame
    void Update()
    {
       if (_startDialogue) // Only run this if you are inside the area collider
       {         
           IfDialogue();
           if (DialogueManager.instance._DialogueExited) // Only run this when _dialogueExited bool from the singleton is true. used to check when you exit the dialogue
           {
                UpdateDialogue();
                _interactionChecker = false; // you can now show interaction again
                _highlightScript.TriggerUse(false);
            }
           if (DialogueManager.instance._StartedDialogue && !_interactionChecker) // Only run this when you start a dialogue
           {
                _interactionChecker = true;
                _highlightScript.TriggerUse(true);
                OnDialogueStartedEvent?.Invoke(); // This event is called such NPC do not move during dialogue
           }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && !DialogueManager.instance._Oneclick) // Detect if the collision is the gameobject called Player
        {

            _highlightScript.TriggerEnter(gameObject);
            if (!_highlightScript.TriggerEnter(gameObject) && PriorityManager._PriorityInteractable)
            {
                Debug.Log("cannot continue");
                return;
            }

            DialogueManager.instance._NPCName = _NPCName;
            UpdateDialogue();      

            _startDialogue = true; // Set to true allowing start dialogue (Warning be sure there arent overlapping triggers, might cause problems)
            _highlightScript.TriggerEnter(gameObject);
            if (!GameManager._hideEInteractables)
            {
                _startDialogue = true; // Set to true allowing start dialogue (Warning be sure there arent overlapping triggers, might cause problems)
                //_highlightScript.TriggerEnter(gameObject);
                Debug.Log("Hide E");
            }           
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player" && !DialogueManager.instance._Oneclick)
        {
            _highlightScript.TriggerEnter(gameObject);
            if (!_highlightScript.TriggerEnter(gameObject))
            {
                Debug.Log("cannot continue");
                return;
            }
            DialogueManager.instance._NPCName = _NPCName;
            UpdateDialogue();

            _startDialogue = true; // Set to true allowing start dialogue (Warning be sure there arent overlapping triggers, might cause problems)
            _highlightScript.TriggerEnter(gameObject);
            if (!GameManager._hideEInteractables)
            {
                _startDialogue = true; // Set to true allowing start dialogue (Warning be sure there arent overlapping triggers, might cause problems)
                //_highlightScript.TriggerEnter(gameObject);
                Debug.Log("Hide E");
            }

        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player") // Detect if the collision is the gameobject called Player
        {
            _startDialogue = false; // Set to false to disable dialogue options
            _highlightScript.TriggerExit(gameObject);
        }
    }

    void IfDialogue()
    {
        if (!DialogueManager.instance._Oneclick) // Ensures only 1 UI can be open at a time
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

        foreach (var tag in DialogueManager.instance._SavedTags) // Search singleton list for tags
        {
            if(tagToDialogueMap.TryGetValue(tag, out TextAsset dialogue)) // If tag matches the tags in dictionary 
            {
                _chosenDialogue = dialogue; // Assign tag to choosen dialogue
                //Debug.Log("you found" + tag);
            }
        }
    }
}
