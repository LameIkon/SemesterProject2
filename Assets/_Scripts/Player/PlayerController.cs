using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float _moveSpeed = 5f;
    [SerializeField, Tooltip("Remember to drag the child collider MovePoint of the player into this field")] Transform _movePoint;
    [SerializeField, Tooltip("Select what layers should block movement")] List<LayerMask> _whatStopsMovementList = new List<LayerMask>();



    // Start is called before the first frame update
    void Start()
    {
        _movePoint.parent = null; //detachs the MovePoint as a child of player. Not acutally needed. 

    }


    void FixedUpdate()
    {
        Vector3 horizontal = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f); //Vector for moving Horizontal
        Vector3 vertical = new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f); //Vector for moving Vertical

        transform.position = Vector3.MoveTowards(transform.position, _movePoint.position, _moveSpeed * Time.deltaTime); // this "transforms our position to move towards the new point

        if (Vector3.Distance(transform.position, _movePoint.position) <= .05f) //makes sure you can't move if u have not reached ur new position yet.
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f) //if the X-axes is 1 or -1 we move horizontal. Mathf.abs tracks if its 1 regardsless of negative or positive 1.
            {
                Move(horizontal);
            }

            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f) //if the Y-axes is 1 or -1 we move vertical. REMOVE "ELSE" to move diagonal
            {
                Move(vertical);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void Move(Vector3 direction)
    {
        Vector3 newPosition = _movePoint.position + direction; //direction here is either the local variables in FixedUpate (horizontal or vertical)
        bool cantMove0 = Physics2D.OverlapCircle(newPosition, 0.2f, _whatStopsMovementList[0]); //bool to determine if we overlap with layer in index 0 of our list.


        if (!cantMove0) // if we DON't overlap with any colliders with that layer we CAN move.
        {
            _movePoint.position = newPosition; //basicly we dont actually move the "player" we move the invisible movePoint, and the player constantly "MoveTowards" that point in FixedUpdate.
        }
    }
}
