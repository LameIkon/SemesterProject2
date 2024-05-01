using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : PriorityManager
{
    [SerializeField] private GameObject _showInteraction; // Used to get the GameObject named showInteraction
    //public static bool _HasBeenEntered;


    private void Start()
    {
        _showInteraction.SetActive(false); // Set false if by chance its active
        Debug.Log("loaded");
    }


 

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
