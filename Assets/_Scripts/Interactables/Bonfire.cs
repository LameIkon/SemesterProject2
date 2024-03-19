using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bonfire : MonoBehaviour
{
    [SerializeField] private FloatVariable _systemFloat;
    [SerializeField] private float _restoreValue;
   

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {                
            _systemFloat.ApplyChange(_restoreValue);
        }       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {    
            _systemFloat.ApplyChange(-_restoreValue);
        }
    }
}
