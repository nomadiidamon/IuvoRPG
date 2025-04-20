using System;
using UnityEngine;


namespace IuvoUnity
{

    namespace IuvoRPG
    {
        [System.Serializable]
        [SerializeField]
        public class Mana : Stat
        {
            [Header("Values")]
            [SerializeField] public int currentManaPoints;
            [SerializeField] public int maxManaPoints;


            public Mana(string name, int level, int expToNextLevel, float levelRate)
                : base(name, level)
            {
                _expToNextLevel = expToNextLevel;
                _levelRate = levelRate;

                CalculateInitialManaPoints();
            }

            private void CalculateInitialManaPoints()
            {
                if (_value > 1)
                {
                    currentManaPoints = 75 * _value;
                    maxManaPoints = 120 * _value;
                }
                else
                {
                    currentManaPoints = 75;
                    maxManaPoints = 120;
                }
            }
        }
    }
}