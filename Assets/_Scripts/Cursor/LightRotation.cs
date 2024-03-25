using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class LightRotation : MonoBehaviour
{
    private Transform _lightTransform;
    private Quaternion _rotation;

    private void Start()
    {
        _lightTransform = this.transform; 
    }

    void Update()
    {
        CursorManager.MouseRotation(_lightTransform);
    }

    //transform.up = CursorManager._mousePosition;  // Bruges i forlængelse med metoden ovenover
    //  private void MouseRotation()                // Denne metode centraliserer ikke muspositionen i midten af skærmen hvilket har konsekvenser for rotationen
    // {
    //     float angle = Mathf.Atan2(CursorManager._MousePosition.y, CursorManager._MousePosition.x) * Mathf.Rad2Deg;
    //     _rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    //     _lightTransform.rotation = _rotation;
    // }
}
