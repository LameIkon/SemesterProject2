using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : PriorityManager
{
    [SerializeField] private GameObject _showInteraction; // Used to get the GameObject named showInteraction
    public static GameObject _gameObject;
    //public static bool _HasBeenEntered;


    private void Start()
    {
        _showInteraction.SetActive(false); // Set false if by chance its active
    }


 

    public override bool TriggerEnter(GameObject gameobject)
    {
        if (_gameObject == null)
        {
            _gameObject = gameobject;
        }

        if (_gameObject == gameobject)
        {
            AdditionalTriggerEnterImplementation();
            base.TriggerEnter(gameobject);
            return true;
        }
        return false;
    }

    public override void TriggerExit(GameObject gameobject)
    {
        Debug.Log("analysing");
        if (_gameObject == gameobject)
        {
            AdditionalTriggerExitImplementation();
            base.TriggerExit(gameobject);
            Debug.Log("exited");
            _gameObject = null;
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
        GameManager._hideEInteractables = true;
    }
}
