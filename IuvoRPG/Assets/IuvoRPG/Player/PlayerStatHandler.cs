using System.Collections.Generic;
using UnityEngine;

public class PlayerStatHandler : SemiBehaviorManager, IPlayerHandler
{
    public Context playerContext { get; set; }

    public ContextPlayerHandlerKey HandlerKey => ContextPlayerHandlerKey.StatHandler;

    [Header("Player Stats")]
    [SerializeField] public CharacterStats playerStats;

    public void Awake()
    {
        playerContext.Set<CharacterStats>(ContextStatKey.PlayerStats, playerStats);
    }

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
        IPlayerHandler handler = this;

        if (this != null && playerContext != null)
        {
            handler.UpdateHandlerInContext();
        }

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


    #region Getters


    #region Core Stats

    public Level GetPlayerLevel() => playerStats.GetCharacterLevel();
    public Health GetPlayerHealth() => playerStats.GetCharacterHealth();
    public Stamina GetPlayerStamina() => playerStats.GetCharacterStamina();
    public Mana GetPlayerMana() => playerStats.GetCharacterMana();



    #endregion
    
    #region Secondary Stats

    public Strength GetPlayerStrength() => playerStats.GetCharacterStrength();
    public Dexterity GetPlayerDexterity() => playerStats.GetCharacterDexterity();
    public Agility GetPlayerAgility() => playerStats.GetCharacterAgility();
    public Endurance GetPlayerEndurance() => playerStats.GetCharacterEndurance();
    public Luck GetPlayerLuck() => playerStats.GetCharacterLuck();
    public Faith GetPlayerFaith() => playerStats.GetCharacterFaith();
    public Intelligence GetPlayerIntelligence() => playerStats.GetCharacterIntelligence();
    public Wisdom GetPlayerWisdom() => playerStats.GetCharacterWisdom();


    #endregion


    #endregion
}
