using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

// TODO: Transition class from Monobehaviour to POCO
public class PlayerAimHandler : MonoBehaviour, IPlayerHandler
{
    [Header("References")]
    [SerializeField] private PlayerAnimatorHandler playerAnimatorHandler;
    [SerializeField] private PlayerUIHandler playerUIHandler;
    [SerializeField] public Transform playerTransform;
    [SerializeField] private Transform aimReferenceSphere;
    [SerializeField] private IStayHere aimPoint;


    [SerializeField] private PlayerCameraHandler cameraHandler;
    [SerializeField] private CinemachinePanTilt cameraPanTiltComponent;
    [SerializeField] private CinemachinePositionComposer positionComposer;

    [SerializeField] public Context playerContext { get; set; }


    [Header("Settings")]
    [SerializeField] private float maxAimDistance = 100f;
    [SerializeField] private float aimSpeed = 2.5f;
    [SerializeField] private float shoulderSwitchSpeed = 0.75f;
    [SerializeField] private bool isSwitchingShoulders = false;

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

        }
    }

    public void Start()
    {
        originalTargetOffset = positionComposer.TargetOffset;
        if (aimReferenceSphere != null)
        {
            aimPoint = aimReferenceSphere.GetComponent<IStayHere>();
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        //playerAnimatorHandler.animator.SetBool("IsAiming", true);
        int layerIndex = playerAnimatorHandler.animator.GetLayerIndex("Aim");
        StartCoroutine(StartAiming(layerIndex, aimSpeed));
        playerContext.Set<bool>(ContextStateKey.IsAiming, isAiming);
    }
    public void OnAimCanceled()
    {
        playerUIHandler.AimReticle.transform.localScale /= 2;
        cameraHandler.SwitchCameraStyles(CameraStyle.EXPLORATION);
        isAiming = false;
        //playerAnimatorHandler.animator.SetBool("IsAiming", false);
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

        isSwitchingShoulders = true;

        while (time < duration)
        {
            float newX = Mathf.Lerp(startX, targetX, time / duration);
            var offset = positionComposer.TargetOffset;
            offset.x = newX;
            positionComposer.TargetOffset = offset;
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
        if (isAiming)
        {
            ray = SceneManager.Instance.cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0));

            if (Physics.Raycast(ray, out hit, maxAimDistance))
            {
                AimDistance = hit.distance;
                AimTarget = hit.point;

                if (!aimPoint.IsApproximatelySame(hit.point))
                {
                    aimPoint.SetPosition(hit.point);
                }
            }
        }
    }

    #region Getters & Setters
    public void GetRay(out Ray ray) => ray = this.ray;
    public void GetRayHit(out RaycastHit hit) => hit = this.hit;

    public bool RayHitHasCollider() => hit.collider != null;
    public bool RayHitHasRigidbody() => hit.rigidbody != null;
    #endregion
}
