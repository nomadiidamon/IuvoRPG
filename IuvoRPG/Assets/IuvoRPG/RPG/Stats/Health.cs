using IuvoUnity._DataStructs;
using System;
using UnityEngine.Bindings;
using UnityEngine;

[Serializable]
public class Health : Stat, IRecharge
{
    [Header("Health")]
    [SerializeField] private int currentHealth = 35;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int maxRechargeValue = 75;
    [SerializeField] private float statLerpSpeed = 0.43f;

    [Header("IRecharge")]
    [SerializeField] private float rechargeAmount = 5.0f;
    [SerializeField] private float rechargeSpeed = 0.25f;

    public float _RechargeAmount { get => rechargeAmount; set => _RechargeAmount = rechargeAmount; }
    public float _RechargeSpeed { get => rechargeSpeed; set => _RechargeSpeed = rechargeSpeed; }

    public float currentTime = 0.0f;

    public FlexibleEvent OnTakeDamage = new FlexibleEvent();

    public override void OnStart()
    {
        updateMode = UpdateMode.Regular;
        PriorityLevel = PriorityLevel.High;
        priorityScale = new ClampedFloat(new RangeF(0.0f, 1.0f), 1.0f);
    }

    public override void OnUpdate()
    {
        Recharge();
    }

    public void Recharge()
    {
        if (maxRechargeValue > maxHealth)
        {
            maxRechargeValue = maxHealth;
        }

        if (currentHealth > maxRechargeValue)
        {
            currentHealth = maxRechargeValue;
        }


        if (currentHealth != maxRechargeValue)
        {
            if (currentTime >= _RechargeSpeed)
            {
                currentTime = 0.0f;
                currentHealth += (int)_RechargeAmount;
                currentHealth = Mathf.Clamp(currentHealth + (int)_RechargeAmount, 0, maxRechargeValue);
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
    public void TakeDamage(int damageAmount)
    {
        Mathf.Clamp(currentHealth - damageAmount, 0, maxHealth);
        OnTakeDamage.Invoke();
    }

    #endregion
}

