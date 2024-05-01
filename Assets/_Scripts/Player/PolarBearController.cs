using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolarBearController : MovementController
{

    [SerializeField] private GameObject _attackColliderRange;
    [SerializeField] private CircleCollider2D _aggroColliderRange;
    private void Start()
    {
        //_moveSpeed = _speedReference.GetMaxValue();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    public override void Move(Vector3 direction)
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
            StartAnimation();
        }
    }

    protected override void IdleAnimation()
    {
        switch (_lookingDirection)
        {
            case "Right":
                _animator.Play("Idle_Right");
                return;
            case "Left":
                _animator.Play("Idle_Left");
                return;
            default:
                _animator.Play("Idle_Left");
                break;
        }
    }

    protected override void MoveAnimation()
    {
        switch (_lookingDirection)
        {
            case "Right":
                _animator.Play("Walking_Right");
                return;
            case "Left":
                _animator.Play("Walking_Left");
                return;
            default:
                _animator.Play("Walking_Left");
                break;
        }
    }

    protected override void RunningAnimation()
    {
        switch (_lookingDirection)
        {
            case "Right":
                _animator.Play("Running_Right");
                return;
            case "Left":
                _animator.Play("Running_Left");
                return;
            default:
                _animator.Play("Running_Left");
                break;
        }
    }
}
