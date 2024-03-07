using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ToolbarManager : MonoBehaviour
{
    [SerializeField] InventoryObject _toolbarInventory;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {      
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            var item = _toolbarInventory.GetSlots[0]; //creates a var that holds the ItemObject at that index slot
            if (item.ItemObject != null) //checks that there is an item object in the slot
            {
                item.ItemObject.Action(); //calls the action function on that object
                item.AddAmount(-1); //substract 1 from the amount
                if (item._Amount <= 0) //Checks if the amount of the item is 0
                {
                    item.RemoveItem(); //removes item, so that we can use it infinitely
                }
            }         
        }              
    }
}
