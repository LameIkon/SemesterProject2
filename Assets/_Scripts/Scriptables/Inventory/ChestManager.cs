using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    [SerializeField] private GameObject _chestCanvas;
    [SerializeField] private InventoryObject _chestInventory;
    [SerializeField] private ChestFiller _chestFiller;

    private bool _canOpenChest = false;


    private void Start()
    {
        _chestCanvas.SetActive(false);
    }


   
    void OnEnable()
    {       
        InputReader.OnInteractEvent += HandleInteract;
        InputReader.OnPickEvent += HandleInteract;

        if (_chestFiller != null)
        {
            _chestInventory.InitializeInventory(_chestFiller.ReturnSlots());
        }
    }

    private void OnDisable()
    {     
        InputReader.OnInteractEvent -= HandleInteract;
        InputReader.OnPickEvent -= HandleInteract;
    }


    void HandleInteract()
    {
        if (_canOpenChest)
        {
            OpenChest();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player") 
        {
            _canOpenChest = true;
           
        }        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            _chestCanvas.SetActive(false);
            _canOpenChest=false;
        }
    }


    private void OpenChest()
    {
        _chestCanvas.SetActive(true);
    }



}
