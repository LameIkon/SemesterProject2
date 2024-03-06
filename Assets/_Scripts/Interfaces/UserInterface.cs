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

public abstract class UserInterface: MonoBehaviour
{
    
    public InventoryObject Inventory;
  

    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();


    void Start()
    {
       
        for (int i = 0; i < Inventory.Container.Items.Length; i++)
        {
            Inventory.Container.Items[i].parent = this;
        }

        CreateSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });

    }


    void Update()
    {
        slotsOnInterface.UpdateSlotDisplay();
    }


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
        MouseData.slotHoveredOver = obj;
     
    }

    public void OnExit(GameObject obj)
    {
        MouseData.slotHoveredOver = null;

    }

    public void OnEnterInterface(GameObject obj)
    {
       MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface>();

    }

    public void OnExitInterface(GameObject obj)
    {

        MouseData.interfaceMouseIsOver = null;
    }

    public void OnDragStart(GameObject obj)
    {       
        MouseData.tempItemBeingDragged = CreateTempItem(obj);        
    }

    public GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if (slotsOnInterface[obj].Item.ID >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            tempItem.transform.SetParent(transform.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].ItemObject.ItemDisplayed;
            img.raycastTarget = false;
        } 
        return tempItem;
    }

    public void OnDragEnd(GameObject obj)
    {

        Destroy(MouseData.tempItemBeingDragged);
        if (MouseData.interfaceMouseIsOver == null)
        {
            slotsOnInterface[obj].RemoveItem();
            return;
        }
        if(MouseData.slotHoveredOver)
        {
            InventorySlot mouseHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoveredOver];
            Inventory.SwapItems(slotsOnInterface[obj], mouseHoverSlotData);
        }

       
    }

    public void OnDrag(GameObject obj)
    {
        if (MouseData.tempItemBeingDragged != null)
        {
            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }   
}



public static class MouseData
{
    public static UserInterface interfaceMouseIsOver;
    public static GameObject tempItemBeingDragged;  
    public static GameObject slotHoveredOver;
}



public static class ExtensionMethods
{
    public static void UpdateSlotDisplay(this Dictionary<GameObject, InventorySlot> slotsOnInterface)
    {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in slotsOnInterface)
        {
            if (slot.Value.Item.ID >= 0)
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = slot.Value.ItemObject.ItemDisplayed;
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                slot.Key.GetComponentInChildren<TextMeshProUGUI>().text = slot.Value.Amount == 1 ? "" : slot.Value.Amount.ToString("n0");
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
