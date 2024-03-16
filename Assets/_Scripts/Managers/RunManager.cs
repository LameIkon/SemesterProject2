using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RunManager : MonoBehaviour, ITireable
{

    [SerializeField] private FloatVariable _staminaValue;
    [SerializeField] private FloatReference _staminaUseOnRun;
    [SerializeField] private FloatReference _staminaRegen;
    [SerializeField] private FloatReference _maxStamina;

    //public static bool _isMoving;
    private bool _isRunning;
    private bool _isMoving;
    private bool _checkOnce = true;
    private bool _checkOnce1 = true;
    void OnEnable() 
    {
        InputReader.OnMoveEvent += MovingChecker;
        InputReader.OnRunStartEvent += RunningChecker;
        InputReader.OnRunCancelEvent += HandleRunCancel;
    }

    void OnDisable() 
    {
        InputReader.OnMoveEvent -= MovingChecker;
        InputReader.OnRunStartEvent -= RunningChecker;
        InputReader.OnRunCancelEvent -= HandleRunCancel;
    }

    Coroutine running;
    Coroutine regen;
    private void Update()
    {
        if (_isMoving && _isRunning && _checkOnce) // Only start running if you are moving and pressing run
        {
            StartRunning();
            _checkOnce = false; // Ensure only 1 instance of the Coroutine
        }
        if (!_isMoving && !_checkOnce || !_isRunning && !_checkOnce)
        {
            _checkOnce = true;
            if (running != null)
            {
                StopCoroutine(running);

            }
            regen = StartCoroutine(RegenStamina()); // Start regen when you stop running
        }
    }
    void StartRunning()
    {
        running = StartCoroutine(UseStamina());
        if (regen != null)
        {
            StopCoroutine(regen); // Stop regen when you start to run
        }
    }
    void RunningChecker() 
    {
        _isRunning = true; // You are now pressing run
    }

    void MovingChecker(Vector2 dir)
    {
        _isMoving = true; // You are now pressing move

        if (dir == Vector2.zero)
        {
            _isMoving = false; // you released pressing move
        }
    }

    void HandleRunCancel() 
    {
        _isRunning = false;
    }


    private IEnumerator UseStamina() 
    {
        while (_staminaValue.GetValue() > 0f) 
        {
            LoseStamina(_staminaUseOnRun);
            yield return null;
        }
        yield return null;
    }

    private IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(3f); // Small delay before recovering stamina
        while (_staminaValue.GetValue() <= _maxStamina) // While value is below max possible stamina
        {
            GainStamina(_staminaRegen);
            yield return null;
        }
        yield return null; // Stop when reached max
    }

    public void LoseStamina(float amount)
    {
        _staminaValue.ApplyChange(-amount);
    }

    public void GainStamina(float amount)
    {
        _staminaValue.ApplyChange(amount);
    }
}
