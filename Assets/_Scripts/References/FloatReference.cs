using System;
using UnityEngine;

[Serializable]
public class FloatReference
{
    [SerializeField] private bool UseConstant = true;
    [SerializeField] private float ConstantValue;
    [SerializeField] private FloatVariable Variable;

    public FloatReference()
    { }

    public FloatReference(float value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public float Value 
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }

    public static implicit operator float(FloatReference reference)
    {
        return reference.Value;
    }
}
