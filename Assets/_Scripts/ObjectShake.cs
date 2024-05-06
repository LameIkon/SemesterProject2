using System.Collections;
using System.Threading;
using Cinemachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObjectShake : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Vector2 _originalPosition;
    private float _speed = 1.0f;
    private float _amount = 10.0f;
    
    // public float _ShakeAmount;
    // public bool _CanShake;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originalPosition = _rectTransform.anchoredPosition;
    }

    private void Update()
    {
        Shake(_rectTransform, _originalPosition, _speed, _amount);
    }

    private void Shake(RectTransform rectTransform, Vector2 originalPosition, float speed, float amount)
    { 
        float offsetX = Mathf.Sin(Time.time * speed) * amount;
        rectTransform.anchoredPosition = originalPosition + new Vector2(offsetX, 0f);
    }

    private void Shaker()
    {
        // Vector3 newPos = Random.insideUnitSphere * (Time.deltaTime * _ShakeAmount);
        // newPos.y = _rectTransform.transform.position.y;
        // newPos.z = _rectTransform.transform.position.x;
        //
        // _rectTransform.transform.position = newPos;
    }
    
}
