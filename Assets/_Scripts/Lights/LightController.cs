using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class LightController : MonoBehaviour
{
    [SerializeField] ColorReference _lightColor;
    [SerializeField] RangedFloat _lightInnerRadius;
    [SerializeField] RangedFloat _lightOuterRadius;
    [SerializeField] FloatReference _lightSize;
    [SerializeField] float _lightTime;
    [SerializeField] float _lerpAmount;
 
    private Light2D _light;
    private bool _isMax = false;
    private float _percentage;
    private float _elapsedTime;

    private void Awake()
    {
        _light = GetComponent<Light2D>();
        _light.color = _lightColor;
    }

    private void Start() 
    {
        StartCoroutine(LightFlicker());
    }


    IEnumerator LightFlicker() 
    {
        float intensity = _lightInnerRadius.MinValue;

        while (true)
        {
            _percentage = _elapsedTime / _lerpAmount;
            
            _light.pointLightInnerRadius = Mathf.Lerp(_lightInnerRadius.MinValue, _lightInnerRadius.MaxValue, _percentage);
            _light.pointLightOuterRadius = Mathf.Lerp(_lightOuterRadius.MinValue, _lightOuterRadius.MaxValue, _percentage);

            yield return new WaitForSeconds(_lightTime);

            if (_isMax)
            {
                _elapsedTime -= Time.deltaTime;

                if (_percentage <= 0f) 
                {
                    _isMax = false;
                }
            }
            else 
            {
                _elapsedTime += Time.deltaTime;

                if (_percentage >= 1f) 
                {
                    _isMax = true;
                }
            }

        }
    }

}
