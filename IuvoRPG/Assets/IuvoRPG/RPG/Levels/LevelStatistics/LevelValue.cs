using System;
using UnityEngine;


namespace IuvoUnity
{

    namespace IuvoRPG
    {
        [System.Serializable]
        [SerializeField]
        public class LevelValue : MonoBehaviour
        {
            [Header("Level Value")]
            [SerializeField] public int _value;

            [Header("Experience / Progression")]
            [SerializeField] public int _currExperience;
            [SerializeField] public int _expToNextLevel;
            [SerializeField] public float _levelRate;


            public LevelValue(int value, int currExperience = 0, int expToNextLevel = 100, float levelRate = 1.0f)
            {
                _value = value;
                _currExperience = currExperience;
                _expToNextLevel = expToNextLevel;
                _levelRate = levelRate;
            }

            public bool AddExperience(int exp)
            {
                _currExperience += exp;

                if (_currExperience >= _expToNextLevel)
                {
                    LevelUp();
                    return true;
                }

                return false;
            }

            private void LevelUp()
            {
                _value++;
                _currExperience -= _expToNextLevel;
                _expToNextLevel = (int)(_expToNextLevel * _levelRate * _value);
            }

        }
    }
}