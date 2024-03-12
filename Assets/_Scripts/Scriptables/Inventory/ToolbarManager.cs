using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToolbarManager : MonoBehaviour
{
    [SerializeField] InventoryObject _toolbarInventory;
    [SerializeField] GameObject [] _inventorySlotPrefabs;
    private int _selectedSlot = -1;
    [SerializeField] private Color _selectedColor, _notSelectedColor;
    [SerializeField] private GameObject _lantern;
    public bool _LightIsActive = false;

    public int saveLightItemIndex;

    void OnEnable() 
    {
        InputReader.OnButtonPressEvent += HandleButtonPress;
        InputReader.OnInteractEvent += HandleInteract;
        InputReader.OnPickEvent += HandleInteract;
    }

    private void OnDisable()
    {
        InputReader.OnButtonPressEvent -= HandleButtonPress;
        InputReader.OnInteractEvent -= HandleInteract;
        InputReader.OnPickEvent -= HandleInteract;
    }

    //private void Update()
    //{
    //    FindLightObject();
    //}

    void HandleInteract() 
    {
        UseFoodInSlot();
        UseLightInSlot();
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

            //if(j != i)
            //{
            //    Debug.Log("Light should disable");
            //     _toolbarInventory.GetSlots[saveLightItemIndex].ItemObject.DisableAction();
            //}
        }
    }

    //public void FindLightObject()
    //{
    //    for (int j = 0; j < _toolbarInventory.GetSlots.Length; j++)
    //    {
    //        if (_toolbarInventory.GetSlots[j].ItemObject._ItemType == ItemType.Light)
    //        {
    //            saveLightItemIndex = j;
    //        }
    //    }
    //}


    public void UseFoodInSlot()
    {
        var itemInSlot = _toolbarInventory.GetSlots[_selectedSlot];
        if (_selectedSlot < 0)
        {
            return; 
        }
      
        if (itemInSlot.ItemObject != null && itemInSlot.ItemObject._ItemType == ItemType.Food) //checks that there is an item object in the slot
        {
            itemInSlot.ItemObject.Action(); //calls the action function on that object
       
            if(itemInSlot.ItemObject._Stackable) //checks if the item is stackable, otherwise no need to change the amount
            {
                itemInSlot.AddAmount(-1); //substract 1 from the amount
                if (itemInSlot._Amount <= 0) //Checks if the amount of the item is 0
                {
                    itemInSlot.RemoveItem(); //removes item, so that we can't use it infinitely
                }
            }           
        }
    }

    public void UseLightInSlot()
    {
        var itemInSlot = _toolbarInventory.GetSlots[_selectedSlot];
       

        if (_selectedSlot < 0)
        {
            return;
        }


        if (itemInSlot.ItemObject != null && itemInSlot.ItemObject._ItemType == ItemType.Light && !_LightIsActive)
        {
            itemInSlot.ItemObject.Action();
            saveLightItemIndex = _selectedSlot;
            _LightIsActive = true;
        }

        else if (_LightIsActive)
        {
            itemInSlot.ItemObject.DisableAction();
            _LightIsActive = false;
        }
    }

    

    public void DisableLight(int i)
    {
        if (_selectedSlot < 0)
        {
            return;
        }


        if (i != saveLightItemIndex)
        {          
            //_toolbarInventory.GetSlots[saveLightItemIndex].ItemObject.DisableAction();
            _lantern.SetActive(false);
        }


    }

    //public void DisableLightInSlot()
    //{
    //    var itemInSlot = _toolbarInventory.GetSlots[_selectedSlot];
    //    if (_selectedSlot < 0)
    //    {
    //        return;
    //    }

    //    if (itemInSlot.ItemObject != null && itemInSlot.ItemObject._ItemType == ItemType.Light && _lightActivated == true)
    //    {
    //        itemInSlot.ItemObject.DisableAction();
    //        _lightActivated = false;

    //    }
    //}

}
