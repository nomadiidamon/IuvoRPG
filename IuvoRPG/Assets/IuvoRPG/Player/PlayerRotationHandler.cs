using Unity.Mathematics;
using UnityEngine;

public class PlayerRotationHandler : MonoBehaviour, IPlayerHandler
{
    [SerializeField] Transform playerTransform;
    [SerializeField] PlayerCameraHandler cameraHandler;
    [SerializeField] PlayerAimHandler aimHandler;
    [SerializeField] PlayerMovementHandler movementHandler;
    [SerializeField] Transform aimTargetTransform;

    [SerializeField] private float MoveRotVelocity;
    [SerializeField] private float AimRotVelocity;


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
        playerContext.Set<Transform>(ContextTransformKey.Transform, playerTransform);
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
        if (!playerContext.TryGet<bool>(ContextStateKey.IsAiming, out bool isAiming) || !isAiming)
            return;

        if (!playerContext.TryGet<CharacterStats>(ContextStatKey.PlayerStats, out CharacterStats stats))
            return;

        Agility playerAgility = stats.GetCharacterAgility(); playerAgility = stats.GetCharacterAgility();

        playerContext.TryGet<bool>(ContextStateKey.IsSprinting, out bool isSprinting);

        playerContext.TryGet<bool>(ContextStateKey.IsMoving, out bool isMoving);

        float targetAngle = GetTargetAngle(aimHandler.AimTarget);

        float angle = 0.0f;
        if (isMoving)
        {
            angle = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, targetAngle, ref AimRotVelocity, playerAgility.GetAimRotationSpeed() * playerAgility.GetMoveSpeed(isSprinting));
        }
        else
        {
            angle = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, targetAngle, ref AimRotVelocity, playerAgility.GetAimRotationSpeed());
        }
        playerTransform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

    }

    public void RotateForMoving()
    {
        if (!playerContext.TryGet<bool>(ContextStateKey.IsMoving, out bool isMoving) || !isMoving)
            return;

        if (!playerContext.TryGet<Vector3>(ContextTransformKey.InputDirection, out Vector3 lastMoveDirection))
            return;

        if (lastMoveDirection == Vector3.zero)
            return;

        if (!playerContext.TryGet<CharacterStats>(ContextStatKey.PlayerStats, out CharacterStats stats))
            return;

        Agility playerAgility = stats.GetCharacterAgility();

        float targetAngle = GetTargetAngle(lastMoveDirection);

        bool isSprinting;
        playerContext.TryGet<bool>(ContextStateKey.IsSprinting, out isSprinting);
        float angle = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, targetAngle, ref MoveRotVelocity, playerAgility.GetMoveRotationSpeed() * playerAgility.GetMoveSpeed(isSprinting));
        playerTransform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);

    }


    public float GetTargetAngle(Vector3 direction)
    {
        return Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraHandler.GetMainCamera().transform.eulerAngles.y;
    }
}

