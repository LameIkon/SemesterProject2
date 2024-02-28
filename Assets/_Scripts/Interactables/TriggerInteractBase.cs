using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
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
        _inputs.OnInteractEvent += HandleInteract;
    }

    private void OnDisable()
    { 
        _inputs.OnInteractEvent -= HandleInteract;
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



    private void OnMouseDown() 
    {
        Interact();
    }


    private void Reset() 
    {
        CircleCollider2D circle = GetComponent<CircleCollider2D>();
        circle.isTrigger = true;
        circle.radius = 5f;
    }

}
