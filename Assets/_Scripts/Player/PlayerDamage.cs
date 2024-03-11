using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour, IDamageable
{

    [SerializeField] private FloatVariable _healthPoints;

    public void Die()
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(float damageAmount)
    {
        _healthPoints.ApplyChange(-damageAmount);
    }
}
