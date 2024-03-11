using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[RequireComponent(typeof(BoxCollider2D))]
public class TriggerInteractBase : MonoBehaviour, IInteractable
{
    public GameObject _Player { get; set; }
    public bool _CanInteract { get; set; }

    private void OnEnable()
    {
        InputReader.OnInteractEvent += HandleInteract;
    }

    private void OnDisable()
    {
        InputReader.OnInteractEvent -= HandleInteract;
    }

    public virtual void Interact()
    {
        Debug.Log("Interacted");
    }
    
    private void HandleInteract() // Hvornår bliver den kaldt??  &&   Eventuelt bruge update?? && hvordan referer man til Interact knappen og hvornår den bliver trykket på
    {
        if (_CanInteract) 
        {
            Interact();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enter");
        if (collision.gameObject == _Player) 
        {
            _CanInteract = true;
        } 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exit");
        if (collision.gameObject == _Player) 
        {
            _CanInteract = false;
        }
    }
    
    private void Reset() 
    {
        BoxCollider2D trigger = GetComponent<BoxCollider2D>();
        trigger.isTrigger = true;
        trigger.size = new Vector2(3,3);
    }

}
