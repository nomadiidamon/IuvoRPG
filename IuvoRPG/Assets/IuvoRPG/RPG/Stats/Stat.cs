using IuvoUnity._BaseClasses._RPG;
using System;
using UnityEngine;

[Serializable]
public class Stat : MonoBehaviour
{
    [SerializeField] private string _statName;
    [SerializeField] private int _levelValue;


    #region Getters & Setters

    public int GetLevel() => _levelValue;
    public string GetName() => _statName;

    protected void SetLevel(int newLevel) => _levelValue = newLevel;
    protected void SetName(string newName) => _statName = newName;

    #endregion
}