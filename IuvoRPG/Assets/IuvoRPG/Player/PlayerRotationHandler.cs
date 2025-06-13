using UnityEngine;

public class PlayerRotationHandler : MonoBehaviour, IPlayerHandler
{
    [SerializeField] Transform playerTransform;
    [SerializeField] PlayerCameraHandler cameraHandler;
    [SerializeField] PlayerAimHandler aimHandler;
    [SerializeField] PlayerMovementHandler movementHandler;

    [SerializeField] Transform aimTargetTransform;

    [SerializeField] float moveRotationSpeed = 5.5f;
    [SerializeField] float aimRotationSpeed = 3.0f;

    [SerializeField] public Context playerContext { get; set; }


    public void Start()
    {
        if (aimTargetTransform == null) Debug.LogError("AimTargetTransform can not be NULL");
        if (playerTransform == null) Debug.LogError("PlayerTransform cannot be NULL");
        if (cameraHandler == null) Debug.LogError("CameraHandler cannot be NULL");
        if (aimHandler == null) Debug.LogError("AimHandler cannot be NULL");
        if (movementHandler == null) Debug.LogError("MovementHandler cannot be NULL");
    }

    public void Update()
    {
        Rotate();
    }

    public void Rotate()
    {
        var cameraStyle = cameraHandler.GetCurrentCameraStyle();

        switch (cameraStyle)
        {
            case CameraStyle.EXPLORATION:
                RotateForMoving();
                break;
            case CameraStyle.THIRD_PERSON_SHOOTER:
                RotateForAiming();
                break;

            // Add more cases for other camera styles as needed
            default:
                RotateForMoving();
                break;
        }
    }

    public void RotateForAiming()
    {

    }

    public void RotateForMoving()
    {
        if (!playerContext.TryGet<bool>(ContextStateKey.IsMoving, out bool isMoving) || !isMoving)
            return;

        if (!playerContext.TryGet<Vector3>(ContextTransformKey.Direction, out Vector3 lastMoveDirection))
            return;

        if (lastMoveDirection == Vector3.zero)
            return;

        // Rotate the actual player transform toward the direction of movement
        
    }

}

