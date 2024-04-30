using Cinemachine;
using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ChestManager : MonoBehaviour
{
    [SerializeField] private GameObject _chestCanvas;

   
    [SerializeField] private InventoryObject _chestInventory;
    [SerializeField] private ChestFiller _chestFiller;
    [SerializeField] private ItemDatabaseObject _database;
    private StaticInterface _chestInterface;
    
    
    private bool _turn = false;
    private InventorySlot[] _slots = new InventorySlot[6];

    [SerializeField] private bool _canOpenChest = false;
    [SerializeField] private bool _chestIsfilled = false;

    private void Start()
    {

        _chestInterface = _chestCanvas.GetComponent<StaticInterface>();
        _chestInterface._Inventory = _chestInventory;
        _chestCanvas.SetActive(false);


        if (!_chestIsfilled)
        {
            _chestIsfilled = true;

            FillUpChest();

        }
    }

    void OnEnable()
    {
        InputReader.OnInteractEvent += HandleInteract;
        _chestCanvas.SetActive(false);
    }

    private void OnDisable()
    {
        InputReader.OnInteractEvent -= HandleInteract;
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
            _canOpenChest = false;
            CloseChest();
        }
    }
    private void FillUpChest()
    {
        if (_chestFiller != null)
        {
            _chestInventory.InitializeInventory(_chestFiller.ReturnSlots(), _database, new Inventory(_slots));
        }
    }


    private void OpenChest()
    {
        _turn = !_turn;
        _chestCanvas.SetActive(_turn);
        // check interactability 
        Interactable();

        GameManager._inventoryMenuSTATIC.SetActive(_turn);
    }

    public void CloseChest()
    {
        _chestCanvas.SetActive(false);      
        GameManager._inventoryMenuSTATIC.SetActive(false);
        _turn = false;
    }

    void Interactable()
    {
        // Show or disable E highlight
        GameManager._hideEInteractables = _turn;
    }

    private void OnApplicationQuit()
    {        
        _chestInventory._Container.Clear();       
    }



}
