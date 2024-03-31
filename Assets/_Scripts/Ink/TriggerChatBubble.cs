using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerChatBubble : MonoBehaviour
{
    [SerializeField] private TypeWriterEffect _chatBubble; // Get the assigned chat that should be started

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            _chatBubble.StartChatBubble(); // Communicate to the gameobject to start the chat
            gameObject.gameObject.SetActive(false); // Deactivate this script since it served its purpose. Might be changed to destroy later
        }
    }
}
