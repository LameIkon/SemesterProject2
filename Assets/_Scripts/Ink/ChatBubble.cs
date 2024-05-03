using System.Collections;
using System;
using TMPro;
using UnityEngine;

public class ChatBubble : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textComponent;
    [SerializeField, TextArea(2,4)] private string[] _lines;
    [SerializeField] private float _textSpeed;
    [SerializeField] private float _nextLineShowUp = 1;
    private int _index;

    [Space (10), SerializeField] private bool _automaticStart; // Used to start a chat right away
    [SerializeField] private bool _hideEOnEnteract; // used to hide interact

    [Space(5), Header("Do not touch!"), SerializeField, Tooltip("Used to call the ShipIn scene set to false!!!!! except on StoryManager")] private bool _shipInLoad = false;
    [SerializeField, Tooltip("Used to call the MainMenu scene set to false!!!!! except on StoryManager")] private bool _mainMenuLoad = false;
    public static event Action OnChatEndEvent;
    public static event Action OnGameEndEvent;

    private void Awake()
    {
        if (!_automaticStart)
        {
            gameObject.SetActive(false); // if there isnt any dialogue deactivate.
        }
    }

    private void Start()
    {
        if (_automaticStart) // Used if you want to start the chat right away
        {
            StartChatBubble();
        }
    }

    public void StartChatBubble()
    {
        gameObject.SetActive(true); // Show dialogue chat bubble
        _textComponent.text = string.Empty; // removes text if there is any
        StartDialogue();
    }

    void StartDialogue()
    {
        _index = 0; // The choosen index with dialogue
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        if (_hideEOnEnteract)
        {
            PriorityManager._canInteractDialogue = false;
        }
        foreach (char c in _lines[_index].ToCharArray()) // checks of many letters there is in the given line
        {
            _textComponent.text += c; // Input the letter in the dialogue
            yield return new WaitForSeconds(_textSpeed); // wait before looking for next letter
        }

        yield return new WaitForSeconds(_nextLineShowUp); // When dialogue is finished start this

        NextLine(); // Look for next line
    }

    void NextLine()
    {
        if (_index < _lines.Length - 1) // if there is more lines to be read
        {
            _index++; // Go to next index
            _textComponent.text = string.Empty; // Reset dialogue
            StartCoroutine(TypeLine()); // Start over again in coroutine
        }
        else
        {
            if (_shipInLoad)
            {
                OnChatEndEvent?.Invoke();
            }
            else if (_mainMenuLoad) 
            {
                OnGameEndEvent?.Invoke();
            }
            gameObject.SetActive(false); // if there isnt any dialogue deactivate.
            if (_hideEOnEnteract)
            {
                PriorityManager._canInteractDialogue = true;  
            }
        }
    }
}
