using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInterface : UserInterface
{

    public GameObject _InventoryPrefab;

    public int X_START;
    public int Y_START;
    public int X_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMN;
    public int Y_SPACE_BETWEEN_ITEM;

    public bool _ClearInventory;

    public override void CreateSlots()
    {
        _SlotsOnInterface = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < _Inventory.GetSlots.Length; i++)
        {
            var obj = Instantiate(_InventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });
            _Inventory.GetSlots[i]._SlotDisplay = obj;
            _SlotsOnInterface.Add(obj, _Inventory.GetSlots[i]);
        }
    }

    private Vector3 GetPosition(int i)
    {

        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM * (i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEM * (i / NUMBER_OF_COLUMN)), 0f);

    }

    public void OnApplicationQuit()
    {
        if (!_ClearInventory)
        {
            for (int i = 0; i < _Inventory.GetSlots.Length; i++)
            {
                _Inventory.GetSlots[i].RemoveItem();
            }
        }   
    }
}
