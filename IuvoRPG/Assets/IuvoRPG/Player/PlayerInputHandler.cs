using System;
using UnityEngine;
using UnityEngine.InputSystem;

// TODO: Transition class from Monobehaviour to POCO
[RequireComponent (typeof(InputSystem_Actions))]
public class PlayerInputHandler : MonoBehaviour, IPlayerHandler
{
    private InputSystem_Actions inputActions;

    private InputBuffer inputBuffer = new InputBuffer();
    private InputComboParser comboParser = new InputComboParser();



    public FlexibleEvent<Vector2> OnMovePerformed = new FlexibleEvent<Vector2>();
    public FlexibleEvent OnMoveCanceled = new FlexibleEvent();

    public FlexibleEvent OnSprintStarted = new FlexibleEvent();
    public FlexibleEvent OnSprintCanceled = new FlexibleEvent();

    public FlexibleEvent OnAimStarted = new FlexibleEvent();
    public FlexibleEvent OnAimCanceled = new FlexibleEvent();

    public FlexibleEvent OnSwitchShoulders = new FlexibleEvent();

    [SerializeField] public Context playerContext { get; set; }
    public ContextPlayerHandlerKey HandlerKey => ContextPlayerHandlerKey.InputHandler;



    private void Awake()
    {
        inputActions = GetComponent<InputSystem_Actions>();
        if (inputActions == null)
            Debug.LogError($"{nameof(PlayerInputHandler)} is missing {nameof(InputSystem_Actions)} component.");

        InitializeCombos();
    }


    public void Update()
    {
        comboParser.Parse(inputBuffer);
    }

    private void SubscribeInputs()
    {
        var player = inputActions.Player;

        player.Move.performed += HandleMovePerformed;
        player.Move.canceled += HandleMoveCanceled;

        player.Sprint.performed += HandleSprintStarted;
        player.Sprint.canceled += HandleSprintCanceled;

        player.Aim.performed += HandleAimStarted;
        player.Aim.canceled += HandleAimCanceled;

        player.SwitchShoulders.performed += HandleSwitchShoulders;
    }

    private void UnsubscribeInputs()
    {
        var player = inputActions.Player;

        player.Move.performed -= HandleMovePerformed;
        player.Move.canceled -= HandleMoveCanceled;

        player.Sprint.performed -= HandleSprintStarted;
        player.Sprint.canceled -= HandleSprintCanceled;

        player.Aim.performed -= HandleAimStarted;
        player.Aim.canceled -= HandleAimCanceled;

        player.SwitchShoulders.performed -= HandleSwitchShoulders;
    }

    private void InitializeCombos()
    {

    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        SubscribeInputs();
    }

    private void OnDisable()
    {
        UnsubscribeInputs();
        inputActions.Player.Disable();
    }

    // Input callback methods
    private void HandleMovePerformed(InputAction.CallbackContext ctx)
    {
        OnMovePerformed.Invoke(ctx.ReadValue<Vector2>());
    }

    private void HandleMoveCanceled(InputAction.CallbackContext ctx)
    {
        OnMoveCanceled.Invoke();
    }

    private void HandleSprintStarted(InputAction.CallbackContext ctx)
    {
        OnSprintStarted.Invoke();
    }

    private void HandleSprintCanceled(InputAction.CallbackContext ctx)
    {
        OnSprintCanceled.Invoke();
    }

    private void HandleAimStarted(InputAction.CallbackContext ctx)
    {
        OnAimStarted.Invoke();
    }

    private void HandleAimCanceled(InputAction.CallbackContext ctx)
    {
        OnAimCanceled.Invoke();
    }

    private void HandleSwitchShoulders(InputAction.CallbackContext ctx)
    {
        OnSwitchShoulders.Invoke();
    }
}
