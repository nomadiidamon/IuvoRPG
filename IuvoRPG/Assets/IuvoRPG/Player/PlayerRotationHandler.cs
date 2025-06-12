using UnityEngine;

public class PlayerRotationHandler : MonoBehaviour, IPlayerHandler
{
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform aimTargetTransform;

    [SerializeField] float moveRotationSpeed = 2.5f;
    [SerializeField] float aimRotationSpeed = 1.0f;

    [SerializeField] public Context playerContext { get; set; }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        if (playerTransform == null) Debug.LogError("PlayerTransform can not be NULL");

        if (cameraTransform == null) Debug.LogError("CameraTransform can not be NULL");

        if (aimTargetTransform == null) Debug.LogError("AimTargetTransform can not be NULL");

    }

    // Update is called once per frame
    public void Update()
    {
        Rotate();
    }

    public void Rotate()
    {
        AdjustPlayerCameraRotation();
        UpdatePlayerCameraRotation();
        AdjustPlayerRotation();
        UpdatePlayerRotation();
        SyncPlayerCameraAndPlayerRotations(0.02f);
    }

    public void AdjustPlayerCameraRotation()
    {

    }

    public void UpdatePlayerCameraRotation()
    {

    }

    public void AdjustPlayerRotation()
    {

    }

    public void UpdatePlayerRotation()
    {

    }

    public void UpdatePlayerRotationOverTime(float adjustmentRate)
    {

    }

    public void SyncPlayerCameraAndPlayerRotations(float aprox)
    {

    }
}
