using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DialogueManager : MonoBehaviour
{
    /// <summary>
    /// indtil videre ser jeg ingen grund til at have et scriptable object af _dialogueData da det er præcis det samme jeg gør. Måske hvis jeg havde mere kode.
    /// Som det er sat op nu, vil den ændre data hver gang attribute bliver ændret. udfordringen jeg står i lige nu er hvordan jeg skal kommunikere at 
    /// jeg har brug for ny dialogue. 
    /// </summary>
    /// 

    public static DialogueManager Instance { get; private set; }

    private Story _story;

    [SerializeField] private TextAsset _emptyTemplate;
    //[SerializeField] private DialogueData _dialogueData;
    public TextAsset DialogueData;
    [SerializeField] private TextMeshProUGUI _textPrefab;
    [SerializeField] private Button _buttonPrefab;
    [SerializeField] private Image _dialogueImagePrefab;

    public bool _oneclick; // for testing purpose only. only activate once
    public bool _newFile;

    // Start is called before the first frame update
    void Awake()
    {
          //store the ink story in a textfile
          //_story = new Story(_dialogueData.InkJSON.text);
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }

        _story = new Story(_emptyTemplate.text); // change file to empty data
    }
    

    public void InsertDialogue()
    {
        if (DialogueData !=null /*&& _dialogueData.InkJSON != null*/)
        {
            Debug.Log("InkJSON file inserted");
            _newFile = true; // new file means you can open up dialogue
            _story = new Story(DialogueData/*.InkJSON.*/.text);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void OpenUI()
    {
        if (DialogueData != null /*&& _dialogueData.InkJSON != null*/)
        {
            //just to test
            if (Input.GetKeyDown(KeyCode.E))
            {
                _oneclick = true;
                refreshUI();
            }
        }
    }

    public void refreshUI()
    {
        eraseUI(); // delete gameobjects if there is any

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
        _story = new Story(DialogueData.text); // change file to empty data
        _oneclick = false; // ensures 1 instance
        _newFile = false; // cannot open up dialogue if data hasnt been changed out. validate
    }

}
