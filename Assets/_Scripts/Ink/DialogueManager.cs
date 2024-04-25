using Ink.Runtime;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
//using UnityEditor.Animations;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance { get; private set; }

    private Story _story; // Dialogue will be stored in this value

    [Header("UI Elements")] // each prefab has components that determinds the layout of the dialogue
    [SerializeField] private GameObject _dialogueLayoutPrefab; // Parent GameObject
    [SerializeField] private GameObject _nameHolderPrefab; // Child GameObject of _dialogueLayoutPrefab
    [SerializeField] private GameObject _dialogueAnswerHolderPrefab;  // Child GameObject of _dialogueLayoutPrefab
    [SerializeField] private GameObject _buttonHolderPrefab; // Child GameObject of _dialogueAnswerPrefab
    [SerializeField] private Button _buttonPrefab; // Buttons on _buttonHolderPrefab

    [Header("Animations Elements")]
    [SerializeField] private GameObject _captainProfile; // Child GameObject of _nameHolderPrefab
    [SerializeField] private GameObject _scientistProfile; // Child GameObject of _nameHolderPrefab
    [SerializeField] private GameObject _spyProfile; // Child GameObject of _nameHolderPrefab
    [SerializeField] private GameObject _defaultProfile; // Child GameObject of _nameHolderPrefab

    [Header("Stored data")]
    public bool _Oneclick; // Used to ensure that only 1 dialogue can happen at a time
    public bool _DialogueExited; // Used to check if an dialogue is ongoing. Same as _oneclick but just made easier to understand the use of the bool
    public bool _StartedDialogue; // Used to check if an dialogue is started. Used in other scripts to call if an dialogue was called
    public List<string> _SavedTags = new List<string>(); // Save all tags and store them for other scripts to use. 
    public string _NPCName; // Use the name of the current person you talk with
    public bool _OnlyOneInteractionActive = true; // Used in other scritps to ensure instances will only run if we allow it to. for example chat highlight should be disabled when in dialogue

    [Header("Selected Person")]
    public string _NPCImage;

    [Header("Selected Dialogue")]
    public TextAsset DialogueData; // Used in other scripts to change the data. Other scripts will store their choosen dialogue data here

    [Header("Global Ink Variables"), SerializeField]
    private TextAsset _globalVariables;
    

    public static event Action OnDialogueEndedEvent; // Event for when Dialogue is finnished

    private bool _hasInteracted = false;

    private DialogueVariables _dialogueVariables;
        
    void Awake()
    {
        // Ensure only 1 singleton of this script
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
        _OnlyOneInteractionActive = true;


        Story globalVariables = new Story(_globalVariables.text);
        _dialogueVariables = new DialogueVariables(globalVariables);

    }

    private void OnEnable()
    {
        InputReader.OnInteractEvent += HandleInteract;
        InputReader.OnInteractEventCancled += HandleInteractCancled;
    }

    private void OnDisable()
    {
        InputReader.OnInteractEvent -= HandleInteract;
        InputReader.OnInteractEventCancled -= HandleInteractCancled;
    }

    public void InsertDialogue()
    {
        if (DialogueData != null) // Only if there is data are you allowed to start a dialogue
        {
            //Debug.Log("InkJSON file inserted");
            _story = new Story(DialogueData.text); // Insert dialogue Data into the story data.

            _dialogueVariables.StartListening(_story); // TROELS
        }
    }


    public void OpenUI()
    {
        if (DialogueData != null) // Only if there is data are you allowed to start a dialogue
        {
            if (_hasInteracted)
            {
                _hasInteracted= false;
                _Oneclick = true; // Ensure only 1 instance. Used in other scripts
                _StartedDialogue = true; // Used to to tell that an dialogue was started
                refreshUI();
            }

        }
    }

    public void refreshUI()
    {
        eraseUI(); // Delete gameobjects if there is any

        // Creating the dialogue layout
        GameObject dialogueLayout = Instantiate(_dialogueLayoutPrefab); // Create an Empty GameObject to store the dialogue as children
        dialogueLayout.transform.SetParent(transform, false); // Set GameObject to the parent but keep its own transform

        // Creating Nameholder for dialoguebox
        GameObject nameholder = Instantiate(_nameHolderPrefab); // Takes a image prefab
        nameholder.transform.SetParent(dialogueLayout.transform, false); // Set Image to the parent but keep its own transform


        // Creating the dialogue and answer box
        GameObject dialogueAnswerPrefab = Instantiate(_dialogueAnswerHolderPrefab); // Takes a textfile prefab with predefined settings
        dialogueAnswerPrefab.transform.SetParent(dialogueLayout.transform, false); // Set dialogue to the parent but keep its own transform

        // Creating the Answer/button holder
        GameObject buttonHolder = Instantiate(_buttonHolderPrefab); // Create an GameObject to hold the buttons
        buttonHolder.transform.SetParent(dialogueAnswerPrefab.transform, false); // Set GameObject to dialogueAnswerPrefab as parent


        // Insert Data

        // Insert data into nameholder
        TextMeshProUGUI nameHolderText = nameholder.GetComponentInChildren<TextMeshProUGUI>();
        nameHolderText.text = LoadNameOFNPC();

        //Inser Image into profile
        LoadImageOFNPC(nameholder);

        // Insert dialogue data into dialogue and answer box
        TextMeshProUGUI dialogueText = dialogueAnswerPrefab.GetComponentInChildren<TextMeshProUGUI>();
        dialogueText.text = loadStoryChunk();

        if (_story.currentChoices.Count > 0) // If there is at least 1 choice button
        {
            foreach (Choice choice in _story.currentChoices) // CurrentChoices are a list used for ink for the choices you get
            {
                Button choiceButton = Instantiate(_buttonPrefab); // Create button with prefab
                choiceButton.transform.SetParent(buttonHolder.transform, false); // Set buttons to the parent but keep its own transform

                TextMeshProUGUI choiceText = choiceButton.GetComponentInChildren<TextMeshProUGUI>(); // Create a new attribute choiceText as TextMeshProGUI
                choiceText.text = choice.text; // The button text is set to the current choice in the list

                choiceButton.onClick.AddListener(delegate
                {
                    chooseStoryChoice(choice);
                }); // Clicking button will go down that path
            }
        }
        else // If there is no choices to choose 
        {
            Button endDialogueButton = Instantiate(_buttonPrefab); // Create button with prefab
            endDialogueButton.transform.SetParent(buttonHolder.transform, false); // Set buttons to the parent but keep its own transform
            TextMeshProUGUI endDialogueText = endDialogueButton.GetComponentInChildren<TextMeshProUGUI>(); // Create a new attribute choiceText as TextMeshProGUI
            endDialogueText.text = "[End Dialogue]"; // The button text is set to End Dialogue 

            endDialogueButton.onClick.AddListener(delegate
            {
                exitDialogue();
                eraseUI();
            }); // Clicking button will call the methods 
        }

        GetTag();
    }

    // Destroy all child objects on the parent object which this script is on
    void eraseUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    // Get the tag from the dialogue and store it
    void GetTag()
    {
        List<string> tags = _story.currentTags; // Store the tag of the text that correlate to the button
        if (tags.Count > 0) // If there is more than 0 tags it will search
        {
            string tagToAdd = tags[0]; // Store the tag temporarily here
            {
                if (!_SavedTags.Contains(tagToAdd)) // Checks if there is a tag already with that name. If its isnt, it gets stored
                {
                    _SavedTags.Add(tagToAdd); // Store the tag in a list to be used for alternative texts
                }
                else
                {
                    Debug.Log("Already stored that tag");
                }
            }
        }
    }
    void chooseStoryChoice(Choice choice) // When button clicked 
    {
        _story.ChooseChoiceIndex(choice.index); // Show new dialogue from the choosen button path
        refreshUI();
    }

    // Takes the ink file to read 
    string loadStoryChunk()
    {
        string text = "";
        if (_story.canContinue)
        {
            text = _story.ContinueMaximally(); // Print all lines of rows to text
        }
        return text;
    }

    // Exit Dialouge
    private void exitDialogue()
    {
        _story = new Story(DialogueData.text); // Change file to the same dialogueData. Must be done otherwise you cant repeat the same dialouge

        _dialogueVariables.StopListening(_story);
        _Oneclick = false; // Ensures 1 instance
        _DialogueExited = true; // Announces that the exitDialogue was called (used to check if player exited dialogue)
        _StartedDialogue = false; // announces that the dialogue has ended
        Invoke("SetDialogueExitFalse", 0f); // Used to give a small frame for the text to change to the alternative text
    }

    // Invoked almost next frame to make boolean false
    void SetDialogueExitFalse()
    {
        _DialogueExited = false;
        OnDialogueEndedEvent?.Invoke(); // Event for when the dialogue is finnished 
    }

    string LoadNameOFNPC()
    {
        if (_NPCName == null)
        {
            _NPCName = "";
        }
        return _NPCName;
    }

    void LoadImageOFNPC(GameObject nameholder)
    {
        // Creating image for dialoguebox
       

        if (_NPCName == "Hans")
        {
            Debug.Log("Scientist");
            GameObject profile = Instantiate(_scientistProfile); // Takes a gameobject prefab
            profile.transform.SetParent(nameholder.transform, false); // Set Image to the parent but keep its own transform
        }
        else if (_NPCName == "Ejnar")
        {
            Debug.Log("Captain");
            GameObject profile = Instantiate(_captainProfile); // Takes a gameobject prefab
            profile.transform.SetParent(nameholder.transform, false); // Set Image to the parent but keep its own transform
        }
        else if (_NPCName == "Laub")
        {
            Debug.Log("Spy");
            GameObject profile = Instantiate(_spyProfile); // Takes a gameobject prefab
            profile.transform.SetParent(nameholder.transform, false); // Set Image to the parent but keep its own transform
        }
        else
        {
            Debug.Log("default");
            GameObject profile = Instantiate(_defaultProfile); // Takes a gameobject prefab
            profile.transform.SetParent(nameholder.transform, false); // Set Image to the parent but keep its own transform
        }
    }


    void HandleInteract() 
    {
        _hasInteracted = true;
    }

    void HandleInteractCancled() 
    {
        _hasInteracted = false;
    }

}
