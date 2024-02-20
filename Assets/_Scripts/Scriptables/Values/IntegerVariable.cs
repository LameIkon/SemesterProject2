using UnityEngine;

[CreateAssetMenu(fileName = "New Integer Variable", menuName = "Variable/Integer")]
public class IntegerVariable : VariableBase
{
    public int Value;

    public void SetValue(int value)
    {
        Value = value;
    }

    public void SetValue(IntegerVariable value)
    {
        Value = value.Value;
    }

    public void ApplyChange(int amount)
    {
        Value += amount;
    }

    public void ApplyChange(IntegerVariable amount)
    {
        Value += amount.Value;
    }
}
