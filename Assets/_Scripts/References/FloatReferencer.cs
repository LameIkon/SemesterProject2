using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Referencer/Float")]
public class FloatReferencer : ScriptableObject
{

    [SerializeField] private FloatVariable _maniputatedValue;
    [SerializeField] private FloatReference _maxValue;
    [SerializeField] private FloatReference _minValue;


    public void ApplyChange(float newValue) 
    {
        _maniputatedValue.ApplyChange(newValue);
        HandleValueManip();
    }

    public void SetValue(float newValue) 
    {
        _maniputatedValue.SetValue(newValue);
        HandleValueManip();
    }

    private void HandleValueManip() 
    {

        if (_maniputatedValue.GetValue() > _maxValue) 
        {
            _maniputatedValue.SetValue(_maxValue);
        }

        if (_maniputatedValue.GetValue() < _minValue) 
        {
            _maniputatedValue.SetValue(_minValue);
        }
    
    }

    public float GetMaxValue() 
    {
        return _maxValue;
    }

    public float GetMinValue() 
    {
        return _minValue; 
    }

    public float GetValue() 
    {
        return _maniputatedValue.GetValue();
    }

}
