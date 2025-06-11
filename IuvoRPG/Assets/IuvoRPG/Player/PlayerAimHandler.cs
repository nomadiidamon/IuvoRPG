using UnityEngine;
using Unity.Cinemachine;

public class PlayerAimHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Transform playerTransform;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private PlayerCameraHandler cameraHandler;
    [SerializeField] private CinemachinePanTilt cameraPanTiltComponent;

    [Header("Settings")]
    [SerializeField] private float aimRotationSpeed = 10f;
    [SerializeField] private float maxAimDistance = 100f;



    private Ray ray;
    private RaycastHit hit;

    public Vector3 AimTarget { get; private set; }
    public float AimDistance { get; private set; }

    private CameraStyle CurrentCameraStyle => cameraHandler.GetCurrentCameraStyle();

    private void Awake()
    {

        if (cameraPanTiltComponent == null)
        {
            cameraPanTiltComponent = cameraHandler.tpsCam.GetComponent<CinemachinePanTilt>();
        }
    }

    public void Update()
    {
      playerTransform = PlayerManager.Instance.playerRef.transform;
    }

    public void UpdateAim(bool isAiming)
    {
        ray = SceneManager.Instance.cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));

        Vector3 targetPoint;
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

        AimTarget = targetPoint;    // come back and move this into the true portion of the above if check

        if (isAiming)
        {
            Vector3 lookDir = AimTarget - playerTransform.position;
            lookDir.y = 0;

            if (lookDir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDir);
                playerTransform.rotation = Quaternion.Slerp(
                    playerTransform.rotation,
                    targetRotation,
                    aimRotationSpeed * Time.deltaTime
                );
            }
            Debug.DrawRay(playerTransform.position, lookDir.normalized * 2f, Color.red);
        }

        HandleCameraPanRanges();

    }

    private void HandleCameraPanRanges()
    {
        if (CurrentCameraStyle != CameraStyle.THIRD_PERSON_SHOOTER) return;


    }


    



    public void GetRay(out Ray ray) => ray = this.ray;
    public void GetRayHit(out RaycastHit hit) => hit = this.hit;

    public bool RayHitHasCollider() => hit.collider == null;
    public bool RayHitHasRigidbody() => hit.rigidbody == null;
}
