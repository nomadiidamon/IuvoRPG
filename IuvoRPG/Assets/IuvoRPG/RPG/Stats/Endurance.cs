using System;
using UnityEngine;

[Serializable]
public class Endurance : Stat
{

    [SerializeField] float currentCarryWeight = 0.0f;
    [SerializeField] float maxCarryWeight = 100.0f;


    [SerializeField] float frostResistance = 0.25f;
    [SerializeField] float maxFrostResitance = 0.75f;

    [SerializeField] float lungCapacity = 0.35f;
    [SerializeField] float maxLungCapacity = 1.0f;

    [SerializeField] Agility myAgility;
    
    public override void OnStart()
    {
        _levelValue = 1;
        _statName = "Endurance";
    }
}
