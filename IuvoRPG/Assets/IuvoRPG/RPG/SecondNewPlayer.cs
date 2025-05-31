using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SecondNewPlayer : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InputSystem_Actions inputActions;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerCameraManager cameraManager;
    [SerializeField] private Transform camTransform;


    [Header("Movement Settings")]
    private float originalMoveSpeed;
    public float moveSpeed = 5f;
    public float sprintMultiplier = 2f;


    [Header("Rotation Settings")]
    public float moveRotationSpeed = 0.75f;

    [Header("Custom Gravity Settings")]
    public Vector3 customGravityDirection = Vector3.down;
    public float customGravityStrength = 9.81f;

    [Space(5)]
    [Header("Runtime Values")]
    [SerializeField] private Vector3 movementDir;
    [Space(2)]
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private bool isSprinting = false;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        originalMoveSpeed = moveSpeed;
        SetUpInputs();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (camTransform == null)
        {
            camTransform = SceneManager.Instance.cam.transform;
        }
        
    }

    void SetUpInputs()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += ctx =>
        {
            Vector2 moveInput = ctx.ReadValue<Vector2>();
            movementDir = new Vector3(moveInput.x, 0, moveInput.y);

            animator.SetFloat("DirectionX", moveInput.x);
            animator.SetFloat("DirectionY", moveInput.y);
        };
        inputActions.Player.Move.canceled += ctx =>
        {
            movementDir = Vector3.zero; 
            animator.SetFloat("DirectionX", 0f);
            animator.SetFloat("DirectionY", 0f);
        };


        inputActions.Player.Sprint.performed += ctx =>
        {
            moveSpeed = originalMoveSpeed * sprintMultiplier;
            isSprinting = true;
        };
        inputActions.Player.Sprint.canceled += ctx =>
        {
            moveSpeed = originalMoveSpeed;
            isSprinting = false;
        };

        inputActions.Player.Aim.performed += ctx =>
        {
            cameraManager.SwitchCameraStyles(CameraStyle.THIRD_PERSON_SHOOTER);
        };
        inputActions.Player.Aim.canceled += ctx =>
        {
            cameraManager.SwitchCameraStyles(CameraStyle.EXPLORATION);
        };
    }


    void Update()
    {
        Move();        
    }



    public virtual void Move()
    {
        if (movementDir.magnitude > 0.1f)
        {
            // Calculate the camera-relative movement direction
            Vector3 camForward = camTransform.forward;
            camForward.y = 0;
            camForward.Normalize();

            Vector3 camRight = camTransform.right;
            camRight.y = 0;
            camRight.Normalize();

            Vector3 moveDir = camForward * movementDir.z + camRight * movementDir.x;
            moveDir.Normalize();

            characterController.Move(moveDir * moveSpeed * Time.deltaTime);

            if (moveDir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, moveRotationSpeed * moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            characterController.Move(customGravityDirection * 0.1f * Time.deltaTime);
        }
        isGrounded = characterController.isGrounded;
    }

    void FixedUpdate()
    {


    }

}
