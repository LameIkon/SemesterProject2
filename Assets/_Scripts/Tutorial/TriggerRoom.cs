using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRoom : MonoBehaviour
{
    [SerializeField] private bool _triggerStay;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( GuidelineManager.instance._finishedInsideTutorial) // Wont get triggered if tutorial finished
        {
            this.gameObject.SetActive(false);
            return;
        }
        else if (collision.gameObject.name == "Player")
        {
            GuidelineManager.instance._isOngoingEvent = true;
            this.gameObject.SetActive(false);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && _triggerStay)
        {
            GuidelineManager.instance._isOngoingEvent = true;
            this.gameObject.SetActive(false);
        }
    }
}
