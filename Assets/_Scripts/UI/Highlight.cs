using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : PriorityManager
{
    [SerializeField] private GameObject _showInteraction; // Used to get the GameObject named showInteraction
    private bool _triggerOnce;
   
    //public static bool _HasBeenEntered;


    private void Start()
    {
        _showInteraction.SetActive(false); // Set false if by chance its active
    }


    public override bool TriggerEnter(GameObject gameobject)
    {
        if (_CompareGameObject == null)
        {
            _CompareGameObject = gameobject;
        }

        if (_CompareGameObject == gameobject && _PriorityInteractable && _canInteractChest)
        {         
            base.TriggerEnter(gameobject);
            AdditionalTriggerEnterImplementation();
            return true;
        }
        return false;
    }

    public override void TriggerExit(GameObject gameobject)
    {
        if (_CompareGameObject == gameobject)
        {
            AdditionalTriggerExitImplementation();
            base.TriggerExit(gameobject);
            _CompareGameObject = null;
        }
    }

    public override void TriggerUse(bool state)
    {
        if (state)
        {
            _showInteraction.SetActive(false);
        }
        else if (!state)
        {
            _showInteraction.SetActive(true);
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
    }
}
