using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BlizzardBarrerTrigger : MonoBehaviour
{
    private GameObject _container; // Used to move choosen gameobject.
    private Transform _originalParent; // Used to take the container back
    

    private void Awake()
    {
        if (transform.childCount > 0) // If the parent has any children, start search.
        {
            // This should store the GameObject named "InsideBarrier" since it should be the only child
            _container = GetComponentInChildren<Transform>().gameObject; 
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (transform.childCount > 0) // If the parent has any children. The Child should be named "InsideBarrier"
            {
                ParticleSystem barrierBlizzardEffect = GetComponentInChildren<ParticleSystem>(); // Get the particleSystem that is in "InsideBarrier"
                VisualEffect barrierBlizzardFogEffect = GetComponentInChildren<VisualEffect>(); // Get the visualSystem that is in the "InsideBarrier"

                EnvironmentManager.instance._barrierBlizzard = gameObject; // Store this blizzardBarrier as the one that was entered
                EnvironmentManager.instance._barrierBlizzardEffect = barrierBlizzardEffect; // Store the particleSystem into the Environment instance
                EnvironmentManager.instance._barrierBlizzardFogEffect = barrierBlizzardFogEffect; // Store the visualSystem into the Environment instance

                EnvironmentManager.instance.BarrierBlizzard(); // Start the Blizzard conditions
                _originalParent = _container.transform.parent; // Store the original parent that the "InsideBarrier" is originally on
                _container.transform.SetParent(collision.gameObject.transform); // Set the "InsideBarrier" as child of player (to follow him where he goes)
            }        
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            EnvironmentManager.instance.ExitBlizzardBarrier(); // Notify that you exited the weather condition
            _container.transform.SetParent(_originalParent); // Set the "InsideBarrier" as child of the barrier gameobject
            _container.transform.position = _originalParent.position; // Set the "InsideBarrier" back to its original position
        }
    }



}
