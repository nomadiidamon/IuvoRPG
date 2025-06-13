using System.Collections.Generic;
using UnityEngine;

public class Chain<T>
{
    public List<Link<T>> Links = new List<Link<T>>();

    public void PushFront(Link<T> toAdd)
    {
        if (Links.Contains(toAdd))
            return;

        if (Links.Count == 0)
        {
            Links.Add(toAdd);
            return;
        }

        var first = Links[0];
        toAdd.SetNext(first);
        first.SetPrevious(toAdd);
        toAdd.SetPrevious(null);
        Links.Insert(0, toAdd);
    }

    public void PushBack(Link<T> toAdd)
    {
        if (Links.Contains(toAdd))
            return;

        if (Links.Count == 0)
        {
            Links.Add(toAdd);
            return;
        }

        var last = Links[Links.Count - 1];
        last.SetNext(toAdd);
        toAdd.SetPrevious(last);
        toAdd.SetNext(null);
        Links.Add(toAdd);
    }

    public void RebuildLinks()
    {
        for (int i = 0; i < Links.Count; i++)
        {
            var current = Links[i];
            Link<T> next = i < Links.Count - 1 ? Links[i + 1] : null;
            Link<T> prev = i > 0 ? Links[i - 1] : null;

            current.SetNext(next);
            current.SetPrevious(prev);
        }
    }

    public void Remove(Link<T> toRemove)
    {
        if (!Links.Contains(toRemove)) return;

        var prev = toRemove.TryGetPrevious();
        var next = toRemove.TryGetNext();

        if (prev != null) prev.SetNext(next);
        if (next != null) next.SetPrevious(prev);

        toRemove.SetNext(null);
        toRemove.SetPrevious(null);

        Links.Remove(toRemove);
    }

    public void RemoveAll()
    {
        foreach (var link in Links)
        {
            link.SetNext(null);
            link.SetPrevious(null);
        }
        Links.Clear();
    }

    public Link<T> FindClosest(System.Func<T, float> distanceFunc)
    {
        Link<T> closest = null;
        float minDistance = float.MaxValue;
        foreach (var link in Links)
        {
            float dist = distanceFunc(link.GetData());
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = link;
            }
        }
        return closest;
    }

    public Chain<T> Clone()
    {
        var newChain = new Chain<T>();
        foreach (var link in Links)
        {
            newChain.PushBack(link.Clone());
        }
        newChain.RebuildLinks();
        return newChain;
    }

    public static List<Link<T>> GetForwardPath(Link<T> start, int max = 100)
    {
        var list = new List<Link<T>>();
        var current = start;
        int count = 0;
        while (current != null && count++ < max)
        {
            list.Add(current);
            current = current.TryGetNext();
        }
        return list;
    }

    public void Reverse()
    {
        Links.Reverse();
        RebuildLinks();
    }

    public void MakeCircular()
    {
        if (Links.Count < 2) return;

        var first = Links[0];
        var last = Links[Links.Count - 1];

        last.SetNext(first);
        first.SetPrevious(last);
    }

    public void InsertAt(Link<T> toAdd, Link<T> prev, Link<T> next)
    {
        if (toAdd == null) return;
        if (Links.Contains(toAdd)) return;

        if (prev != null && !Links.Contains(prev))
        {
            Debug.LogWarning("InsertAt: prev not in list");
            return;
        }
        if (next != null && !Links.Contains(next))
        {
            Debug.LogWarning("InsertAt: next not in list");
            return;
        }

        if (prev != null)
            prev.SetNext(toAdd);
        if (next != null)
            next.SetPrevious(toAdd);

        toAdd.SetPrevious(prev);
        toAdd.SetNext(next);

        int index = Links.IndexOf(prev);
        if (index >= 0)
            Links.Insert(index + 1, toAdd);
        else
            Links.Add(toAdd);

        RebuildLinks();
    }

    public void ValidateLinks()
    {
        for (int i = 0; i < Links.Count; i++)
        {
            var link = Links[i];

            var next = link.TryGetNext();
            if (next != null && next.TryGetPrevious() != link)
            {
                Debug.LogWarning($"Next link mismatch at index {i}");
            }

            var prev = link.TryGetPrevious();
            if (prev != null && prev.TryGetNext() != link)
            {
                Debug.LogWarning($"Previous link mismatch at index {i}");
            }
        }
    }

}
