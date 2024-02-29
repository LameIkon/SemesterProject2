using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InkTestingScript : MonoBehaviour
{
    public TextAsset InkJSON;
    private Story _story;

    public TextMeshProUGUI TextPrefab;
    public Button ButtonPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //store the ink story in a textfile
        _story = new Story(InkJSON.text);
    }


    // Update is called once per frame
    void Update()
    {
        //just to test
        if (Input.GetKeyDown(KeyCode.Space))
        {
            refreshUI();
        }
    }

    void refreshUI()
    {
        eraseUI();

        //creating the dialogue
        TextMeshProUGUI storyText = Instantiate(TextPrefab); //takes a textfile prefab with predefined settings
        storyText.text = loadStoryChunk();
        storyText.transform.SetParent(this.transform, false); //set dialogue to the parent but keep its own transform


        //creating the answers (buttons)
        GameObject buttonsContainer = new GameObject("ButtonsContainer"); //create an empty gameobject to store buttons as children
        buttonsContainer.transform.SetParent(this.transform, false); //set gameobject to the parent but keep its own transform
        VerticalLayoutGroup layoutSettings = buttonsContainer.AddComponent<VerticalLayoutGroup>(); // add vertical layout group componennt to buttonContainer
        layoutSettings.childControlHeight = false; // disable control child height, meaning height is constant now

        foreach (Choice choice in _story.currentChoices) //currentChoices are a list used for ink for the choices you get
        {
            Button choiceButton = Instantiate(ButtonPrefab); // create button
            choiceButton.transform.SetParent(buttonsContainer.transform, false); //set buttons to the parent but keep its own transform

            TextMeshProUGUI choiceText = choiceButton.GetComponentInChildren<TextMeshProUGUI>(); // create a new attribute choiceText as TextMeshProGUI
            choiceText.text = choice.text; //the button text is set to the current choice in the list

            choiceButton.onClick.AddListener(delegate
            {
                chooseStoryChoice(choice);
            });
        }
    }

    void eraseUI()
    {
        for(int i = 0; i < this.transform.childCount; i++)
        {
            Destroy(this.transform.GetChild(i).gameObject);
        } 
    }

    void chooseStoryChoice(Choice choice)
    {
        _story.ChooseChoiceIndex(choice.index);
        refreshUI();
    }

    //takes the ink file to read 
    string loadStoryChunk()
    {
        string text = "";
        if (_story.canContinue)
        {
            text = _story.ContinueMaximally();
        }
        return text;
    }
}
