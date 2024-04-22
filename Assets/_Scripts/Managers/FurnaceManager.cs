using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceManager : MonoBehaviour
{
    [SerializeField] private UnityEngine.GameObject _furnaceCanvas; //we cant have this static or we cant see it in inspector and drag canvas into slot
    public static UnityEngine.GameObject _FurnaceCanvasSTATIC; //we need a static variable so that we can acces it in Bonfire.Script

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
        InputReader.OnPickEvent += HandleInteract;
    }

    private void OnDisable()
    {
        InputReader.OnInteractEvent -= HandleInteract;
        InputReader.OnPickEvent -= HandleInteract;
    }


    void HandleInteract()
    {
        if (Furnace._canOpenFurnace)
        {

            OpenFurnace();
        }
    }

    private void OpenFurnace()
    {
        _turn = !_turn;
        _furnaceCanvas.SetActive(_turn);
        GameManager._inventoryMenuSTATIC.SetActive(_turn);

        // check interactability 
        Interactable();
    }

    void Interactable()
    {
        // Show or disable E highlight
        GameManager._hideEInteractables = _turn;
    }
}
