using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ToolbarManager : MonoBehaviour
{
    [SerializeField] InventoryObject _toolbarInventory;
    private int _selectedSlot = -1;



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


    void HandleInteract() 
    {
        SelectSlot();
    }

    void HandleButtonPress(int i) 
    {
        _selectedSlot = i;
    }

    // Update is called once per frame
    void Update()
    {
       // SelectSlot();       


        //if (Input.GetKeyUp(KeyCode.Alpha1))
        //{
        //    var item = _toolbarInventory.GetSlots[0]; //creates a var that holds the ItemObject at that index slot
        //    if (item.ItemObject != null) //checks that there is an item object in the slot
        //    {
        //        item.ItemObject.Action(); //calls the action function on that object
        //        item.AddAmount(-1); //substract 1 from the amount
        //        if (item._Amount <= 0) //Checks if the amount of the item is 0
        //        {
        //            item.RemoveItem(); //removes item, so that we can use it infinitely
        //        }
        //    }         
        //}              
    }

    public void SelectSlot()
    {
        var itemInSlot = _toolbarInventory.GetSlots[_selectedSlot];
        if (_selectedSlot < 0)
        {
            return; 
        }
        if (itemInSlot.ItemObject != null) //checks that there is an item object in the slot
        {
            itemInSlot.ItemObject.Action(); //calls the action function on that object
            itemInSlot.AddAmount(-1); //substract 1 from the amount
            if (itemInSlot._Amount <= 0) //Checks if the amount of the item is 0
            {
                itemInSlot.RemoveItem(); //removes item, so that we can't use it infinitely
            }
        }
    }

   

    public void TriggerSlot (int slot)
    {
        
        //var itemInSlot = _toolbarInventory.GetSlots[slot];
        
        if (_toolbarInventory.GetSlots[slot].ItemObject != null) //checks that there is an item object in the slot
        {
            _toolbarInventory.GetSlots[slot].ItemObject.Action(); //calls the action function on that object
            _toolbarInventory.GetSlots[slot].AddAmount(-1); //substract 1 from the amount
            if (_toolbarInventory.GetSlots[slot]._Amount <= 0) //Checks if the amount of the item is 0
            {
                _toolbarInventory.GetSlots[slot].RemoveItem(); //removes item, so that we can't use it infinitely
            }
        }
    }

    //public void ChangeSelectedSlot(int slot)
    //{
    //   if (_selectedSlot > -1)
    //    {
    //        _selectedSlot = slot;
    //        Debug.Log("SelectedSlot value changed to " + _selectedSlot);
    //    }
    //}

}
