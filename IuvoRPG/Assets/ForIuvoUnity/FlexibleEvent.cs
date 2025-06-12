using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class FlexibleEvent
{
    // reference to c# delegates for cleanup
    public List<Action> internalActions = new List<Action>();
    // C# delegate-based event
    private event Action internalEvent;

    // reference to Unity delegats for cleanup

    // UnityEvent exposed to the inspector
    [SerializeField] private UnityEvent unityEvent = new UnityEvent();

    // Invoke both UnityEvent and C# event
    public void Invoke()
    {
        unityEvent?.Invoke();
        internalEvent?.Invoke();
    }

    // Add C# listener
    public void AddListener(Action listener)
    {
        internalEvent += listener;
        internalActions.Add(listener);
    }

    // Remove C# listener
    public void RemoveListener(Action listener)
    {
        internalEvent -= listener;
        internalActions.Remove(listener);
    }

    // Add UnityAction listener (if needed in code)
    public void AddUnityListener(UnityAction listener)
    {
        unityEvent.AddListener(listener);
    }

    // Remove UnityAction listener
    public void RemoveUnityListener(UnityAction listener)
    {
        unityEvent.RemoveListener(listener);
    }

    public void RemoveAllFlexibleEventListeners()
    {
        foreach (var evt in internalActions)
        {
            RemoveListener(evt);
        }

        unityEvent.RemoveAllListeners();
    }
}


[Serializable]
public class FlexibleEvent<T>
{
    private event Action<T> internalEvent;

    [SerializeField] private UnityEvent<T> unityEvent = new UnityEvent<T>();

    public void Invoke(T arg)
    {
        unityEvent?.Invoke(arg);
        internalEvent?.Invoke(arg);
    }

    public void AddListener(Action<T> listener)
    {
        internalEvent += listener;
    }

    public void RemoveListener(Action<T> listener)
    {
        internalEvent -= listener;
    }

    public void AddUnityListener(UnityAction<T> listener)
    {
        unityEvent.AddListener(listener);
    }

    public void RemoveUnityListener(UnityAction<T> listener)
    {
        unityEvent.RemoveListener(listener);
    }
}