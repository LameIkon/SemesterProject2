using System;
using UnityEngine;

[Serializable]
public class ColorReference
{
    public bool UseConstant = true;
    public Color ConstantColor;
    public ColorVariable Variable;

    #region Constructers

    public ColorReference(ColorVariable variable) 
    {
        UseConstant = false;
        Variable = variable;
    }

    public ColorReference(Color color) 
    {
        UseConstant= true;
        ConstantColor = color;
    }

    #endregion


    public Color Color
    {
        get { return UseConstant ? ConstantColor : Variable.GetColor(); }
    }

    public static implicit operator Color(ColorReference reference) 
    {
        return reference.Color;
    }

}
