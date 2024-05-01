using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : PriorityManager
{
    [SerializeField] private GameObject _showInteraction; // Used to get the GameObject named showInteraction
    private PriorityManager _priorityManager;
    //public static bool _HasBeenEntered;


    private void Start()
    {
        // Fist child is canvas and the next is the child of the canvas
        //_showInteraction = transform.GetChild(1).GetChild(0).gameObject; // Get the child of child attached to the parent.
        _showInteraction.SetActive(false); // Set false if by chance its active
        Debug.Log("loaded");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.gameObject.name == "Player" && !GameManager._hideEInteractables && PriorityManager._PriorityInteractable) // Detect if the collision is the gameobject called Player
        //{
        //    PriorityManager._PriorityInteractable = false;
        //    _showInteraction.SetActive(true);
            
        //}
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.name == "Player" && GameManager._hideEInteractables && PriorityManager._PriorityInteractable)
    //    {
    //        PriorityManager._PriorityInteractable = false;
    //        _showInteraction.SetActive(false);
    //    }
    //    else if (collision.gameObject.name == "Player" && !GameManager._hideEInteractables)
    //    {
    //        _showInteraction.SetActive(true);
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.name == "Player") // Detect if the collision is the gameobject called Player
    //    {
    //        _showInteraction.SetActive(false);
    //        GameManager._hideEInteractables = false;

    //        if (!PriorityManager._PriorityInteractable)
    //        {
    //            PriorityManager._PriorityInteractable = true;
    //        }
    //    }
    //}
   

    public override void TriggerEnter()
    {
        AdditionalTriggerEnterImplementation();
        base.TriggerEnter();
        Debug.Log("entered");
    }

    public override void TriggerExit()
    {
        AdditionalTriggerExitImplementation();
        base.TriggerExit();
        Debug.Log("exited");
    }

    public override void TriggerUse(bool state)
    {
        if (state)
        {
            _showInteraction.SetActive(false);
            Debug.Log("Use");
        }
        else if (!state)
        {
            _showInteraction.SetActive(true);
            Debug.Log("closed");
        }
    }

    // changed
    protected override void AdditionalTriggerEnterImplementation()
    {
        _showInteraction.SetActive(true);
    }

    protected override void AdditionalTriggerExitImplementation()
    {
        _showInteraction.SetActive(false);
        _PriorityInteractable = true;
        GameManager._hideEInteractables = true;
    }
}
