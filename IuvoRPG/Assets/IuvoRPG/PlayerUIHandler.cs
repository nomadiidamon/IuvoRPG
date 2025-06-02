using UnityEngine;
using UnityEngine.UI;

public class PlayerUIHandler : MonoBehaviour
{
    [Header("Player reference")]
    [SerializeField] public SecondNewPlayer player;
    [SerializeField] public Health playerHealth;

    [Header("UI Components")]
    [SerializeField] public GameObject UI;

    [Header("Player HUD")]
    [SerializeField] public GameObject PlayerHUD;
    [SerializeField] public Image healthBarImage;

    [SerializeField] public GameObject PlayerStats;
    [SerializeField] public GameObject PlayerToolBag;
    [SerializeField] public GameObject AimReticle;
    [Space(5)]
    [SerializeField] private float playerCurrentHealth;
    [SerializeField] private float playerMaxHealth;
    [SerializeField] private float playerMinHealth;
    [Space(2)]
    [SerializeField] private float playerCurrentStamina;
    [SerializeField] private float playerMaxStamina;
    [SerializeField] private float playerMinStamina;
    [Space(2)]
    [SerializeField] private float playerCurrentMana;
    [SerializeField] private float playerMaxMana;
    [SerializeField] private float playerMinMana;
    [Space(2)]
    [SerializeField] private float playerCurrentExperience;
    [SerializeField] private float playerMaxExperience;
    [SerializeField] private float playerMinExperience;
    [Space(2)]
    [SerializeField] private int playerLevel;


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

    }

    void Start()
    {
        
    }

    void Update()
    {
        UpdatePlayerHealthBar();
    }

    void UpdatePlayerHealthBar()
    {
        healthBarImage.fillAmount = ((float)playerHealth.GetCurrentHealth()/playerHealth.GetMaxHealth());
    }
}
