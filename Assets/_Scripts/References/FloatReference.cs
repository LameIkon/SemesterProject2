using System;
using UnityEngine;

[Serializable]
public class FloatReference
{
     public bool UseConstant = true;
     public float ConstantValue;
     public FloatVariable Variable;

    public FloatReference(FloatVariable variable)
    {
        UseConstant = false; 
        Variable = variable;
    }

    public FloatReference(float value)
    {
        UseConstant = true;
        ConstantValue = value;
    }

    public float Value 
    {
        get { return UseConstant ? ConstantValue : Variable.GetValue(); }
    }

    public static implicit operator float(FloatReference reference)
    {
        return reference.Value;
    }
}
