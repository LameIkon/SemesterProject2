using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalManager : MonoBehaviour, IDamageable, IFreezeable, IStarveable, ITireable
{

    [SerializeField] private int _tikBetweenMax = 10;
    [SerializeField] private int _tikBetweenMin = 0;
    private int counter = 0;

    private bool _isFull;
    private bool _isWarm;
    private bool _isStarving;
    private bool _isFreezing;

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

    [Space(4f)]

    [Header("Stamina")]
    [SerializeField] private FloatReferencer _staminaPoint;
    [SerializeField] private FloatReference _maxStamina;



    public void Die()
    {
        Debug.Log("Death comes for us all");
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

        if (_freezePoint.GetValue() > _freezeMaxThreshold)
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

        if (_hungerPoint.GetValue() > _hungerMaxThreshold)
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


    void Start()
    {
        _healthPoint.SetValue(70);
        _hungerPoint.SetValue(100);
        _freezePoint.SetValue(100);
        _staminaPoint.SetValue(100);
    }


    void FixedUpdate()
    {

        Tiker();

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
            _outSideTemp.ConstantValue = WeatherCondition._CurrentOutsideTemperature; // Checks the current temperature outside


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
}
