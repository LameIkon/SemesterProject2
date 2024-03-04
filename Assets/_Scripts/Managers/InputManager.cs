using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public InputReader _inputs;

    private void Awake() 
    {
        if (_inputs == null) 
        {
            _inputs = new InputReader();
        }        
    }
}
