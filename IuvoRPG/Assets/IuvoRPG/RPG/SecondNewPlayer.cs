using Unity.Cinemachine;
using IuvoUnity._Extensions;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SecondNewPlayer : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;
    [Space(2)]
    [SerializeField] private PlayerUIHandler playerUIManager;
    [SerializeField] private PlayerInputHandler inputManager;
    [SerializeField] private PlayerCameraHandler cameraManager;
    [SerializeField] private Transform camTransform;
    [SerializeField] private Transform playerForward;


    [Header("Movement Settings")]
    private float originalMoveSpeed;
    public float moveSpeed = 5f;
    public float sprintMultiplier = 2f;


    [Header("Rotation Settings")]
    public float moveRotationSpeed = 0.75f;

    [Header("AimSettings")]
    [SerializeField] private SecondPlayerAimHandler aimManager;

    [Header("Custom Gravity Settings")]
    public Vector3 customGravityDirection = Vector3.down;
    public float customGravityStrength = 9.81f;

    [Space(5)]
    [Header("Runtime Values")]
    [SerializeField] public Vector3 movementDir;
    [Space(2)]
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool isSprinting = false;
    [SerializeField] private bool isAiming = false;


    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        originalMoveSpeed = moveSpeed;

        if (camTransform == null)
        {
            camTransform = SceneManager.Instance.cam.transform;
        }

        SetUpInputs();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        aimManager.playerForward = playerForward;
    }

    void SetUpInputs()
    {
        if (inputManager == null)
        {
            Debug.LogError("PlayerInputManager is not assigned.");
            return;
        }

        inputManager.OnMovePerformed.AddListener(OnMoveInput);
        inputManager.OnMoveCanceled.AddListener(OnMoveCanceled);
        inputManager.OnSprintStarted.AddListener(OnSprintStarted);
        inputManager.OnSprintCanceled.AddListener(OnSprintCanceled);
        inputManager.OnAimStarted.AddListener(OnAimStarted);
        inputManager.OnAimCanceled.AddListener(OnAimCanceled);
        inputManager.OnSwitchShoulders.AddListener(OnSwitchShoulders);
    }

    void Update()
    {
        Move();
        aimManager.UpdateAim(isAiming, isMoving, movementDir);
        if (aimManager.playerForward != playerForward)
        {
            aimManager.playerForward = playerForward;
            
        }
    }

    public virtual void Move()
    {
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

            characterController.Move(moveDir * moveSpeed * Time.deltaTime);

            if (moveDir.sqrMagnitude > 0.001f)
            {
                if (!isAiming)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(moveDir);
                    playerForward.transform.rotation = Quaternion.Slerp(playerForward.transform.rotation, targetRotation, moveRotationSpeed * moveSpeed * Time.deltaTime);
                }
            }
            Debug.DrawRay(playerForward.transform.position, moveDir.normalized * 2f, Color.green);
        }
        else
        {
            isMoving = false;
            characterController.Move(customGravityDirection * 0.1f * Time.deltaTime);
        }
        isGrounded = characterController.isGrounded;
    }

    #region InputCallbacks
    void OnMoveInput(Vector2 moveInput)
    {
        movementDir = new Vector3(moveInput.x, 0, moveInput.y);
        animator.SetFloat("DirectionX", moveInput.x);
        animator.SetFloat("DirectionY", moveInput.y);
    }

    void OnMoveCanceled()
    {
        movementDir = Vector3.zero;
        animator.SetFloat("DirectionX", 0f);
        animator.SetFloat("DirectionY", 0f);
    }

    void OnSprintStarted()
    {
        moveSpeed = originalMoveSpeed * sprintMultiplier;
        isSprinting = true;
    }

    void OnSprintCanceled()
    {
        moveSpeed = originalMoveSpeed;
        isSprinting = false;
    }

    void OnAimStarted()
    {
        playerUIManager.AimReticle.transform.localScale *= 2;
        cameraManager.SwitchCameraStyles(CameraStyle.THIRD_PERSON_SHOOTER);
        isAiming = true;
        animator.SetBool("IsAiming", true);
    }

    void OnAimCanceled()
    {
        playerUIManager.AimReticle.transform.localScale /= 2;
        cameraManager.SwitchCameraStyles(CameraStyle.EXPLORATION);
        isAiming = false;
        animator.SetBool("IsAiming", false);
    }

    void OnSwitchShoulders()
    {

        if (isAiming)
        {
            Debug.Log("Shoulder Switched");

           // aimManager.ToggleShoulder();

        }
    }
    #endregion
}
