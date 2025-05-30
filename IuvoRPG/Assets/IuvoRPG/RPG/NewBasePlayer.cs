using IuvoUnity._Input._Controllers;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(GravityBody))]
public class BasePlayer : MonoBehaviour 
{
    [Header("Components")]
    private CharacterController characterController;
    private GravityBody gravityBody;
    private Rigidbody rb;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float moveRotationSpeed = 0.75f;
    public float dodgeMultiplier = 1.5f;
    public float dodgeDuration = 0.3f;
    public float jumpHoldForce = 5f;
    public float jumpHoldDuration = 1.0f;

    [Header("Runtime Values")]
    private Vector3 inputDir;
    private Vector3 velocity;
    private bool isInPhysicsMode = false;
    private bool isDodging = false;
    private bool isJumping = false;
    private float jumpTimer = 0f;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        gravityBody = GetComponent<GravityBody>();
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;

        gravityBody.onlyApplyGravityWhenAirborne = false; // We'll handle that here.
    }

    void Update()
    {
        if (isInPhysicsMode) return;

        // Movement input
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        inputDir = new Vector3(h, 0f, v).normalized;

        // Face movement direction
        if (inputDir.magnitude > 0.1f)
        {
            transform.forward = Vector3.Lerp(base.transform.forward, inputDir, ((moveRotationSpeed * moveSpeed) * Time.deltaTime));
        }

        // Simulate custom gravity movement
        Vector3 gravity = gravityBody.currGravDirection;
        velocity += gravity * Time.deltaTime;

        // Move with CharacterController
        Vector3 finalMove = inputDir * moveSpeed;// + velocity;
        characterController.Move(finalMove * Time.deltaTime);

        // Store current velocity
        velocity = characterController.velocity;

        // Dodge (tap)
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDodging)
        {
            StartCoroutine(Dodge());
        }

        // Jump (hold)
        if (Input.GetKey(KeyCode.Space) && characterController.isGrounded)
        {
            if (!isJumping)
            {
                isJumping = true;
                jumpTimer = 0f;
            }
        }
        else if (isJumping)
        {
            isJumping = false;
        }
    }

    void FixedUpdate()
    {
        // Jump force application
        if (!isInPhysicsMode && isJumping && jumpTimer < jumpHoldDuration)
        {
            jumpTimer += Time.fixedDeltaTime;
            Vector3 jumpDir = -gravityBody.currGravDirection.normalized;
            characterController.Move(jumpDir * jumpHoldForce * Time.fixedDeltaTime);
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
        gravityBody.EnableGravity(); // use gravity from GravityBody
        isInPhysicsMode = true;
    }

    private void ExitPhysicsMode()
    {
        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
        gravityBody.DisableGravity(); // stop applying force when back in CC
        characterController.enabled = true;
        isInPhysicsMode = false;
    }
}
