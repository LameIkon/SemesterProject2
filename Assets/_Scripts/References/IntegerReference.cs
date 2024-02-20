using System;

[Serializable]
public class IntegerReference 
{
    public bool UseConstant = true;
    public int ConstantValue;
    public IntegerVariable Variable;

    public int Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
    }
}
