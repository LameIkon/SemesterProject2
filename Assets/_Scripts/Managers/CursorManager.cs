using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CursorManager : MonoBehaviour
{

    private static Vector2 _mousePosition;           // Til at "detecte" objekter
    private static Vector2 _mousePositionRotation;   // Til at rotere objekter
    private static Quaternion _rotation;             // Bruges i forlængelse af _mousePositionRotation
    private Camera _mainCamera;

    void Start() 
    {
        _mainCamera = Camera.main;
    }
    
    void OnEnable() 
    {
        InputReader.OnMousePositionEvent += HandleMousePosition;
        InputReader.OnLeftClickEvent += HandleLeftMouseClick;
    }

    void OnDisable()
    {
        InputReader.OnMousePositionEvent -= HandleMousePosition;
        InputReader.OnLeftClickEvent -= HandleLeftMouseClick;
    }

    void DetectObject() 
    {
        Ray ray = _mainCamera.ScreenPointToRay(_mousePosition);
        
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

        if (hit.collider != null) 
        {
            //Debug.Log("Name Of Object: " + hit.collider.name);
        }
    }
    
    private void HandleMousePosition(Vector2 pos) 
    {
        _mousePosition = pos;

    }
    
    private void HandleLeftMouseClick() 
    {
        DetectObject();
    }

    public static void MouseRotation(Transform transform)   // Metoden sørger for at koordinatsystemet (som sporer muspositionen) forbliver i midten af skæremen og ikke i nederste venstre hjørne
    {

        if (_mousePosition == Vector2.zero) // the mouse event will be a zero vector when the inventory is opened, this will return and not recalculate the rotation
        {
            return;
        }
        else 
        {
        
            _mousePositionRotation = Camera.main.ScreenToWorldPoint(new Vector3(_mousePosition.x, _mousePosition.y, 0)) - transform.position;  // Her centraliserer vi muspositionen i midten af skærmen
            float angle = Mathf.Atan2(_mousePositionRotation.y, _mousePositionRotation.x) * Mathf.Rad2Deg; 
            _rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

            transform.rotation = _rotation;

        }

    }

}

