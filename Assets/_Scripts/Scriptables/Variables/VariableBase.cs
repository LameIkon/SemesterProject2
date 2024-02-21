using UnityEngine;

public class VariableBase : ScriptableObject
{
#if UNITY_EDITOR
    [Multiline]
    public string DevDescription = "";
#endif
}
