using UnityEngine;

[CreateAssetMenu(fileName = "New Float Variable", menuName = "Variable/Float")]
public class FloatVariable : VariableBase
{

    [SerializeField] private float _value;

    [SerializeField] private GameEvent _onValueChanged;

    public void SetValue(float value)
    {
        _value = value;
    }

    public void SetValue(FloatVariable value)
    {
        _value = value._value;
    }

    public void ApplyChange(float amount)
    {
        _value += amount;
    }

    public void ApplyChange(FloatVariable amount)
    {
        _value += amount._value;
    }

    public float GetValue() 
    {  
        return _value; 
    }

    private void OnValueChanged() 
    {
        if (_onValueChanged != null)
        {
            _onValueChanged.Raise();
        }
    }
}
