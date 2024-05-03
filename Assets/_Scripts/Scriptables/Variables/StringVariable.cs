using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New String Variable", menuName = "Variable/String")]
public class StringVariable : VariableBase
{

    [SerializeField, TextArea(2,4)] private string _value;

    #region Getters and Setters 

    public void SetString(string value)
    {
        _value = value;
    }

    public void SetString(StringVariable value)
    {
        _value = value._value;
    }

    public string GetString()
    {
        return _value;
    }

    public char[] ToCharArray() 
    {
        char[] chars = new char[_value.Length];

        for (int i = 0; i < _value.Length; i++) 
        {
            chars[i] = _value[i];
        }

        return chars;
    }
    #endregion
}
