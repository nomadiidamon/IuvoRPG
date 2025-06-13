using System.Collections.Generic;
using UnityEngine;

public class Waypoint : SemiBehavior
{
    private Vector3 _position;

    private Waypoint _previous;
    private Waypoint _next;

    public Waypoint()
    {
        SetPosition(Vector3.zero);
        _previous = null;
        _next = null;
    }

    public Waypoint(Vector3 position)
    {
        SetPosition(position);
        _previous = null;
        _next = null;
    }

    public void SetPosition(Vector3 pos) => _position = pos;
    public void SetNextWaypoint(Waypoint newNext) => _next = newNext;
    public void SetPreviousWaypoint(Waypoint newPrevious) => _previous = newPrevious;

    public Vector3 GetPosition() => _position;

    public Waypoint TryGetNextWaypoint()
    {
        if (IsNextWaypointNull()) return null;

        return GetNextWaypoint();
    }
    private Waypoint GetNextWaypoint() => _next;
    public bool IsNextWaypointNull() => _next == null;


    public Waypoint TryGetPreviousWaypoint()
    {
        if (IsPreviousWaypointNull()) return null;

        return GetPreviousWaypoint();
    }
    private Waypoint GetPreviousWaypoint() => _previous;
    public bool IsPreviousWaypointNull() => _previous == null;

    public IEnumerable<Waypoint> TraverseForward(int maxSteps = 1000)
    {
        var current = this;
        int count = 0;
        while (current != null && count++ < maxSteps)
        {
            yield return current;
            current = current.TryGetNextWaypoint();
        }
    }

}
