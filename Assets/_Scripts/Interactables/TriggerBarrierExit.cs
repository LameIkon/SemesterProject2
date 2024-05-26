using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBarrierExit : MonoBehaviour
{
    [SerializeField] private Transform _insideBarrier; // Used to move choosen gameobject.
    [SerializeField] private Transform _originalParent; // Used to take the container back

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("exit");
            EnvironmentManager.instance.ExitBlizzardBarrier(); // Notify that you exited the weather condition
            _insideBarrier.transform.SetParent(_originalParent); // Set the "InsideBarrier" as child of the barrier gameobject
            _insideBarrier.transform.position = _originalParent.position; // Set the "InsideBarrier" back to its original position
            BlizzardBarrerTrigger._IsInsidebarrier = false;
        }
    }
}
