using System.Collections;
using UnityEngine;

public class RunManager : MonoBehaviour, ITireable
{

    [SerializeField] private FloatVariable _maxStaminaValue;
    [SerializeField] private FloatReference _staminaRegenValue;
    [SerializeField] private FloatReference _staminaUseOnRun;


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
    Coroutine regenStamina;

    void HandleRunStart() 
    {
        running = StartCoroutine(UseStamina());
        if (regenStamina != null)
        {
            StopCoroutine(regenStamina);
        }
    }

    void HandleRunCancel() 
    {
        StopCoroutine(running);
        regenStamina = StartCoroutine(RegenStamina());
    }


    private IEnumerator UseStamina() 
    {
        while (_maxStaminaValue.GetValue() > 0f) 
        {
            LoseStamina(_staminaUseOnRun);
            yield return null;
        }
        yield return null;
    }

    private IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(3f);
        while (_maxStaminaValue.GetValue() < 0f)
        {
            GainStamina(_staminaRegenValue);
            Debug.Log("regen");
            yield return null;
        }
        yield return null;
    }

    public void LoseStamina(float amount)
    {
        _maxStaminaValue.ApplyChange(-amount);
    }

    public void GainStamina(float amount)
    {
        _maxStaminaValue.ApplyChange(amount);
    }
}
