using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InkTestingScript : MonoBehaviour
{
    /// <summary>
    /// Nuværende udfordringer er at få forskellige inkJSON filer herind uden at skabe dublicationer. Dette laves sikkert om til en singleton, hvor vi så skal give 
    /// information udefra. Derudover skal vi også gemme de svarmuligheder man tager.
    /// 
    /// Ligenu har vi en primitiv måde at huske svar, men hvis vi vil være mere præcise i hvilke svar vi skal gemme kunne vi måske gøre brug af tags
    /// til de få specialle dialogues der kan bestemme spillets retning. fx. kan vi have uniq tags, som så de bliver taget vil give 
    /// nye bestemte dialogues.
    /// </summary>

    private Story _story;
    
    [SerializeField] private TextAsset _inkJSON;
    [SerializeField] private TextMeshProUGUI _textPrefab;
    [SerializeField] private Button _buttonPrefab;
    [SerializeField] private Image _dialogueImagePrefab;

    private bool _oneclick; // for testing purpose only. only activate once

    // Start is called before the first frame update
    void Awake()
    {
        //store the ink story in a textfile
        _story = new Story(_inkJSON.text);
    }


    // Update is called once per frame
    void Update()
    {
        //just to test
        if (Input.GetKeyDown(KeyCode.Space) && !_oneclick)
        {
            _oneclick = true;
            refreshUI();
        }
    }

    void refreshUI()
    {
        eraseUI();

        

        //creating the dialogue Gameobject container
        GameObject dialogueContainer = new GameObject("DialogueContainer");
        dialogueContainer.transform.SetParent(transform, false);

        //creating background image for dialogue
        Image dialogueBackground = Instantiate(_dialogueImagePrefab);
        dialogueBackground.transform.SetParent(dialogueContainer.transform, false);

        //creating the dialogue
        TextMeshProUGUI storyText = Instantiate(_textPrefab); //takes a textfile prefab with predefined settings
        storyText.transform.SetParent(dialogueContainer.transform, false); //set dialogue to the parent but keep its own transform
       
        //implement vertical layout group for the container with settings
        VerticalLayoutGroup dialogueLayoutSettings = dialogueContainer.AddComponent<VerticalLayoutGroup>();
        dialogueLayoutSettings.spacing = -300;
        dialogueLayoutSettings.childControlHeight = false;

        storyText.text = loadStoryChunk();

        GetTag();

        

            //creating the answers (buttons)
            GameObject buttonsContainer = new GameObject("ButtonsContainer"); //create an empty gameobject to store buttons as children
            buttonsContainer.transform.SetParent(transform, false); //set gameobject to the parent but keep its own transform
            VerticalLayoutGroup buttonLayoutSettings = buttonsContainer.AddComponent<VerticalLayoutGroup>(); // add vertical layout group componennt to buttonContainer
            buttonLayoutSettings.childControlHeight = false; // disable control child height, meaning height is constant now
        
        if (_story.currentChoices.Count > 0)
        {
            foreach (Choice choice in _story.currentChoices) //currentChoices are a list used for ink for the choices you get
            {
                Button choiceButton = Instantiate(_buttonPrefab); // create button
                choiceButton.transform.SetParent(buttonsContainer.transform, false); //set buttons to the parent but keep its own transform

                TextMeshProUGUI choiceText = choiceButton.GetComponentInChildren<TextMeshProUGUI>(); // create a new attribute choiceText as TextMeshProGUI
                choiceText.text = choice.text; //the button text is set to the current choice in the list

                choiceButton.onClick.AddListener(delegate
                {
                    chooseStoryChoice(choice);
                });
            }
        }
        else
        {
            Button endDialogueButton = Instantiate(_buttonPrefab);
            endDialogueButton.transform.SetParent(buttonsContainer.transform, false);
            TextMeshProUGUI endDialogueText = endDialogueButton.GetComponentInChildren<TextMeshProUGUI>();
            endDialogueText.text = "End Dialogue";

            endDialogueButton.onClick.AddListener(delegate
            {
                exitDialogue();
                eraseUI();
            });
        }
    }

    //destroy all child objects on the parent object which this script is on
    void eraseUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    void GetTag()
    {
        //Testing if i can get the tags
        List<string> tags = _story.currentTags; //store the tag of the text that correlate to the button
        if (tags.Count > 0)
        {
            Debug.Log(tags[0]); //there will always be 1 tag at most since currentTags only checks on the text at which the button correlate to
        }
    }
    void chooseStoryChoice(Choice choice)
    {
        _story.ChooseChoiceIndex(choice.index);
        refreshUI();

        //can be used to remember choices
        //Debug.Log("The answer you took is index: "+choice.index + " Which can be be used to remeber the choices you made");
    }

    //takes the ink file to read 
    string loadStoryChunk()
    {
        string text = "";
        if (_story.canContinue)
        {
            text = _story.ContinueMaximally(); //print all lines of rows to text
        }
        return text;
    }

    void exitDialogue()
    {
        Debug.Log("you exited dialogue");
        _oneclick = false; // open up dialogue again
    }

}
