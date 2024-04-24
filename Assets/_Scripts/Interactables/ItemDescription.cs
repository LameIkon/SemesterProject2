using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField, Tooltip("Set images you want to ignore")] private Sprite[] _ignoreImages; // Ignore if its a image you dont want shown

    private Image _slot; // Slot image to check
    static bool _hovering; // Used to ensure hovingering over image
    private bool _showDescription;
    [SerializeField] private GameObject _itemDescriptionCanvas; // The description Canvas

    private void Update()
    {
        // Invenotory slots only gets instantiated when it gets open. thats why it might not find it first time
        if (_itemDescriptionCanvas == null) // if for some reason it cant find the canvas try activate the canvas
        {
            //StartCoroutine(ItemDescriptionHandler.instance.Disable()); // make the canvas true for a moment
            _itemDescriptionCanvas = ItemDescriptionHandler.instance._handler;
            //_itemDescriptionCanvas = GameObject.FindWithTag("ItemDescriptionHolder"); // Find the Canvas
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData) // When mouse is hovering over it
    {       
        _slot = GetComponent<Image>();
        
        _showDescription = true;

        foreach (Sprite IgnoreImage in _ignoreImages)
        {
            // drumstick is equl to null
            if (_slot.sprite == IgnoreImage) // If the image is not ignored then show description
            {
                _showDescription = false;

                break; // no need to continue looking when at least 1 is true
            }
        }
        if (_showDescription)
        {           
            ShowDescription();
            _hovering = true;
            CancelInvoke("HideDescription"); // Stop the script from trying to close it when we want it to be open
        }

        
    }

    public void OnPointerExit(PointerEventData eventData) // When mouse exit hovering over it
    {
        if (_hovering)
        {
            _hovering = false;
            Invoke("HideDescription", 0.3f);
        }
    }

    private void ShowDescription()
    {
        ItemDescriptionHandler.instance.ItemType(_slot); // Something happens here

        _itemDescriptionCanvas.GetComponentInChildren<TextMeshProUGUI>().text = ItemDescriptionHandler.instance._CurrentDescription; // Insert the description from the GameObject

        Image replaceImage = _itemDescriptionCanvas.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Image>(); // Get the specific image from the ItemDescriptionCanvas
        replaceImage.sprite = _slot.sprite;

        _itemDescriptionCanvas.SetActive(true);
    }

    private void HideDescription()
    {
        if (!_hovering)
        {
            _itemDescriptionCanvas.SetActive(false);
        }
    }
}
