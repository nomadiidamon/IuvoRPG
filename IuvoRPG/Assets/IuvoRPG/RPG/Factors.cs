using System;
using UnityEngine;

namespace IuvoUnity
{
    namespace IuvoRPG
    {

        public interface IStatusEffect
        {
            public void ApplyStatusEffect();
            public float m_effectDuration { get; set; }
        }

        // TODO: Make a generic class for status effects
        [System.Serializable]
        public enum PositiveStatusType { equilibrium, other, FlowState, gainHealth, gainMana, gainStamina, breakOpponenet, executeOpponent, dodge };
        
        // TODO: Make a generic class for status effects
        [System.Serializable]
        public enum NegativeStatusType { equilibrium, other, Burning, Freezing, Drenched, Shocked, Burdened, Poisoned, Bleeding, stunned }

        [System.Serializable]
        public class Status : IStatusEffect
        {
            [SerializeField] public PositiveStatusType m_positiveStatus { get; set; }
            [SerializeField] public NegativeStatusType m_negativeStatus { get; set; }
            [SerializeField] public bool m_isActive { get; set; }

            [SerializeField] public float m_effectDuration { get; set; }

            public virtual void ApplyStatusEffect()
            {

            }



        }

    }
}