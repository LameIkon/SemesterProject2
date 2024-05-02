using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PriorityManager : MonoBehaviour 
{
    //public static PriorityManager _Instance;

    private PriorityManager _PriorityManager; // used for other scripts to get it
    public static bool _PriorityInteractable;
    public static bool _canInteractChest = true; // call from other scripts to say that this should be called
    public static bool _canInteractDialogue = true; // call from other scripts to say that this should be called
    public static GameObject _CompareGameObject;
    //public bool _PriorityThis;

    private void OnEnable()
    {
        if (_PriorityManager == null)
        {
            _PriorityManager = GetComponent<PriorityManager>();
        }
        _PriorityInteractable = true;
        _canInteractChest = true;
        _canInteractDialogue = true;
    }


    public virtual bool TriggerEnter(GameObject gameObject) // On Trigger enter call this
    {
        if (_PriorityInteractable)
        {
            _PriorityInteractable = false;
            Debug.Log("enter");

            //AdditionalTriggerEnterImplementation(); should be added here
            return true;
        }
        return false;
    }

    protected virtual void AdditionalTriggerEnterImplementation()
    {
        // Here all new code will come as overriding
    }

    public virtual void TriggerExit(GameObject gameObject) // On trigger exit call this
    {
        if (!_PriorityInteractable)
        {
            AdditionalTriggerExitImplementation();
            _PriorityInteractable = true;
            Debug.Log("exit");
        }
        else
        {
            return;
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
