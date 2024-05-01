using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityManager : MonoBehaviour 
{


    public PriorityManager _PriorityManager; // used for other scripts to get it
    public static bool _PriorityInteractable;
    public bool _PriorityThis;

    private void OnEnable()
    {
        if (_PriorityManager == null)
        {
            _PriorityManager = GetComponent<PriorityManager>();
        }
        _PriorityInteractable = true;
        _PriorityThis = true;
    }

    public void ChangeBool()
    {
        _PriorityThis = true;
    }

    public virtual void TriggerEnter() // On Trigger enter call this
    {
        if (_PriorityInteractable && !_PriorityThis)
        {
            _PriorityInteractable = false;
            _PriorityThis = true;

            AdditionalTriggerEnterImplementation(); 
        }
    }

    protected virtual void AdditionalTriggerEnterImplementation()
    {
        // Here all new code will come as overriding
    }

    public virtual void TriggerExit() // On trigger exit call this
    {
        if (!_PriorityInteractable && _PriorityThis)
        {
            _PriorityInteractable = true;
            _PriorityThis = false;
            Debug.Log("exit");
        }
    }

    protected virtual void AdditionalTriggerExitImplementation()
    {
        // Here all new code will come as overriding
    }

    public virtual void TriggerUse(bool state) // On activate call this
    {
        if (state) // if bool is true use
        {
            Debug.Log("Use");
        }
        else // else set to false
        {
            Debug.Log("closed");
        }
    }

    protected virtual void AdditionalTriggerUseImplementation()
    {
        // Here all new code will come as overriding
    }

}
