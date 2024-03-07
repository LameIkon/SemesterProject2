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
            var item = _toolbarInventory.GetSlots[0];
            if (item.ItemObject != null) 
            {
                item.ItemObject.Action();
                item.AddAmount(-1);
                if (item._Amount <= 0)
                {
                    item.RemoveItem();
                }
            }         
        }              
    }
}
