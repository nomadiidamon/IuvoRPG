using System;
using UnityEngine;

[Serializable]
public class Wisdom : Stat
{
    [SerializeField] Intelligence myIntelligence;

    public override void OnStart()
    {
        _levelValue = 1;
        _statName = "Wisdom";
    }

}
