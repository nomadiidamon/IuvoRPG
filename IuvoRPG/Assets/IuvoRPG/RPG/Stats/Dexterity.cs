using System;
using UnityEngine;

[Serializable]
public class Dexterity : Stat
{
    [SerializeField] private float aimSpeed = 2.5f;
    [SerializeField] private float shoulderSwitchSpeed = 0.75f;
    [SerializeField] private float reloadSpeed = 1.25f;


    [SerializeField] Strength myStrength;

    public override void OnStart()
    {
        _levelValue = 1;
        _statName = "Dexterity";
    }
}
