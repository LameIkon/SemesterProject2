using UnityEngine;
using UnityEngine.InputSystem;
using System;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "Reader/Input"), HelpURL("https://www.youtube.com/watch?v=ZHOWqF-b51k")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions, GameInput.IUIActions
{

    private GameInput _gameInput;

    private bool _isUI = false;

    void Awake() 
    {
       // _mainCamera = Camera.main;
    
    }

    void Start() 
    {
        _isUI = false;
    }

    private void OnEnable() 
    {
        // This instanciates the GameInput script if there is non
        if (_gameInput == null) 
        {
            _gameInput = new GameInput();

            _gameInput.Gameplay.SetCallbacks(this); 
            _gameInput.UI.SetCallbacks(this);

            SetGameplay(); // This is what picks what keys are chosen when the game loads

        }
    }


    // The events that other scripts can subscribe to, for information for gameplay.
    public static event Action<Vector2> OnMoveEvent; // This sends a Vector2 along with the event
    public static event Action OnInteractEvent; 
    public static event Action OnPauseEvent;
    public static event Action OnInventoryOpenEvent;

    // The events that other scripts can subscribe to, for information about UI.
    public static event Action<Vector2> OnNavigateEvent;
    public static event Action OnPickEvent; 
    public static event Action OnResumeEvent;
    public static event Action OnInventoryCloseEvent;

    public static event Action OnLeftClickEvent;
    public static event Action OnRightClickEvent;
    public static event Action<Vector2> OnMousePositionEvent;

    #region Change between Layouts

    // These two methods switch the the layouts that are used
    
    private void SetGameplay() 
    {
        _gameInput.UI.Disable();
        _gameInput.Gameplay.Enable();
    }

    private void SetUI() 
    {
        _gameInput.Gameplay.Disable();
        _gameInput.UI.Enable();
    }

    #endregion

    #region Ingame Controls


    
    // This method listens to the move inputs that are connected in the Input System
    public void OnMove(InputAction.CallbackContext context)
    {
        OnMoveEvent?.Invoke(context.ReadValue<Vector2>()); // The context.ReadValue is used because the input is set to Value in the Input System, the OnMoveEvent will then invoke the Event with the Vector2
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)  // This is only true on the Performed stage when the button is pressed, there is also the .Canceled and .Started
        {
            OnInteractEvent?.Invoke();
        }
    }


    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _isUI = true; // This is so the inventory event does not interfere with the Pause button.
            OnPauseEvent?.Invoke();
            SetUI(); // Important, here we switch to the UI control scheme
        }
        
    }
    public void OnInventoryOpen(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && !_isUI) 
        {
            OnInventoryOpenEvent?.Invoke();
            SetUI();
        }
    }

    #endregion

    #region UI Controls


    public void OnNavigate(InputAction.CallbackContext context)
    {
        OnNavigateEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnPick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed) 
        {
            OnPickEvent?.Invoke();
        }

    }

    public void OnResume(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _isUI = false;
            OnResumeEvent?.Invoke();
            SetGameplay(); // Important, here we switch to the Gameplay control scheme
        }

    }

    public void OnInventoryClose(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && !_isUI) 
        {
            OnInventoryCloseEvent?.Invoke();
            SetGameplay();
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

}
