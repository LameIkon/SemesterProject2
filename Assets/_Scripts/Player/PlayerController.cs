using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MovementController
{

    private Vector2 _moveVector;



    private void OnEnable()
    {
        InputReader.OnMoveEvent += HandleMove;
        InputReader.OnRunStartEvent += HandleRunStart;
        InputReader.OnRunCancelEvent += HandleRunCancled;
    }

    private void OnDisable()
    {
        InputReader.OnMoveEvent -= HandleMove;
        InputReader.OnRunStartEvent -= HandleRunStart;
        InputReader.OnRunCancelEvent -= HandleRunCancled;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (Vector3.Distance(transform.position, _movePoint.position) <= .05f) //makes sure you can't move if u have not reached ur new position yet.
        {
            Move(_moveVector);
        }

    }

    #region EventHandlers

    // This is the handler for the OnMoveEvent 
    private void HandleMove(Vector2 dir)
    {

        // Because the inputs are normalized we need to set the values to 1 for the move system to work properly
         //_moveSpeed = _moveSpeed -(_moveSpeed*WeatherCondition._CurrentOutsideTemperature/100);


        if (dir.x > .5f)
        {
            dir.x = 1;
        }
        else if (dir.x < -.5f)
        {
            dir.x = -1;
        }

        if (dir.y > .5f)
        {
            dir.y = 1;
        }
        else if (dir.y < -.5f)
        {
            dir.y = -1;
        }

        _moveVector = dir; // Here the direction vector is set to the _moveVector
    }


    // This handler grabs the run key and sets the appropiate speed

    void HandleRunStart() 
    {
        _moveSpeed = _speedReference.GetMaxValue();
        //_moveSpeed = _moveSpeed - (_moveSpeed * WeatherCondition._MovementSpeedDebuff / 100);
    }

    void HandleRunCancled() 
    {
        _moveSpeed = _speedReference.GetMinValue();
        //_moveSpeed = _moveSpeed - (_moveSpeed * WeatherCondition._MovementSpeedDebuff / 100);
    }


    #endregion

}
