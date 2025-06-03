using UnityEngine;
using Unity.Cinemachine;
using UnityEditor.Rendering.LookDev;

public class SecondPlayerAimHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Transform playerForward;
    [SerializeField] private Transform camTransform;
    [SerializeField] private Transform aimTarget;
    [SerializeField] private PlayerCameraHandler cameraManager;
    [SerializeField] private CinemachineCamera tpsCamera;


    [Header("Settings")]
    [SerializeField] private float aimRotationSpeed = 10f;
    [SerializeField] private float maxAimDistance = 100f;



    private Ray ray;
    private RaycastHit hit;

    public Vector3 AimTarget { get; private set; }
    public float AimDistance { get; private set; }

    private CameraStyle CurrentCameraStyle => cameraManager.GetCurrentCameraStyle();

    private void Awake()
    {
        tpsCamera = cameraManager.tpsCam;
        ray.origin = tpsCamera.transform.position;
        ray.direction = tpsCamera.transform.forward * 100.0f;
    }

    public void UpdateAim(bool isAiming, bool isMoving, Vector3 playerMoveDir)
    {
        if (CurrentCameraStyle == CameraStyle.EXPLORATION) return;
        ray.origin = camTransform.position;
        ray.direction = camTransform.forward * 100.0f;

        Vector3 targetPoint = Vector3.zero;

        if (Physics.Raycast(ray, out hit, maxAimDistance))
        {
            targetPoint = hit.point;
            AimDistance = hit.distance;
        }
        else
        {
            targetPoint = ray.origin + ray.direction * maxAimDistance;
            AimDistance = maxAimDistance;
        }

        aimTarget.position = targetPoint;
        AimTarget = targetPoint;

        if (isAiming)
        {
            // Get camera's forward and right, projected onto XZ
            Vector3 camForward = camTransform != null ? camTransform.forward : Vector3.forward;
            Vector3 camRight = camTransform != null ? camTransform.right : Vector3.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            // Get movement direction from input
            Vector3 moveDir = Vector3.zero;
            moveDir = camForward * playerMoveDir.z + camRight * playerMoveDir.x;
            moveDir.y = 0;

            Quaternion targetRot = playerForward.rotation;
            targetRot.y = aimTarget.rotation.y;

            playerForward.rotation = targetRot;

            //Quaternion targetRotation = Quaternion.LookRotation(aimTarget.position);
            //transform.rotation = Quaternion.Slerp(
            //    playerTransform.rotation,
            //    targetRotation,
            //    aimRotationSpeed * Time.deltaTime
            //);

        }

    }




    public void GetRay(out Ray ray) => ray = this.ray;
    public void GetRayHit(out RaycastHit hit) => hit = this.hit;

    public bool RayHitHasCollider() => hit.collider == null;
    public bool RayHitHasRigidbody() => hit.rigidbody == null;
}
