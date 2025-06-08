using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class FlexibleEvent
{
    // C# delegate-based event
    private event Action internalEvent;

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
    }

    // Remove C# listener
    public void RemoveListener(Action listener)
    {
        internalEvent -= listener;
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