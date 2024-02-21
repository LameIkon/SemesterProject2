using System;
using UnityEngine;

[Serializable]
public class FloatReference
{
    [SerializeField] private bool _useConstant = true;
    [SerializeField] private float _constantValue;
    [SerializeField] private FloatVariable _variable;

    public FloatReference(FloatVariable variable)
    {
        _useConstant = false; 
        _variable = variable;
    }

    public FloatReference(float value)
    {
        _useConstant = true;
        _constantValue = value;
    }

    public float Value 
    {
        get { return _useConstant ? _constantValue : _variable.GetValue(); }
    }

    public static implicit operator float(FloatReference reference)
    {
        return reference.Value;
    }
}
