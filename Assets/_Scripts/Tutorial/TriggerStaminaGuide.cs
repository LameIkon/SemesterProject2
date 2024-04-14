using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerStaminaGuide : MonoBehaviour
{
    [SerializeField] private ChatBubble _chatBubble; // Get the assigned chat that should be started
    private bool _startOnce;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        TemperatureChatGuide();
    }

    void TemperatureChatGuide()
    {
        if (GuidelineManager.instance._showRunning && !_startOnce)
        {
            _startOnce = true;
            _chatBubble.StartChatBubble(); // Communicate to the gameobject to start the chat
            gameObject.SetActive(false); // Deactivate this script since it served its purpose. Might be changed to destroy later
        }

    }
}
