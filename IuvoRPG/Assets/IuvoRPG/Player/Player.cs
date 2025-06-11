using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{

    [Header("Player Context")]
    [Tooltip("All player handlers should refer to this context object for operations")]
    [SerializeField] public Context playerContext;

    [Header("Player Handlers")]
    [SerializeField] private PlayerInputHandler inputHandler;   // input preceds all actions

    [SerializeField] private PlayerRotationHandler rotationHandler;     // if input calls for, rotate the player either by script or animation
    [SerializeField] private PlayerMovementHandler movementHandler;     // if input dictated, move the player in some manner (cc.move, jump, dodge)
    [SerializeField] private PlayerAimHandler aimHandler;     // if input is for aim, call on rotHandler to interpolate correct movement
    [SerializeField] private PlayerCameraHandler cameraHandler;     // camera stays focused on player respective of whatever cameraStyle is needed
    [SerializeField] private PlayerUIHandler playerUIHandler;       // correlates data between the player and their HUD
    [SerializeField] private PlayerStatHandler playerStatHandler;   // holds data necessary for certain player conditions and actions


    private void Awake()
    {

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (inputHandler == null) Debug.LogError("InputHandler should not be NULL!");

        if (rotationHandler == null) Debug.LogError("RotationHandler should not be NULL!");

        if (movementHandler == null) Debug.LogError("MovementHandler should not be NULL!");

        if (aimHandler == null) Debug.LogError("AimHandler should not be NULL!");

        if (cameraHandler == null) Debug.LogError("CameraHandler should not be NULL!");

        if (playerUIHandler == null) Debug.LogError("UIHandler should not be NULL!");

        if (playerStatHandler == null) Debug.LogError("StatHandler should not be NULL!");

    }

    // Update is called once per frame
    void Update()
    {
        // did we get some sort of input
        ReceiveInput();

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

        // if any stats have changes to make, make them and report them to the UI
        UpdateStats();
        
    }

    private void UpdateStats()
    {
        //throw new NotImplementedException();
    }

    private void UpdateUI()
    {
        //throw new NotImplementedException();
    }

    private void Aim()
    {
        //throw new NotImplementedException();
    }

    private void Look()
    {
        //throw new NotImplementedException();
    }

    private void Move()
    {
        //throw new NotImplementedException();
    }

    private void Rotate()
    {
        //throw new NotImplementedException();
    }

    private void ReceiveInput()
    {
        //throw new NotImplementedException();
    }



    #region Getters & Setters

    public PlayerInputHandler GetInputHandler() => inputHandler;
    public PlayerUIHandler GetUIHandler() => playerUIHandler;
    public PlayerCameraHandler GetCameraHandler() => cameraHandler;
    public PlayerAimHandler GetAimHandler() => aimHandler;
    public PlayerMovementHandler GetMovementHandler() => movementHandler;
    public PlayerStatHandler GetStatHandler() => playerStatHandler;

    #endregion
}
