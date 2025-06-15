using System;
using UnityEngine;

[Serializable]
public class CharacterStats : SemiBehavior
{
    public bool logStatsNow = false;

    [Header("Character Level & Experience")]
    [SerializeField] private Level myLevel;

    [Header("Character Core Stats")]
    [SerializeField] private Health myHealth;   // effects player's life expectancey, determined by various other stats
    [SerializeField] private Stamina myStamina; // effects how many actions a player can take before needing a break
    [SerializeField] private Mana myMana; // effects how many speels or special actions can be done before needing a break


    [Header("Character Additional Stats")]
    [SerializeField] private Strength myStrength;    // effects attack power with strength weapons and improves resistances
    [SerializeField] private Dexterity myDexterity;  // effects attack power with dexterious weapons and attack speed

    [SerializeField] private Agility myAgility;  // effects players speed, evasiveness, and overall manueverability
    [SerializeField] private Endurance myEndurance;  // effects players carry weight, stamina, and improves resistances

    [SerializeField] private Luck myLuck;    // effects item discovery, critical hit chance, critical hit damage
    [SerializeField] private Faith myFaith;  // effects NPC interpretation, improves resistances, and allows for special magics

    [SerializeField] private Intelligence myIntelligence;    // effects magical attack power, improves perception
    [SerializeField] private Wisdom myWisdom;    // effects mana level, magic duration, and improves perception

    public override void OnStart()
    {
        CalculateCoreStats();
    }

    public override void OnUpdate()
    {
        LogStats();
    }

    public void LogStats()
    {
        if (!logStatsNow)
        {
            return;
        }

        Debug.Log($"Character Level: {myLevel.GetLevel()}");
        Debug.Log($"Health: {myHealth.GetCurrentHealth()}/{myHealth.GetMaxHealth()}");
        Debug.Log($"Stamina: {myStamina.GetCurrentStamina()}/{myStamina.GetMaxStamina()}");
        Debug.Log($"Mana: {myMana.GetCurrentMana()}/{myMana.GetMaxMana()}");
        Debug.Log($"Strength: {myStrength.GetLevel()}");
        Debug.Log($"Dexterity: {myDexterity.GetLevel()}");
        Debug.Log($"Agility: {myAgility.GetLevel()}");
        Debug.Log($"Endurance: {myEndurance.GetLevel()}");
        Debug.Log($"Luck: {myLuck.GetLevel()}");
        Debug.Log($"Faith: {myFaith.GetLevel()}");
        Debug.Log($"Intelligence: {myIntelligence.GetLevel()}");
        Debug.Log($"Wisdom: {myWisdom.GetLevel()}");

        logStatsNow = false;
    }

    #region Core Stat Calculations

    public void CalculateCoreStats()
    {
        CalculateMaxStamina();
        CalculateMaxMana();
        CalculateMaxHealth();
    }

    public void CalculateMaxHealth()
    {
        int str = myStrength.GetLevel() * 5;
        int end = myEndurance.GetLevel() * 5;
        int stam = myStamina.GetLevel() * 3;
        int wis = myWisdom.GetLevel() * 3;
        int luck = myLuck.GetLevel() * 2;

        myHealth.SetMaxHealth(myLevel.GetLevel() * (str + end + stam + wis + luck));
    }

    public void CalculateMaxStamina()
    {
        int end = myEndurance.GetLevel() * 10;
        int agi = myAgility.GetLevel() * 5;

        myStamina.SetMaxStamina(end + agi);
    }

    public void CalculateMaxMana()
    {
        int wis = myWisdom.GetLevel() * 10;
        int intel = myIntelligence.GetLevel() * 5;

        myMana.SetMaxMana(wis + intel);
    }


    #endregion

    #region Getters & Setters

    public Level GetCharacterLevel() => myLevel;
    public Health GetCharacterHealth() => myHealth;
    public Stamina GetCharacterStamina() => myStamina;
    public Mana GetCharacterMana() => myMana;

    public Strength GetCharacterStrength() => myStrength;
    public Dexterity GetCharacterDexterity() => myDexterity;
    public Agility GetCharacterAgility() => myAgility;
    public Endurance GetCharacterEndurance() => myEndurance;
    public Luck GetCharacterLuck() => myLuck;
    public Faith GetCharacterFaith() => myFaith;
    public Intelligence GetCharacterIntelligence() => myIntelligence;
    public Wisdom GetCharacterWisdom() => myWisdom;

    #endregion
}
