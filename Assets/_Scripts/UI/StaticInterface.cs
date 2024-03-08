using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StaticInterface : UserInterface
{

    public GameObject[] _Slots;



    public override void CreateSlots()
    {
        _SlotsOnInterface = new Dictionary<GameObject, InventorySlot> ();

        for (int i = 0; i < _Inventory.GetSlots.Length; i++)
        {
            var obj = _Slots[i];

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
            _Inventory.GetSlots[i]._SlotDisplay = obj;
            _SlotsOnInterface.Add(obj, _Inventory.GetSlots[i]);

            
            
        }
    }

    //public void InteractWithItem()
    //{
    //    for (int i = 0; i < _Inventory.GetSlots.Length; i++)
    //    {
    //        int idSlot = _Inventory.GetSlots[i]._Item._ID;

    //        for (int j = 0; j < _Inventory._ItemsDataBase._ItemObjects.Length; j++)
    //        {
    //            int idHotbar = _Inventory._ItemsDataBase._ItemObjects[i]._Data._ID;

    //            if(idSlot == idHotbar)
    //            {
    //                _Inventory._ItemsDataBase._ItemObjects[j].Action();
    //                break;
    //            }
    //        }          
    //    }
    //}


    public void OnApplicationQuit()
    {

        for (int i = 0; i < _Inventory.GetSlots.Length; i++)
        {
            _Inventory.GetSlots[i].RemoveItem();
        }        
    }


}
