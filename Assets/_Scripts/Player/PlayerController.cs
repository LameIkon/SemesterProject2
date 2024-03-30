using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MovementController
{
    private Vector2 _moveVector;
    [SerializeField] private FloatVariable _stamina;

    //public string _lookingDirection; // Used to check what direction is moving towards
    //private float _walkingSpeed; // Used to check if the player is running
    private Animator _PlayerAnimations;

    private void OnEnable()
    {
        InputReader.OnMoveEvent += HandleMove;
        InputReader.OnRunStartEvent += HandleRunStart;
        InputReader.OnRunCancelEvent += HandleRunCancled;
    }

    private void OnDisable()
    {
        InputReader.OnMoveEvent -= HandleMove;
        InputReader.OnRunStartEvent -= HandleRunStart;
        InputReader.OnRunCancelEvent -= HandleRunCancled;
    }

    private void Start()
    {
        //_walkingSpeed = _speedReference.GetMinValue();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (Vector3.Distance(transform.position, _movePoint.position) <= .05f) //makes sure you can't move if u have not reached ur new position yet.
        {
            Move(_moveVector);

        }

        //if (Vector3.Distance(transform.position, _movePoint.position) >= 1f)
        //{
        //    if (_walkingSpeed < _moveSpeed) // If your moveSpeed is faster than your walkingpeed it means you are running
        //    {
        //        RunningAnimation();
        //    }
        //    else
        //    {
        //        MoveAnimation(); 
        //    }
        //    //_lookingDirection = MoveAnimation(_lookingDirection);
        //}

        //if (Vector3.Distance(transform.position, _movePoint.position) == 0) //Checks when you are standing still
        //{
        //    IdleAnimation();
        //}

        if (_stamina.GetValue() <= 0f) // Update if stamina reaches 0 to set Movement speed
        {
            HandleRunCancled();
        }
    }


    private void SetSpeed(float speed) 
    {
        _moveSpeed = speed; 
    }

    #region EventHandlers

    // This is the handler for the OnMoveEvent 
    private void HandleMove(Vector2 dir)
    {

        // Because the inputs are normalized we need to set the values to 1 for the move system to work properly

        if (dir.x > .5f)
        {
            dir.x = 1;
            //_lookingDirection = "Right";
        }
        else if (dir.x < -.5f)
        {
            dir.x = -1;
            //_lookingDirection = "Left";
        }

        if (dir.y > .5f)
        {
            dir.y = 1;
            ///_lookingDirection = "Back";
        }
        else if (dir.y < -.5f)
        {
            dir.y = -1;
            //_lookingDirection = "Front";
        }
        //_lookingDirection = DirectionChecker(_lookingDirection); // Store data on which direction is looking
        _moveVector = dir; // Here the direction vector is set to the _moveVector
    }


    // This handler grabs the run key and sets the appropiate speed

    void HandleRunStart() 
    {
        if (_stamina.GetValue() > 0f) 
        {
            SetSpeed(_speedReference.GetMaxValue());
        }
    }

    void HandleRunCancled() 
    {
        SetSpeed(_speedReference.GetMinValue());
    } 
    #endregion

    // <NOT NECESSARY>
    // public static void DeactivatePlayerControls()
    // {
    //     // Kode til at deaktivere movement
    // }
    //
    // public static void ActivatePlayerControls()
    // {
    //     // Kode til at aktivere movement
    // }
    // </NOT NECESSARY>

    string DirectionChecker(string direction) // The order determinds which direction to priotize looking
    {

        if (_moveVector.x > .5f)
        {
            direction = "Right";
        }
        else if (_moveVector.x < -.5f)
        {
            direction = "Left";
        }
        if (_moveVector.y > .5f)
        {
            direction = "Back";
        }
        else if (_moveVector.y < -.5f)
        {
            direction = "Front";
        }

        return direction;
    }

    void IdleAnimation()
    {
        Debug.Log("Idle");
        switch (_lookingDirection)
        {
            case "Right":
                _PlayerAnimations.Play("Idle_SideRight");
                return;
            case "Left":
                _PlayerAnimations.Play("Idle_SideLeft");
                return;
            case "Back":
                _PlayerAnimations.Play("Idle_Back");
                return;
            case "Front":
                _PlayerAnimations.Play("Idle_Front");
                return;
            default: 
                _PlayerAnimations.Play("Idle_Front");
                break;
        }
    }

    void MoveAnimation()
    {
        Debug.Log("moving");
        switch (_lookingDirection)
        {
            case "Right":
                _PlayerAnimations.Play("Moving_SideRight");
                return;
            case "Left":
                _PlayerAnimations.Play("Moving_SideLeft");
                return;
            case "Back":
                _PlayerAnimations.Play("Moving_Back");
                return;
            case "Front":
                _PlayerAnimations.Play("Moving_Front");
                return;
            default: 
                _PlayerAnimations.Play("Moving_Front");
                break;
        }
    }

    void RunningAnimation()
    {
        Debug.Log("Running");
        switch (_lookingDirection)
        {
            case "Right":
                _PlayerAnimations.Play("Running_SideRight");
                return;
            case "Left":
                _PlayerAnimations.Play("Running_SideLeft");
                return;
            case "Back":
                _PlayerAnimations.Play("Running_Back");
                return;
            case "Front":
                _PlayerAnimations.Play("Running_Front");
                return;
            default: 
                _PlayerAnimations.Play("Running_Front");
                break;
        }
    }
}


