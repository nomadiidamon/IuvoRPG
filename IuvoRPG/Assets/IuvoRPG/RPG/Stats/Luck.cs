using UnityEngine;

public class Luck : Stat
{

    Faith myFaith;

    [SerializeField] private float itemDiscovery = 0.35f;
    [SerializeField] private float critHitChance = 0.15f;
    [SerializeField] private float critHitDamageMultiplier = 1.65f;

    [SerializeField] private float avoidDamageChance = 0.05f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Stat Utility Functions
    public bool DidDiscoverItem() => (Random.Range(0, 1) <= itemDiscovery);
    public bool DidCritHit() => (Random.Range(0, 1) <= critHitChance);
    public int GetCritDamageAmount(int damageAmount) => (int)(damageAmount * critHitDamageMultiplier);
    public bool DidAvoidDamage() => (Random.Range(0, 1) <= avoidDamageChance);
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
