using System;
using UnityEngine;

[Serializable]
public class Health : Stat, IRecharge
{
    [SerializeField] private int currentHealth = 35;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int maxRechargeValue = 75;
    
    [Header("IRecharge")]
    [SerializeField] private float rechargeAmount = 5.0f;
    [SerializeField] private float rechargeSpeed = 0.25f;

    public float _RechargeAmount { get => rechargeAmount; set => _RechargeAmount = rechargeAmount; }
    public float _RechargeSpeed { get => rechargeSpeed; set => _RechargeSpeed = rechargeSpeed; }

    public float currentTime = 0.0f;

    public void Update()
    {
        Recharge();
    }

    public void Recharge()
    {
        if (currentHealth != maxRechargeValue)
        {   
            if(currentTime >= _RechargeSpeed)
            {
                currentTime = 0.0f;
                currentHealth += (int)_RechargeAmount;
                currentHealth = Mathf.Clamp(currentHealth + (int) _RechargeAmount, 0, maxRechargeValue);
            }
            currentTime += Time.deltaTime;        
        }
    }

    #region Getters & Setters
    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;
    public bool IsHealthGone() => currentHealth <= 0;
    public void SetMaxHealth(int newMaxHealth) => maxHealth = newMaxHealth;
    public void Heal(int healAmount) => currentHealth = Mathf.Clamp(currentHealth + healAmount, 0, maxHealth);
    public void TakeDamage(int damageAmount) => currentHealth = Mathf.Clamp(currentHealth - damageAmount, 0, currentHealth);
    #endregion
}

