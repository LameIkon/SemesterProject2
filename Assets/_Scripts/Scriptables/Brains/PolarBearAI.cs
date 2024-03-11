using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PolarBearAI : BrainAI
{
    // [SerializeField] private RangedFloat _idleTime;
    // [SerializeField] private RangedFloat _moveTime;  
    // [SerializeField] private RangedFloat _fireTime;
    [SerializeField] private RangedFloat _waitBetweenWalk;
    [SerializeField] private int _squareSize;
    private int counter;

    private const string _stateTimeout = "stateTimeout";
    private const string _walkState = "walkState";
    private const string _target = "target";
    // private const string _state = "state";

    void OnEnable()
    {
        counter = 0;
    }

    public override void Initialize(AIThinker brain)
    {
        brain.Remember(_walkState, Directions.W);
        brain.Remember(_stateTimeout, Random.Range(_waitBetweenWalk.MinValue, _waitBetweenWalk.MaxValue));
    }


    public override void Think(AIThinker brain)
    {
        GameObject target = brain.Remember<GameObject>(_target);
        float stateTimeout = brain.Remember<float>(_stateTimeout);
        stateTimeout -= Time.deltaTime;
        brain.Remember(_stateTimeout, stateTimeout);

        

        var state = brain.Remember<Directions>(_walkState);

        var move = brain.GetComponent<MovementController>();

        if (!target) 
        {
            target = GameObject
                        .FindGameObjectsWithTag("Player")
                        .OrderBy(go => Vector3.Distance(go.transform.position, brain.transform.position))
                        .FirstOrDefault(go => go != brain.gameObject);

            brain.Remember<GameObject>(_target);
        }

        if (stateTimeout < 0)
        {
            SetTimeout(brain);
            Debug.Log(counter);

            switch (GiveDirection())
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

            }

        }

    }


    private Directions GiveDirection()
    {

        counter++;

        if (counter <= 1 * _squareSize)
        {
            return Directions.N;
        }
        else if (counter <= 2 * _squareSize)
        {
            return Directions.E;
        }
        else if (counter <= 3 * _squareSize)
        {
            return Directions.S;
        }
        else
        {
            if (counter >= 4 * _squareSize)
            {
                counter = 0;
            }
            return Directions.W;
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
