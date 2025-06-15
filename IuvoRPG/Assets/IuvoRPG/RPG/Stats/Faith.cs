using System;
using UnityEngine;

[Serializable]
public class Faith : Stat
{
    Luck myLuck;

    [Header("Character's Karmic nature")]
    [SerializeField] private float karma = 0.5f;
    private float minKarma = -1.0f;
    private float maxKarma = 1.0f;

    [Header("Character's Poison Resistances")]
    [SerializeField] private float poisonResistance = 0.15f;
    [SerializeField] private float maxPosionResistance = 0.95f;

    [Header("Character's Fire Resistances")]
    [SerializeField] private float fireResistance = 0.15f;
    [SerializeField] private float maxFireResistance = 1.0f;

    [Header("Faith Based Bonuses")]
    [SerializeField] private float faithSpellMult = 0.35f;

    public override void OnStart()
    {
        _levelValue = 1;
        _statName = "Faith";
    }

    #region Stat Utility Functions
    public int GetFaithSpellDamageAmount(int damageAmount) => (int)(faithSpellMult * damageAmount);
    #endregion


    #region Getters & Setters
    public float GetKarma() => karma;
    // the passed in value is added to the current karma and clamped between min and max karma levels
    public void AlterKarma(float karma) => this.karma = Mathf.Clamp(this.karma + karma, minKarma, maxKarma);
    public float GetMinimumKarma() => minKarma;
    private void SetMinimunKarma(float minKarma) => this.minKarma = minKarma;
    public float GetMaximumKarma() => maxKarma;
    private float SetMaximumKarma(float maxKarma) => this.maxKarma = maxKarma;

    public float GetPoisonResistance() => poisonResistance;
    // the passed in value is added to the current resistance and clamped between 0 and maxPoisonResist
    public void AlterPoisonResistance(float poisonRes) => poisonResistance = Mathf.Clamp(poisonResistance + poisonRes, 0, maxPosionResistance);

    public float GetFaithMultiplier() => faithSpellMult;

    public float GetFireResistance() => fireResistance;
    // the passed in value is added to the current resistance and clamped between 0 and maxPoisonResist
    public void AlterFireResistance(float poisonRes) => fireResistance = Mathf.Clamp(fireResistance + poisonRes, 0, maxFireResistance);
    #endregion

}
