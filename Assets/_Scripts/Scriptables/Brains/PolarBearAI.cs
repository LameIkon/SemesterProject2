using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Brain/PolarBear")]
public class PolarBearAI : BrainAI
{
    [SerializeField] private RangedFloat _idleTime;
    //[SerializeField] private RangedFloat _moveTime;  
    // [SerializeField] private RangedFloat _fireTime;
    [SerializeField] private RangedFloat _waitBetweenWalk;
    [SerializeField] private RangedFloat _waitBetweenRunning;
    [SerializeField] private RangedFloat _waitBetweenAttack;
    [SerializeField] private int _aggroRange;
    [SerializeField] private float _damage;
    [SerializeField] private float _attackRange;

    private PolarBearController _polarBearController;

    private const string _playerTag = "Player";
    private const string _stateTimeout = "stateTimeout";
    private const string _walkState = "walkState";
    private const string _target = "target";
    // private const string _state = "state";

    public void SetPolarBearController(PolarBearController polarBearController)
    {
        _polarBearController = polarBearController;
    }

    public override void Initialize(AIThinker brain)
    {
        brain.Remember(_walkState, Directions.W);
        brain.Remember(_stateTimeout, Random.Range(_waitBetweenWalk.MinValue, _waitBetweenWalk.MaxValue));
    }

    private PolarBearController _move;

    public override void Think(AIThinker brain)
    {
        GameObject target = brain.Remember<GameObject>(_target);
        float stateTimeout = brain.Remember<float>(_stateTimeout);
        stateTimeout -= Time.deltaTime;
        brain.Remember(_stateTimeout, stateTimeout);

        //var state = brain.Remember<Directions>(_walkState);
        if (_move == null)
        {
            _move = brain.GetComponent<PolarBearController>();
        }

        if (!target)
        {
            target = GameObject.FindGameObjectWithTag(_playerTag);

            brain.Remember(_target, target);
        }

        if (stateTimeout < 0)
        {

            Vector3 targetPosition = target.transform.position; // Find the targets location
            Vector3 ownPosition = brain.transform.position; // Own Location
            Vector3 vectorBetween = targetPosition - ownPosition; // The distance between
            Vector3 unitVectorBetween = (vectorBetween).normalized; // Normalized

           

            //if (AttackRange(vectorBetween, _attackRange * unitVectorBetween, _attackRange))
            //{
            //    SetTimeoutAttack(brain);
            //    Debug.Log((ownPosition + _attackRange * unitVectorBetween));
            //    Collider2D[] hits = Physics2D.OverlapBoxAll((ownPosition + _attackRange * unitVectorBetween), _attackRange * Vector2.one, 0f);


            //    foreach (var hit in hits)
            //    {
            //        hit.GetComponent<IDamageable>()?.TakeDamage(_damage);

            //    }
            //}

            if (AttackRange(vectorBetween, _attackRange * unitVectorBetween, _attackRange))
            {
                
                //Debug.Log("attack");
                //Collider2D[] hits = Physics2D.OverlapCircleAll(ownPosition, 1.8f * _attackRange);
                _polarBearController.Attack(ownPosition, _attackRange);


                //foreach (var hit in hits)
                //{
                //    if (hit.gameObject.name == "Player")
                //    {
                //        Debug.Log("Hit!");
                //    }
                //    else
                //    {
                //        Debug.Log("missed");
                //    }

                //    //if (hit.GetComponent<AIThinker>() != null)
                //    //{
                //    //    continue;
                //    //}

                //    //hit.GetComponent<IDamageable>()?.TakeDamage(_damage);

                //}
                SetTimeoutAttack(brain);
            }

            else if (AggroRange(vectorBetween, _aggroRange * unitVectorBetween, _aggroRange))
            {
                _polarBearController.RunSpeed();
                Walk(GiveDirectionTowardsPlayer(unitVectorBetween));
                SetTimeoutAggro(brain);
            }
            else
            {
                _polarBearController.WalkSpeed();
                if (Random.value < 0.05f) // 5% chance of idle
                {
                    SetTimeIdle(brain);
                }
                else
                {
                    WalkRandom();
                    SetTimeoutWalk(brain);
                }
            }
        }    
    }

    private IEnumerator Attack(Vector3 ownPosition, Vector3 unitVectorBetween)
    {
        yield return null;
    }
    

    private void WalkRandom()
    {
        _move.Move(new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), 0));
    }

    private void Walk(Directions direction)
    {
        switch (direction)
        {
            case Directions.N:
                _move.Move(Vector3.up);
                break;

            case Directions.S:
                _move.Move(Vector3.down);
                break;

            case Directions.E:
                _move.Move(Vector3.right);
                break;

            case Directions.W:
                _move.Move(Vector3.left);
                break;

            case Directions.NE:
                _move.Move(Vector3.up + Vector3.right);
                break;

            case Directions.NW:
                _move.Move(Vector3.up + Vector3.left);
                break;

            case Directions.SE:
                _move.Move(Vector3.down + Vector3.right);
                break;

            case Directions.SW:
                _move.Move(Vector3.down + Vector3.left);
                break;

            default:
                break;

        }
    }

    private Directions GiveDirectionTowardsPlayer(Vector3 dir)
    {
        if (dir == Vector3.up) return Directions.N;
        if (dir == Vector3.right) return Directions.E;
        if (dir == Vector3.down) return Directions.S;
        if (dir == Vector3.left) return Directions.W;


        bool isMovingUp = dir.y > 0;
        bool isMovingRight = dir.x > 0;
        if (isMovingUp)
        {
            if (isMovingRight) return (Random.value > 0.8f) ? Directions.N : Directions.NE;
            else return (Random.value > 0.8f) ? Directions.N : Directions.NW;
        }
        else
        {
            if (isMovingRight) return (Random.value > 0.8f) ? Directions.S : Directions.SE;
            else return (Random.value > 0.8f) ? Directions.S : Directions.SW;
        }
    }
    #region TimeOut

    private void SetTimeoutWalk(AIThinker brain)
    {
        brain.Remember(_stateTimeout, Random.Range(_waitBetweenWalk.MinValue, _waitBetweenWalk.MaxValue));
    }
    private void SetTimeoutAttack(AIThinker brain)
    {
        brain.Remember(_stateTimeout, Random.Range(_waitBetweenAttack.MinValue, _waitBetweenAttack.MaxValue));
    }

    private void SetTimeoutAggro(AIThinker brain)
    {
        brain.Remember(_stateTimeout, Random.Range(_waitBetweenRunning.MinValue, _waitBetweenRunning.MaxValue));
    }

    private void SetTimeIdle(AIThinker brain)
    {
        brain.Remember(_stateTimeout, Random.Range(_idleTime.MinValue, _idleTime.MaxValue));
    }
    #endregion

    #region Bool Returns
    public bool AttackRange(Vector3 v1, Vector3 v2, float range)
    {
        if (Vector3.Distance(v1, v2) < range)
        {
            return true;
        }
        else
        {
            return false;
        }           
    }

    public bool AggroRange(Vector3 itself, Vector3 target, float range)
    {
        if (Vector3.Distance(itself, target) < range)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public bool V3GreaterThanEqual(Vector3 v1, Vector3 v2) 
    {
        if (v1.x >= v2.x && v1.y >= v2.y && v1.z >= v2.z)
        {
            return true;
        }
        else
            return false;
    }

    #endregion
}
