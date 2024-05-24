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

    [SerializeField] private Highlight _highlightScript;

    private bool _canOpenChest = false;
    private bool _turn = false;
    private bool _chestIsfilled = false;

    [Header("Story Setter"), SerializeField]
    private StoryStates stateToChange = StoryStates.none; // this is what will change the dialogue story state.

    private void Start()
    {
        _chestInterface = _journalChestCanvas.GetComponent<StaticInterface>();
        _chestInterface._Inventory = _chestInventory;
        _journalChestCanvas.SetActive(false);

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
        _highlightScript.TriggerUse(_turn);
        DialogueManager.instance._DialogueVariables.ChangeMainStoryState(stateToChange);

        _journalChestCanvas.SetActive(_turn);
        GameManager._inventoryMenuSTATIC.SetActive(_turn);
        // check interactability 
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (!_highlightScript.TriggerEnter(gameObject)) 
            {
                return;
            }
            _canOpenChest = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            _journalChestCanvas.SetActive(false);
            _highlightScript.TriggerExit(gameObject);
            GameManager._inventoryMenuSTATIC.SetActive(false);
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
