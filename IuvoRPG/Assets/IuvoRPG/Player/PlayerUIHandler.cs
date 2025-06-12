using UnityEngine;
using UnityEngine.UI;

// TODO: Transition class from Monobehaviour to POCO
public class PlayerUIHandler : MonoBehaviour, IPlayerHandler
{
    [Header("Player reference")]
    [SerializeField] public Player myPlayer;
    [SerializeField] public PlayerStatHandler playerStatHandler;

    [Header("UI Components")]
    [SerializeField] public GameObject UI;

    [Header("Player HUD")]
    [SerializeField] public GameObject PlayerHUD;
    [SerializeField] public Image healthBarImage;
    [SerializeField] public Image staminaBarImage;
    [SerializeField] public Image manaBarImage;
    [SerializeField] public Image expBarImage;


    [SerializeField] public GameObject PlayerStats;
    [SerializeField] public GameObject PlayerToolBag;
    [SerializeField] public GameObject AimReticle;

    [SerializeField] public Context playerContext { get; set; }

    public void Awake()
    {
        if (UI == null)
        {
            Debug.LogError("UI object must not be null");
        }
        if (PlayerHUD == null)
        {
            Debug.LogError("PlayerHUD object must not be null");
        }
        if (PlayerStats == null)
        {
            Debug.LogError("PlayerStats object must not be null");
        }
        if (PlayerToolBag == null)
        {
            Debug.LogError("PlayerToolBag object must not be null");
        }
        if (AimReticle == null)
        {
            Debug.LogError("AimReticle object must not be null");
        }
        if (myPlayer == null)
        {
            Debug.LogError("Player object must not be null");
        }
        if (playerStatHandler == null)
        {
            Debug.LogError("Player Stat Handler must not be null");
        }

    }

    public void Start()
    {
        
    }

    public void Update()
    {
        UpdatePlayerHUD();
    }

    void UpdatePlayerHUD()
    {
        UpdatePlayerHealthBar();
        UpdatePlayerStaminaBar();
        UpdatePlayerManaBar();
        UpdateExpBar();
    }

    void UpdatePlayerHealthBar()
    {
        healthBarImage.fillAmount = ((float)playerStatHandler.GetPlayerHealth().GetCurrentHealth()/ playerStatHandler.GetPlayerHealth().GetMaxHealth());
    }

    void UpdatePlayerStaminaBar()
    {
        staminaBarImage.fillAmount = ((float)playerStatHandler.GetPlayerStamina().GetCurrentStamina() / playerStatHandler.GetPlayerStamina().GetMaxStamina());
    }

    private void UpdatePlayerManaBar()
    {
        manaBarImage.fillAmount = ((float)playerStatHandler.GetPlayerMana().GetCurrentMana() / playerStatHandler.GetPlayerMana().GetMaxMana());
    }

    private void UpdateExpBar()
    {
        expBarImage.fillAmount = ((float)playerStatHandler.GetPlayerLevel().GetCurrentExperience() / playerStatHandler.GetPlayerLevel().GetExpToNextLevel());
    }
}
