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
    [SerializeField] private bool _forceAggro; // Set this to true if you want to always be aggroed on player

    private PolarBearController _polarBearController;

    private const string _playerTag = "Player";
    private const string _stateTimeout = "stateTimeout";
    private const string _walkState = "walkState";
    private const string _target = "target";
    private const string _hasAggro = "hasAggro";
    private const string _idle = "idleNotMoving";

    // private const string _state = "state";

    //private bool _hasAggro;

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
        bool hasAggro = brain.Remember<bool>(_hasAggro);
        bool isNotMoving = brain.Remember<bool>(_idle);

        if (isNotMoving) // if standing still and you move close to it
        {
            Vector3 targetPosition = target.transform.position; // Find the targets location
            Vector3 ownPosition = brain.transform.position; // Own Location
            Vector3 vectorBetween = targetPosition - ownPosition; // The distance between
            Vector3 unitVectorBetween = (vectorBetween).normalized; // Normalized

            if (AggroRange(vectorBetween, _aggroRange * unitVectorBetween, _aggroRange)) // if getting into its zone, cancel its idle time
            {
                stateTimeout = 0f;
            }

        }

        brain.Remember(_stateTimeout, stateTimeout);

        if (_move == null)
        {
            _move = brain.GetComponent<PolarBearController>();
        }

        if (!target)
        {
            target = GameObject.FindGameObjectWithTag(_playerTag);

            brain.Remember(_target, target);
        }

        if (stateTimeout <= 0)
        {

            Vector3 targetPosition = target.transform.position; // Find the targets location
            Vector3 ownPosition = brain.transform.position; // Own Location
            Vector3 vectorBetween = targetPosition - ownPosition; // The distance between
            Vector3 unitVectorBetween = (vectorBetween).normalized; // Normalized

            // if inside its attack range stop up and attack
            if (AttackRange(vectorBetween, _attackRange * unitVectorBetween, _attackRange))
            {
                // Calls the controller for the attack animations
                _polarBearController.Attack(ownPosition, _attackRange, _damage);
                SetTimeoutAttack(brain);
            }

            else if (hasAggro || _forceAggro) // Even if you escape its zone it will still have aggro on you for a moment
            {
                _polarBearController.RunSpeed();
                Walk(GiveDirectionTowardsPlayer(unitVectorBetween));

                if (Random.value < 0.025f) // 2.5% chance for losing aggro every frame
                {
                    SetTimeoutAggro(brain);
                }
                else
                {
                     SetTimeOutRefreshAggro(brain);
                }
            }


            

            // Start running
            else if (AggroRange(vectorBetween, _aggroRange * unitVectorBetween, _aggroRange))
            {
                _polarBearController.RunSpeed();
                Walk(GiveDirectionTowardsPlayer(unitVectorBetween));
                SetTimeoutAggro(brain);
            }
            else // Else just walking around at random or standing still
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

   
    private void WalkRandom() // Random
    {
        _move.Move(new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), 0));
    }

    private void Walk(Directions direction) // Get the direction the moving point is at
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

    private Directions GiveDirectionTowardsPlayer(Vector3 dir) // Try to get its smooth location towards the player
    {
        // Go the direction the player is as accurate as possible
        if (dir == Vector3.up) return Directions.N;
        if (dir == Vector3.right) return Directions.E;
        if (dir == Vector3.down) return Directions.S;
        if (dir == Vector3.left) return Directions.W;


        bool isMovingUp = dir.y > 0;
        bool isMovingRight = dir.x > 0;
        if (isMovingUp) // Introduce a little randomness/smoothness to its pathfinding
        {
            // 80% chance to running straight instead of diagonal
            if (isMovingRight) return (Random.value > 0.8f) ? Directions.N : Directions.NE; // go up 
            else return (Random.value > 0.8f) ? Directions.N : Directions.NW;
        }
        else
        {
            // 80% chance to running straight instead of diagonal
            if (isMovingRight) return (Random.value > 0.8f) ? Directions.S : Directions.SE; // go down
            else return (Random.value > 0.8f) ? Directions.S : Directions.SW;
        }
    }
    #region TimeOut

    private void SetTimeoutWalk(AIThinker brain)
    {
        brain.Remember(_stateTimeout, Random.Range(_waitBetweenWalk.MinValue, _waitBetweenWalk.MaxValue));
        brain.Remember(_hasAggro, false);
        brain.Remember(_idle, false);
    }
    private void SetTimeoutAttack(AIThinker brain)
    {
        brain.Remember(_stateTimeout, Random.Range(_waitBetweenAttack.MinValue, _waitBetweenAttack.MaxValue));
        brain.Remember(_hasAggro, true);
        brain.Remember(_idle, false);
    }

    private void SetTimeoutAggro(AIThinker brain)
    {
        brain.Remember(_stateTimeout, Random.Range(_waitBetweenRunning.MinValue, _waitBetweenRunning.MaxValue));
        brain.Remember(_hasAggro, false);
        brain.Remember(_idle, false);

    }

    private void SetTimeOutRefreshAggro(AIThinker brain)
    {
        brain.Remember(_stateTimeout, Random.Range(_waitBetweenRunning.MinValue, _waitBetweenRunning.MaxValue));
        brain.Remember(_hasAggro, true);
    }

    private void SetTimeIdle(AIThinker brain)
    {
        brain.Remember(_stateTimeout, Random.Range(_idleTime.MinValue, _idleTime.MaxValue));
        brain.Remember(_hasAggro, false);
        brain.Remember(_idle, true);
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
