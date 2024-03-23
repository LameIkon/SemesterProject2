using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class LightController : MonoBehaviour
{
    [SerializeField] ColorReference _lightColor;
    [SerializeField] RangedFloat _lightIntensity;
    [SerializeField] FloatReference _lightSize;
    [SerializeField] float _lightTime;
 
    private Light2D _light;
    private bool _isMax = false;
    

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
        float intensity = _lightIntensity.MinValue;

        while (true)
        {
            //_light.intensity = intensity;
            _light.pointLightInnerRadius = intensity;

            yield return new WaitForSeconds(_lightTime);

            if (_isMax)
            {
                intensity = _lightIntensity.MinValue;
                _isMax = false;
            }
            else 
            {
                intensity = _lightIntensity.MaxValue;
                _isMax = true;
            }

        }
    }

}
