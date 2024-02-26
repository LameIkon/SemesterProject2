using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float _moveSpeed = 5f;
    [SerializeField, Tooltip("Remember to drag the child collider MovePoint of the player into this field")]
    Transform _movePoint;

    [SerializeField, Tooltip("Select what layers should block movement")]
    LayerMask[] _whatStopsMovement; // This is made into an array as the layers that stop Movement should not change during runtime, therefore it is redundat to make it a List


    [SerializeField] private InputReader _input; // The InputReaders script 
    private Vector2 _moveVector; // This vector is set in the HandleMove



    void Start()
    {
        _movePoint.parent = null; //detachs the MovePoint as a child of player. Not acutally needed. 

    }

    private void OnEnable()
    {
        _input.OnMoveEvent += HandleMove; // Here we subscribe to the OnMoveEvent from the InputReader
    }

    private void OnDisable()
    {
        _input.OnMoveEvent -= HandleMove; // Here we unsubscribe from the OnMoveEvent from the InputReader, it is just good coding pratice to unsub from events when you do not follow them anymore
    }

    void FixedUpdate()
    {
        Vector3 vertical = new Vector3(0f, _moveVector.y, 0f); //Vector for moving Vertical
        Vector3 horizontal = new Vector3(_moveVector.x, 0f, 0f); //Vector for moving Horizontal

        transform.position = Vector3.MoveTowards(transform.position, _movePoint.position, _moveSpeed * Time.fixedDeltaTime); // this "transforms our position to move towards the new point


        if (Vector3.Distance(transform.position, _movePoint.position) <= .05f) //makes sure you can't move if u have not reached ur new position yet.
        {
            // The if statements that where here before are no longer needed
            Move(horizontal);
            Move(vertical);
        }

    }

    private void Move(Vector3 direction)
    {

       // Vector3 newPosition = _movePoint.position + direction; //direction here is either the local variables in FixedUpate (horizontal or vertical)
       // bool cantMove0 = Physics2D.OverlapCircle(newPosition, 0.2f, _whatStopsMovementList[0]); //bool to determine if we overlap with layer in index 0 of our list.


        if (CanMove(direction)) // if we DON't overlap with any colliders with that layer we CAN move.
        {
            _movePoint.position = MovePosition(direction); //basicly we dont actually move the "player" we move the invisible movePoint, and the player constantly "MoveTowards" that point in FixedUpdate.
        }
    }

    // This method is made such that you do not need to hard code in the layers that stop movement
    private bool CanMove(Vector3 direction) 
    {
        bool canMove = true; // We asume you can move 
        
        for (int i = 0; i < _whatStopsMovement.Length; i++)   // Here we iterate over the array to check if you can move to that position
        {
            canMove = !Physics2D.OverlapCircle(MovePosition(direction), 0.2f, _whatStopsMovement[i]); // We check if the _movePoint overlaps a layer it is stopped by, if it does it will return true and then we take the oposite of that
        }

        return canMove;
    }

    // A method that gets the new position of the _movePoint
    private Vector3 MovePosition(Vector3 direction) 
    {
        return _movePoint.position + direction;
    }


    #region EventHandlers

    // This is the handler for the OnMoveEvent 
    private void HandleMove(Vector2 dir) 
    {

        // Because the inputs are normalized we need to set the values to 1 for the move system to work properly

        if (dir.x > .5f)
        {
            dir.x = 1;
        }
        else if (dir.x < -.5f)
        {
            dir.x = -1;
        }

        if (dir.y > .5f)
        {
            dir.y = 1;
        }
        else if (dir.y < -.5f)
        {
            dir.y = -1;
        }

        _moveVector = dir; // Here the direction vector is set to the _moveVector
    }


    #endregion
}
