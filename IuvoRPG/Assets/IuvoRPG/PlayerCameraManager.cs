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

[Serializable]
public class PlayerCameraManager : MonoBehaviour
{

    [SerializeField] private CameraStyle currentCameraStyle;

    [Header("Cinemachine Cameras")]
    [SerializeField] private CinemachineCamera explorationCam;
    [SerializeField] private CinemachineCamera tpsCam;
    [SerializeField] private CinemachineCamera fpsCam;
    [SerializeField] private CinemachineCamera combatCam;
    [SerializeField] private CinemachineCamera cinematicCam;

    private Dictionary<CameraStyle, CinemachineCamera> camDict;


    void Start()
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

    void Update()
    {

    }

    void UpdateCamera()
    {

    }

    void UpdateCameraStyle()
    {

        foreach (var kvp in camDict)
        {
            if (kvp.Value != null)
            {
                kvp.Value.Priority = 0;
            }
        }

        if (camDict.TryGetValue(currentCameraStyle, out var activeCam) && activeCam != null)
        {
            activeCam.Priority = 20;
            Debug.Log($"Switched to {currentCameraStyle} camera.");
        }
        else
        {
            Debug.LogWarning($"No camera assigned for {currentCameraStyle}.");
        }
    }


    bool CheckCameraStyle(CameraStyle style)
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
}
