using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private bool _randomLine; // used to get a random line
    private bool _showOnce = true;
    private bool _coroutineRunning;


    private void OnEnable() // called everytime its enabled
    {
        if (!_automaticStart)
        {
            gameObject.SetActive(false); // if there isnt any dialogue deactivate.
        }

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
        if (!_coroutineRunning) // Ensure only 1 instance
        {
            _index = 0; // The choosen index with dialogue
            StartCoroutine(TypeLine());
        }
    }

    IEnumerator TypeLine()
    {
        _coroutineRunning = true;
        if (_hideEOnEnteract)
        {
            PriorityManager._canInteractDialogue = false;
        }
        if (!_randomLine)
        {
            foreach (char c in _lines[_index].ToCharArray()) // checks of many letters there is in the given line
            {
                _textComponent.text += c; // Input the letter in the dialogue
                yield return new WaitForSeconds(_textSpeed); // wait before looking for next letter
            }

            yield return new WaitForSeconds(_nextLineShowUp); // When dialogue is finished start this

            NextLine(); // Look for next line
        }
        else if (_randomLine)
        {
            int number = Random.Range(0, _lines.Length);
            foreach (char c in _lines[number].ToCharArray()) // checks of many letters there is in the given line
            {
                _textComponent.text += c; // Input the letter in the dialogue
                yield return new WaitForSeconds(_textSpeed); // wait before looking for next letter
            }

            yield return new WaitForSeconds(_nextLineShowUp); // When dialogue is finished start this

            EndChat(); // End
        }
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
            EndChat();
        }    
    }
   
    void EndChat()
    {
        _coroutineRunning = false; // coroutine is not running
        gameObject.SetActive(false); // if there isnt any dialogue deactivate.
        if (_hideEOnEnteract)
        {
            PriorityManager._canInteractDialogue = true;
        }
    }
}
