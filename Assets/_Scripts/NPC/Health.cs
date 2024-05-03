using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private FloatReference _maxHealth;
    [SerializeField] private FloatVariable _health;

    protected virtual void Awake()
    {
        if (_health == null)
        {
            _health = ScriptableObject.CreateInstance<FloatVariable>();
            _health.SetValue(_maxHealth);
        }
        
    }

    public void Die()
    {       
    }

    public void TakeDamage(float damageAmount)
    {
        _health.ApplyChange(-damageAmount);

        if (_health <= 0) 
        {
            SurvivalManager._Instance.Die();
        }
    }

}
