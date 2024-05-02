using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class OpenFurnace : MonoBehaviour
{

    public static bool _canOpenFurnace = false;

    [SerializeField] private Highlight _highlightScript;
    [SerializeField] private FurnaceManager _furnaceManager;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    _canOpenFurnace = true;
    //}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!_highlightScript.TriggerEnter(gameObject))
            {
                return;
            }
            _canOpenFurnace = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _furnaceManager._Turn = false;
            _canOpenFurnace = false;
            _highlightScript.TriggerExit(gameObject);
            FurnaceManager._FurnaceCanvasSTATIC.SetActive(false);
            GameManager._inventoryMenuSTATIC.SetActive(false);
        }
    }
}
