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

public class DisplayInventory : MonoBehaviour
{
    public GameObject InventoryPrefab;
    public InventoryObject Inventory;
    public MouseItem MouseItem = new MouseItem();
   
    
    

    public int X_START;
    public int Y_START;

    public int X_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMN;
    public int Y_SPACE_BETWEEN_ITEM;
    Dictionary<GameObject, InventorySlot> itemsDisplayed = new Dictionary<GameObject, InventorySlot>();


    void Start()
    {
        CreateSlots();
        
    }

  
    void Update()
    {
       UpdateSlots();
    }

    public void UpdateSlots()
    {
        foreach (KeyValuePair<GameObject, InventorySlot> slot in itemsDisplayed)
        {
            if(slot.Value.ID >= 0)
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
        

    public void CreateSlots()
    {
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();

        for (int i = 0; i < Inventory.Container.Items.Length; i++)
        {
            var obj = Instantiate(InventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate {OnEnter(obj);} );
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            itemsDisplayed.Add(obj, Inventory.Container.Items[i]);
        }
    }


    private void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnEnter(GameObject obj)
    {
        MouseItem.HoverObj = obj;
        if(itemsDisplayed.ContainsKey(obj))
        {
            MouseItem.HoverItem = itemsDisplayed[obj];
        }
    }

    public void OnExit(GameObject obj)
    {
        MouseItem.HoverObj = null;       
        MouseItem.HoverItem = null;
        
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
        MouseItem.Obj = mouseObject;
        MouseItem.Item = itemsDisplayed[obj];
    }

    public void OnDragEnd(GameObject obj)
    {
        if (MouseItem.HoverObj)
        {
            Inventory.MoveItem(itemsDisplayed[obj], itemsDisplayed[MouseItem.HoverObj]);
        }
        else
        {
            Inventory.RemoveItem(itemsDisplayed[obj].Item);
        }
        Destroy(MouseItem.Obj);
        MouseItem.Item = null;
    }

    public void OnDrag(GameObject obj)
    {
        if(MouseItem.Obj != null)
        {
            MouseItem.Obj.GetComponent<RectTransform>().position = Input.mousePosition;
        }
    }



    public Vector3 GetPosition (int i)
    {
        
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM *(i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEM * (i/NUMBER_OF_COLUMN)), 0f);
        
    }
}



