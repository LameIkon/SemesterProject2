using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MovementController
{
    private Vector2 _moveVector;
    [SerializeField] private FloatVariable _stamina;

    private bool _inDialogue = false;
    public bool _isMoving = false;
    public static event Action<Vector2> OnMovePositionEvent;

    private void OnEnable()
    {
        InputReader.OnMoveEvent += HandleMove;
        InputReader.OnRunStartEvent += HandleRunStart;
        InputReader.OnRunCancelEvent += HandleRunCancled;
        StartDialogue.OnDialogueStartedEvent += HandleDialogueStart;
        DialogueManager.OnDialogueEndedEvent += HandleDialogueEnd;
    }

    private void OnDisable()
    {
        InputReader.OnMoveEvent -= HandleMove;
        InputReader.OnRunStartEvent -= HandleRunStart;
        InputReader.OnRunCancelEvent -= HandleRunCancled;
        StartDialogue.OnDialogueStartedEvent -= HandleDialogueStart;
        DialogueManager.OnDialogueEndedEvent -= HandleDialogueEnd;
    }

    private void Start()
    {
        //_walkingSpeed = _speedReference.GetMinValue();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (!_inDialogue)
        {
            if (Vector3.Distance(transform.position, _movePoint.position) <= .05f) //makes sure you can't move if u have not reached ur new position yet.
            {
                Move(_moveVector);
                if (_isMoving)
                {
                   // OnMovePositionEvent?.Invoke(transform.position); // This sends the position of the player used to get the sound of walking.
                }
            }

            if (_stamina <= 0f) // Update if stamina reaches 0 to set Movement speed
            {
                HandleRunCancled();
            }
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
        if (dir != Vector2.zero)
        {
            _isMoving = true;
        }
        else
        {
            _isMoving = false;
            GuidelineManager.instance.ResetMovingBool(); // Used to check off if the player is not moving
        }
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

    void HandleDialogueStart() 
    {
        _inDialogue = true;
    }

    void HandleDialogueEnd() 
    {
        _inDialogue = false;
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

}


