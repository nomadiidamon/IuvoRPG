using IuvoUnity._DataStructs;
using System;
using UnityEngine;

[Serializable]
public class Stamina : Stat
{
    [Header("Stamina")]
    [SerializeField] private int currentStamina = 35;
    [SerializeField] private int maxStamina = 100;
    [SerializeField] private int maxRechargeValue = 75;

    [Header("IRecharge")]
    [SerializeField] private float rechargeAmount = 5.0f;
    [SerializeField] private float rechargeSpeed = 0.25f;

    public float _RechargeAmount { get => rechargeAmount; set => _RechargeAmount = rechargeAmount; }
    public float _RechargeSpeed { get => rechargeSpeed; set => _RechargeSpeed = rechargeSpeed; }

    public float currentTime = 0.0f;

    public override void OnStart()
    {
        updateMode = UpdateMode.Regular;
        PriorityLevel = PriorityLevel.High;
        priorityScale.Range = new RangeF(0.0f, 1.0f);
        priorityScale.Value = 0.90f;
    }

    public override void OnUpdate()
    {
        Recharge();
    }

    public void Recharge()
    {

        if (maxRechargeValue > maxStamina)
        {
            maxRechargeValue = maxStamina;
        }

        if (currentStamina > maxRechargeValue)
        {
            currentStamina = maxRechargeValue;
        }



        if (currentStamina != maxRechargeValue)
        {
            if (currentTime >= _RechargeSpeed)
            {
                currentTime = 0.0f;
                currentStamina += (int)_RechargeAmount;
                currentStamina = Mathf.Clamp(currentStamina + (int)_RechargeAmount, 0, maxRechargeValue);
            }
            currentTime += Time.deltaTime;
        }
    }

    #region Getters & Setters
    public int GetCurrentStamina() => currentStamina;
    public int GetMaxStamina() => maxStamina;
    public bool IsStaminaGone() => currentStamina <= 0;
    public void SetMaxStamina(int newMaxStamina) => maxStamina = newMaxStamina;
    public void RestoreStamina(int restoreAmount) => currentStamina = Mathf.Clamp(currentStamina + restoreAmount, 0, maxStamina);
    public void SpendStamina(int staminaCost) => currentStamina = Mathf.Clamp(currentStamina - staminaCost, 0, maxStamina);

    #endregion

}
