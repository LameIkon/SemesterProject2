using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireManager : MonoBehaviour
{
    [SerializeField] public UnityEngine.GameObject _bonfireCanvas; //we cant have this static or we cant see it in inspector and drag canvas into slot
    public static UnityEngine.GameObject _bonfireCanvasSTATIC; //we need a static variable so that we can acces it in Bonfire.Script
    [SerializeField] private Highlight _highlightScript;

    //[SerializeField] private bool _useBonfire;
    [SerializeField] private bool _turn = false;


    void Start()
    {
        if (_bonfireCanvas != null)
        {
            _bonfireCanvasSTATIC = _bonfireCanvas;  //set the static variable to be same as our non static variable
          _bonfireCanvas.SetActive(false);
        }
    }


    void Update()
    {
       // _useBonfire = Bonfire._canOpenBonfire;
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
        if (Bonfire._canOpenBonfire)
        {
         
            OpenBonfire();
        }
    }

    private void OpenBonfire()
    {
        _turn = !_turn;
        _highlightScript.TriggerUse(_turn);
        _bonfireCanvas.SetActive(_turn);
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
