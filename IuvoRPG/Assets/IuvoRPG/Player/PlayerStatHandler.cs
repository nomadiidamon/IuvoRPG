using UnityEngine;

// TODO: Transition class from Monobehaviour to POCO
public class PlayerStatHandler : MonoBehaviour, IPlayerHandler
{
    [SerializeField] public Context playerContext { get; set; }


    [Header("Player Level & Experience")]
    [SerializeField] private Level playerLevel;

    [Header("Player Core Stats")]
    [SerializeField] private Health playerHealth;   // effects player's life expectancey, determined by various other stats
    [SerializeField] private Stamina playerStamina; // effects how many actions a player can take before needing a break
    [SerializeField] private Mana playerMana; // effects how many speels or special actions can be done before needing a break


    [Header("Player Additional Stats")]
    [SerializeField] public Strength playerStrength;    // effects attack power with strength weapons and improves resistances
    [SerializeField] public Dexterity playerDexterity;  // effects attack power with dexterious weapons and attack speed

    [SerializeField] public Agility playerAgility;  // effects players speed, evasiveness, and overall manueverability
    [SerializeField] public Endurance playerEndurance;  // effects players carry weight, stamina, and improves resistances

    [SerializeField] public Luck playerLuck;    // effects item discovery, critical hit chance, critical hit damage
    [SerializeField] public Faith playerFaith;  // effects NPC interpretation, improves resistances, and allows for special magics

    [SerializeField] public Intelligence playerIntelligence;    // effects magical attack power, improves perception
    [SerializeField] public Wisdom playerWisdom;    // effects mana level, magic duration, and improves perception




    public void Start()
    {

    }

    public void Update()
    {

    }

    #region Getters & Setters

    public Level GetPlayerLevel() => playerLevel;
    public Health GetPlayerHealth() => playerHealth;
    public Stamina GetPlayerStamina() => playerStamina;
    public Mana GetPlayerMana() => playerMana;



    #endregion
}
