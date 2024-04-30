using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenFurnace : MonoBehaviour
{

    public static bool _canOpenFurnace = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _canOpenFurnace = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        _canOpenFurnace = false;
        GameManager._hideEInteractables = false;
        FurnaceManager._FurnaceCanvasSTATIC.SetActive(false);
        GameManager._inventoryMenuSTATIC.SetActive(false);
    }
}
