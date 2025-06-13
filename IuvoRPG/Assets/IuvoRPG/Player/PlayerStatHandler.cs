using System.Collections.Generic;
using UnityEngine;

// TODO: Transition class from Monobehaviour to POCO
public class PlayerStatHandler : MonoBehaviour, IPlayerHandler
{
    [SerializeField] public Context playerContext { get; set; }

    [Header("Player Progression")]
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


    private readonly List<SemiBehavior> regularUpdateBehaviors = new List<SemiBehavior>();
    private readonly List<SemiBehavior> fixedUpdateBehaviors = new List<SemiBehavior>();
    private readonly List<SemiBehavior> lateUpdateBehaviors = new List<SemiBehavior>();


    public void Start()
    {
        Register(playerLevel);
        Register(playerHealth);
        Register(playerStamina);
        Register(playerMana);
        Register(playerStrength);
        Register(playerDexterity);
        Register(playerAgility);
        Register(playerEndurance);
        Register(playerLuck);
        Register(playerFaith);
        Register(playerIntelligence);
        Register(playerWisdom);
    }

    public void Register(SemiBehavior behavior)
    {
        if (behavior == null) return;
        if (regularUpdateBehaviors.Contains(behavior) ||
            fixedUpdateBehaviors.Contains(behavior) ||
            lateUpdateBehaviors.Contains(behavior)) return;

        behavior.TryInitializeLifecycle();

        switch (behavior.updateMode)
        {
            case SemiBehavior.UpdateMode.Regular:
                regularUpdateBehaviors.Add(behavior);
                SortListByPriority(regularUpdateBehaviors);
                break;
            case SemiBehavior.UpdateMode.Fixed:
                fixedUpdateBehaviors.Add(behavior);
                SortListByPriority(fixedUpdateBehaviors);
                break;
            case SemiBehavior.UpdateMode.Late:
                lateUpdateBehaviors.Add(behavior);
                SortListByPriority(lateUpdateBehaviors);
                break;
        }
    }

    private void SortListByPriority(List<SemiBehavior> list)
    {
        list.Sort((a, b) =>
        {
            int levelComparison = b.PriorityLevel.CompareTo(a.PriorityLevel); // descending
            if (levelComparison != 0)
                return levelComparison;

            return b.priorityScale.Value.CompareTo(a.priorityScale.Value); // descending
        });
    }

    public void RefreshPriorities()
    {
        SortListByPriority(regularUpdateBehaviors);
        SortListByPriority(fixedUpdateBehaviors);
        SortListByPriority(lateUpdateBehaviors);
    }

    public void Unregister(SemiBehavior behavior)
    {
        if (behavior == null) return;

        behavior.DeinitializeLifecycle();

        regularUpdateBehaviors.Remove(behavior);
        fixedUpdateBehaviors.Remove(behavior);
        lateUpdateBehaviors.Remove(behavior);
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

    public void ClearAll()
    {
        foreach (var behavior in regularUpdateBehaviors) behavior.DeinitializeLifecycle();
        foreach (var behavior in fixedUpdateBehaviors) behavior.DeinitializeLifecycle();
        foreach (var behavior in lateUpdateBehaviors) behavior.DeinitializeLifecycle();

        regularUpdateBehaviors.Clear();
        fixedUpdateBehaviors.Clear();
        lateUpdateBehaviors.Clear();
    }

    #region Getters & Setters

    public Level GetPlayerLevel() => playerLevel;
    public Health GetPlayerHealth() => playerHealth;
    public Stamina GetPlayerStamina() => playerStamina;
    public Mana GetPlayerMana() => playerMana;



    #endregion
}
