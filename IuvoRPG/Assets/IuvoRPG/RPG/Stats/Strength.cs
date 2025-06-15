using System;
using UnityEngine;

[Serializable]
public class Strength : Stat
{
    [SerializeField] Dexterity myDexterity;

    public override void OnStart()
    {
        _levelValue = 1;
        _statName = "Strength";
    }

}
