using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{

    [SerializeField] private GameObject _journalCanvas; //need to drag the journal canvas in here
    public static GameObject _JournalCanvasSTATIC;

    [SerializeField] private GameObject _journal1;
    public static GameObject _Journal1STATIC;

    [SerializeField] private GameObject _journal2;
    public static GameObject _Journal2STATIC;

    [SerializeField] private GameObject _journal3;
    public static GameObject _Journal3STATIC;


    private bool _turn = false;


    void Start()
    {
       
        
            _JournalCanvasSTATIC = _journalCanvas;
            

            _Journal1STATIC = _journal1;
            _Journal2STATIC = _journal2;
            _Journal3STATIC = _journal3;

            _journal1.SetActive(false);
            _journal2.SetActive(false);
            _journal3.SetActive(false);

            _JournalCanvasSTATIC.SetActive(false);


            
    }
}
