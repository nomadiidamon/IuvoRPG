using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [SerializeField] public Player playerRef;
    public Transform playerTransform;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        if (playerRef == null)
        {
            playerRef = FindFirstObjectByType<Player>();
        }

    }

    void Start()
    {
        
    }

    void Update()
    {
        playerTransform.position = playerRef.transform.position;
    }
}
