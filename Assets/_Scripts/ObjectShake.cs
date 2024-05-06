using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class ObjectShake : MonoBehaviour
{
    public float _ShakeAmount;
    public bool _CanShake;
    
    private void Update()
    {
        if (_CanShake)
        {
            Shake();
        }
    }

    public void Shake()
    {
        Vector3 newPos = Random.insideUnitSphere * (Time.deltaTime * _ShakeAmount);
        var position = transform.position;
        newPos.y = position.y;
        newPos.z = position.z;

        transform.position = newPos;
    }
}
