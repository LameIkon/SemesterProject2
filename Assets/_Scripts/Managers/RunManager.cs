using System.Collections;
using UnityEngine;

public class RunManager : MonoBehaviour, ITireable
{

    [SerializeField] private FloatVariable _staminaValue;
    [SerializeField] private FloatReference _staminaUseOnRun;
    [SerializeField] private FloatReference _staminaRegen;
    [SerializeField] private FloatReference _maxStamina;


    void OnEnable() 
    {
        InputReader.OnRunStartEvent += HandleRunStart;
        InputReader.OnRunCancelEvent += HandleRunCancel;
    }

    void OnDisable() 
    {
        InputReader.OnRunStartEvent -= HandleRunStart;
        InputReader.OnRunCancelEvent -= HandleRunCancel;
    }

    Coroutine running;
    Coroutine regen;

    void HandleRunStart() 
    {
        running = StartCoroutine(UseStamina());
        if(regen != null) 
        {
            StopCoroutine(regen); // Stop regen when you start to run
        }
    }

    void HandleRunCancel() 
    {
        StopCoroutine(running); 
        regen = StartCoroutine(RegenStamina()); // Start regen when you stop running
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
        Debug.Log("Regen");
        yield return new WaitForSeconds(3f); // Small delay before recovering stamina
        while (_staminaValue.GetValue() <= _maxStamina) // while value is below max possible stamina
        {
            GainStamina(_staminaRegen);
            yield return null;
        }
        yield return null; // when reached max stop
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
