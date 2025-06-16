using IuvoUnity._BaseClasses;
using System.Collections.Generic;
using UnityEngine;

public class InputBuffer : IDataStructBase
{
    private readonly Queue<BufferedInput> buffer = new Queue<BufferedInput>();
    public int Count => buffer.Count;

    public void AddInput(string actionName)
    {
        buffer.Enqueue(new BufferedInput(actionName, Time.time));
    }

    public List<BufferedInput> GetBufferedInputs()
    {
        return new List<BufferedInput>(buffer);
    }

    public void Clear() => buffer.Clear();

    public void RemoveRange(int startIndex, int count)
    {
        var tempList = new List<BufferedInput>(buffer);
        tempList.RemoveRange(startIndex, count);

        buffer.Clear();
        foreach (var item in tempList)
            buffer.Enqueue(item);
    }
}
