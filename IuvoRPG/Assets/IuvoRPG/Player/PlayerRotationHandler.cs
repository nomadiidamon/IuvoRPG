using UnityEngine;

public class PlayerRotationHandler : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] Transform cameraTransform;
    [SerializeField] Transform aimTargetTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (playerTransform == null) Debug.LogError("PlayerTransform can not be NULL");

        if (cameraTransform == null) Debug.LogError("CameraTransform can not be NULL");

        if (aimTargetTransform == null) Debug.LogError("AimTargetTransform can not be NULL");

    }

    // Update is called once per frame
    void Update()
    {
        
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
