using UnityEngine;

public class IStayHere : MonoBehaviour
{
    public void SetPosition(Vector3 position) => transform.position = position;

    public Vector3 GetPosition() => transform.position;

    public bool IsApproximatelySame(Vector3 toCheck) => (transform.position.normalized == toCheck.normalized);

}
