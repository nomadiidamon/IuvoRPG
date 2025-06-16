using IuvoUnity._Extensions;
using System;
using UnityEngine;

public class Player : MonoBehaviour
{

    [Header("Player Context")]
    [Tooltip("All player handlers should refer to this context object for operations")]
    [SerializeField] private Context playerContext;

    [Header("Player Handlers")]
    [SerializeField] private PlayerInputHandler inputHandler;   // input preceds all actions
    [SerializeField] private PlayerRotationHandler rotationHandler;     // if input calls for, rotate the player either by script or animation
    [SerializeField] private PlayerMovementHandler movementHandler;     // if input dictated, move the player in some manner (cc.move, jump, dodge)
    [SerializeField] private PlayerAimHandler aimHandler;     // if input is for aim, call on rotHandler to interpolate correct movement
    [SerializeField] private PlayerCameraHandler cameraHandler;     // camera stays focused on player respective of whatever cameraStyle is needed
    [SerializeField] private PlayerUIHandler playerUIHandler;       // correlates data between the player and their HUD
    [SerializeField] private PlayerStatHandler playerStatHandler;   // holds data necessary for certain player conditions and actions
    [SerializeField] private PlayerCombatHandler playerCombatHandler; // handles combat related actions, such as attacks, abilities, etc.

    [SerializeField] Transform bulletSpawnPos;
    [SerializeField] GameObject projectilePrefab;

    private void Awake()
    {
        CheckHandler(inputHandler, nameof(inputHandler));
        CheckHandler(rotationHandler, nameof(rotationHandler));
        CheckHandler(movementHandler, nameof(movementHandler));
        CheckHandler(aimHandler, nameof(aimHandler));
        CheckHandler(cameraHandler, nameof(cameraHandler));
        CheckHandler(playerUIHandler, nameof(playerUIHandler));
        CheckHandler(playerStatHandler, nameof(playerStatHandler));
        CheckHandler(playerCombatHandler, nameof(playerCombatHandler));

        SetUpInputs();
    }

    private void CheckHandler(object handler, string name)
    {
        if (handler == null)
            Debug.LogError($"{name} should not be NULL!");
    }

    void SetUpInputs()
    {
        if (inputHandler == null)
        {
            Debug.LogError("PlayerInputManager is not assigned.");
            return;
        }

        if (movementHandler != null)
        {
            inputHandler.OnMovePerformed.AddListener(movementHandler.OnMoveInput);
            inputHandler.OnMoveCanceled.AddListener(movementHandler.OnMoveCanceled);
            inputHandler.OnSprintStarted.AddListener(movementHandler.OnSprintStarted);
            inputHandler.OnSprintCanceled.AddListener(movementHandler.OnSprintCanceled);
        }

        if (aimHandler != null)
        {
            inputHandler.OnAimStarted.AddListener(aimHandler.OnAimStarted);
            inputHandler.OnAimCanceled.AddListener(aimHandler.OnAimCanceled);
            inputHandler.OnSwitchShoulders.AddListener(aimHandler.OnSwitchShoulders);
        }

        inputHandler.OnLightAttackPerformed.AddListener(SpawnProjectile);

        UpdateAllHandlerContexts();
    }

    private void Start()
    {
        playerContext.Set<Player>(ContextActorKey.Player, this);
        playerContext.Set<Transform>(ContextTransformKey.Transform, transform);
    }

    void Update()
    {
        // Update all handler's context
        UpdateAllHandlerContexts();


        // do any of the inputs we received require us to rotate? if so, rotate
        Rotate();

        // did we recieve some sort of prompt to relocate in any way? if so move?
        Move();

        // did we look a diff direction? if so, are we currently aiming? if so, notify camera
        Look();

        // if the aimHandler notified the camera of an event, switch to the appropriate camera for aiming
        Aim();

        // did the player request any UI functions, if so invoke, else check for HUD updates
        UpdateUI();

        if (aimHandler.rightShoulder)
        {
            bulletSpawnPos = playerCombatHandler.RightHandPos;
        }
        else
        {
            bulletSpawnPos = playerCombatHandler.LeftHandPos;
        }
        
    }

    private void UpdateAllHandlerContexts()
    {
        if (inputHandler != null) UpdateHandlerContext(inputHandler, playerContext);
        if (rotationHandler != null) UpdateHandlerContext(rotationHandler, playerContext);
        if (movementHandler != null) UpdateHandlerContext (movementHandler, playerContext);
        if (aimHandler != null) UpdateHandlerContext(aimHandler, playerContext);
        if (cameraHandler != null) UpdateHandlerContext(cameraHandler, playerContext);
        if (playerUIHandler != null) UpdateHandlerContext(playerUIHandler, playerContext);
        if (playerStatHandler != null) UpdateHandlerContext(playerStatHandler, playerContext);
    }

    private void UpdateHandlerContext(IPlayerHandler handler, Context playerContext)
    {
        handler.playerContext = playerContext;
    }



    private void UpdateUI()
    {
        playerUIHandler.Update();
    }

    private void Aim()
    {
        aimHandler.UpdateAim(aimHandler.isAiming);
    }

    private void Look()
    {
        cameraHandler.Update();
    }

    private void Move()
    {
        movementHandler.Update();
    }

    private void Rotate()
    {
        rotationHandler.Update();
    }


    public void SpawnProjectile()
    {

        GameObject instance = Instantiate(projectilePrefab, bulletSpawnPos.position, Quaternion.identity);
        ProjectileBase projectileBase = instance.GetComponent<ProjectileBase>();
        if (projectileBase == null)
        {
            Debug.LogError("Projectile prefab needs a ProjectileBase component.");
            return;
        }

        // Set the projectile's owner and target
        projectileBase.projectileOwner = transform;

        // Retrieve the aim target
        if (!playerContext.TryGet<PlayerAimHandler>(ContextPlayerHandlerKey.AimHandler, out PlayerAimHandler aim))
        {
            Debug.LogError("AimHandler is not set in PlayerContext.");
            return;
        }

        Rigidbody rb = projectileBase.GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Projectile prefab needs a Rigidbody component.");
            return;
        }
        projectileBase.spawnPosition = bulletSpawnPos.position;
        projectileBase.projectileTarget.position = aimHandler.AimTarget;
        aimHandler.gameObject.SetActive(false);

        projectileBase.Spawn();

        // Get the Rigidbody component from the instance
        aimHandler.gameObject.SetActive(true);
    }


    #region Getters & Setters

    public Context GetPlayerContext() => playerContext;
    public PlayerInputHandler GetInputHandler() => inputHandler;
    public PlayerUIHandler GetUIHandler() => playerUIHandler;
    public PlayerCameraHandler GetCameraHandler() => cameraHandler;
    public PlayerAimHandler GetAimHandler() => aimHandler;
    public PlayerMovementHandler GetMovementHandler() => movementHandler;
    public PlayerStatHandler GetStatHandler() => playerStatHandler;

    #endregion
}
