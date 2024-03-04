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
    public CursorManager Cursor;

    public Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();


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
        UpdateSlots();
    }

    public void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in itemsDisplayed)
        {
            if (slot.Value.ID >= 0)
            {
                slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = Inventory._dataBase.GetItem[slot.Value.Item.ID].ItemDisplay;
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
        Cursor.MouseItem.HoverObj = obj;
        if (itemsDisplayed.ContainsKey(obj))
        {
            Cursor.MouseItem.HoverItem = itemsDisplayed[obj];
        }
    }

    public void OnExit(GameObject obj)
    {
        Cursor.MouseItem.HoverObj = null;
        Cursor.MouseItem.HoverItem = null;

    }

    public void OnEnterInterface(GameObject obj)
    {
       Cursor.MouseItem.UI = obj.GetComponent<UserInterface>();

    }

    public void OnExitInterface(GameObject obj)
    {

        Cursor.MouseItem.UI = null;
    }

    public void OnDragStart(GameObject obj)
    {
        var mouseObject = new GameObject();
        var rt = mouseObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(50, 50);
        mouseObject.transform.SetParent(transform.parent);
        if (itemsDisplayed[obj].ID >= 0)
        {
            var img = mouseObject.AddComponent<Image>();
            img.sprite = Inventory._dataBase.GetItem[itemsDisplayed[obj].ID].ItemDisplay;
            img.raycastTarget = false;
        }
        Cursor.MouseItem.Obj = mouseObject;
        Cursor.MouseItem.Item = itemsDisplayed[obj];
    }

    public void OnDragEnd(GameObject obj)
    {
        var itemOnMouse = Cursor.MouseItem;
        var mouseHoverItem = itemOnMouse.HoverItem;
        var mouseHoverObj = itemOnMouse.HoverObj;
        var GetItemObject = Inventory._dataBase.GetItem;

        if (itemOnMouse.UI != null)
        {
            if (mouseHoverObj)
            {
                if (mouseHoverItem.CanPlaceInSlot(GetItemObject[itemsDisplayed[obj].ID]) && (mouseHoverItem.Item.ID <= -1 || (mouseHoverItem.Item.ID >= 0 && itemsDisplayed[obj].CanPlaceInSlot(GetItemObject[mouseHoverItem.Item.ID]))))
                {
                    Inventory.MoveItem(itemsDisplayed[obj], mouseHoverItem.parent.itemsDisplayed[itemOnMouse.HoverObj]);
                }
            }
        }
        else
        {
            // Inventory.RemoveItem(itemsDisplayed[obj].Item);
        }
        Destroy(itemOnMouse.Obj);
        itemOnMouse.Item = null;
    }

    public void OnDrag(GameObject obj)
    {
        if (Cursor.MouseItem.Obj != null)
        {
            Cursor.MouseItem.Obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }   
}



public class MouseItem
{
    public UserInterface UI;
    public GameObject Obj;
    public InventorySlot Item;
    public InventorySlot HoverItem;
    public GameObject HoverObj;
}
