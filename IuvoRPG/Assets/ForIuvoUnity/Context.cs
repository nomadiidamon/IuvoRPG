using IuvoUnity._BaseClasses;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum ContextActorKey { Player, Target, Ally, Enemy, Camera, Interactable }
public enum ContextPlayerHandlerKey { AimHandler, AnimatorHandler, CameraHandler, CombatHandler, InputHandler, 
    MovementHandler, RotationHandler, StatHandler, UIHandler, PlayerManager} 
public enum ContextStateKey { IsUnderAttack, IsVisible, IsMoving, IsGrounded, IsAiming, IsDead, IsPaused, IsSprinting }
public enum ContextStatKey { PlayerStats, CharcterStats, Score, DistanceToTarget, TimeSinceLastSeen, CooldownTime }
public enum ContextTransformKey { Transform, Position, Destination, Velocity, Direction, LookTarget, LastDirection, InputDirection }
public enum ContextEventKey { OnDamageTaken, OnDeath, OnItemPickup, OnObjectiveComplete, OnInteract, OnPause }
public enum ContextSystemKey { InputData, Inventory, DialogueState, QuestStatus, AudioCue }
public enum ContextCustomKey { Custom0, Custom1, Custom2 }

public static class ContextKeyGroups
{
    public static readonly Type[] AllowedKeys = new[]
    {
        typeof(ContextActorKey),
        typeof(ContextPlayerHandlerKey),
        typeof(ContextStateKey),
        typeof(ContextStatKey),
        typeof(ContextTransformKey),
        typeof(ContextEventKey),
        typeof(ContextSystemKey),
        typeof(ContextCustomKey)
    };

    public static bool IsValidKey(Enum key)
    {
        return Array.Exists(AllowedKeys, t => t == key.GetType());
    }

    public static void CheckForDuplicateEnumNames()
    {
        // Dictionary to track which enum names appear in which enum types.
        // Key = enum field name (string), Value = list of enum types where this name is found.
        Dictionary<string, List<Type>> nameToEnumTypes = new Dictionary<string, List<Type>>();

        // Iterate through all allowed enum types defined in ContextKeyGroups.
        foreach (Type enumType in AllowedKeys)
        {
            // Safety check to ensure the type is actually an enum.
            if (!enumType.IsEnum)
            {
                Debug.LogWarning($"[ContextKeyGroups] Type {enumType.Name} in AllowedKeys is not an enum.");
                continue;
            }

            // Loop through all enum field names (as strings) in the current enum type.
            foreach (string fieldName in Enum.GetNames(enumType))
            {
                // Add the field name to the map, associating it with the current enum type.
                if (!nameToEnumTypes.ContainsKey(fieldName))
                {
                    nameToEnumTypes[fieldName] = new List<Type>();
                }

                nameToEnumTypes[fieldName].Add(enumType);
            }
        }

        // Check for enum field names that appear in more than one enum type.
        foreach (var kvp in nameToEnumTypes)
        {
            string fieldName = kvp.Key;
            List<Type> typesWithField = kvp.Value;

            // If the field name is present in multiple enum types, issue a warning.
            if (typesWithField.Count > 1)
            {
                string typeList = string.Join(", ", typesWithField.Select(t => t.Name));
                Debug.LogWarning($"[ContextKeyGroups] Duplicate enum field name \"{fieldName}\" found in multiple enums: {typeList}");
            }
        }
    }


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void ValidateContextKeys()
    {
#if UNITY_EDITOR
        ContextKeyGroups.CheckForDuplicateEnumNames();
#endif
    }
}



[Serializable]
public class Context : IDataStructBase
{
    private ConcurrentDictionary<Enum, object> _data = new ConcurrentDictionary<Enum, object>();

    private ConcurrentDictionary<Enum, object> _eventMap = new ConcurrentDictionary<Enum, object>();


    public Context()
    {

    }

    public Context(ref ConcurrentDictionary<Enum, object> contextDictionary)
    {
        _data = new ConcurrentDictionary<Enum, object>(contextDictionary);
    }

    // context data functions
    public void Set<T>(Enum key, T value)
    {
        if (!ContextKeyGroups.AllowedKeys.Contains(key.GetType()))
        {
            Debug.LogWarning($"Key {key} not in approved ContextKey groups.");
            return;
        }

        _data[key] = value;
    }

    public T Get<T>(Enum key)
    {
        if (!ContextKeyGroups.IsValidKey(key))
            throw new InvalidOperationException($"Invalid key type: {key.GetType().Name}");

        if (_data.TryGetValue(key, out var value) && value is T t)
            return t;

        throw new KeyNotFoundException($"Key {key} not found or wrong type");
    }

    public bool TryGet<T>(Enum key, out T value)
    {
        if (!ContextKeyGroups.IsValidKey(key))
        {
            value = default;
            return false;
        }

        if (_data.TryGetValue(key, out var obj) && obj is T t)
        {
            value = t;
            return true;
        }

        value = default;
        return false;
    }

    public bool Has(Enum key) => _data.ContainsKey(key);
    public bool Remove(Enum key) => _data.TryRemove(key, out _);
    public void Clear() => _data.Clear();

    // Context helpers
    public Context CloneContext()
    {
        var newDict = new ConcurrentDictionary<Enum, object>(_data);
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
    public void RegisterEvent(Enum key, FlexibleEvent evt)
    {
        if (!ContextKeyGroups.IsValidKey(key)) return;
        _eventMap[key] = evt;
    }

    public void RegisterEvent<T>(Enum key, FlexibleEvent<T> evt)
    {
        if (!ContextKeyGroups.IsValidKey(key)) return;
        _eventMap[key] = evt;
    }

    public bool TryGetEvent(Enum key, out FlexibleEvent evt)
    {
        if (_eventMap.TryGetValue(key, out var val) && val is FlexibleEvent e)
        {
            evt = e;
            return true;
        }

        evt = null;
        return false;
    }

    public bool TryGetEvent<T>(Enum key, out FlexibleEvent<T> evt)
    {
        if (_eventMap.TryGetValue(key, out var val) && val is FlexibleEvent<T> e)
        {
            evt = e;
            return true;
        }

        evt = null;
        return false;
    }


    public void InvokeEvent(Enum key)
    {
        if (TryGetEvent(key, out FlexibleEvent evt))
        {
            evt.Invoke();
        }
    }

    public void InvokeEvent<T>(Enum key, T value)
    {
        if (TryGetEvent<T>(key, out FlexibleEvent<T> evt))
        {
            evt.Invoke(value);
        }
    }

    public bool HasEvent(Enum key) => _eventMap.ContainsKey(key);
    public bool RemoveEvent(Enum key) => _eventMap.TryRemove(key, out _);
    public void ClearEvents() => _eventMap.Clear();
}
