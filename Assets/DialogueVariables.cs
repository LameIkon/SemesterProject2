using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

[HelpURL("https://www.youtube.com/watch?v=fA79neqH21s")]
public class DialogueVariables
{

    private Dictionary<string, Ink.Runtime.Object> _variables;

    public DialogueVariables(Story globalVariables) 
    {
        _variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string name in globalVariables.variablesState) 
        {
            Ink.Runtime.Object value = globalVariables.variablesState.GetVariableWithName(name);
            _variables.Add(name, value);
        }
    }
    
    public void StartListening(Story story) 
    {
        VariablesToStory(story);

        story.variablesState.variableChangedEvent += VariableChanged;
    }

    public void StopListening(Story story) 
    {
        story.variablesState.variableChangedEvent -= VariableChanged;
    }

    private void VariableChanged(string name, Ink.Runtime.Object value) 
    {
        if (_variables.ContainsKey(name)) 
        {
            _variables[name] = value;
        }

    }

    private void VariablesToStory(Story story) 
    {
        foreach (KeyValuePair<string, Ink.Runtime.Object> variable in _variables) 
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }

}
