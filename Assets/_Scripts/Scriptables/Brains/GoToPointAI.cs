using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Brain/FollowObject")]
public class GoToPointAI : BrainAI
{
    // [SerializeField] private RangedFloat _idleTime;
    // [SerializeField] private RangedFloat _moveTime;  
    // [SerializeField] private RangedFloat _fireTime;
    [SerializeField] private RangedFloat _waitBetweenWalk;
    [SerializeField] private bool _shouldWaitBetweenOnPointReached; // Currently just been set in inspector
    [SerializeField] private RangedFloat _waitBetweenReachingDestination;
    [SerializeField] private float _rangeToPlayerStop;
    


    [SerializeField] private string _destination = "Destination";
    public static bool _ReachedDestination;

    private const string _stateTimeout = "stateTimeout";
    private const string _walkState = "walkState";
    private const string _target = "target";
    // private const string _state = "state";

    public override void Initialize(AIThinker brain)
    {
        brain.Remember(_walkState, Directions.W);
        brain.Remember(_stateTimeout, Random.Range(_waitBetweenWalk.MinValue, _waitBetweenWalk.MaxValue));
        brain.Remember(_stateTimeout, Random.Range(_waitBetweenReachingDestination.MinValue, _waitBetweenReachingDestination.MaxValue));
    }

    private MovementController _move;

    public override void Think(AIThinker brain)
    {
        GameObject target = brain.Remember<GameObject>(_target);
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

            target = GameObject.Find(_destination);

            brain.Remember(_target, target);
        }


        if (stateTimeout < 0)
        {

            Vector3 targetPosition = target.transform.position;
            Vector3 ownPosition = brain.transform.position;
            Vector3 vectorBetween = targetPosition - ownPosition;
            Vector3 unitVectorBetween = (vectorBetween).normalized;

            if (RangeHolder(vectorBetween, _rangeToPlayerStop * unitVectorBetween, _rangeToPlayerStop))
            {

                Debug.Log("destination");
                SetTimeoutWalk(brain);

                if (_ReachedDestination == false)
                {
                    _ReachedDestination = true;
                }

            }
            else
            {
                if (!_shouldWaitBetweenOnPointReached)
                {
                    SetTimeoutWalk(brain);
                    if (_ReachedDestination == true)
                    {
                        Debug.Log("destination");
                        _ReachedDestination = false;
                    }
                    Walk(GiveDirectionTowardsPlayer(unitVectorBetween), brain);
                }
                else
                {

                    if (_ReachedDestination == true)
                    {
                        Debug.Log("walking");
                        SetTimeoutReachedDestination(brain);
                        _ReachedDestination = false;                     
                    }
                    Walk(GiveDirectionTowardsPlayer(unitVectorBetween), brain);
                }
            }
            
        }

    }

    public bool RangeHolder(Vector3 v1, Vector3 v2, float range)
    {
        if (Vector3.Distance(v1, v2) < range)
        {
            return true;
        }
        else
            return false;
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

        // Go the direction the target is as accurate as possible
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

    private void SetTimeoutWalk(AIThinker brain)
    {
        brain.Remember(_stateTimeout, Random.Range(_waitBetweenWalk.MinValue, _waitBetweenWalk.MaxValue));
    }

    private void SetTimeoutReachedDestination(AIThinker brain)
    {
        brain.Remember(_stateTimeout, Random.Range(_waitBetweenReachingDestination.MinValue, _waitBetweenReachingDestination.MaxValue));
    }


}
