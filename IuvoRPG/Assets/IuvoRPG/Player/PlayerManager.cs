using UnityEngine;

public class PlayerManager : MonoBehaviour, IPlayerHandler
{
    public static PlayerManager Instance;

    [SerializeField] public Player playerRef;
    public Transform playerTransform;

    public Context playerContext { get; set; }
    public ContextPlayerHandlerKey HandlerKey => ContextPlayerHandlerKey.PlayerManager;
    


    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (playerRef == null)
        {
            playerRef = FindFirstObjectByType<Player>();
        }

        IPlayerHandler handler = this;
        if (handler != null) handler.UpdateHandlerInContext();

    }

    void Update()
    {
        playerTransform.position = playerRef.transform.position;
    }

}
