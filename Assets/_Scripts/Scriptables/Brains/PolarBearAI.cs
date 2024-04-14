using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Brain/PolarBear")]
public class PolarBearAI : BrainAI
{
    // [SerializeField] private RangedFloat _idleTime;
    // [SerializeField] private RangedFloat _moveTime;  
    // [SerializeField] private RangedFloat _fireTime;
    [SerializeField] private RangedFloat _waitBetweenWalk;
    [SerializeField] private RangedFloat _waitBetweenAttack;
    [SerializeField] private int _aggroRange;
    [SerializeField] private float _damage;
    [SerializeField] private float _attackRange;



    private const string _playerTag = "Player";

    private const string _stateTimeout = "stateTimeout";
    private const string _walkState = "walkState";
    private const string _target = "target";
    // private const string _state = "state";

    public override void Initialize(AIThinker brain)
    {
        brain.Remember(_walkState, Directions.W);
        brain.Remember(_stateTimeout, Random.Range(_waitBetweenWalk.MinValue, _waitBetweenWalk.MaxValue));
    }

    private MovementController _move;

    public override void Think(AIThinker brain)
    {
        UnityEngine.GameObject target = brain.Remember<UnityEngine.GameObject>(_target);
        float stateTimeout = brain.Remember<float>(_stateTimeout);
        stateTimeout -= Time.deltaTime;
        brain.Remember(_stateTimeout, stateTimeout);


        var state = brain.Remember<Directions>(_walkState);

        if (_move == null)
        {
            _move = brain.GetComponent<MovementController>();
        }

        if (!target)
        {
            //target = GameObject
            //            .FindGameObjectsWithTag("Player")
            //            .OrderBy(go => Vector3.Distance(go.transform.position, brain.transform.position))
            //            .FirstOrDefault(go => go != brain.gameObject);

            target = UnityEngine.GameObject.FindGameObjectWithTag(_playerTag);

            brain.Remember(_target, target);
        }

        Vector3 _aggroVector = new Vector3(_aggroRange, _aggroRange, 0);

        if (stateTimeout < 0)
        {
            

            Vector3 targetPosition = target.transform.position;
            Vector3 ownPosition = brain.transform.position;
            Vector3 vectorBetween = targetPosition - ownPosition;
            Vector3 unitVectorBetween = (vectorBetween).normalized;


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
            SetTimeoutAttack(brain);
            Collider2D[] hits = Physics2D.OverlapCircleAll(ownPosition, 1.8f*_attackRange);


            foreach (var hit in hits)
            {
                if (hit.GetComponent<AIThinker>() != null) 
                {
                    continue;
                }

                hit.GetComponent<IDamageable>()?.TakeDamage(_damage);

            }
        }
        else
            {
                SetTimeoutWalk(brain);
                
                Walk(GiveDirectionTowardsPlayer(unitVectorBetween), brain);
                
            }


            //if (vectorBetween.x > _aggroVector.x || vectorBetween.x < -_aggroVector.x && vectorBetween.y > _aggroVector.y || vectorBetween.y < -_aggroVector.y)
            //{
            //    WalkRandom();
            //}
            //else
            //{
            //}
        }

    }
    

    private void WalkRandom()
    {
        _move.Move(new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), 0));
    }

    private void Walk(Directions direction, AIThinker brain)
    {
        switch (direction)
        {
            case Directions.N:

                SetTimeoutWalk(brain);

                _move.Move(Vector3.up);
                break;

            case Directions.S:

                SetTimeoutWalk(brain);

                _move.Move(Vector3.down);
                break;

            case Directions.E:
                SetTimeoutWalk(brain);

                _move.Move(Vector3.right);
                break;

            case Directions.W:
                SetTimeoutWalk(brain);

                _move.Move(Vector3.left);
                break;

            case Directions.NE:
                SetTimeoutWalk(brain);

                _move.Move(Vector3.up + Vector3.right);
                break;

            case Directions.NW:
                SetTimeoutWalk(brain);

                _move.Move(Vector3.up + Vector3.left);
                break;

            case Directions.SE:
                SetTimeoutWalk(brain);

                _move.Move(Vector3.down + Vector3.right);
                break;

            case Directions.SW:
                SetTimeoutWalk(brain);

                _move.Move(Vector3.down + Vector3.left);
                break;

            default:

                break;

        }
    }


    private Directions GiveDirectionTowardsPlayer(Vector3 dir)
    {

        int i = Random.Range(0, 2);

        if (dir == Vector3.up)
        {
            return Directions.N;
        }
        else if (dir == Vector3.right)
        {
            return Directions.E;
        }
        else if (dir == Vector3.down)
        {
            return Directions.S;
        }
        else if (dir == Vector3.left)
        {
            return Directions.W;
        }
        else if (dir == new Vector3())
        {
            return Directions.NW;
        }
        else if (dir.y > 0 && dir.x > 0)
        {
            if (i == 0)
            {
                return Directions.N;
            }
            else
            {
                return Directions.NE;
            }
        }
        else if (dir.y > 0 && dir.x < 0)
        {
            if (i == 0)
            {
                return Directions.N;
            }
            else
            {
                return Directions.NW;
            }
        }
        else if (dir.y < 0 && dir.x > 0)
        {
            if (i == 0)
            {
                return Directions.S;
            }
            else
            {
                return Directions.SE;
            }
        }
        else if (dir.y < 0 && dir.x < 0)
        {
            if (i == 0)
            {
                return Directions.S;
            }
            else
            {
                return Directions.SW;
            }
        }
        else
        {
            return Directions.None;
        }


    }

    private void SetTimeoutWalk(AIThinker brain)
    {
        brain.Remember(_stateTimeout, Random.Range(_waitBetweenWalk.MinValue, _waitBetweenWalk.MaxValue));
    }
    private void SetTimeoutAttack(AIThinker brain)
    {
        brain.Remember(_stateTimeout, Random.Range(_waitBetweenAttack.MinValue, _waitBetweenAttack.MaxValue));
    }


    //Look at this, this is coursing bugs

    public bool AttackRange(Vector3 v1, Vector3 v2, float range)
    {
        if (Vector3.Distance(v1, v2) < range)
        {
            return true;
        }
        else
            return false; 
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

    /*
                switch (GiveDirection(dir))
        {
            case Directions.N:

                SetTimeout(brain);

                _move.Move(Vector3.up);
                break;

            case Directions.S:

                SetTimeout(brain);

                _move.Move(Vector3.down);
                break;

            case Directions.E:
                SetTimeout(brain);

                _move.Move(Vector3.right);
                break;

            case Directions.W:
                SetTimeout(brain);

                _move.Move(Vector3.left);
                break;

            case Directions.NE:
                SetTimeout(brain);

                _move.Move(Vector3.up + Vector3.right);
                break;

            case Directions.NW:
                SetTimeout(brain);

                _move.Move(Vector3.up + Vector3.left);
                break;

            case Directions.SE:
                SetTimeout(brain);

                _move.Move(Vector3.down + Vector3.right);
                break;

            case Directions.SW:
                SetTimeout(brain);

                _move.Move(Vector3.down + Vector3.left);
                break;

            default:

                break;

        }
     */


}
