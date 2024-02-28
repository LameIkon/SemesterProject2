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

        TextMeshProUGUI storyText = Instantiate(TextPrefab); //takes a textfile prefab with predefined settings
        storyText.text = loadStoryChunk();
        storyText.transform.SetParent(this.transform, false);


        GameObject buttonsContainer = new GameObject("ButtonsContainer");
        buttonsContainer.transform.SetParent(this.transform, false);
        VerticalLayoutGroup layoutSettings = buttonsContainer.AddComponent<VerticalLayoutGroup>();
        layoutSettings.childControlHeight = false;

        foreach (Choice choice in _story.currentChoices)
        {
            Button choiceButton = Instantiate(ButtonPrefab);
            choiceButton.transform.SetParent(buttonsContainer.transform, false);

            TextMeshProUGUI choiceText = choiceButton.GetComponentInChildren<TextMeshProUGUI>();
            choiceText.text = choice.text;

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
