using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour, IPlayerHandler
{
    [SerializeField] private CharacterController playerCharacterController;
    [SerializeField] private PlayerAnimatorHandler playerAnimatorHandler;
    [SerializeField] private PlayerCameraHandler playerCameraHandler;

    [SerializeField] public Context playerContext { get; set; }


    [Header("Custom Gravity Settings")]
    public Vector3 customGravityDirection = Vector3.down;
    public float customGravityStrength = 9.81f;

    [Header("Runtime Values")]
    [SerializeField] public Vector3 movementDir;
    [SerializeField] private float velocity;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool isSprinting = false;
    public void Start()
    {

    }

    public void Update()
    {
        Move();
    }

    public virtual void Move()
    {
        isGrounded = playerCharacterController.isGrounded;
        playerContext.Set<bool>(ContextStateKey.IsGrounded, isGrounded);

        if (movementDir.magnitude > 0.1f)
        {
            isMoving = true;
            playerContext.Set<bool>(ContextStateKey.IsMoving, isMoving);
            CharacterStats stats;
            Agility playerAgility;
            playerContext.TryGet<CharacterStats>(ContextStatKey.PlayerStats, out stats);
            playerAgility = stats.GetCharacterAgility();

            float targetAngle = Mathf.Atan2(movementDir.x, movementDir.z) * Mathf.Rad2Deg + playerCameraHandler.GetMainCamera().transform.eulerAngles.y;
            Transform playerTransform;
            playerContext.TryGet<Transform>(ContextTransformKey.Transform, out playerTransform);
            float angle = Mathf.SmoothDampAngle(playerTransform.eulerAngles.y, targetAngle, ref velocity, playerAgility.GetMoveRotationSpeed());

            Vector3 moveDir = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
            playerContext.Set<Vector3>(ContextTransformKey.LastDirection, moveDir);
            playerCharacterController.Move(moveDir * playerAgility.GetMoveSpeed(isSprinting) * Time.deltaTime);
        }
        else
        {
            isMoving = false;
            playerContext.Set<bool>(ContextStateKey.IsMoving, isMoving);
        }
        playerCharacterController.Move(customGravityDirection * customGravityStrength * Time.deltaTime);
    }

    public void OnMoveInput(Vector2 moveInput)
    {
        movementDir = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        playerContext.Set<Vector3>(ContextTransformKey.InputDirection, movementDir);
        playerAnimatorHandler.animator.SetFloat("DirectionX", movementDir.x);
            playerAnimatorHandler.animator.SetFloat("DirectionY", movementDir.z);
    }

    public void OnMoveCanceled()
    {
        movementDir = Vector3.zero;
        playerContext.Set<Vector3>(ContextTransformKey.InputDirection, movementDir);
        playerAnimatorHandler.animator.SetFloat("DirectionX", 0f);
            playerAnimatorHandler.animator.SetFloat("DirectionY", 0f);
    }

    public void OnSprintStarted()
    {
        isSprinting = true;
        playerContext.Set<bool>(ContextStateKey.IsSprinting, isSprinting);
    }

    public void OnSprintCanceled()
    {
        isSprinting = false;
        playerContext.Set<bool>(ContextStateKey.IsSprinting, isSprinting);
    }
}
