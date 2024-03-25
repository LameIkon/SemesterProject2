using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Color Variable", menuName = "Variable/Color")]
public class ColorVariable : ScriptableObject
{

    [SerializeField] private Color _value;

    #region Getters and Setters 

    public void SetColor(Color value) 
    {
        _value = value;
    }

    public void SetColor(ColorVariable value) 
    {
        _value = value._value;
    }

    public Color GetColor() 
    {
        return _value;
    }

    #endregion

}
