using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceManager : MonoBehaviour
{
    [SerializeField] private UnityEngine.GameObject _furnaceCanvas; //we cant have this static or we cant see it in inspector and drag canvas into slot
    public static UnityEngine.GameObject _FurnaceCanvasSTATIC; //we need a static variable so that we can acces it in Bonfire.Script
    [SerializeField] private Highlight _highlightScript;

    [SerializeField] private bool _turn = false;


    void Start()
    {
        if (_furnaceCanvas != null)
        {
            _FurnaceCanvasSTATIC = _furnaceCanvas;  //set the static variable to be same as our non static variable
            _furnaceCanvas.SetActive(false);
        }
    }

    void OnEnable()
    {
        InputReader.OnInteractEvent += HandleInteract;
    }

    private void OnDisable()
    {
        InputReader.OnInteractEvent -= HandleInteract;
    }


    void HandleInteract()
    {
        if (OpenFurnace._canOpenFurnace)
        {

            OpenFurnaceMethod();
        }
    }

    private void OpenFurnaceMethod()
    {
        _turn = !_turn;
        _highlightScript.TriggerUse(_turn);
        _furnaceCanvas.SetActive(_turn);
        GameManager._inventoryMenuSTATIC.SetActive(_turn);

        // check interactability 
        Interactable();
    }

    public void CloseFire()
    {
        _FurnaceCanvasSTATIC.SetActive(false);
        if (_furnaceCanvas.activeInHierarchy)
        {
            GameManager._inventoryMenuSTATIC.SetActive(false);
        }
        _turn = false;
    }

    void Interactable()
    {
        // Show or disable E highlight
        GameManager._hideEInteractables = _turn;
    }
}
