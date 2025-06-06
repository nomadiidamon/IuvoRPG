using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHandler : MonoBehaviour
{
    [Header("Player reference")]
    [SerializeField] public SecondNewPlayer player;
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


    private void Awake()
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
        if (player == null)
        {
            Debug.LogError("Player object must not be null");
        }
        if (playerStatHandler == null)
        {
            Debug.LogError("Player Stat Handler must not be null");
        }

    }

    void Start()
    {
        
    }

    void Update()
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
        healthBarImage.fillAmount = ((float)playerStatHandler.playerHealth.GetCurrentHealth()/ playerStatHandler.playerHealth.GetMaxHealth());
    }

    void UpdatePlayerStaminaBar()
    {
        staminaBarImage.fillAmount = ((float)playerStatHandler.playerStamina.GetCurrentStamina() / playerStatHandler.playerStamina.GetMaxStamina());
    }

    private void UpdatePlayerManaBar()
    {
        manaBarImage.fillAmount = ((float)playerStatHandler.playerMana.GetCurrentMana() / playerStatHandler.playerMana.GetMaxMana());
    }

    private void UpdateExpBar()
    {
        expBarImage.fillAmount = ((float)playerStatHandler.playerLevel.GetCurrentExperience() / playerStatHandler.playerLevel.GetExpToNextLevel());
    }
}
