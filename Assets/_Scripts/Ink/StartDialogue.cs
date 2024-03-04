using Ink.Parsed;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogue : MonoBehaviour
{
    //public static event Action testevent;
    // Start is called before the first frame update

    private bool _startDialogue;
    public TextAsset Dialogue;

    // Update is called once per frame
    void Update()
    {
       if (_startDialogue)
        {
            IfDialogue();
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            Debug.Log("Enter");
            Dia.instance.DialogueData = Dialogue;
            DialogueManager.instance.InsertDialogue();
            _startDialogue = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            Debug.Log("Exit");
            _startDialogue = false;
        }
    }

    void IfDialogue()
    {
        if (DialogueManager.instance._oneclick == false)
        {     
            DialogueManager.instance.OpenUI();
        }
        
    }

}
