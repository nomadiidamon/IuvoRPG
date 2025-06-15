using System;
using UnityEngine;

[Serializable]
public class Dexterity : Stat
{
    [SerializeField] Strength myStrength;

    public override void OnStart()
    {
        _levelValue = 1;
        _statName = "Dexterity";
    }
}
