using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MovementController : MonoBehaviour
{

    protected float _moveSpeed;
    [SerializeField] protected FloatReferencer _speedReference;
    [SerializeField, Tooltip("Remember to drag the child collider MovePoint of the player into this field")]
    protected Transform _movePoint;

    [SerializeField, Tooltip("Select what layers should block movement")]
    LayerMask[] _whatStopsMovement; // This is made into an array as the layers that stop Movement should not change during runtime, therefore it is redundat to make it a List
    [SerializeField] LayerMask _snowLayer;
    [SerializeField] LayerMask _woodLayer;

    [Header("Audio Sounds"), SerializeField]
    private AudioEvent _snowStepSound;
    [SerializeField] private AudioEvent _woodStepSound;
    [SerializeField] private AudioEvent _stopMoveSound;
    private AudioSource _audioSource;



    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _movePoint.parent = null; //detachs the MovePoint as a child of player. Not acutally needed. 
        _moveSpeed = _speedReference.GetMinValue(); // Sets the walking speed 
    }

    protected virtual void FixedUpdate() 
    {
        transform.position = Vector3.MoveTowards(transform.position, _movePoint.position, _moveSpeed * Time.fixedDeltaTime); // this "transforms our position to move towards the new point
    }

    public void Move(Vector3 direction)
    {
        if (CanMove(direction)) // if we DON't overlap with any colliders on nonwalkable layers, we CAN move.
        {
            _movePoint.position = MovePosition(direction); //basicly we dont actually move the "player" we move the invisible movePoint, and the player constantly "MoveTowards" that point in FixedUpdate.

            FootstepSound(direction); // Does net work
        }
    }


    private void FootstepSound(Vector3 dir)  // This does not work yet.
    {
        Ray ray = Camera.main.ScreenPointToRay(_movePoint.position);
        RaycastHit2D[] hit = Physics2D.GetRayIntersectionAll(ray);

        if (Physics2D.OverlapCircle(MovePosition(dir), 0.2f, _snowLayer))
        {
            Debug.Log("WoodLayer");
            if (_woodStepSound != null)
            {
                _woodStepSound.Play(_audioSource);
            }
        }
        else if (Physics2D.OverlapCircle(MovePosition(dir), 0.2f, _snowLayer))
        {
            Debug.Log("SnowLayer");
            if (_snowStepSound != null)
            {
                _snowStepSound.Play(_audioSource);
            }
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
