using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ItemDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{


    [Header("Data on this GameObject")]
    [SerializeField] private Image _slot; // Slot image to check
    [SerializeField, TextArea(2, 4)] private string _description; // The items description

    [Header("Description Canvas")]
    [SerializeField] private GameObject _itemDescriptionCanvas; // The description Canvas
    //[SerializeField] private Sprite _previousImage; // Used to compare with the previous
    [SerializeField] private Sprite[] _ignoreImages; // Ignore if its a image you dont want shown

    private bool _hovering;

    private void Awake()
    {
        _hovering = false;
        _itemDescriptionCanvas.SetActive(false);

    }

    public void OnPointerEnter(PointerEventData eventData) // When mouse is hovering over it
    {
        _slot = GetComponent<Image>();

        foreach (Sprite notIgnoredImage in _ignoreImages)
        {
            // drumstick is equl to null
            if (_slot.sprite != notIgnoredImage) // If the image is not ignored then show description
            {
                ShowDescription();
                Debug.Log("true");
            }
        }


    }

    public void OnPointerExit(PointerEventData eventData) // When mouse exit hovering over it
    {
        _hovering = false;
        Invoke("HideDescription", 0.3f);
    }

    private void ShowDescription()
    {
        _itemDescriptionCanvas.SetActive(true);
    }

    private void HideDescription()
    {
        _itemDescriptionCanvas.SetActive(false);
    }

}
