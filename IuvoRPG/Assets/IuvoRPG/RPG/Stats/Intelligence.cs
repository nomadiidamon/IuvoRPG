using System;
using UnityEngine;

[Serializable]
public class Intelligence : Stat
{
    [SerializeField] Wisdom myWisdom;


    public override void OnStart()
    {
        _levelValue = 1;
        _statName = "Intelligence";
    }
}
