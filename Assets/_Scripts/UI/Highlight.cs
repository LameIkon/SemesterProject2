using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    [SerializeField] private UnityEngine.GameObject _showInteraction; // Used to get the GameObject named showInteraction

    private void Start()
    {
        // Fist child is canvas and the next is the child of the canvas
        //_showInteraction = transform.GetChild(1).GetChild(0).gameObject; // Get the child of child attached to the parent.
        _showInteraction.SetActive(false); // Set false if by chance its active
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && DialogueManager.instance._OnlyOneInteractionActive && !GameManager._hideEInteractables) // Detect if the collision is the gameobject called Player
        {
            _showInteraction.SetActive(true);
            
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && DialogueManager.instance._OnlyOneInteractionActive && GameManager._hideEInteractables)
        {
            _showInteraction.SetActive(false);
        }
        else if (collision.gameObject.name == "Player" && DialogueManager.instance._OnlyOneInteractionActive && !GameManager._hideEInteractables) 
        {
            _showInteraction.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player") // Detect if the collision is the gameobject called Player
        {
            _showInteraction.SetActive(false);
        }
    }
}
