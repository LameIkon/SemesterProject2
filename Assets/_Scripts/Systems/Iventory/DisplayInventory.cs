using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using TMPro;
using UnityEngine.UI;

public class DisplayInventory : MonoBehaviour
{
    public GameObject InventoryPrefab;
    public InventoryObject Inventory;

    public int X_START;
    public int Y_START;

    public int X_SPACE_BETWEEN_ITEM;
    public int NUMBER_OF_COLUMN;
    public int Y_SPACE_BETWEEN_ITEM;
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();


    void Start()
    {
        CreateDisplay();
    }

  
    void Update()
    {
       UpdateDisplay();
    }


    public void UpdateDisplay()
    {
        for (int i = 0; i < Inventory.Container.Items.Count; i++)
        {

            InventorySlot slot = Inventory.Container.Items[i];

            if (itemsDisplayed.ContainsKey(slot))
            {
                itemsDisplayed[slot].GetComponentInChildren<TextMeshProUGUI>().text = slot.Amount.ToString("n0");
            }

            else 
            {
               CreateObject(i);
            }
        }
    }

    public void CreateDisplay()
    {
        

        for (int i = 0; i < Inventory.Container.Items.Count; i++)
        {
            CreateObject(i);
        }
    }

    public Vector3 GetPosition (int i)
    {
        
        return new Vector3(X_START + (X_SPACE_BETWEEN_ITEM *(i % NUMBER_OF_COLUMN)), Y_START + (-Y_SPACE_BETWEEN_ITEM * (i/NUMBER_OF_COLUMN)), 0f);
        
    }


    public void CreateObject (int i)
    {
        InventorySlot slot = Inventory.Container.Items[i];

        var obj = Instantiate(InventoryPrefab, Vector3.zero, Quaternion.identity, transform);
        obj.transform.GetChild(0).GetComponentInChildren<Image>().sprite = Inventory._dataBase.GetItem[slot.Item.ID].ItemDisplay;
        obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
        obj.GetComponentInChildren<TextMeshProUGUI>().text = slot.Amount.ToString();

        itemsDisplayed.Add(slot, obj);

    }
}
