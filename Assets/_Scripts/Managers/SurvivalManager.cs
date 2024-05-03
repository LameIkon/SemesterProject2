using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalManager : Singleton<SurvivalManager>, IDamageable, IFreezeable, IStarveable, ITireable
{

    [SerializeField] private int _tikBetweenMax = 10;
    [SerializeField] private int _tikBetweenMin = 0;
    private int counter = 0;

    private bool _inDialogue = false;
    private bool _isFull;
    private bool _isWarm;
    // Pragma used to ignore the warnings
    #pragma warning disable 0414

    private bool _isStarving;
    private bool _isFreezing;

    #pragma warning restore 0414

    // [SerializeField] private FloatVariable _healthPoints;
    // [SerializeField] private FloatVariable _hungerPoints;
    // [SerializeField] private FloatVariable _freezePoints;
    // [SerializeField] private FloatVariable _staminaPoints;
    [Header("Health")]
    [SerializeField] private FloatReferencer _healthPoint;
    [SerializeField] private FloatReference _maxHealth;

    [Space(4f)]


    [Header("Temperature")]
    [SerializeField] private FloatReferencer _freezePoint;
    [SerializeField] private FloatReference _maxFreeze;
    [SerializeField] private FloatReference _freezeDamage;
    [SerializeField] private FloatReference _freezeMinThreshold;
    [SerializeField] private FloatReference _freezeMaxThreshold;
    [SerializeField] private FloatReference _healthGainOnFreeze;
    [SerializeField] private FloatReference _healthLossOnFreeze;
    [SerializeField] private FloatReference _zeroTempAmount;
    [Space(2f)]
    [SerializeField] private FloatReference _outSideTemp;
    [SerializeField] private FloatReference _heatSource;
    [SerializeField] private FloatReference _coldResist;
    [Space(4f)]

    [Header("Hunger")]
    [SerializeField] private FloatReferencer _hungerPoint;
    [SerializeField] private FloatReference _maxHunger;
    [SerializeField] private FloatReference _starveDamage;
    [SerializeField] private FloatReference _hungerMaxThreshold;
    [SerializeField] private FloatReference _hungerMinThreshold;
    [SerializeField] private FloatReference _healthGainOnHunger;
    [SerializeField] private FloatReference _healthLossOnHunger;
    [SerializeField] private FloatReference _zeroHungerAmount;

    [Space(4f)]

    [Header("Stamina")]
    [SerializeField] private FloatReferencer _staminaPoint;
    [SerializeField] private FloatReference _maxStamina;

    [Header ("Player")]
    [SerializeField] private Animator _playerAnimator;
    [SerializeField] private MovementController _movementController;
    [SerializeField] private GameObject _lantern;
    [SerializeField] private GameObject _playerChatBubble;

    // chat calls
    private bool _lowHunger;
    private bool _lowTemperature;
    private bool _isTimingOut;


    #region Unity Methods
    protected override void Awake() 
    {
        base.Awake();
        _inDialogue = false;
    }

    private void OnEnable()
    {
        Invoke(nameof(SetSurvivalbars), 0.01f); // need a small delay
        StartDialogue.OnDialogueStartedEvent += HandleDialogueStart;
        DialogueManager.OnDialogueEndedEvent += HandleDialogueEnd;
    }

    private void OnDisable()
    {
        StartDialogue.OnDialogueStartedEvent -= HandleDialogueStart;
        DialogueManager.OnDialogueEndedEvent -= HandleDialogueEnd;
    }


    void FixedUpdate()
    {
        if (!_inDialogue)
        {
            Tiker();
        }
    }


    #endregion

    void HandleDialogueStart() 
    {
        _inDialogue = true;
    }

    void HandleDialogueEnd() 
    {
        _inDialogue = false;
    }

    void SetSurvivalbars()
    {
        _healthPoint.SetValue(100);
        _hungerPoint.SetValue(100);
        _freezePoint.SetValue(100);
        _staminaPoint.SetValue(100);
    }

    public void Die()
    {
        Debug.Log("Death comes for us all");
        _movementController.enabled = false;
        _lantern.SetActive(false);
        _playerAnimator.Play("Dying");
    }

    public void TakeDamage(float damageAmount)
    {
        _healthPoint.ApplyChange(-damageAmount);

        if (_healthPoint.GetValue() <= 0)
        {
            Die();
        }
    }

    public void Freeze(float amount)
    {

        _freezePoint.ApplyChange(amount * TemperatureChecker());
        
        if (_freezePoint.GetValue() <= 1) 
        {
            _isFreezing = true;
            TakeDamage(_zeroTempAmount * _healthLossOnFreeze);
        }
        else if (_freezePoint.GetValue() > _freezeMaxThreshold)
        {
            _isWarm = true;
            if (_isFull)
            {
                TakeDamage(-_healthGainOnFreeze);
            }
        }
        else if (_freezePoint.GetValue() < _freezeMinThreshold)
        {
            _isFreezing = true;
            TakeDamage(_healthLossOnFreeze);
            if (!_lowTemperature && !_isTimingOut)
            {
                _lowTemperature = true;
                _isTimingOut = true;
                _playerChatBubble.transform.GetChild(2).gameObject.SetActive(true);
                Invoke(nameof(TimeOutChat),30f); // Set bool to true after 30 secs
            }
        }
        else
        {
            _isWarm = false;
            _isFreezing = false;
        }

    }
    public void Starve(float amount)
    {

        _hungerPoint.ApplyChange(-amount); 

        if (_hungerPoint.GetValue() < 1)
        {
            _isStarving = true;
            _isFull = false;
            TakeDamage(_zeroHungerAmount *  _healthLossOnHunger);
        }
        else if (_hungerPoint.GetValue() > _hungerMaxThreshold)
        {
            _isFull = true;
            _isStarving = false;
            if (_isWarm)
            {
                TakeDamage(-_healthGainOnHunger);

            }
        }
        else if (_hungerPoint.GetValue() < _hungerMinThreshold)
        {
            _isStarving = true;
            _isFull = false;
            TakeDamage(_healthLossOnHunger);
            if (!_lowHunger && !_isTimingOut)
            {
                _lowTemperature = true;
                _isTimingOut = true;
                _playerChatBubble.transform.GetChild(1).gameObject.SetActive(true);
                Invoke(nameof(TimeOutChat), 30f); // Set bool to true after 30 secs
            }
        }
        else
        {
            _isFull = false;
            _isStarving = false;
        }

    }

    public void LoseStamina(float amount)
    {

    }


    private void Tiker()
    {
        counter--;

        if (counter <= 0)
        {
            Freeze(_freezeDamage);
            Starve(_starveDamage);
            counter = Random.Range(_tikBetweenMin, _tikBetweenMax);
        }

    }


    private float TemperatureChecker()
    {

        if (_heatSource <= 0 && ColdResChecker())
        {
            return 0;
        }
        else if (ColdResChecker())
        {
            return _heatSource;
        }
        else 
        {
            return _outSideTemp + _heatSource + _coldResist; 
        }

    }

    private bool ColdResChecker() 
    {
        if (_outSideTemp + _coldResist > 0)
        {
            return true;
        }
        else
        { 
            return false;
        }
    
    }

    private void TimeOutChat()
    {
        _lowTemperature = false;
        _lowHunger = false;
    }

    private void OnApplicationQuit()
    {
        SetSurvivalbars();
    }
}
