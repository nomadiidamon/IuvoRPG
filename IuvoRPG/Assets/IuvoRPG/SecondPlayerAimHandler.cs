using UnityEngine;
using Unity.Cinemachine;
using UnityEditor.Rendering.LookDev;

public class SecondPlayerAimHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Transform playerTransform;
    [SerializeField] private Transform camTransform;
    [SerializeField] private PlayerCameraHandler cameraManager;
    [SerializeField] private CinemachinePositionComposer positionComposer;


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
        cameraSide = positionComposer.TargetOffset.x;
    }

    public void UpdateAim(bool isAiming, bool isMoving, Vector3 playerMoveDir)
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


            Vector3 lookDir;
            if (isMoving && moveDir.sqrMagnitude > 0.001f)
            {
                lookDir = moveDir.normalized;
            }
            else
            {
                lookDir = camForward;
            }

            if (lookDir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDir);
                playerTransform.rotation = Quaternion.Slerp(
                    playerTransform.rotation,
                    targetRotation,
                    aimRotationSpeed * Time.deltaTime
                );
            }
            Debug.DrawRay(playerTransform.position, lookDir * 2f, Color.red);
        }

        //SmoothShoulderToggle();
    }



    private void SmoothShoulderToggle()
    {
        float targetSide = rightShoulder ? cameraSide : -cameraSide;
        positionComposer.TargetOffset.x = Mathf.Lerp(positionComposer.TargetOffset.x, targetSide, shoulderSwitchSpeed * Time.deltaTime);
    }

    public void ToggleShoulder()
    {
        rightShoulder = !rightShoulder;
        SmoothShoulderToggle();
    }

    public void GetRay(out Ray ray) => ray = this.ray;
    public void GetRayHit(out RaycastHit hit) => hit = this.hit;

    public bool RayHitHasCollider() => hit.collider == null;
    public bool RayHitHasRigidbody() => hit.rigidbody == null;
}
