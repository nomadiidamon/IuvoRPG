using IuvoUnity._DataStructs;
using UnityEngine;

public enum PriorityLevel
{
    None,
    Low,
    Moderate,
    High,
    Emergency
}

public interface IPriority
{
    public PriorityLevel PriorityLevel { get; set; }
    public ClampedValue<float> priorityScale { get; set; }
}
