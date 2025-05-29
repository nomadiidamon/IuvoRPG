using UnityEngine;
using IuvoUnity._Input._Controllers;
using IuvoUnity._BaseClasses._InputBases._Legacy;
using IuvoUnity._DataStructs;

public class TestPlayer : PlayerControllerBase
{
    public float speed = 5f;
    public float jumpForce = 5f;
    public float jumpReEnter = 0.5f;
    public float currentJumpTime;
    public bool isOnGround = false;
    //public bool isForcingDown = false;
    [SerializeField] public CapsuleCollider capsuleCollider;

    public float levitateForce = 0.75f;
    public KeyHoldInputAction levitateInput;

    Animator animator;

    protected override void SetupInputs()
    {
        base.SetupInputs();
        levitateInput = gameObject.AddComponent<KeyHoldInputAction>();
        levitateInput.SetKey(KeyCode.E);
        levitateInput.neededHoldTime = 0.20f;
        levitateInput.holdTimeRange = (new RangeF(levitateInput.neededHoldTime, 1.25f));

        levitateInput.name = "Levitate Action";
        levitateInput.OnPerformed += OnLevitate;
    }

    protected override void Awake()
    {
        base.Awake();
        SetupInputs();
        IsEnabled = true;
        IsActive = true;
        animator = GetComponentInParent<Animator>();

        if (capsuleCollider != null)
        {
            groundCheck.checkOrigin = capsuleCollider.bounds.min;
            groundCheck.distanceToCheck = 0.75f;
            groundCheck.radiusToCheck = capsuleCollider.radius;
        }
        levitateForce = jumpForce / 3.0f;

    }

    protected override void Update()
    {
        if (IsActive && IsEnabled)
        {
            HandleMoveInput();
            HandleMovement();
            if (isOnGround != IsGrounded())
            {
                isOnGround = IsGrounded();
            }
        }
    }

    protected override void OnJump()
    {
        if (currentJumpTime == 0 || (currentJumpTime - Time.time) > jumpReEnter)
        {
            currentJumpTime = Time.time;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetTrigger("StartJump");
            return;
        }

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        currentJumpTime = 0;

    }

    protected virtual void OnLevitate()
    {
        animator.SetTrigger("StartJump");

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Acceleration);

    }

    public bool IsGrounded()
    {
        if (!groundCheck.isGrounded)
        {
            animator.SetBool("IsFalling", true);
            isOnGround = false;
            return false;
        }
        else
        {
            isOnGround = true;
            animator.SetBool("IsGrounded", true);
            animator.SetBool("IsFalling", false);
            return true;
        }
    }

    protected override void HandleMoveInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(horizontal, vertical).normalized;
        animator.SetFloat("Direction", horizontal);
        animator.SetFloat("Velocity", vertical);

    }
}
