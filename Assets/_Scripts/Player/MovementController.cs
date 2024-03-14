using UnityEditor.Rendering;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    protected float _moveSpeed;
    [SerializeField] protected FloatReferencer _speedReference;
    [SerializeField, Tooltip("Remember to drag the child collider MovePoint of the player into this field")]
    protected Transform _movePoint;

    [SerializeField, Tooltip("Select what layers should block movement")]
    LayerMask[] _whatStopsMovement; // This is made into an array as the layers that stop Movement should not change during runtime, therefore it is redundat to make it a List

    [SerializeField] private float _currentSpeed;
    void Awake()
    {
        _movePoint.parent = null; //detachs the MovePoint as a child of player. Not acutally needed. 
        _moveSpeed = _speedReference.GetMinValue(); // Sets the walking speed
        _currentSpeed = _moveSpeed;

    }



    protected virtual void FixedUpdate() 
    {

        _currentSpeed = _moveSpeed - (_moveSpeed * WeatherCondition._MovementSpeedDebuff / 100);
        
       
        transform.position = Vector3.MoveTowards(transform.position, _movePoint.position, _currentSpeed * Time.fixedDeltaTime); // this "transforms our position to move towards the new point
    }

    public void Move(Vector3 direction)
    {
        if (CanMove(direction)) // if we DON't overlap with any colliders on nonwalkable layers, we CAN move.
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

}
