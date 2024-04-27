using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRoom : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            GuidelineManager.instance._isOngoingEvent = true;
            this.gameObject.SetActive(false);
        }
    }
}
