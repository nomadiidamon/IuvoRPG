using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent (typeof(InputSystem_Actions))]
public class PlayerInputHandler : MonoBehaviour
{
    private InputSystem_Actions inputActions;

    public FlexibleEvent<Vector2> OnMovePerformed = new FlexibleEvent<Vector2>();
    public FlexibleEvent OnMoveCanceled = new FlexibleEvent();

    public FlexibleEvent OnSprintStarted = new FlexibleEvent();
    public FlexibleEvent OnSprintCanceled = new FlexibleEvent();

    public FlexibleEvent OnAimStarted = new FlexibleEvent();
    public FlexibleEvent OnAimCanceled = new FlexibleEvent();

    public FlexibleEvent OnSwitchShoulders = new FlexibleEvent();

    private void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += HandleMovePerformed;
        inputActions.Player.Move.canceled += HandleMoveCanceled;

        inputActions.Player.Sprint.performed += HandleSprintStarted;
        inputActions.Player.Sprint.canceled += HandleSprintCanceled;

        inputActions.Player.Aim.performed += HandleAimStarted;
        inputActions.Player.Aim.canceled += HandleAimCanceled;

        inputActions.Player.SwitchShoulders.performed += HandleSwitchShoulders;
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();

        inputActions.Player.Move.performed -= HandleMovePerformed;
        inputActions.Player.Move.canceled -= HandleMoveCanceled;

        inputActions.Player.Sprint.performed -= HandleSprintStarted;
        inputActions.Player.Sprint.canceled -= HandleSprintCanceled;

        inputActions.Player.Aim.performed -= HandleAimStarted;
        inputActions.Player.Aim.canceled -= HandleAimCanceled;

        inputActions.Player.SwitchShoulders.performed -= HandleSwitchShoulders;
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
