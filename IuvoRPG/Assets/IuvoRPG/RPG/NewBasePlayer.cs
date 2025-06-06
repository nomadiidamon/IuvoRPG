using IuvoUnity._Input._Controllers;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(GravityBody))]
public class BasePlayer : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InputSystem_Actions inputActions;
    [SerializeField] private CharacterController characterController;
    [SerializeField] private GravityBody gravityBody;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rb;
    private Transform camTransform;


    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float sprintMultiplier = 2f;
    [Space(2)]
    public float dodgeMultiplier = 1.5f;
    public float dodgeDuration = 0.3f;
    [Space(2)]
    public float jumpHoldForce = 5f;
    public float jumpMaxHoldTime = 1.0f;
    [Space(2)]
    public float crouchSpeed = 2.5f;
    public bool IsCrouching { get; private set; } = false;


    [Header("Rotation Settings")]
    public float moveRotationSpeed = 0.75f;


    [Space(10)]
    [Header("Runtime Values")]
    [SerializeField] private Vector3 movementDir;
    [SerializeField] private Vector3 lookDirection;
    [SerializeField] private Vector3 velocity;
    [SerializeField] private Vector3 currRigidbidy;
    [Space(2)]
    [SerializeField] private float jumpTimer = 0f;
    [Space(2)]
    [SerializeField] private bool isInPhysicsMode = false;
    [SerializeField] private bool isDodging = false;
    [SerializeField] private bool isGrounded;
    [SerializeField] public bool isTakingCover = false;
    [SerializeField] public bool isCrouching = false;
    [SerializeField] private bool isJumping = false;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        gravityBody = GetComponent<GravityBody>();
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;

        gravityBody.onlyApplyGravityWhenAirborne = false; // We'll handle that here.

        SetUpInputs();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        camTransform = SceneManager.Instance.cam.transform;

    }

    void SetUpInputs()
    {
        inputActions = new InputSystem_Actions();
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += ctx =>
        {
            Vector2 moveInput = ctx.ReadValue<Vector2>();
            movementDir = new Vector3(moveInput.x, 0, moveInput.y);

            velocity = movementDir * moveSpeed;
            animator.SetFloat("Velocity", velocity.magnitude);
            animator.SetFloat("DirectionX", moveInput.x);
            animator.SetFloat("DirectionY", moveInput.y);
        };
        inputActions.Player.Move.canceled += ctx =>
        {
            movementDir = Vector3.zero; // Reset movement direction when input is canceled
            velocity = Vector3.zero;
            animator.SetFloat("Velocity", 0f);
            animator.SetFloat("DirectionX", 0f);
            animator.SetFloat("DirectionY", 0f);
        };

        inputActions.Player.Look.performed += ctx =>
        {
            Vector2 lookInput = ctx.ReadValue<Vector2>();
            lookDirection = new Vector3(lookInput.x, 0, lookInput.y).normalized;
            if (lookDirection != Vector3.zero)
            {

            }
        };

        inputActions.Player.Look.canceled += ctx =>
        {
            lookDirection = Vector3.zero; // Reset look direction when input is canceled
        };

        //inputActions.Player.Interact.performed += ctx =>
        //{
        //    if (isInPhysicsMode || isDodging) return;
        //    // Handle interaction logic here
        //    Debug.Log("Interact action performed");
        //};

        inputActions.Player.Crouch.performed += ctx =>
        {
            if (isInPhysicsMode || isDodging) return;
            IsCrouching = !IsCrouching;
            moveSpeed = IsCrouching ? crouchSpeed : moveSpeed; // Adjust speed for crouch
            animator.SetBool("IsCrouching", IsCrouching);
        };

        inputActions.Player.Dodge.performed += ctx =>
        {
            if (isInPhysicsMode || isDodging || !isGrounded) return;
            StartCoroutine(Dodge());
        };





        inputActions.Player.Sprint.performed += ctx =>
        {
            if (isInPhysicsMode || isDodging) return;
            moveSpeed *= sprintMultiplier;
            animator.SetBool("IsSprinting", true);
        };
        inputActions.Player.Sprint.canceled += ctx =>
        {
            if (isInPhysicsMode || isDodging) return;
            moveSpeed /= sprintMultiplier;
            animator.SetBool("IsSprinting", false);
        };

        inputActions.Player.Jump.performed += ctx =>
        {
            if ((isInPhysicsMode || isDodging) && !isGrounded) return;

            isJumping = true;
            EnterPhysicsMode();
        };

        inputActions.Player.Jump.canceled += ctx =>
        {
            isJumping = false;
        };


    }

    void Update()
    {
        if (isInPhysicsMode) return;

        Move();
        isGrounded = characterController.isGrounded;
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

            velocity = moveDir * moveSpeed;
            characterController.Move(velocity * Time.deltaTime);

            if (moveDir.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, moveRotationSpeed * moveSpeed * Time.deltaTime);
            }
        }
        else
        {
            characterController.Move(velocity * Time.deltaTime);
        }
        isGrounded = gravityBody.groundCheck.isGrounded;
    }

    void FixedUpdate()
    {

        Jump();

        if (isInPhysicsMode && isGrounded)
        {
            ExitPhysicsMode();
        }
    }

    public virtual void Jump()
    {
        if (isJumping && jumpTimer < jumpMaxHoldTime)
        {
            // Apply jump force
            rb.AddForce(-gravityBody.currGravDirection * jumpHoldForce, ForceMode.Impulse);
            jumpTimer += Time.fixedDeltaTime;
        }
        else if (isJumping && jumpTimer >= jumpMaxHoldTime)
        {
            // Stop applying jump force after max hold time
            isJumping = false;
            jumpTimer = 0f;
            //ExitPhysicsMode();
        }
        else if (!isJumping)
        {
            jumpTimer = 0f;
        }
    }

    private IEnumerator Dodge()
    {
        isDodging = true;
        EnterPhysicsMode();

        Vector3 dodgeDir = velocity.normalized;
        if (dodgeDir == Vector3.zero)
            dodgeDir = transform.forward;

        transform.forward = dodgeDir;

        rb.AddForce(dodgeDir * velocity.magnitude * dodgeMultiplier, ForceMode.VelocityChange);

        yield return new WaitForSeconds(dodgeDuration);

        rb.linearVelocity = Vector3.zero;
        ExitPhysicsMode();

        isDodging = false;
    }

    private void EnterPhysicsMode()
    {
        characterController.enabled = false;
        rb.isKinematic = false;
        //gravityBody.EnableGravity(); // use gravity from GravityBody
        isInPhysicsMode = true;
    }

    private void ExitPhysicsMode()
    {
        rb.linearVelocity = gravityBody.currGravDirection;
        rb.isKinematic = true;
        //gravityBody.DisableGravity(); // stop applying force when back in CC
        characterController.enabled = true;
        isInPhysicsMode = false;
    }
}
