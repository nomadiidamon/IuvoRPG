using IuvoUnity._BaseClasses;
using UnityEngine;

public struct BufferedInput : IDataStructBase
{
    public string ActionName;
    public float Timestamp;

    public BufferedInput(string actionName, float timestamp)
    {
        ActionName = actionName;
        Timestamp = timestamp;
    }
}
