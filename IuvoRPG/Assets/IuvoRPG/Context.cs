using IuvoUnity._BaseClasses;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum ContextKey
{
    // GameObject references
    Player,
    Target,
    Enemy,
    Camera,
    Interactable,

    // State flags
    IsUnderAttack,
    IsVisible,
    IsMoving,
    IsGrounded,
    IsAiming,
    IsDead,
    IsPaused,

    // Numbers / Stats
    Health,
    MaxHealth,
    Mana,
    MaxMana,
    Stamina,
    MaxStamina,
    Experience,
    ExpToNextLevel,
    Score,
    DistanceToTarget,
    TimeSinceLastSeen,
    CooldownTime,

    // Vectors / Transforms
    Position,
    Destination,
    Velocity,
    Direction,
    LookTarget,

    // Events or Triggers
    OnDamageTaken,
    OnDeath,
    OnItemPickup,
    OnObjectiveComplete,
    OnInteract,

    // Systems
    InputData,
    Inventory,
    DialogueState,
    QuestStatus,
    AudioCue,

    // Custom or Reserved
    Custom0,
    Custom1,
    Custom2
}

[Serializable]
public class Context : IDataStructBase
{
    private Dictionary<ContextKey, object> _data = new Dictionary<ContextKey, object>();

    private Dictionary<ContextKey, object> _eventMap = new Dictionary<ContextKey, object>();


    public Context()
    {
        _data = new Dictionary<ContextKey, object>();
    }

    public Context(ref Dictionary<ContextKey, object> contextDictionary)
    {
        _data = new Dictionary<ContextKey, object>(contextDictionary);
    }

    // context data functions
    public void Set<T>(ContextKey key, T value)
    {
        _data[key] = value;
    }

    public T Get<T>(ContextKey key)
    {
        if (_data.TryGetValue(key, out object value) && value is T typedValue)
        {
            return typedValue;
        }

        throw new KeyNotFoundException($"Key '{key}' not found in context or of incorrect type.");
    }

    public bool TryGet<T>(ContextKey key, out T value)
    {
        if (_data.TryGetValue(key, out object obj) && obj is T t)
        {
            value = t;
            return true;
        }
        Debug.LogWarning($"Context key '{key}' found but type mismatch. Expected: {typeof(T)}, Actual: {obj.GetType()}");
        value = default;
        return false;
    }

    public bool Has(ContextKey key) => _data.ContainsKey(key);
    public bool Remove(ContextKey key) => _data.Remove(key);
    public void Clear() => _data.Clear();

    // Context helpers
    public Context CloneContext()
    {
        var newDict = new Dictionary<ContextKey, object>(_data);
        return new Context(ref newDict);
    }

    public void MergeContexts(Context other, bool overwrite = true)
    {
        foreach (var kvp in other._data)
        {
            if (overwrite || !_data.ContainsKey(kvp.Key))
            {
                _data[kvp.Key] = kvp.Value;
            }
        }
    }

    // event map functions
    public void RegisterEvent(ContextKey key, FlexibleEvent evt)
    {
        _eventMap[key] = evt;
    }

    public void RegisterEvent<T>(ContextKey key, FlexibleEvent<T> evt)
    {
        _eventMap[key] = evt;
    }

    public bool TryGetEvent(ContextKey key, out FlexibleEvent evt)
    {
        if (_eventMap.TryGetValue(key, out var val) && val is FlexibleEvent casted)
        {
            evt = casted;
            return true;
        }

        evt = null;
        return false;
    }

    public bool TryGetEvent<T>(ContextKey key, out FlexibleEvent<T> evt)
    {
        if (_eventMap.TryGetValue(key, out var val) && val is FlexibleEvent<T> casted)
        {
            evt = casted;
            return true;
        }

        evt = null;
        return false;
    }

    public bool HasEvent(ContextKey key) => _eventMap.ContainsKey(key);
    public bool RemoveEvent(ContextKey key) => _eventMap.Remove(key);

    public void InvokeEvent(ContextKey key)
    {
        if (TryGetEvent(key, out FlexibleEvent evt))
        {
            evt.Invoke();
        }
    }

    public void InvokeEvent<T>(ContextKey key, T value)
    {
        if (TryGetEvent<T>(key, out FlexibleEvent<T> evt))
        {
            evt.Invoke(value);
        }
    }

    public void ClearEvents() => _eventMap.Clear();
}
