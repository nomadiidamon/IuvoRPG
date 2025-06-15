using System.Collections.Generic;
using UnityEngine;

// TODO: Transition class from Monobehaviour to POCO
public class PlayerStatHandler : SemiBehaviorManager, IPlayerHandler
{
    [SerializeField] public Context playerContext { get; set; }

    [Header("Player Stats")]
    [SerializeField] public CharacterStats playerStats;

    public void Start()
    {
        Register(playerStats);
        Register(playerStats.GetCharacterLevel());
        Register(playerStats.GetCharacterHealth());
        Register(playerStats.GetCharacterStamina());
        Register(playerStats.GetCharacterMana());
        Register(playerStats.GetCharacterStrength());
        Register(playerStats.GetCharacterDexterity());
        Register(playerStats.GetCharacterAgility());
        Register(playerStats.GetCharacterEndurance());
        Register(playerStats.GetCharacterLuck());
        Register(playerStats.GetCharacterFaith());
        Register(playerStats.GetCharacterIntelligence());
        Register(playerStats.GetCharacterWisdom());
    }

    private void Update()
    {
        foreach (var behavior in regularUpdateBehaviors)
        {
            if (behavior != null && behavior.isInitialized)
                behavior.Tick();
        }
    }

    private void FixedUpdate()
    {
        foreach (var behavior in fixedUpdateBehaviors)
        {
            if (behavior != null && behavior.isInitialized)
                behavior.Tick();
        }
    }

    private void LateUpdate()
    {
        foreach (var behavior in lateUpdateBehaviors)
        {
            if (behavior != null && behavior.isInitialized)
                behavior.Tick();
        }
    }


    #region Getters & Setters

    public Level GetPlayerLevel() => playerStats.GetCharacterLevel();
    public Health GetPlayerHealth() => playerStats.GetCharacterHealth();
    public Stamina GetPlayerStamina() => playerStats.GetCharacterStamina();
    public Mana GetPlayerMana() => playerStats.GetCharacterMana();

    public Strength GetPlayerStrength() => playerStats.GetCharacterStrength();
    public Dexterity GetPlayerDexterity() => playerStats.GetCharacterDexterity();
    public Agility GetPlayerAgility() => playerStats.GetCharacterAgility();
    public Endurance GetPlayerEndurance() => playerStats.GetCharacterEndurance();
    public Luck GetPlayerLuck() => playerStats.GetCharacterLuck();
    public Faith GetPlayerFaith() => playerStats.GetCharacterFaith();
    public Intelligence GetPlayerIntelligence() => playerStats.GetCharacterIntelligence();
    public Wisdom GetPlayerWisdom() => playerStats.GetCharacterWisdom();


    #endregion
}
