using IuvoUnity._DataStructs;
using System;
using UnityEngine;

[Serializable]
public class Mana : Stat
{
    [SerializeField] private int currentMana = 35;
    [SerializeField] private int maxMana = 100;
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
        priorityScale.Value = 0.80f;
    }

    public override void OnUpdate()
    {
        Recharge();
    }

    public void Recharge()
    {

        if (maxRechargeValue > maxMana)
        {
            maxRechargeValue = maxMana;
        }

        if (currentMana > maxRechargeValue)
        {
            currentMana = maxRechargeValue;
        }


        if (currentMana != maxRechargeValue)
        {
            if (currentTime >= _RechargeSpeed)
            {
                currentTime = 0.0f;
                currentMana += (int)_RechargeAmount;
                currentMana = Mathf.Clamp(currentMana + (int)_RechargeAmount, 0, maxRechargeValue);
            }
            currentTime += Time.deltaTime;
        }
    }

    #region Getters & Setters
    public int GetCurrentMana() => currentMana;
    public int GetMaxMana() => maxMana;
    public bool IsManaGone() => currentMana <= 0;
    public void SetMaxMana(int newMaxMana) => maxMana = newMaxMana;
    public void RestoreMana(int restoreAmount) => currentMana = Mathf.Clamp(currentMana + restoreAmount, 0, maxMana);
    public void SpendMana(int manaCost) => currentMana = Mathf.Clamp(currentMana - manaCost, 0, maxMana);
    #endregion
}
