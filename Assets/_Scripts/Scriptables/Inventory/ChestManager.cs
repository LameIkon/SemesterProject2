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
    private bool _neverOpenedBefore = true;
    private bool _chestIsfilled = false;

    private void Start()
    {
        _chestCanvas.SetActive(false);
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

    private void Update()
    {
        if(!_neverOpenedBefore && !_chestIsfilled)
        {
            _chestIsfilled = true;
            FillUpChest();  
        }
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
            _neverOpenedBefore = false;
           
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
    private void FillUpChest()
    {
        if (_chestFiller != null)
        {
            _chestInventory.InitializeInventory(_chestFiller.ReturnSlots());
        }
    }

    private void OpenChest()
    {
        _chestCanvas.SetActive(true);
    }

}
