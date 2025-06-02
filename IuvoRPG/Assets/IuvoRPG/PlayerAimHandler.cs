using UnityEngine;
using Unity.Cinemachine;
using UnityEditor.Rendering.LookDev;

public class PlayerAimHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Transform playerTransform;
    [SerializeField] private Transform camTransform;
    [SerializeField] private PlayerCameraHandler cameraManager;
    [SerializeField] private CinemachineThirdPersonFollow cameraFollowComponent;
    [SerializeField] private CinemachinePanTilt cameraPanTiltComponent;

    [Header("Settings")]
    [SerializeField] private float aimRotationSpeed = 10f;
    [SerializeField] private float moveRotationSpeed = 5f;
    [SerializeField] private float maxAimDistance = 100f;
    [SerializeField] private float shoulderSwitchSpeed = 5f;
    [SerializeField] private int rightSideMaxAimPan = 45;
    [SerializeField] private int rightSideMinAimPan = -45;
    [SerializeField] private int leftSideMaxAimPan = 45;
    [SerializeField] private int leftSideMinAimPan = -45;

    private bool hasSwitchedShoulderAutomatically = false;
    private bool rightShoulder = true;
    private float cameraSide = 1.0f;

    private Ray ray;
    private RaycastHit hit;

    public Vector3 AimTarget { get; private set; }
    public float AimDistance { get; private set; }

    private CameraStyle CurrentCameraStyle => cameraManager.GetCurrentCameraStyle();

    private void Awake()
    {
        if (cameraFollowComponent == null)
        {
            cameraFollowComponent = cameraManager.tpsCam.GetComponent<CinemachineThirdPersonFollow>();
        }
        if (cameraPanTiltComponent == null)
        {
            cameraPanTiltComponent = cameraManager.tpsCam.GetComponent<CinemachinePanTilt>();
        }
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

        AimTarget = targetPoint;

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
        AutoShoulderSwitch();
        SmoothShoulderToggle();


    }

    private void HandleCameraPanRanges()
    {
        if (CurrentCameraStyle != CameraStyle.THIRD_PERSON_SHOOTER) return;

        cameraFollowComponent.CameraSide = cameraSide;
        if (cameraSide == 1)
        {
            cameraPanTiltComponent.PanAxis.Range.Set(rightSideMinAimPan, rightSideMaxAimPan);
        }
        else
        {
            cameraPanTiltComponent.PanAxis.Range.Set(leftSideMinAimPan, leftSideMaxAimPan);
        }
    }

    private void AutoShoulderSwitch()
    {
        if (cameraPanTiltComponent.PanAxis.Value <= cameraPanTiltComponent.PanAxis.Range.x && !hasSwitchedShoulderAutomatically)
        {
            ToggleShoulder();
            hasSwitchedShoulderAutomatically = true;
        }
        else if (cameraPanTiltComponent.PanAxis.Value > cameraPanTiltComponent.PanAxis.Range.x + 1f)
        {
            hasSwitchedShoulderAutomatically = false;
        }
    }

    private void SmoothShoulderToggle()
    {
        float targetSide = rightShoulder ? 1f : 0f;
        cameraFollowComponent.CameraSide = Mathf.Lerp(cameraFollowComponent.CameraSide, targetSide, shoulderSwitchSpeed * Time.deltaTime);
        cameraSide = cameraFollowComponent.CameraSide;
    }

    public void ToggleShoulder()
    {
        rightShoulder = !rightShoulder;
    }

    public void GetRay(out Ray ray) => ray = this.ray;
    public void GetRayHit(out RaycastHit hit) => hit = this.hit;

    public bool RayHitHasCollider() => hit.collider == null;
    public bool RayHitHasRigidbody() => hit.rigidbody == null;
}
