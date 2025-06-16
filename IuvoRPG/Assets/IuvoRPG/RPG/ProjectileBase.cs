using UnityEngine;

public class ProjectileBase : MonoBehaviour
{
    public GameObject go;
    public Rigidbody rb;
    public SphereCollider coll;


    public Vector3 spawnPosition { get; set; }
    public Transform projectileOwner;
    public Transform projectileTarget;
    public float projectileSpeed = 200.0f;
    public float projectileLifespan = 3.5f;
    public int damageAmount = 2;

    public LayerMask ignoreLayers = new LayerMask();



    public FlexibleEvent OnSpawn = new FlexibleEvent();
    public FlexibleEvent OnDestroy = new FlexibleEvent();
    public FlexibleEvent OnHit = new FlexibleEvent();

    public void Spawn()
    {
        if (go == null)
        {
            Debug.LogError("Projectile GameObject is not assigned.");
            return;
        }

        //GameObject.Instantiate(go, spawnPosition, Quaternion.identity);
        rb = go.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component is missing from the projectile GameObject.");
            return;
        }
        coll = go.GetComponent<SphereCollider>();
        if (coll == null)
        {
            Debug.LogError("SphereCollider component is missing from the projectile GameObject.");
            return;
        }
        coll.isTrigger = true; // Set the collider to be a trigger

        rb.isKinematic = false;
        rb.AddForce((projectileTarget.position - spawnPosition).normalized * projectileSpeed, ForceMode.Acceleration);

        Destroy(go, projectileLifespan);

        OnSpawn?.Invoke();
    }

    public void Destroy(float delay = 0.0f)
    {
        if (go != null)
        {
            Object.Destroy(go, delay);
            OnDestroy?.Invoke();
        }
        else
        {
            Debug.LogWarning("Attempted to destroy a null GameObject.");
        }
    }

    public void Hit(Collision collision)
    {
        if (collision.gameObject.layer.CompareTo(ignoreLayers) == 0)
        {
            // Ignore collision with specified layers
            return;
        }


        // handles when an object without a Health component is hit
        Destroy();

        OnHit?.Invoke();
    }

    public void OnCollisionEnter(Collision collision)
    {
        Hit(collision);
        Destroy();
    }
}
