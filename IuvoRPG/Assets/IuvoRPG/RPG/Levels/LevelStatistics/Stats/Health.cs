using System;
using UnityEngine;


namespace IuvoUnity
{

    namespace IuvoRPG
    {
        [System.Serializable]
        [SerializeField]
        public class Health : Stat
        {
            [Header("Values")]
            [SerializeField] public int currentHealthPoints;
            [SerializeField] public int maxHealthPoints;

            public Health(string name, int level, int expToNextLvl, float levelRate)
                : base(name, level)
            {
                _expToNextLevel = expToNextLvl;
                _levelRate = levelRate;

                CalculateInitialHealthPoints();
            }

            public void CalculateInitialHealthPoints()
            {
                if (_value > 1)
                {
                    int target = 175 * _value;
                    currentHealthPoints = target;
                    maxHealthPoints = 300 * _value;
                }
                else
                {
                    currentHealthPoints = 175;
                    maxHealthPoints = 300;
                }
            }
        }
    }
}