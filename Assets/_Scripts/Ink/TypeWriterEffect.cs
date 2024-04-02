using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TypeWriterEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textComponent;
    [SerializeField, TextArea(2,4)] private string[] _lines;
    [SerializeField] private float _textSpeed;
    private int _index;


    private void Awake()
    {
        gameObject.SetActive(false); // if there isnt any dialogue deactivate.
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
        foreach (char c in _lines[_index].ToCharArray()) // checks of many letters there is in the given line
        {
            _textComponent.text += c; // Input the letter in the dialogue
            yield return new WaitForSeconds(_textSpeed); // wait before looking for next letter
        }

        yield return new WaitForSeconds(2f); // When dialogue is finished start this

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
            gameObject.SetActive(false); // if there isnt any dialogue deactivate.
        }
    }
}
