using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance { get; private set; }

    private Story _story; // Dialogue will be stored in this value

    [Header("UI Elements")]
    [SerializeField] private TextAsset _emptyTemplate; // Changes to empty template allowing Ink to update Dialogue   
    [SerializeField] private TextMeshProUGUI _textPrefab;
    [SerializeField] private Button _buttonPrefab;
    [SerializeField] private Image _dialogueImagePrefab;

    [Header("Stored data")]
    public bool _oneclick;
    public bool _dialogueExited = false;
    public List<string> savedTags = new List<string>(); //save all tags and store them for other scripts to use

    [Header("Selected Dialogue")]
    public TextAsset DialogueData; // used in other scripts to change the data. Other scripts will store their choosen dialogue data here


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
        _story = new Story(_emptyTemplate.text); // change file to empty data. IF this shows up then there have happened an error
    }


    public void InsertDialogue()
    {
        if (DialogueData != null) // Only if there is data are you allowed to start a dialogue
        {
            //Debug.Log("InkJSON file inserted");
            _story = new Story(DialogueData.text); // Insert dialogue Data into the story data.
        }
    }


    public void OpenUI()
    {
        if (DialogueData != null) // Only if there is data are you allowed to start a dialogue
        {
            //just to test
            if (Input.GetKeyDown(KeyCode.E)) // Press E to open dialogue UI
            {
                _oneclick = true; // Ensure only 1 instance. Used in other scripts
                refreshUI();
            }
        }
    }

    public void refreshUI()
    {
        eraseUI(); // Delete gameobjects if there is any

        // Creating the dialogue Gameobject container
        GameObject dialogueContainer = new GameObject("DialogueContainer"); // Create an Empty GameObject to store the dialogue as children
        dialogueContainer.transform.SetParent(transform, false); // Set GameObject to the parent but keep its own transform

        // Creating background image for dialogue
        Image dialogueBackground = Instantiate(_dialogueImagePrefab); // Takes a image prefab
        dialogueBackground.transform.SetParent(dialogueContainer.transform, false); // Set Image to the parent but keep its own transform

        // Creating the dialogue
        TextMeshProUGUI storyText = Instantiate(_textPrefab); // Takes a textfile prefab with predefined settings
        storyText.transform.SetParent(dialogueContainer.transform, false); // Set dialogue to the parent but keep its own transform

        // Implement vertical layout group for the container with settings
        VerticalLayoutGroup dialogueLayoutSettings = dialogueContainer.AddComponent<VerticalLayoutGroup>(); // Add vertical layout group componennt to buttonContainer
        dialogueLayoutSettings.spacing = -300; // Change spacing to match the dialogue box and text to be inside each other 
        dialogueLayoutSettings.childControlHeight = false; // Disable control child height, meaning height is constant now, no matter the parents height size

        storyText.text = loadStoryChunk();

        GetTag();



        // Creating the answers (buttons)
        GameObject buttonsContainer = new GameObject("ButtonsContainer"); // Create an empty gameobject to store buttons as children
        buttonsContainer.transform.SetParent(transform, false); // Set gameobject to the parent but keep its own transform
        VerticalLayoutGroup buttonLayoutSettings = buttonsContainer.AddComponent<VerticalLayoutGroup>(); // Add vertical layout group componennt to buttonContainer
        buttonLayoutSettings.childControlHeight = false; // Disable control child height, meaning height is constant now, no matter the parents height size

        if (_story.currentChoices.Count > 0) // If there is at least 1 choice button
        {
            foreach (Choice choice in _story.currentChoices) // CurrentChoices are a list used for ink for the choices you get
            {
                Button choiceButton = Instantiate(_buttonPrefab); // Create button with prefab
                choiceButton.transform.SetParent(buttonsContainer.transform, false); // Set buttons to the parent but keep its own transform

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
            endDialogueButton.transform.SetParent(buttonsContainer.transform, false); // Set buttons to the parent but keep its own transform
            TextMeshProUGUI endDialogueText = endDialogueButton.GetComponentInChildren<TextMeshProUGUI>(); // Create a new attribute choiceText as TextMeshProGUI
            endDialogueText.text = "End Dialogue"; // The button text is set to End Dialogue 

            endDialogueButton.onClick.AddListener(delegate
            {
                exitDialogue();
                eraseUI();
            }); // Clicking button will call the methods 
        }
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
            //Debug.Log(tags[0]);
            savedTags.Add(tags[0]); // Store the tag in a list to be used for alternative texts
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
        _oneclick = false; // Ensures 1 instance
        _dialogueExited = true; // Announces that the exitDialogue was called (used to check if player exited dialogue)
        Invoke("SetDialogueExitFalse", 0f); // Used to give a small frame for the text to change to the alternative text
    }

    // Invoked almost next frame to make boolean false
    void SetDialogueExitFalse()
    {
        _dialogueExited = false;
    }

    

}
