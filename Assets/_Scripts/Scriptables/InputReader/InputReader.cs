using UnityEngine;
using UnityEngine.InputSystem;
using System;

[CreateAssetMenu(fileName = "New Input Reader", menuName = "Reader/Input"), HelpURL("https://www.youtube.com/watch?v=ZHOWqF-b51k")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions, GameInput.IUIActions
{

    private GameInput _gameInput;

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

    // The events that other scripts can subscribe to, for information.
    public event Action<Vector2> OnMoveEvent;
    public event Action OnInteractEvent;
    public event Action OnPauseEvent;

    
    public void OnMove(InputAction.CallbackContext context)
    {
        //Debug.Log(context.ReadValue<Vector2>());
        OnMoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        //Debug.Log(context.phase);

        if (context.phase == InputActionPhase.Performed) 
        {
            OnInteractEvent?.Invoke();
        }
    }


    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            OnPauseEvent?.Invoke();
            SetUI(); // Important, here we switch to the UI control scheme
        }
        
    }

    #endregion

    #region UI Controls

    // The events that other scripts can subscribe to, for information.
    public event Action<Vector2> OnNavigateEvent;
    public event Action OnPickEvent; 
    public event Action OnResumeEvent;

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
            OnResumeEvent?.Invoke();
            SetGameplay(); // Important, here we switch to the Gameplay control scheme
        }

    }

    #endregion 
}
