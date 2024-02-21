using UnityEngine;

[CreateAssetMenu(fileName = "New Float Variable", menuName = "Variable/Float")]
public class FloatVariable : VariableBase
{

    [SerializeField] private float _value;

    [SerializeField, Tooltip("Called when the value of the Variable changes, it can be null")] 
    private GameEvent _onValueChanged;



    #region Setters and Getters    

    public void SetValue(float value)
    {
        _value = value;
        OnValueChanged();
    }

    public void SetValue(FloatVariable value)
    {
        _value = value._value;
        OnValueChanged();
    }

    public void ApplyChange(float amount)
    {
        _value += amount;
        OnValueChanged();
    }

    public void ApplyChange(FloatVariable amount)
    {
        _value += amount._value;
        OnValueChanged();
    }

    public float GetValue() 
    {  
        return _value; 
    }

    #endregion

    // Used to update other scripts when they the value is changed
    private void OnValueChanged() 
    {
        if (_onValueChanged != null)
        {
            _onValueChanged.Raise();
        }
    }

}
