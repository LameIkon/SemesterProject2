using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class RandomWalkAI : BrainAI
{
    // [SerializeField] private RangedFloat _idleTime;
    // [SerializeField] private RangedFloat _moveTime;  
    // [SerializeField] private RangedFloat _fireTime;
    [SerializeField] private RangedFloat _waitBetweenWalk;

    private const string _stateTimeout = "stateTimeout";
    private const string _walkState = "walkState";
    // private const string _state = "state";



    public override void Initialize(AIThinker brain)
    {
        brain.Remember(_walkState, Directions.W);
        brain.Remember(_stateTimeout, Random.Range(_waitBetweenWalk.MinValue, _waitBetweenWalk.MaxValue));
    }


    public override void Think(AIThinker brain)
    {
        float stateTimeout = brain.Remember<float>(_stateTimeout);
        stateTimeout -= Time.deltaTime;
        brain.Remember(_stateTimeout, stateTimeout);


        var state = brain.Remember<Directions>(_walkState);

        var move = brain.GetComponent<MovementController>();

        if (stateTimeout < 0)
        {
            SetTimeout(brain);
            move.Move(new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), 0));

        }

    }

    private void SetTimeout(AIThinker brain) 
    {
        brain.Remember(_stateTimeout, Random.Range(_waitBetweenWalk.MinValue, _waitBetweenWalk.MaxValue));
    }


    /*
        switch ((Directions)Random.Range(0, 10))
            {
                case Directions.N:

                    SetTimeout(brain);

                    move.Move(Vector3.up);
                    break;

                case Directions.S:

                    SetTimeout(brain);

                    move.Move(Vector3.down);
                    break;

                case Directions.E:
                    SetTimeout(brain);

                    move.Move(Vector3.right);
                    break;

                case Directions.W:
                    SetTimeout(brain);

                    move.Move(Vector3.left);
                    break;

                case Directions.NE:
                    SetTimeout(brain);

                    move.Move(Vector3.up + Vector3.right);
                    break;

                case Directions.NW:
                    SetTimeout(brain);

                    move.Move(Vector3.up + Vector3.left);
                    break;

                case Directions.SE:
                    SetTimeout(brain);

                    move.Move(Vector3.down + Vector3.right);
                    break;

                case Directions.SW:
                    SetTimeout(brain);

                    move.Move(Vector3.down + Vector3.left);
                    break;


                default:
                    SetTimeout(brain);

                    Debug.Log("Default");
                    move.Move(new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), 0));
                    break;
            }
     */


}
