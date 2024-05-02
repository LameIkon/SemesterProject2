using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

[HelpURL("https://www.youtube.com/watch?v=fA79neqH21s")]
public class DialogueVariables
{

    private Dictionary<string, Ink.Runtime.Object> _variables;

    private Ink.Runtime.Object trueStatement; 

    public DialogueVariables(Story globalVariables) 
    {
        _variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string name in globalVariables.variablesState) 
        {
            Ink.Runtime.Object value = globalVariables.variablesState.GetVariableWithName(name);
            _variables.Add(name, value);

            if (name == "True") 
            {
                trueStatement = value;
            }

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

    public void ChangeMainStoryState(StoryStates state)
    {
        string variableName;

        switch (state) 
        {
            case StoryStates.joergen:
                variableName = "joergenDiaryFound";
                break;
            case StoryStates.niels:
                variableName = "nielsDiaryFound";
                break;
            case StoryStates.ludvig:
                variableName = "ludvigDiaryFound";
                break;
            case StoryStates.map:
                variableName = "theCardFound";
                break;

            case StoryStates.none:
                variableName = ""; 
                break;
            default:
                variableName = "";
                break;

        }



        if (_variables.ContainsKey(variableName) && variableName != "")
        {
            _variables[variableName] = trueStatement; 
        }
    }

}


public enum StoryStates 
{
    joergen,
    niels,
    ludvig,
    map,
    none

}
