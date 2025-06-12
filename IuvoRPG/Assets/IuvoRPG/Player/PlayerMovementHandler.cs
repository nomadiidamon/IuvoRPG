using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour, IPlayerHandler
{
    [SerializeField] private CharacterController playerCharacterController;
    [SerializeField] private PlayerAnimatorHandler playerAnimatorHandler;
    [SerializeField] private Transform camTransform;
    [SerializeField] private Transform playerForward;
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
        if (movementDir.magnitude > 0.1f)
        {
            isMoving = true;
            // Calculate the camera-relative movement direction
            Vector3 camForward = camTransform.forward;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 camRight = camTransform.right;
            camRight.y = 0;
            camRight.Normalize();

            Vector3 moveDir = camForward * movementDir.z + camRight * movementDir.x;
            moveDir.Normalize();

            playerCharacterController.Move(moveDir * playerAgility.GetMoveSpeed(isSprinting) * Time.deltaTime);

            Debug.DrawRay(playerForward.transform.position, moveDir.normalized * 2f, Color.green);
        }
        else
        {
            isMoving = false;
        }
        playerCharacterController.Move(customGravityDirection * customGravityStrength * Time.deltaTime);
    }

    public void OnMoveInput(Vector2 moveInput)
    {
        movementDir = new Vector3(moveInput.x, 0, moveInput.y);
        playerAnimatorHandler.animator.SetFloat("DirectionX", moveInput.x);
        playerAnimatorHandler.animator.SetFloat("DirectionY", moveInput.y);
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
