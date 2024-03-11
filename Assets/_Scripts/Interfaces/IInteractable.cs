using UnityEngine;

public interface IInteractable
{
    
    GameObject _Player { get; set; }
    bool _CanInteract { get; set; }
    void Interact();
}
