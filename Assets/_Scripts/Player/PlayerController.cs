using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MovementController
{
    private Vector2 _moveVector;
    [SerializeField] private FloatVariable _stamina;

    private bool _inDialogue = false;
    public static bool _isMoving = false;
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

        if (GuidelineManager.instance.isActiveAndEnabled)
        {
            if (!GuidelineManager.instance._isOngoingEvent)
            {
                return;
            }
        }

        if (!_inDialogue)
        {
            if (Vector3.Distance(transform.position, _movePoint.position) <= .05f) //makes sure you can't move if u have not reached ur new position yet.
            {
                Move(_moveVector);
                if (_isMoving)
                {
                    OnMovePositionEvent?.Invoke(transform.position); // This sends the position of the player used to get the sound of walking.
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

    #region Animations
    /// <summary>
    /// If the Lantern is active it will replace all animations with sprites that are designed to hold something in his hand
    /// </summary>
 
    private bool _isLanternActive; // Used to check once every time lantern activates
    private bool _wasLanternActive = true; // Used to check once evert time the lantern deactivates
    protected override void Update()
    {

        if (LanternDisabler._LanternSTATIC != null) // if the lantern GameObject is active
        {
            ActivatedLantern(); // Update idle animations 

            if (_animator != null && LanternDisabler._LanternSTATIC.activeSelf)
            {
                if (Vector3.Distance(transform.position, _movePoint.position) == 0) //Checks when you are standing still
                {
                    if (!_isIdling) // ensure 1 instance, to reduce potential data usage
                    {
                        IdleLanternAnimation();                        
                        _isIdling = true;
                    }
                }
                return; // Stop the void method here
            }
        }
        base.Update();
    }

    // We have to start idle animations from here since the idleanimations from Update is already running and cannot be called again
    // because of a boolean
    void ActivatedLantern()
    {
        _isLanternActive = LanternDisabler._LanternSTATIC.activeSelf;

        // Start once
        if (_isLanternActive && !_wasLanternActive) // When lantern active
        {
            IdleLanternAnimation(); // Start holding lantern animation
            _wasLanternActive = true;
        }
        else if (!_isLanternActive && _wasLanternActive) // When lantern inactive
        {
            IdleAnimation(); // Start normal animation
            _wasLanternActive = false;
        }
    }

    protected override void StartAnimation()
    {
        if (LanternDisabler._LanternSTATIC.activeSelf) // if the lantern GameObject is active
        {
            if (_animator != null)
            {
                if (Vector3.Distance(transform.position, _movePoint.position) >= 1f) // Only change when you move 1 tile
                {
                    _isIdling = false;
                    if (_walkingSpeed < _moveSpeed) // If your moveSpeed is faster than your walkingpeed it means you are running
                    {
                        RunningLanternAnimation();
                    }
                    else
                    {
                        WalkingLanternAnimation();
                    }
                }
            }
            return; // Stop the void method here
        }
        base.StartAnimation();

    }

    void IdleLanternAnimation()
    {
        switch (_lookingDirection)
        {
            case "Right":
                _animator.Play("Holding_Idle_SideRight");
                return;
            case "Left":
                _animator.Play("Holding_Idle_SideLeft");
                return;
            case "Back":
                _animator.Play("Holding_Idle_Back");
                return;
            case "Front":
                _animator.Play("Holding_Idle_Front");
                return;
            default:
                _animator.Play("Holding_Idle_Front");
                break;
        }
    }

    void WalkingLanternAnimation()
    {
        switch (_lookingDirection)
        {
            case "Right":
                _animator.Play("Holding_Walking_SideRight");
                return;
            case "Left":
                _animator.Play("Holding_Walking_SideLeft");
                return;
            case "Back":
                _animator.Play("Holding_Walking_Back");
                return;
            case "Front":
                _animator.Play("Holding_Walking_Front");
                return;
            default:
                _animator.Play("Holding_Walking_Front");
                break;
        }
    }

    void RunningLanternAnimation()
    {
        switch (_lookingDirection)
        {
            case "Right":
                _animator.Play("Holding_Running_SideRight");
                return;
            case "Left":
                _animator.Play("Holding_Running_SideLeft");
                return;
            case "Back":
                _animator.Play("Holding_Running_Back");
                return;
            case "Front":
                _animator.Play("Holding_Running_Front");
                return;
            default:
                _animator.Play("Holding_Running_Front");
                break;
        }
    }

    #endregion



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
            //GuidelineManager.instance.ResetMovingBool(); // Used to check off if the player is not moving
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


