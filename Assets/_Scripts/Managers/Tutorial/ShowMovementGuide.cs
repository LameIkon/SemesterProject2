using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMovementGuide : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GuidelineManager.instance.ShowMovement();
        this.gameObject.SetActive(false);
    }
}
