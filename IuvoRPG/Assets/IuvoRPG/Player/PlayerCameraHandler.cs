using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

[Serializable]
public enum CameraStyle
{
    EXPLORATION,
    THIRD_PERSON_SHOOTER,
    FIRST_PERSON_SHOOTER,
    THIRD_PERSON_COMBAT,
    CINEMATIC,
    OTHER
}

// TODO: Transition class from Monobehaviour to POCO
[Serializable]
public class PlayerCameraHandler : MonoBehaviour, IPlayerHandler
{
    [SerializeField] private Transform playerForward;
    [SerializeField] private Transform playerAim;
    [SerializeField] private Camera mainCamera;

    [SerializeField] private CameraStyle currentCameraStyle;

    [Header("Cinemachine Cameras")]
    [SerializeField] public CinemachineCamera activeCam;

    [SerializeField] public CinemachineCamera explorationCam;
    [SerializeField] public CinemachineCamera tpsCam;
    [SerializeField] public CinemachineCamera fpsCam ;
    [SerializeField] public CinemachineCamera combatCam ;
    [SerializeField] public CinemachineCamera cinematicCam;

    private Dictionary<CameraStyle, CinemachineCamera> camDict;

    [SerializeField] public Context playerContext { get; set; }
    public ContextPlayerHandlerKey HandlerKey => ContextPlayerHandlerKey.CameraHandler;



    public void Start()
    {
        camDict = new Dictionary<CameraStyle, CinemachineCamera>
        {
            { CameraStyle.EXPLORATION, explorationCam },
            { CameraStyle.THIRD_PERSON_SHOOTER, tpsCam },
            { CameraStyle.FIRST_PERSON_SHOOTER, fpsCam },
            { CameraStyle.THIRD_PERSON_COMBAT, combatCam },
            { CameraStyle.CINEMATIC, cinematicCam }
        };

        foreach (var kvp in camDict)
        {
            if (kvp.Value == null)
            {
                Debug.LogWarning($"Camera not assigned for style: {kvp.Key}");
            }
        }

        UpdateCameraStyle();
    }

    public void UpdateCameraStyle()
    {

        foreach (var kvp in camDict)
        {
            if (kvp.Value != null)
            {
                kvp.Value.Priority = 0;
            }
        }

        if (camDict.TryGetValue(currentCameraStyle, out activeCam) && activeCam != null)
        {
            activeCam.Priority = 20;
            Debug.Log($"Switched to {currentCameraStyle} camera.");
        }
        else
        {
            Debug.LogWarning($"No camera assigned for {currentCameraStyle}.");
        }
    }

    public void Update()
    {
        foreach (var cam in camDict)
        {
            cam.Value.transform.forward = playerForward.forward;
            cam.Value.transform.LookAt(playerAim);
        }
    }


    public bool CheckCameraStyle(CameraStyle style)
    {
        return currentCameraStyle == style;
    }

    public void SwitchCameraStyles(CameraStyle newCameraStyle)
    {
        if (newCameraStyle == currentCameraStyle) return;

        currentCameraStyle = newCameraStyle;
        UpdateCameraStyle();
    }

    public CameraStyle GetCurrentCameraStyle()
    {
        return currentCameraStyle;
    }

    public Transform GetCurrentCameraTransform() => activeCam.transform;

    public Camera GetMainCamera() => mainCamera;
};
