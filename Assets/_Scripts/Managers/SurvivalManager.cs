using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalManager : MonoBehaviour, IDamageable, IFreezeable, IStarveable, ITireable
{
    public void Die()
    {
        Debug.Log("Death comes for us all");
    }

    public void TakeDamage(float damageAmount)
    {
        _healthPoints.ApplyChange(-damageAmount);

        if (_healthPoints.GetValue() >= _maxHealth)
        {
            _healthPoints.SetValue(_maxHealth);
        }
        else if (_healthPoints.GetValue() <= 0f)
        {
            _healthPoints.SetValue(0f);
        }

        if (_healthPoints.GetValue() < 0) 
        {
            Die();    
        }
    }

    public void Freeze(float amount)
    {
        _freezePoints.ApplyChange(-amount);

        if (_freezePoints.GetValue() <= 0f)
        {
            _freezePoints.SetValue(0f);
        }


        if (_freezePoints.GetValue() > _freezeMaxThreshold)
        {
            _isWarm = true;
            if (_isFull)
            {
                TakeDamage(-_healthGainOnFreeze);
            }
        }
        else if (_freezePoints.GetValue() < _freezeMinThreshold)
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
        _hungerPoints.ApplyChange(-amount);
        
        if (_hungerPoints.GetValue() <= 0f) 
        {
            _hungerPoints.SetValue(0f);
        }


        if (_hungerPoints.GetValue() > _hungerMaxThreshold)
        {
            _isFull = true;
            _isStarving = false;
            if (_isWarm)
            {
                TakeDamage(-_healthGainOnHunger);
                
            }
        }
        else if (_hungerPoints.GetValue() < _hungerMinThreshold)
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
        _healthPoints.SetValue(70);
        _hungerPoints.SetValue(100);
        _freezePoints.SetValue(100);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Starve(_starveDamage);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Freeze(_freezeDamage);
        }
        if (Input.GetKey(KeyCode.Z))
        {
            Starve(-_starveDamage);
        }
        if (Input.GetKey(KeyCode.X))
        {
            Freeze(-_freezeDamage);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            _hungerPoints.ApplyChange(_healthGainOnHunger);
        }
        if (Input.GetKey(KeyCode.W))
        {
            _freezePoints.ApplyChange(_healthGainOnFreeze);
        }
    }


    private bool _isFull;
    private bool _isWarm;
    private bool _isStarving;
    private bool _isFreezing;

    [SerializeField] private FloatVariable _healthPoints;
    [SerializeField] private FloatVariable _hungerPoints;
    [SerializeField] private FloatVariable _freezePoints;
    [SerializeField] private FloatVariable _staminaPoints;

    [SerializeField] private FloatReference _hungerMinThreshold;
    [SerializeField] private FloatReference _hungerMaxThreshold;
    [SerializeField] private FloatReference _freezeMinThreshold;
    [SerializeField] private FloatReference _freezeMaxThreshold;
    [SerializeField] private FloatReference _starveDamage;
    [SerializeField] private FloatReference _freezeDamage;
    [SerializeField] private FloatReference _healthGainOnHunger;
    [SerializeField] private FloatReference _healthGainOnFreeze;
    [SerializeField] private FloatReference _healthLossOnHunger;
    [SerializeField] private FloatReference _healthLossOnFreeze;
    [SerializeField] private FloatReference _maxHealth;
    [SerializeField] private FloatReference _maxFreeze;
    [SerializeField] private FloatReference _maxHunger;
    [SerializeField] private FloatReference _maxStamina;




}
