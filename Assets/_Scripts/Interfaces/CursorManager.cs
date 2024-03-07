using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
  
    private Vector2 _mousePosition;
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
            Debug.Log("Name Of Object: " + hit.collider.name);
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

}

