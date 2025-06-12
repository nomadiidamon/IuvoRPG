using System.Collections.Generic;
using UnityEngine;

public class SemiBehaviorManager : MonoBehaviour
{
    public static SemiBehaviorManager Instance { get; private set; }

    private readonly List<SemiBehavior> regularUpdateBehaviors = new();
    private readonly List<SemiBehavior> fixedUpdateBehaviors = new();
    private readonly List<SemiBehavior> lateUpdateBehaviors = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Register(SemiBehavior behavior)
    {
        if (behavior == null) return;
        if (regularUpdateBehaviors.Contains(behavior) ||
            fixedUpdateBehaviors.Contains(behavior) ||
            lateUpdateBehaviors.Contains(behavior)) return;

        behavior.TryInitializeLifecycle();

        switch (behavior.updateMode)
        {
            case SemiBehavior.UpdateMode.Regular:
                regularUpdateBehaviors.Add(behavior);
                SortListByPriority(regularUpdateBehaviors);
                break;
            case SemiBehavior.UpdateMode.Fixed:
                fixedUpdateBehaviors.Add(behavior);
                SortListByPriority(fixedUpdateBehaviors);
                break;
            case SemiBehavior.UpdateMode.Late:
                lateUpdateBehaviors.Add(behavior);
                SortListByPriority(lateUpdateBehaviors);
                break;
        }
    }

    private void SortListByPriority(List<SemiBehavior> list)
    {
        list.Sort((a, b) =>
        {
            int levelComparison = b.PriorityLevel.CompareTo(a.PriorityLevel); // descending
            if (levelComparison != 0)
                return levelComparison;

            return b.priorityScale.Value.CompareTo(a.priorityScale.Value); // descending
        });
    }

    public void RefreshPriorities()
    {
        SortListByPriority(regularUpdateBehaviors);
        SortListByPriority(fixedUpdateBehaviors);
        SortListByPriority(lateUpdateBehaviors);
    }

    public void Unregister(SemiBehavior behavior)
    {
        if (behavior == null) return;

        behavior.DeinitializeLifecycle();

        regularUpdateBehaviors.Remove(behavior);
        fixedUpdateBehaviors.Remove(behavior);
        lateUpdateBehaviors.Remove(behavior);
    }

    private void Update()
    {
        foreach (var behavior in regularUpdateBehaviors)
        {
            if (behavior != null && behavior.isInitialized)
                behavior.Tick();
        }
    }

    private void FixedUpdate()
    {
        foreach (var behavior in fixedUpdateBehaviors)
        {
            if (behavior != null && behavior.isInitialized)
                behavior.Tick();
        }
    }

    private void LateUpdate()
    {
        foreach (var behavior in lateUpdateBehaviors)
        {
            if (behavior != null && behavior.isInitialized)
                behavior.Tick();
        }
    }

    public void ClearAll()
    {
        foreach (var behavior in regularUpdateBehaviors) behavior.DeinitializeLifecycle();
        foreach (var behavior in fixedUpdateBehaviors) behavior.DeinitializeLifecycle();
        foreach (var behavior in lateUpdateBehaviors) behavior.DeinitializeLifecycle();

        regularUpdateBehaviors.Clear();
        fixedUpdateBehaviors.Clear();
        lateUpdateBehaviors.Clear();
    }


}
