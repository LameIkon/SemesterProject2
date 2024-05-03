using System;
using UnityEngine;

[Serializable]
public class StringReference
{

    public bool UseConstant = true;
    public string ConstantString;
    public StringVariable Variable;

    #region Constructers

    public StringReference(StringVariable variable) 
    {
        UseConstant = false;
        Variable = variable;
    }

    public StringReference(string _string) 
    {
        UseConstant= true;
        ConstantString = _string;
    }

    #endregion

    public char[] ToCharArray()
    {
        return UseConstant ? ConstantString.ToCharArray() : Variable.ToCharArray();
    }

    public string String
    {
        get { return UseConstant ? ConstantString : Variable.GetString(); }
    }

    public static implicit operator string(StringReference reference) 
    {
        return reference.String;
    }

}
