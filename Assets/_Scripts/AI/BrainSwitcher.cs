using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AIThinker))]
public class BrainSwitcher : MonoBehaviour
{

    private AIThinker _thinker;
    [SerializeField] private CircleCollider2D _range;

    [SerializeField] private float _idleRange;
    [SerializeField] private float _aggroRange;

    [SerializeField] private BrainAI _idleBrain;
    [SerializeField] private BrainAI _aggroBrain;

    void Awake() 
    {
        _thinker = GetComponent<AIThinker>();
        _range.isTrigger = true;
        _range.radius = _idleRange;
    }



    //void Reset() 
    //{
    //    _thinker.AddComponent<AIThinker>();
    //    _range.AddComponent<CircleCollider2D>();
    //    _range.isTrigger = true;
    //    _range.radius = 5f;

    //}

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.GetComponent<IDamageable>() != null) 
        {
            Debug.Log("Brain Switch");
            SetRadius(_aggroRange);
            _thinker.SetBrain(_aggroBrain);

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<IDamageable>() != null)
        {
            SetRadius(_idleRange);
            _thinker.SetBrain(_idleBrain);

        }
    }


    private void FixedUpdate()
    {
        Debug.Log(_range.radius);

        Collider2D[] enemys = Physics2D.OverlapCircleAll(transform.position, _range.radius);

        foreach (var enemy in enemys)
        {
            if (enemy.GetComponent<IDamageable>() != null)
            {
                SetRadius(_aggroRange);
            }
            else
                SetRadius(_idleRange);
        }


    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _idleRange);

        Gizmos.DrawWireSphere(transform.position, _aggroRange);
    }


    private void SetRadius(float radius) 
    {
        _range.radius = radius;
    }
}