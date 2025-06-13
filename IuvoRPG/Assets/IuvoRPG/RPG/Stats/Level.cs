using System;
using UnityEngine;

[Serializable]
public class Level : Stat
{
    [Header("Level 'EXP'")]
    [SerializeField] private int currentExperience = 0;
    [SerializeField] private int expToNextLevel = 50;
    [SerializeField] private float levelRate = 0.5f;
    
    public bool LevelUp()
    {
        if (currentExperience >= expToNextLevel)
        {
            int difference = currentExperience - expToNextLevel;
            expToNextLevel = (int)(expToNextLevel * levelRate);
            SetLevel(GetLevel() + 1);
            currentExperience = difference;
            return true;
        }
        return false;
    }

    public void AddExperience(int experience)
    {
        currentExperience += experience;
        LevelUp();
    }

    public void RemoveExperience(int experience)
    {
        currentExperience -= experience;
        if (currentExperience <= 0)
        {
            currentExperience = 0;
        }
    }

    #region Getters & Setters
    public int GetCurrentExperience() => currentExperience;
    public int GetExpToNextLevel() => expToNextLevel;
    public float GetLevelRate() => levelRate;

    public void SetCurrentExperience(int currentExperience) => this.currentExperience = currentExperience;

    public void SetExperienceToNextLevel(int expToNext) => this.expToNextLevel = expToNext;
    public void SetLevelRate(float levelRate) => this.levelRate = levelRate;



    #endregion

}
