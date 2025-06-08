using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Components")]
    [SerializeField] public CharacterController characterController;

    [Header("Player Context")]
    [Tooltip("All player handlers should refer to this context object for operations")]
    [SerializeField] public Context playerContext;

    [Header("Player Handlers")]
    [SerializeField] private PlayerInputHandler inputHandler;
    [SerializeField] private PlayerUIHandler playerUIHandler;
    [SerializeField] private PlayerCameraHandler cameraHandler;
    [SerializeField] private PlayerAimHandler aimHandler;
    [SerializeField] private PlayerMovementHandler movementHandler;
    [SerializeField] private PlayerStatHandler playerStatHandler;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
