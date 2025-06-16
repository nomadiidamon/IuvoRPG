using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using Unity.VisualScripting;

// TODO: Transition class from Monobehaviour to POCO
public class PlayerAimHandler : MonoBehaviour, IPlayerHandler
{
    [Header("References")]
    [SerializeField] private PlayerAnimatorHandler playerAnimatorHandler;
    [SerializeField] private PlayerUIHandler playerUIHandler;
    [SerializeField] public Transform playerTransform;
    [SerializeField] private Transform debugTransform;


    [SerializeField] private PlayerCameraHandler cameraHandler;
    [SerializeField] private CinemachinePanTilt cameraPanTiltComponent;
    [SerializeField] private CinemachinePositionComposer positionComposer;

    public Context playerContext { get; set; }
    public ContextPlayerHandlerKey HandlerKey => ContextPlayerHandlerKey.AimHandler;



    [Header("Settings")]
    [SerializeField] private float maxAimDistance = 100f;
    [SerializeField] private float aimSpeed = 2.5f;
    [SerializeField] private float lookAtAimTargetSpeed = 5.0f;
    [SerializeField] private float shoulderSwitchSpeed = 0.75f;
    [SerializeField] private bool isSwitchingShoulders = false;
    [SerializeField] private LayerMask aimColliderLayer = new LayerMask();

    [Header("Runtime Values")]
    [SerializeField] public bool isAiming = false;
    [SerializeField] public bool rightShoulder = true;
    [SerializeField] public Vector3 originalTargetOffset = Vector3.zero;


    private Ray ray;
    private RaycastHit hit;

    public Vector3 AimTarget { get; private set; }
    public float AimDistance { get; private set; }

    private CameraStyle CurrentCameraStyle => cameraHandler.GetCurrentCameraStyle();

    public void Awake()
    {
        if (cameraHandler != null && cameraHandler.tpsCam != null)
        {
            if (cameraPanTiltComponent == null)
            {
                cameraPanTiltComponent = cameraHandler.tpsCam.GetComponent<CinemachinePanTilt>();
            }

            if (positionComposer == null)
            {
                positionComposer = cameraHandler.tpsCam.GetComponent<CinemachinePositionComposer>();
            }

            if (debugTransform == null)
            {
                Debug.LogWarning("Debug Tansform is NULL");
            }
        }
    }

    public void Start()
    {
        originalTargetOffset = positionComposer.TargetOffset;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerContext.Set<PlayerAimHandler>(ContextPlayerHandlerKey.AimHandler, this);
    }

    public void Update()
    {
        UpdateAim(isAiming);

    }

    #region Input Callbacks
    public void OnAimStarted()
    {
        playerUIHandler.AimReticle.transform.localScale *= 2;
        cameraHandler.SwitchCameraStyles(CameraStyle.THIRD_PERSON_SHOOTER);
        isAiming = true;
        if (rightShoulder) playerAnimatorHandler.animator.SetBool("IsAimRightShoulder", true);
        else playerAnimatorHandler.animator.SetBool("IsAimRightShoulder", false);
        int layerIndex = playerAnimatorHandler.animator.GetLayerIndex("Aim");
        StartCoroutine(StartAiming(layerIndex, aimSpeed));
        playerContext.Set<bool>(ContextStateKey.IsAiming, isAiming);
    }
    public void OnAimCanceled()
    {
        playerUIHandler.AimReticle.transform.localScale /= 2;
        cameraHandler.SwitchCameraStyles(CameraStyle.EXPLORATION);
        isAiming = false;
        int layerIndex = playerAnimatorHandler.animator.GetLayerIndex("Aim");
        StartCoroutine(StopAiming(layerIndex, aimSpeed));
        playerContext.Set<bool>(ContextStateKey.IsAiming, isAiming);

    }
    public void OnSwitchShoulders()
    {
        if (isAiming)
        {
            Debug.Log("Shoulder Switched");
            ToggleAimShoulder();
        }
    }
    public void ToggleAimShoulder()
    {
        if (isSwitchingShoulders) return;

        rightShoulder = !rightShoulder;
        float targetX = rightShoulder ? Mathf.Abs(originalTargetOffset.x) : -Mathf.Abs(originalTargetOffset.x);
        StartCoroutine(SwitchShoulder(shoulderSwitchSpeed, targetX));
    }
    private IEnumerator SwitchShoulder(float duration, float targetX)
    {
        float time = 0f;
        float startX = positionComposer.TargetOffset.x;
        bool paramSet = false;
        float triggerTime = duration * (2f / 3f);

        isSwitchingShoulders = true;

        while (time < duration)
        {
            float newX = Mathf.Lerp(startX, targetX, time / duration);
            var offset = positionComposer.TargetOffset;
            offset.x = newX;
            positionComposer.TargetOffset = offset;

            // Set animator param after trigger time has been reached
            if (!paramSet && time >= triggerTime)
            {
                playerAnimatorHandler.animator.SetBool("IsAimRightShoulder", rightShoulder);
                paramSet = true;
            }

            time += Time.deltaTime;
            yield return null;
        }

        var finalOffset = positionComposer.TargetOffset;
        finalOffset.x = targetX;
        positionComposer.TargetOffset = finalOffset;
        isSwitchingShoulders = false;
    }
    private IEnumerator StartAiming(int layerIndex, float duration)
    {
        float time = 0f;

        float startingWeight = 0f;
        float targetWeight = 1f;

        while (time < duration)
        {
            // Get the value of the weight between 0 and 1 based on the accumulated time.
            float layerWeight = Mathf.Lerp(startingWeight, targetWeight, time / duration);
            // Set the weight of the layer based on its index.
            playerAnimatorHandler.animator.SetLayerWeight(layerIndex, layerWeight);
            time += Time.deltaTime;
            // this pauses the coroutine until the next frame.
            yield return null;
        }
        playerAnimatorHandler.animator.SetLayerWeight(layerIndex, targetWeight);
        playerAnimatorHandler.animator.SetBool("IsAiming", true);
    }
    private IEnumerator StopAiming(int layerIndex, float duration)
    {
        float time = 0f;

        float startingWeight = 1f;
        float targetWeight = 0f;

        while (time < duration)
        {
            // Get the value of the weight between 1 and 0 based on the accumulated time.
            float layerWeight = Mathf.Lerp(startingWeight, targetWeight, time / duration);
            // Set the weight of the layer based on its index.
            playerAnimatorHandler.animator.SetLayerWeight(layerIndex, layerWeight);
            time += Time.deltaTime;
            // this pauses the coroutine until the next frame.
            yield return null;
        }
        playerAnimatorHandler.animator.SetLayerWeight(layerIndex, targetWeight);
        playerAnimatorHandler.animator.SetBool("IsAiming", false);
    }
    #endregion


    public void UpdateAim(bool isAiming)
    {
        Vector3 mouseWorldPosition = Vector3.zero;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        ray = cameraHandler.GetMainCamera().ScreenPointToRay(screenCenterPoint);

        if (Physics.Raycast(ray, out hit, maxAimDistance, aimColliderLayer))
        {
            debugTransform.position = hit.point;
            mouseWorldPosition = hit.point;
            AimTarget = hit.point;

            AimDistance = hit.distance;
        }

        if (isAiming)
        {
            if (!playerContext.TryGet<CharacterStats>(ContextStatKey.PlayerStats, out CharacterStats stats))
                return;
            Agility playerAgility = stats.GetCharacterAgility();



            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = playerTransform.position.y;
            Vector3 aimDirection = (worldAimTarget - playerTransform.position).normalized;

            playerTransform.forward = Vector3.Lerp(playerTransform.forward, aimDirection, lookAtAimTargetSpeed * Time.deltaTime);

        }
    }

    #region Getters & Setters
    public void GetRay(out Ray ray) => ray = this.ray;
    public void GetRayHit(out RaycastHit hit) => hit = this.hit;

    public bool RayHitHasCollider() => hit.collider != null;
    public bool RayHitHasRigidbody() => hit.rigidbody != null;
    #endregion
}
