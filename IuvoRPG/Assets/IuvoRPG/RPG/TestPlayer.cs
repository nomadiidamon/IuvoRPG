using UnityEngine;
using IuvoUnity._Input._Controllers;

public class TestPlayer : PlayerControllerBase
{
    public float speed = 5f;
    public float jumpForce = 5f;
    public bool isOnGround = false;
    //public bool isForcingDown = false;

    protected override void Awake()
    {
        base.Awake();
        SetupInputs();
        IsEnabled = true;
        IsActive = true;
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

            //if (!isOnGround && activelyApplyingGravity)
            //{
            //    isForcingDown = true;
            //}
            //else
            //{
            //    isForcingDown = false;
            //}
        }
    }

    protected override void OnJump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    public bool IsGrounded()
    {
        return groundCheck.isGrounded;
        //return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    protected override void HandleMoveInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(horizontal, vertical).normalized;

    }
}
