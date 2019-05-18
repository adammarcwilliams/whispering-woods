using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "ScriptableObjects/Game Event", order = 51)]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> listeners = new List<GameEventListener>();

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(GameEventListener gameEventListener)
    {
        listeners.Add(gameEventListener);
    }

    public void UnregisterListener(GameEventListener gameEventListener)
    {
        listeners.Remove(gameEventListener);
    }
}
