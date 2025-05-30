using IuvoUnity._BaseClasses;
using UnityEngine;

public interface IObjectBehavior : IuvoInterfaceBase
{
    GameObject BehavioralObject { get; set; }

    void PerformBehavior();
    
}
