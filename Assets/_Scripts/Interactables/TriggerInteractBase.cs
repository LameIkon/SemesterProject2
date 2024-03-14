using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class TriggerInteractBase : MonoBehaviour, IInteractable
{
    public GameObject _Player { get; set; }  // Initialiseres i Start()
    public bool _CanInteract { get; set; }   // Nødvendig i sammenhæng med loading af scener MED Interaction-knappen
    public virtual void Interact() { }  // Overrides i DoorTriggerInteraction.cs
    
    // private void OnEnable() { InputReader.OnInteractEvent += HandleInteract; }   // Nødvendig i sammenhæng med loading af scener MED Interaction-knappen
    // private void OnDisable() { InputReader.OnInteractEvent -= HandleInteract; }  // Nødvendig i sammenhæng med loading af scener MED Interaction-knappen
    private void Start()
    {
        _Player = GameObject.FindWithTag("Player"); // Sørger for at det kun er spilleren der kan interagere med døren
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Enter");
        if (collision.gameObject == _Player) 
        {
            Interact();
            //_CanInteract = true;  // Nødvendig i sammenhæng med loading af scener MED Interaction-knappen
        } 
    }

    // private void OnTriggerExit2D(Collider2D collision)   // Nødvendig i sammenhæng med loading af scener MED Interaction-knappen
    // {
    //     Debug.Log("Exit");
    //     if (collision.gameObject == _Player) 
    //     {
    //         _CanInteract = false;
    //     }
    // }
    
    // private void HandleInteract()    // Er denne metode ikke nødvendig?
    // {
    //     if (_CanInteract) 
    //     {
    //         Interact();
    //     }
    // }
    
    // private void Reset() 
    // {
    //     BoxCollider2D trigger = GetComponent<BoxCollider2D>();
    //     trigger.isTrigger = true;
    //     trigger.size = new Vector2(3,3);
    // }
}
