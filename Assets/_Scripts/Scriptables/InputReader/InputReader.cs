using UnityEngine;
using UnityEngine.InputSystem;
using System;

//[CreateAssetMenu(fileName = "New Input Reader", menuName = "Reader/Input"), HelpURL("https://www.youtube.com/watch?v=ZHOWqF-b51k")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions
{

    public static GameInput _gameInput;

    public static bool _isUI = false;


    private void OnEnable() 
    {
        // This instanciates the GameInput script if there is non
        if (_gameInput == null) 
        {
            _gameInput = new GameInput();

            _gameInput.Gameplay.SetCallbacks(this); 

            SetGameplay(); // This is what picks what keys are chosen when the game loads

        }
    }


    // The events that other scripts can subscribe to, for information for gameplay.
    public static event Action<Vector2> OnMoveEvent; // This sends a Vector2 along with the event
    public static event Action OnInteractEvent;
    public static event Action OnInteractEventCancled;
    public static event Action OnPauseEvent;
    public static event Action OnInventoryEvent;
    public static event Action OnEatEvent;

    public static event Action OnLeftClickEvent;
    public static event Action OnRightClickEvent;
    public static event Action<Vector2> OnMousePositionEvent;


    public static event Action OnInventoryCloseEvent;
    public static event Action OnRunStartEvent;
    public static event Action OnRunCancelEvent;
    public static event Action<int> OnButtonPressEvent;

    #region Change between Layouts

    // These two methods switch the the layouts that are used

    public static void SetGameplay() 
    {
        _gameInput.Gameplay.Enable();
    }

    #endregion

    #region Ingame Controls


    
    // This method listens to the move inputs that are connected in the Input System
    public void OnMove(InputAction.CallbackContext context)
    {
        OnMoveEvent?.Invoke(context.ReadValue<Vector2>()); // The context.ReadValue is used because the input is set to Value in the Input System, the OnMoveEvent will then invoke the Event with the Vector2

    }

    public void OnEat(InputAction.CallbackContext context) 
    {
        if (context.phase == InputActionPhase.Performed)
        {
            OnEatEvent?.Invoke();
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)  // This is only true on the Performed stage when the button is pressed, there is also the .Canceled and .Started
        {
            OnInteractEvent?.Invoke();
        }

        if (context.phase == InputActionPhase.Canceled) 
        {
            OnInteractEventCancled?.Invoke();
        }
    }


    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            OnPauseEvent?.Invoke();
        }
        
    }
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && !_isUI) 
        {
            OnInventoryEvent?.Invoke();
        }
    }

    #endregion


    public void OnLeftClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) 
        {
            OnLeftClickEvent?.Invoke();
        }
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            OnRightClickEvent?.Invoke();
        }
    }

    public void OnPostion(InputAction.CallbackContext context)
    {
        //Debug.Log(context.ReadValue<Vector2>());
        OnMousePositionEvent?.Invoke(context.ReadValue<Vector2>());
    }



    public void OnRun(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Performed) 
        {
            OnRunStartEvent?.Invoke();
        }

        if (context.phase == InputActionPhase.Canceled) 
        {
            OnRunCancelEvent?.Invoke();
        }

    }



    public void OnButton1(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) 
        {
            OnButtonPressEvent?.Invoke(0);
        }
    }

    public void OnButton2(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            OnButtonPressEvent?.Invoke(1);
        }
    }
}
