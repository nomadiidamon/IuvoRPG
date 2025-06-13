using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WaypointPath : SemiBehavior
{
    public Vector3 AvgPosition;
    public List<Waypoint> waypoints = new List<Waypoint>();

    public WaypointPath()
    {
        AvgPosition = Vector3.zero;
    }

    public WaypointPath(List<Waypoint> waypoints)
    {
        this.waypoints = waypoints;
        AvgPosition = CalculateAveragePosition(waypoints);
    }



    #region Validation

    public static Vector3 CalculateAveragePosition(List<Waypoint> points)
    {
        if (points == null || points.Count == 0) return Vector3.zero;

        Vector3 averagePosition = Vector3.zero;
        for (int i = 0; i < points.Count; i++)
        {
            Waypoint waypoint = points[i];
            averagePosition += waypoint.GetPosition();
        }
        averagePosition /= points.Count;
        return averagePosition;
    }

    public void RecalculateAverage()
    {
        AvgPosition = CalculateAveragePosition(waypoints);
    }


    public bool IsCircularPath()
    {
        if (waypoints.Count == 0)
            return false;

        var first = waypoints[0];
        var last = waypoints[waypoints.Count - 1];

        return first.TryGetPreviousWaypoint() == last &&
               last.TryGetNextWaypoint() == first;
    }



    public bool IsPathLooped(int maxCheck = 1000)
    {
        var visited = new HashSet<Waypoint>();
        Waypoint current = waypoints.Count > 0 ? waypoints[0] : null;

        int count = 0;
        while (current != null && count < maxCheck)
        {
            if (visited.Contains(current))
                return true;

            visited.Add(current);
            current = current.TryGetNextWaypoint();
            count++;
        }

        return false;
    }

    public void ValidateBidirectionalLinks()
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            var wp = waypoints[i];
            if (wp.TryGetNextWaypoint() != null && wp.TryGetNextWaypoint().TryGetPreviousWaypoint() != wp)
            {
                Debug.LogWarning($"Next link mismatch at index {i}");
            }
            if (wp.TryGetPreviousWaypoint() != null && wp.TryGetPreviousWaypoint().TryGetNextWaypoint() != wp)
            {
                Debug.LogWarning($"Previous link mismatch at index {i}");
            }
        }
    }

    public void RebuildLinks()
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            var current = waypoints[i];
            Waypoint next = i < waypoints.Count - 1 ? waypoints[i + 1] : null;
            Waypoint prev = i > 0 ? waypoints[i - 1] : null;

            current.SetNextWaypoint(next);
            current.SetPreviousWaypoint(prev);
        }
    }

    private void ValidateState(bool deep = false)
    {
        RebuildLinks();
        RecalculateAverage();
        ValidateBidirectionalLinks();

        if (deep && IsPathLooped())
        {
            Debug.LogWarning("Path contains a loop!");
        }
    }


    #endregion

    #region Adding & Removing Waypoints

    public void PushFront(Waypoint toAdd)
    {
        if (waypoints.Contains(toAdd))
            return;

        if (waypoints.Count == 0)
        {
            waypoints.Add(toAdd);
            return;
        }

        Waypoint first = waypoints[0];
        toAdd.SetNextWaypoint(first);
        first.SetPreviousWaypoint(toAdd);
        toAdd.SetPreviousWaypoint(null);

        waypoints.Insert(0, toAdd);
        ValidateState();
    }

    public void PushBack(Waypoint toAdd)
    {
        if (waypoints.Contains(toAdd))
            return;

        if (waypoints.Count == 0)
        {
            waypoints.Add(toAdd);
            return;
        }

        Waypoint last = waypoints[waypoints.Count - 1];
        last.SetNextWaypoint(toAdd);
        toAdd.SetPreviousWaypoint(last);
        toAdd.SetNextWaypoint(null);

        waypoints.Add(toAdd);
        ValidateState();
    }

    public void Insert(Waypoint toAdd, Waypoint prev, Waypoint next)
    {
        if (toAdd == null) return;

        if (waypoints.Contains(toAdd))
            return;

        if (prev != null && !waypoints.Contains(prev))
        {
            Debug.LogWarning("Insert: prev not in list");
            return;
        }

        if (next != null && !waypoints.Contains(next))
        {
            Debug.LogWarning("Insert: next not in list");
            return;
        }

        if (prev != null)
            prev.SetNextWaypoint(toAdd);
        if (next != null)
            next.SetPreviousWaypoint(toAdd);

        toAdd.SetPreviousWaypoint(prev);
        toAdd.SetNextWaypoint(next);

        int index = waypoints.IndexOf(prev);
        if (index >= 0)
            waypoints.Insert(index + 1, toAdd);
        else
            waypoints.Add(toAdd); // fallback

        ValidateState();
    }

    public void InsertByDistance(Waypoint toAdd)
    {
        if (waypoints.Contains(toAdd))
            return;

        if (waypoints.Count < 2)
        {
            PushBack(toAdd);
            return;
        }

        float minDistance = float.MaxValue;
        int bestIndex = 0;

        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            Vector3 a = waypoints[i].GetPosition();
            Vector3 b = waypoints[i + 1].GetPosition();
            Vector3 midpoint = (a + b) / 2f;
            float dist = Vector3.Distance(midpoint, toAdd.GetPosition());

            if (dist < minDistance)
            {
                minDistance = dist;
                bestIndex = i;
            }
        }

        Insert(toAdd, waypoints[bestIndex], waypoints[bestIndex + 1]);
    }

    public void InsertRandomly(Waypoint toAdd)
    {
        if (waypoints.Contains(toAdd))
            return;

        if (waypoints.Count < 2)
        {
            PushBack(toAdd);
            return;
        }

        int index = Random.Range(0, waypoints.Count - 1);
        Insert(toAdd, waypoints[index], waypoints[index + 1]);
    }

    public void Remove(Waypoint toRemove)
    {
        if (!waypoints.Contains(toRemove)) return;

        Waypoint prev = toRemove.TryGetPreviousWaypoint();
        Waypoint next = toRemove.TryGetNextWaypoint();

        if (prev != null) prev.SetNextWaypoint(next);
        if (next != null) next.SetPreviousWaypoint(prev);

        toRemove.SetPreviousWaypoint(null);
        toRemove.SetNextWaypoint(null);

        waypoints.Remove(toRemove);
        ValidateState();
    }

    public void RemoveAll()
    {
        foreach (var wp in waypoints)
        {
            wp.SetNextWaypoint(null);
            wp.SetPreviousWaypoint(null);
        }
        waypoints.Clear();
        RecalculateAverage();
    }

    #endregion

    #region DynamicEditing

    public void MakeCircular()
    {
        if (IsCircularPath()) return;

        if (waypoints.Count < 2) return;

        Waypoint first = waypoints[0];
        Waypoint last = waypoints[waypoints.Count - 1];

        last.SetNextWaypoint(first);
        first.SetPreviousWaypoint(last);
    }

    public Waypoint FindClosest(Vector3 position)
    {
        Waypoint closest = null;
        float minDist = float.MaxValue;
        foreach (var wp in waypoints)
        {
            float dist = Vector3.Distance(position, wp.GetPosition());
            if (dist < minDist)
            {
                minDist = dist;
                closest = wp;
            }
        }
        return closest;
    }

    public static List<Waypoint> GetForwardPath(Waypoint start, int max = 100)
    {
        var list = new List<Waypoint>();
        Waypoint current = start;
        int count = 0;
        while (current != null && count++ < max)
        {
            list.Add(current);
            current = current.TryGetNextWaypoint();
        }
        return list;
    }

    public void ReversePath()
    {
        waypoints.Reverse();
        RebuildLinks();
    }


    public static WaypointPath ConnectPaths(WaypointPath mainPath, Waypoint mainPathConnectPoint, WaypointPath pathToConnect, Waypoint pointToConnect)
    {
        if (mainPath == null || mainPathConnectPoint == null || pathToConnect == null || pointToConnect == null)
        {
            Debug.LogWarning("Invalid parameters passed to ConnectPaths.");
            return null;
        }

        WaypointPath combinedPath = new WaypointPath();

        // Copy forward path from main
        foreach (var wp in mainPathConnectPoint.TraverseForward())
        {
            combinedPath.PushBack(wp.Clone());
        }

        // Midpoint
        Vector3 midPoint = (mainPathConnectPoint.GetPosition() + pointToConnect.GetPosition()) / 2f;
        Waypoint transitionWaypoint = new Waypoint(midPoint);
        combinedPath.PushBack(transitionWaypoint);

        // Copy forward path from the pointToConnect path
        foreach (var wp in pointToConnect.TraverseForward())
        {
            combinedPath.PushBack(wp.Clone());
        }

        combinedPath.ValidateState(true);
        return combinedPath;
    }


    #endregion



}
