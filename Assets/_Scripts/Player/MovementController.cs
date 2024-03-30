using System.Collections;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class MovementController : MonoBehaviour
{

    protected float _moveSpeed;
    [SerializeField] protected FloatReferencer _speedReference;
    [SerializeField, Tooltip("Remember to drag the child collider MovePoint of the player into this field")]
    protected Transform _movePoint;




    [SerializeField, Tooltip("Select what layers should block movement")]
    LayerMask[] _whatStopsMovement; // This is made into an array as the layers that stop Movement should not change during runtime, therefore it is redundat to make it a List


    [SerializeField] protected Animator _animator;
    protected string _lookingDirection; // Used to check what direction is moving towards
    protected float _walkingSpeed; // Used to check if the player is running
    protected bool _isIdling;

    void Awake()
    {
        _movePoint.parent = null; //detachs the MovePoint as a child of player. Not acutally needed. 
        _moveSpeed = _speedReference.GetMinValue(); // Sets the walking speed 
        _walkingSpeed = _speedReference.GetMinValue(); // Used to check if _moveSpeed gets changed.
        StopAllCoroutines();
    }

    private void Start()
    {
        //StartCoroutine(IdleAnimate());         
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

            //Checks direction looking at
            if (direction.x > .5f)
            {
            direction.x = 1;
            _lookingDirection = "Right";
            }
            else if (direction.x < -.5f)
            {
            direction.x = -1;
            _lookingDirection = "Left";
            }

            if (direction.y > .5f)
            {
            direction.y = 1;
            _lookingDirection = "Back";
            }
            else if (direction.y < -.5f)
            {
            direction.y = -1;
            _lookingDirection = "Front";
            }
        StartAnimation();
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

      
    private void Update() // Animations
    {
        if (Vector3.Distance(transform.position, _movePoint.position) == 0) //Checks when you are standing still
        {
            if (!_isIdling)
            {
                IdleAnimation();
                _isIdling = true;
            }
                
        }
    }

    void StartAnimation()
    {
        if (_animator != null)
            {
            if (Vector3.Distance(transform.position, _movePoint.position) >= 1f) // Only change when you move 1 tile
            {
                _isIdling = false;
                if (_walkingSpeed < _moveSpeed) // If your moveSpeed is faster than your walkingpeed it means you are running
                {
                    RunningAnimation();
                }
                else
                {
                    MoveAnimation(); 
                }
            }
        } 
    }

    void CheckIdleState()
    {
         if (Vector3.Distance(transform.position, _movePoint.position) == 0) //Checks when you are standing still
            {
                IdleAnimation();
            }
    }

    IEnumerator idleCoroutine;
    void IdleAnimation()
    {
        switch (_lookingDirection)
        {
            case "Right":
                _animator.Play("Idle_SideRight");
                return;
            case "Left":
                _animator.Play("Idle_SideLeft");
                return;
            case "Back":
                _animator.Play("Idle_Back");
                return;
            case "Front":
                _animator.Play("Idle_Front");
                //if (idleCoroutine != null)
                //{
                //    StopCoroutine(idleCoroutine);
                //    Debug.Log("stop");
                //}
                //idleCoroutine = IdleAnimate();
                //StartCoroutine(idleCoroutine);
                return;
            default: 
                //if (idleCoroutine != null)
                //{
                //    StopCoroutine(idleCoroutine);
                //    Debug.Log("stop");
                //}
                //idleCoroutine = IdleAnimate();
                //StartCoroutine(idleCoroutine);
                _animator.Play("Idle_Front");
                break;
        }
    }

    ////IEnumerator IdleAnimate()
    ////{
    ////    while (true)
    ////    {
    ////        Debug.Log("Cycle");
    ////        IdleFrontVariations();
    ////        yield return new WaitUntil(() => IsAnimationFinished());
    ////        Debug.Log("Animation finished");
    ////        yield return new WaitForSeconds(2);
    ////    }
    ////}
    //bool IsAnimationFinished()
    //{
    //    // Check if the current animation is finished playing
    //    return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1; // 1 indicate the end of an animation se;
    //}

    //void IdleFrontVariations()
    //{
    //    int number = Random.Range(0,8);
    //    Debug.Log(number);
    //    switch (number)
    //    {
    //        case 0:
    //        case 1:
    //        case 2:
    //        case 3:
    //        case 4:
    //        case 5:
    //            _animator.Play("Idle_Front");
    //            return;
    //        case 6:
    //            _animator.Play("Idle_Front_Var_1_1");
    //            return;
    //        case 7:
    //            _animator.Play("Idle_Front_Var_1_2");
    //            return;
    //        default:
    //            _animator.Play("Idle_Front");
    //            break;

    //    }
    //}

    void MoveAnimation()
    {
        switch (_lookingDirection)
        {
            case "Right":
                _animator.Play("Moving_SideRight");
                return;
            case "Left":
                _animator.Play("Moving_SideLeft");
                return;
            case "Back":
                _animator.Play("Moving_Back");
                return;
            case "Front":
                _animator.Play("Moving_Front");
                return;
            default: 
                _animator.Play("Moving_Front");
                break;
        }
    }

    void RunningAnimation()
    {
        switch (_lookingDirection)
        {
            case "Right":
                _animator.Play("Running_SideRight");
                return;
            case "Left":
                _animator.Play("Running_SideLeft");
                return;
            case "Back":
                _animator.Play("Running_Back");
                return;
            case "Front":
                _animator.Play("Running_Front");
                return;
            default: 
                _animator.Play("Running_Front");
                break;
        }
    }
}
