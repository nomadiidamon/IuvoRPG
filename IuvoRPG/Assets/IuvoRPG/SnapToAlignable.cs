using IuvoUnity._Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SnapToAlignable : MonoBehaviour
{
    public BoxCollider BoxCollider;
    public LayerMask alignableLayer;
    public bool isAligned = false;
    public List<GameObject> alignableObjects = new List<GameObject>();
    public Dictionary<GameObject, float> alignments = new Dictionary<GameObject, float>();
    private List<KeyValuePair<GameObject, float>> sortedAlignments;

    public bool ReassessAlignmentTrigger = false;


    public void Start()
    {
        BoxCollider = GetComponent<BoxCollider>();
        AssessAlignments();
        Align();
    }

    public void Update()
    {
        if (ReassessAlignmentTrigger)
        {
            ReassessAndAlign();
            ReassessAlignmentTrigger = false;
        }
    }

    public void AssessAlignments()
    {
        alignments.Clear();
        GetAlignableObjects();
        foreach (GameObject obj in alignableObjects)
        {
            if (obj == this)
            {
                continue;
            }
            else
            {
                CheckAlignments(obj);
            }
        }
        SortAlignments();
    }

    void GetAlignableObjects()
    {
        alignableObjects.Clear();
        alignableObjects = Physics.OverlapBox(transform.position, BoxCollider.bounds.extents)
            .Where(col => col.CompareTag("Alignable") && col.gameObject != this.gameObject)
            .Select(col => col.gameObject)
            .ToList();
    }

    void CheckAlignments(GameObject obj)
    {
        float totalDistance = 0f;

        if (this.transform.IsAbove(obj.transform))
            totalDistance += Mathf.Abs(this.transform.position.y - obj.transform.position.y);
        else if (obj.transform.IsAbove(this.transform))
            totalDistance += Mathf.Abs(obj.transform.position.y - this.transform.position.y);

        if (this.transform.IsBehind(obj.transform))
            totalDistance += Mathf.Abs(this.transform.position.z - obj.transform.position.z);
        else if (obj.transform.IsBehind(this.transform))
            totalDistance += Mathf.Abs(obj.transform.position.z - this.transform.position.z);

        if (this.transform.IsLeftOf(obj.transform))
            totalDistance += Mathf.Abs(this.transform.position.x - obj.transform.position.x);
        else if (obj.transform.IsLeftOf(this.transform))
            totalDistance += Mathf.Abs(obj.transform.position.x - this.transform.position.x);

        if (!alignments.ContainsKey(obj))
            alignments.Add(obj, totalDistance);
        else
            alignments[obj] = Mathf.Min(alignments[obj], totalDistance); // keep smallest
    }

    private void SortAlignments()
    {
        sortedAlignments = alignments.OrderBy(pair => pair.Value).ToList();
    }

    public void Align()
    {
        if (sortedAlignments == null || sortedAlignments.Count == 0)
        {
            Debug.LogWarning("No alignments to use.");
            isAligned = false;
            return;
        }

        GameObject bestMatch = sortedAlignments[0].Key;

        if (bestMatch != null)
        {
            this.transform.AlignWith(bestMatch.transform, bestMatch.transform.localScale);
            isAligned = true;
        }
    }

    public void ReassessAndAlign()
    {
        AssessAlignments();
        Align();
    }
}
