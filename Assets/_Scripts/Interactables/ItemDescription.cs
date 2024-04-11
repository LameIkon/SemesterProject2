using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class ItemDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _slot;
    [SerializeField] private GameObject _itemDescription;
    [SerializeField, TextArea(2, 4)] private string _description;

    [SerializeField] private GameObject _itemDescriptionPrefab;

    [SerializeField] private GameObject _canvas;

    private bool _hovering;


    public void OnPointerEnter(PointerEventData eventData) // When mouse is hovering over it
    {
        _hovering = true;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;

        GameObject description = Instantiate(_itemDescriptionPrefab);

        description.transform.position = mousePos;
        description.transform.SetParent(_canvas.transform, false);

    }

    public void OnPointerExit(PointerEventData eventData) // When mouse exit hovering over it
    {

        Invoke("HideDescription", 1f);
    }

    private void HideDescription()
    {
        //_descriptionbox.SetActive(false);
    }

}
