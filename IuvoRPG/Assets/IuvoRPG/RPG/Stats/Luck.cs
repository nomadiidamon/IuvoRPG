using System;
using UnityEngine;

[Serializable]
public class Luck : Stat
{

    Faith myFaith;

    [SerializeField] private float itemDiscovery = 0.35f;
    [SerializeField] private float critHitChance = 0.15f;
    [SerializeField] private float critHitDamageMultiplier = 1.65f;

    [SerializeField] private float avoidDamageChance = 0.05f;

    public override void OnStart()
    {
        _levelValue = 1;
        _statName = "Luck";
    }

    #region Stat Utility Functions
    public bool DidDiscoverItem() => (UnityEngine.Random.Range(0, 1) <= itemDiscovery);
    public bool DidCritHit() => (UnityEngine.Random.Range(0, 1) <= critHitChance);
    public int GetCritDamageAmount(int damageAmount) => (int)(damageAmount * critHitDamageMultiplier);
    public bool DidAvoidDamage() => (UnityEngine.Random.Range(0, 1) <= avoidDamageChance);
    #endregion


    #region Getters & Setters
    public void SetItemDiscoveryRate(float newdiscoveryRate) => itemDiscovery = newdiscoveryRate;
    public float GetItemDiscoveryRate() => itemDiscovery;
    public void SetCritHitChance(float critChance) => critHitChance = critChance;
    public float GetCritHitChance() => critHitChance;
    public float GetCritDamageMultiplier() => critHitDamageMultiplier;
    public void SetCritHitDamageMultiplier(float critDamageMult) => critHitDamageMultiplier = critDamageMult;
    public float GetAvoidDamageChance() => avoidDamageChance;
    public void SetAvoidDamageChance(float avoideChance) => avoidDamageChance = avoideChance;
    #endregion

}
