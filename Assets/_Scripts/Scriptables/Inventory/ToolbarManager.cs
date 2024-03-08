using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ToolbarManager : MonoBehaviour
{
    [SerializeField] InventoryObject _toolbarInventory;
    private int _selectedSlot = -1;
    

    // Update is called once per frame
    void Update()
    {      


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
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int pressedNumber);
            if (isNumber && pressedNumber >= 1 && pressedNumber <= 6)
            {
                ChangeSelectedSlot(pressedNumber - 1);
                
            }
        }      
    }

    public void TriggerSlot (int slot)
    {
        var itemInSlot = _toolbarInventory.GetSlots[slot];
        slot = _selectedSlot;
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

    public void ChangeSelectedSlot(int slot)
    {
       if (_selectedSlot >= 0)
        {
            _selectedSlot = slot;
        }
    }

}
