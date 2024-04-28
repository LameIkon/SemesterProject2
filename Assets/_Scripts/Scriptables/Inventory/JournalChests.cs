using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalChests : MonoBehaviour
{

    [SerializeField] private GameObject _journalChestCanvas;
    [SerializeField] private ChestFiller _chestFiller;
    [SerializeField] private InventoryObject _chestInventory;
    [SerializeField] private ItemDatabaseObject _database;
    private StaticInterface _chestInterface;

    private InventorySlot[] _slots = new InventorySlot[1];


    private bool _canOpenChest = false;
    private bool _turn = false;
    private bool _chestIsfilled = false;


    private void Start()
    {
        _chestInterface = _journalChestCanvas.GetComponent<StaticInterface>();
        _chestInterface._Inventory = _chestInventory;

        if (!_chestIsfilled)
        {
            _chestIsfilled = true;
            FillUpChest();
        }
    }

    void OnEnable()
    {
        InputReader.OnInteractEvent += HandleInteract;
        _journalChestCanvas.SetActive(false);
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

    private void OpenChest()
    {
        _turn = !_turn;
        _journalChestCanvas.SetActive(_turn);
        GameManager._inventoryMenuSTATIC.SetActive(_turn);
        // check interactability 
        Interactable();
    }

    void Interactable()
    {
        // Show or disable E highlight
        GameManager._hideEInteractables = _turn;
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
            _journalChestCanvas.SetActive(false);
            GameManager._inventoryMenuSTATIC.SetActive(_turn);
            _canOpenChest = false;
            _turn = false;
        }
    }

    private void FillUpChest()
    {
        if (_chestFiller != null)
        {
            _chestInventory.InitializeInventory(_chestFiller.ReturnSlots(), _database, new Inventory(_slots));
        }
    }
}
