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
        //Destroy(gameObject);        
    }

    public void TakeDamage(float damageAmount)
    {
        _health.ApplyChange(-damageAmount);

        if (_health.GetValue() <= 0) 
        {
            Die();
        }
    }

    public float GetHealthValue()
    { 
        return _health.GetValue(); 
    }
}
