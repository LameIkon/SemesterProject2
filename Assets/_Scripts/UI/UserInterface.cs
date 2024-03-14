using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using TMPro;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Image = UnityEngine.UI.Image;
using System;

public abstract class UserInterface: MonoBehaviour
{
    
    public InventoryObject _Inventory;


    public Dictionary<GameObject, InventorySlot> _SlotsOnInterface = new Dictionary<GameObject, InventorySlot>();


    //void OnEnable() 
    //{
    //    InputReader.OnInventoryOpenEvent += HandleInventoryOpen;
    //}

    //void OnDisable() 
    //{
    //    InputReader.OnInventoryOpenEvent -= HandleInventoryOpen;
    //}

    //void HandleInventoryOpen() 
    //{

    //}

    void Start()
    {
       
        for (int i = 0; i < _Inventory.GetSlots.Length; i++)
        {
            _Inventory.GetSlots[i]._Parent = this;
            _Inventory.GetSlots[i]._OnAfterUpdate += OnSlotUpdate;
        }

        CreateSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });

    }

    private void OnSlotUpdate(InventorySlot slot)
    {
        if (slot._Item._ID >= 0)
        {
            slot._SlotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = slot.ItemObject._ItemDisplayed;
            slot._SlotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            slot._SlotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = slot._Amount == 1 ? "" : slot._Amount.ToString("n0");
        }

        else
        {
            slot._SlotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
            slot._SlotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
            slot._SlotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }

    //void Update()
    //{
    //    slotsOnInterface.UpdateSlotDisplay();
    //}


    public abstract void CreateSlots();     


    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        MouseData._SlotHoveredOver = obj;
     
    }

    public void OnExit(GameObject obj)
    {
        MouseData._SlotHoveredOver = null;

    }

    public void OnEnterInterface(GameObject obj)
    {
       MouseData._InterfaceMouseIsOver = obj.GetComponent<UserInterface>();

    }

    public void OnExitInterface(GameObject obj)
    {

        MouseData._InterfaceMouseIsOver = null;
    }

    public void OnDragStart(GameObject obj)
    {       
        MouseData._TempItemBeingDragged = CreateTempItem(obj);        
    }

    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if (_SlotsOnInterface[obj]._Item._ID >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            tempItem.transform.SetParent(transform.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = _SlotsOnInterface[obj].ItemObject._ItemDisplayed;
            img.raycastTarget = false;
        } 
        return tempItem;
    }

    public void OnDragEnd(GameObject obj)
    {

        Destroy(MouseData._TempItemBeingDragged);
        if (MouseData._InterfaceMouseIsOver == null)
        {
            //_SlotsOnInterface[obj].RemoveItem(); //Destroys the object if dragged out of interface
            return;
        }
        if(MouseData._SlotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData._InterfaceMouseIsOver._SlotsOnInterface[MouseData._SlotHoveredOver];
            _Inventory.SwapItems(_SlotsOnInterface[obj], mouseHoverSlotData);
        }       
    }

    public void OnDrag(GameObject obj)
    {
        if (MouseData._TempItemBeingDragged != null)
        {
            MouseData._TempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }   
}



public static class MouseData
{
    public static UserInterface _InterfaceMouseIsOver;
    public static GameObject _TempItemBeingDragged;  
    public static GameObject _SlotHoveredOver;
}



public static class ExtensionMethods
{
    public static void UpdateSlotDisplay(this Dictionary<GameObject, InventorySlot> slotsOnInterface)
    {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in slotsOnInterface)
        {
            if (slot.Value._Item._ID >= 0)
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = slot.Value.ItemObject._ItemDisplayed;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value._Amount == 1 ? "" : slot.Value._Amount.ToString("n0");
            }

            else
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }
    }
}
