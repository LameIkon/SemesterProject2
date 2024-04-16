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

    [SerializeField]private bool _canOpenChest = false;
    private bool _chestIsfilled = false;

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


    private void Update()
    {
        //if (LanternDisabler._LoadedSTATIC) 
        //{
        //   //  _chestCanvas = UnityEngine.GameObject.FindWithTag("Chest");
         
        //}
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
            GameManager._inventoryMenuSTATIC.SetActive(false);
            _canOpenChest=false;
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
        GameManager._inventoryMenuSTATIC.SetActive(_turn);
    }
}
