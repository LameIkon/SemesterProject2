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



    public override void Initialize(AIThinker ai)
    {
        ai.Remember(_walkState, Directions.W);
        ai.Remember(_stateTimeout, Random.Range(_waitBetweenWalk.MinValue, _waitBetweenWalk.MaxValue));
    }


    public override void Think(AIThinker ai)
    {
        float stateTimeout = ai.Remember<float>(_stateTimeout);
        stateTimeout -= Time.deltaTime;
        ai.Remember(_stateTimeout, stateTimeout);


        var state = ai.Remember<Directions>(_walkState);

        var move = ai.GetComponent<PlayerController>();

        if (stateTimeout < 0) 
        {
            SetTimeout(ai);

            Debug.Log("Default");
            move.Move(new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), 0));
        }


    }

    private void SetTimeout(AIThinker ai) 
    {
        ai.Remember(_stateTimeout, Random.Range(_waitBetweenWalk.MinValue, _waitBetweenWalk.MaxValue));
    }


    /*
     switch ((Directions)Random.Range(0,10)) 
            {
                case Directions.N:

                    SetTimeout(ai);

                    move.Move(Vector3.up);
                    break;

                case Directions.S:

                    SetTimeout(ai);

                    move.Move(Vector3.down);
                    break;

                case Directions.E:
                    SetTimeout(ai);

                    move.Move(Vector3.right);
                    break;

                case Directions.W:
                    SetTimeout(ai);

                    move.Move(Vector3.left);
                    break;

                case Directions.NE:
                    SetTimeout(ai);

                    move.Move(Vector3.up + Vector3.right);
                    break;

                case Directions.NW:
                    SetTimeout(ai);

                    move.Move(Vector3.up + Vector3.left);
                    break;

                case Directions.SE:
                    SetTimeout(ai);

                    move.Move(Vector3.down + Vector3.right);
                    break;

                case Directions.SW:
                    SetTimeout(ai);

                    move.Move(Vector3.down + Vector3.left);
                    break;


                default:
                    SetTimeout(ai);

                    Debug.Log("Default");
                    move.Move(new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), 0));
                    break;
     */


}
