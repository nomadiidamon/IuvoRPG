using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    [Header("Character Level & Experience")]
    [SerializeField] private Level playerLevel;

    [Header("Character Core Stats")]
    [SerializeField] private Health playerHealth;   // effects player's life expectancey, determined by various other stats
    [SerializeField] private Stamina playerStamina; // effects how many actions a player can take before needing a break
    [SerializeField] private Mana playerMana; // effects how many speels or special actions can be done before needing a break


    [Header("Character Additional Stats")]
    [SerializeField] public Strength playerStrength;    // effects attack power with strength weapons and improves resistances
    [SerializeField] public Dexterity playerDexterity;  // effects attack power with dexterious weapons and attack speed

    [SerializeField] public Agility playerAgility;  // effects players speed, evasiveness, and overall manueverability
    [SerializeField] public Endurance playerEndurance;  // effects players carry weight, stamina, and improves resistances

    [SerializeField] public Luck playerLuck;    // effects item discovery, critical hit chance, critical hit damage
    [SerializeField] public Faith playerFaith;  // effects NPC interpretation, improves resistances, and allows for special magics

    [SerializeField] public Intelligence playerIntelligence;    // effects magical attack power, improves perception
    [SerializeField] public Wisdom playerWisdom;    // effects mana level, magic duration, and improves perception

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
