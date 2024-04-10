using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{

    [SerializeField] private GameObject _journalCanvas; //need to drag the journal canvas in here
    public static GameObject _JournalCanvasSTATIC;

    private bool _turn = false;


    void Start()
    {
        if(_journalCanvas != null)
        {
            _JournalCanvasSTATIC = _journalCanvas;
            _journalCanvas.SetActive(false);
        }        
    }
}
