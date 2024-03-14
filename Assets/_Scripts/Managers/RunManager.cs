using System.Collections;
using UnityEngine;

public class RunManager : MonoBehaviour, ITireable
{

    [SerializeField] private FloatVariable _staminaValue;
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

    void HandleRunStart() 
    {
        running = StartCoroutine(UseStamina());
    }

    void HandleRunCancel() 
    {
        StopCoroutine(running);
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

    public void LoseStamina(float amount)
    {
        _staminaValue.ApplyChange(-amount);
    }
}
