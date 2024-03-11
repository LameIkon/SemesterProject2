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
            _range.radius = _aggroRange;
            _thinker.SetBrain(_aggroBrain);

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<IDamageable>() != null)
        {
            _range.radius = _idleRange;
            _thinker.SetBrain(_idleBrain);

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, _idleRange);

        Gizmos.DrawWireSphere(transform.position, _aggroRange);
    }

}