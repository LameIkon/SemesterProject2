using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Event", menuName ="Event/GameEvent")]
public class GameEvent : ScriptableObject
{

    private readonly List<GameEventListener> eventListeners = new List<GameEventListener>();

    public void Raise()
    {
        for (int i = eventListeners.Count - 1; i >= 0; i--)
            eventListeners[i].OnEventRaised();
    }

    public void RegisterListener(GameEventListener listener)
    {
        if (!eventListeners.Contains(listener))
            eventListeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if (eventListeners.Contains(listener))
            eventListeners.Remove(listener);
    }
}


///<License>
///
///MIT License
///
///Copyright (c) 2018 Ryan Hipple
///
///Permission is hereby granted, free of charge, to any person obtaining a copy
///of this software and associated documentation files (the "Software"), to deal
///in the Software without restriction, including without limitation the rights
///to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
///copies of the Software, and to permit persons to whom the Software is
///furnished to do so, subject to the following conditions:
///
///The above copyright notice and this permission notice shall be included in all
///copies or substantial portions of the Software.
///