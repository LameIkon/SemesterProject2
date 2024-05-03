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
                SetTimeoutWalk(brain);
                if (_ReachedDestination == false)
                {
                    _ReachedDestination = true;
                }
                
            }
            else
            {
                SetTimeoutWalk(brain);
                if (_ReachedDestination == true)
                {
                    _ReachedDestination = false;
                }

                Walk(GiveDirectionTowardsPlayer(unitVectorBetween), brain);
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


}
