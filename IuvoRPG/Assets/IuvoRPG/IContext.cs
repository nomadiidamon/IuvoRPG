using IuvoUnity._BaseClasses;
using System;
using UnityEngine;

public interface IContext : IDataStructBase
{
    Type Context { get; set; }
}
