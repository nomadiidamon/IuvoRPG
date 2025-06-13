using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour, IPlayerHandler
{
    [SerializeField] private CharacterController playerCharacterController;
    [SerializeField] private PlayerAnimatorHandler playerAnimatorHandler;
    [SerializeField] private PlayerCameraHandler playerCameraHandler;
    [SerializeField] private Agility playerAgility;

    [SerializeField] public Context playerContext { get; set; }


    [Header("Custom Gravity Settings")]
    public Vector3 customGravityDirection = Vector3.down;
    public float customGravityStrength = 9.81f;

    [Header("Runtime Values")]
    [SerializeField] public Vector3 movementDir;
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

            Transform activeCam = playerCameraHandler.GetCurrentCameraTransform();
            Vector3 camForward = activeCam.forward;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 camRight = activeCam.right;
            camRight.y = 0;
            camRight.Normalize();


            Vector3 moveDir = camForward * movementDir.z + camRight * movementDir.x;
            moveDir.Normalize();

            playerContext.Set<Vector3>(ContextTransformKey.Direction, moveDir);


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
        movementDir = new Vector3(moveInput.x, 0, moveInput.y);
            playerAnimatorHandler.animator.SetFloat("DirectionX", movementDir.x);
            playerAnimatorHandler.animator.SetFloat("DirectionY", movementDir.z);
    }

    public void OnMoveCanceled()
    {
        movementDir = Vector3.zero;
            playerAnimatorHandler.animator.SetFloat("DirectionX", 0f);
            playerAnimatorHandler.animator.SetFloat("DirectionY", 0f);
    }

    public void OnSprintStarted()
    {
        isSprinting = true;
    }

    public void OnSprintCanceled()
    {
        isSprinting = false;
    }
}
