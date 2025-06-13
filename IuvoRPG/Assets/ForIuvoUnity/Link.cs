using System.Collections.Generic;
using Unity.VisualScripting;

public class Link<T>
{
    private T _data;
    private Link<T> _previous;
    private Link<T> _next;

    public Link()
    {
        SetData(default);
        _previous = null;
        _next = null;
    }

    public Link(T data)
    {
        SetData(data);
        _previous = null;
        _next = null;
    }

    public void SetData(T data) => _data = data;
    public T GetData() => _data;

    public void SetNext(Link<T> next)
    {
        _next = next;
        next._previous = this;
    }
    public void SetPrevious(Link<T> prev)
    {
        _previous = prev;
        prev._next = this;
    }

    public Link<T> TryGetNext() => _next;
    public Link<T> TryGetPrevious() => _previous;

    public bool IsNextNull() => _next == null;
    public bool IsPreviousNull() => _previous == null;

    public IEnumerable<Link<T>> TraverseForward(int maxSteps = 1000)
    {
        var current = this;
        int count = 0;
        while (current != null && count++ < maxSteps)
        {
            yield return current;
            current = current.TryGetNext();
        }
    }

    public Link<T> Clone()
    {
        return new Link<T>(_data);
    }
}
