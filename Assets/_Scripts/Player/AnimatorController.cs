using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MovementController
{

    public Vector2 _moveVector;
    
    public string _lookingDirection; // Used to check what direction is moving towards
    private float _walkingSpeed; // Used to check if the player is running
    public Animator _PlayerAnimations;

    // Start is called before the first frame update
    void Start()
    {
        _walkingSpeed = _speedReference.GetMinValue();
    }

    // Update is called once per frame
    void Update()
    {



    }


    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (Vector3.Distance(transform.position, _movePoint.position) <= .05f) //makes sure you can't move if u have not reached ur new position yet.
        {
            //Move(_moveVector);
        }

        if (Vector3.Distance(transform.position, _movePoint.position) >= 1f)
        {
            if (_walkingSpeed < _moveSpeed) // If your moveSpeed is faster than your walkingpeed it means you are running
            {
                RunningAnimation();
            }
            else
            {
                MoveAnimation(); 
            }
            //_lookingDirection = MoveAnimation(_lookingDirection);
        }

        if (Vector3.Distance(transform.position, _movePoint.position) == 0) //Checks when you are standing still
        {
            IdleAnimation();
        }

    }

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
    

