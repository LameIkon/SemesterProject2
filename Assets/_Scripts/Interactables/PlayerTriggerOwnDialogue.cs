using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerOwnDialogue : MonoBehaviour
{
    private GameObject _chatBubble;
    [SerializeField] private int _childBubbleIndex = 0; // change in inspector for the gameobject to activate

    [SerializeField] private bool _callTriggerStay;
    [SerializeField] private float _callBackStayTime = 30;
    private bool _oneInstance;


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


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && _callTriggerStay)
        {
            if (!_oneInstance)
            {
                _oneInstance = true;
                Invoke(nameof(TriggerStay), _callBackStayTime);
            }
        }
    }

    void TriggerStay()
    {
        _oneInstance = true;
        _chatBubble.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CancelInvoke(nameof(TriggerStay));
    }
}
