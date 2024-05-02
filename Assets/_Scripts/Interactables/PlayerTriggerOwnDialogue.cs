using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerOwnDialogue : MonoBehaviour
{
    private GameObject _chatBubble;
    [SerializeField] private int _childBubbleIndex = 0; // change in inspector for the gameobject to activate

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (_chatBubble == null)
            {
                _chatBubble = collision.transform.GetChild(1).GetChild(_childBubbleIndex).gameObject;
            }
            _chatBubble.SetActive(true);
        }
    }

}
