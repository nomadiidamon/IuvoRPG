using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    [Header("Character Level & Experience")]
    [SerializeField] private Level myLevel;

    [Header("Character Core Stats")]
    [SerializeField] private Health myHealth;   // effects player's life expectancey, determined by various other stats
    [SerializeField] private Stamina myStamina; // effects how many actions a player can take before needing a break
    [SerializeField] private Mana myMana; // effects how many speels or special actions can be done before needing a break


    [Header("Character Additional Stats")]
    [SerializeField] public Strength myStrength;    // effects attack power with strength weapons and improves resistances
    [SerializeField] public Dexterity myDexterity;  // effects attack power with dexterious weapons and attack speed

    [SerializeField] public Agility myAgility;  // effects players speed, evasiveness, and overall manueverability
    [SerializeField] public Endurance myEndurance;  // effects players carry weight, stamina, and improves resistances

    [SerializeField] public Luck myLuck;    // effects item discovery, critical hit chance, critical hit damage
    [SerializeField] public Faith myFaith;  // effects NPC interpretation, improves resistances, and allows for special magics

    [SerializeField] public Intelligence myIntelligence;    // effects magical attack power, improves perception
    [SerializeField] public Wisdom myWisdom;    // effects mana level, magic duration, and improves perception

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
