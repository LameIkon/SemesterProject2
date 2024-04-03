using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireManager : MonoBehaviour
{
    [SerializeField] public GameObject _bonfireCanvas; //we cant have this static or we cant see it in inspector and drag canvas into slot
    public static GameObject _bonfireCanvasSTATIC; //we need a static variable so that we can acces it in Bonfire.Script

    //[SerializeField] private bool _useBonfire;
    private bool _turn = false;


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
        InputReader.OnPickEvent += HandleInteract;
    }

    private void OnDisable()
    {
        InputReader.OnInteractEvent -= HandleInteract;
        InputReader.OnPickEvent -= HandleInteract;
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
        _bonfireCanvas.SetActive(_turn);
    }


}
