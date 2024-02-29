using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class TriggerInteractBase : MonoBehaviour, IInteractable
{

    [SerializeField] private GameObject _player;

    public bool _canInteract;

    [SerializeField] private InputReader _inputs;

    public virtual void Interact()
    {
        Debug.Log("Interacted");
    }


    private void OnEnable() 
    {
        _inputs.OnLeftClickEvent += HandleInteract;
    }

    private void OnDisable()
    { 
        _inputs.OnLeftClickEvent -= HandleInteract;
    }


    private void HandleInteract() 
    {
        if (_canInteract) 
        {
            Interact();
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enter");
        if (collision.gameObject == _player) 
        {
            _canInteract = true;
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exit");
        if (collision.gameObject == _player) 
        {
            _canInteract = false;
        }
    }




    private void Reset() 
    {
        BoxCollider2D trigger = GetComponent<BoxCollider2D>();
        trigger.isTrigger = true;
        trigger.size = new Vector2(3,3);
    }

}
