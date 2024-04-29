using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToolbarManager : MonoBehaviour
{
    [SerializeField] InventoryObject _toolbarInventory;
    [SerializeField] UnityEngine.GameObject [] _inventorySlotPrefabs;
    private int _selectedSlot = -1;
    [SerializeField] private Color _selectedColor, _notSelectedColor;
    private UnityEngine.GameObject _lantern;
    private bool _lightIsActive = false;
    private bool _journalIsActive = false;
    private int _saveLightItemIndex;

    void OnEnable() 
    {
        InputReader.OnButtonPressEvent += HandleButtonPress;
        InputReader.OnEatEvent += HandleEating;
        InputReader.OnRightClickEvent += HandleRightClick;
    }

    private void OnDisable()
    {
        InputReader.OnButtonPressEvent -= HandleButtonPress;
        InputReader.OnEatEvent -= HandleEating;
        InputReader.OnRightClickEvent -= HandleRightClick;
    }

    void HandleRightClick() 
    {
        UseLightInSlot();
    }

    void HandleEating() 
    {
        UseFoodInSlot();
        UseJournalInSlot();
    }

    void HandleButtonPress(int i) 
    {
        _selectedSlot = i;
        OnSelectSlot(i);
        DisableLight(i);
    }

    public void OnSelectSlot(int i)
    {
        for (int j = 0;  j < _inventorySlotPrefabs.Length; j++)
        {
            _inventorySlotPrefabs[j].GetComponent<Image>().color = _notSelectedColor;
            
            if(j == i)
            {
                _inventorySlotPrefabs[j].GetComponent<Image>().color = _selectedColor;
            }

        }
    }



    public void UseFoodInSlot()
    {
        if (_selectedSlot < 0)
        {
            return; 
        }

        var itemInSlot = _toolbarInventory.GetSlots[_selectedSlot];
      
        if (itemInSlot.ItemObject != null && itemInSlot.ItemObject._ItemType == ItemType.Food) //checks that there is an item object in the slot
        {
            itemInSlot.ItemObject.Action(); //calls the action function on that object
       
            if(itemInSlot.ItemObject._Consumable) //checks if the item is stackable, otherwise no need to change the amount
            {
                itemInSlot.AddAmount(-1); //substract 1 from the amount
                if (itemInSlot._Amount <= 0) //Checks if the amount of the item is 0
                {
                    itemInSlot.RemoveItem(); //removes item, so that we can't use it infinitely
                }
            }           
        }
    }


    public void UseJournalInSlot ()
    {
        if (_selectedSlot < 0)
        {
            return;
        }

        var itemInSlot = _toolbarInventory.GetSlots[_selectedSlot];

        if (itemInSlot.ItemObject != null && itemInSlot.ItemObject._ItemType == ItemType.Journal && !_journalIsActive)
        {
            itemInSlot.ItemObject.Action();
            _journalIsActive = true;
            Debug.Log("Toolbar Action");
        }

        else if (_journalIsActive)
        {
            itemInSlot.ItemObject.DisableAction();
            _journalIsActive = false;
            Debug.Log("Toolbar Disable");
        }
    }


    public void UseLightInSlot()
    {
       
        if (_selectedSlot < 0)
        {
            return;
        }

        var itemInSlot = _toolbarInventory.GetSlots[_selectedSlot];

        if (itemInSlot.ItemObject != null && itemInSlot.ItemObject._ItemType == ItemType.Light && !_lightIsActive)
        {            
            itemInSlot.ItemObject.Action();
            _saveLightItemIndex = _selectedSlot;
            _lightIsActive = true;
            _lantern = UnityEngine.GameObject.FindGameObjectWithTag("Lantern");
        }

        else if (_lightIsActive)
        {
            itemInSlot.ItemObject.DisableAction();
            _lightIsActive = false;
        }

    }

    public void DisableLight(int i)
    {
        if (_selectedSlot < 0 ||  _lantern == null)
        {
            return;
        }

        if (i != _saveLightItemIndex)
        {
            _lightIsActive = false;
            _lantern.SetActive(false);
        }      
    }

    private void Update()
    {        
        if (_lightIsActive)
        {
            if (_toolbarInventory.GetSlots[_selectedSlot].ItemObject == null || _toolbarInventory.GetSlots[_selectedSlot].ItemObject._ItemType != ItemType.Light)
            {
                _lightIsActive = false;
                _lantern.SetActive(false);
            }
        }

        if (_journalIsActive)
        {
            if (_toolbarInventory.GetSlots[_selectedSlot].ItemObject == null || _toolbarInventory.GetSlots[_selectedSlot].ItemObject._ItemType != ItemType.Journal)
            {
                _journalIsActive = false;
                ItemManager._JournalCanvasSTATIC.SetActive(false);
            }
        }
    }
}
