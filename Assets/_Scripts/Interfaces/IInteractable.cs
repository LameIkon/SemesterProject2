using UnityEngine;

public interface IInteractable
{

    UnityEngine.GameObject _Player { get; set; }
    bool _CanInteract { get; set; }
    void Interact();
}
