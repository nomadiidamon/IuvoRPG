using Unity.Cinemachine;
using UnityEngine;

public class PlayerCombatHandler : MonoBehaviour, IPlayerHandler
{

    [SerializeField] public Transform LeftHandPos;
    [SerializeField] public Transform RightHandPos;
    [SerializeField] public CinemachineGroupFraming groupOfTargets;
    

    Context IPlayerHandler.playerContext { get; set; }

    ContextPlayerHandlerKey IPlayerHandler.HandlerKey => ContextPlayerHandlerKey.CombatHandler;

    void Start()
    {

    }

    void Update()
    {

    }
}
