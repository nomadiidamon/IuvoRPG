using UnityEngine;

namespace IuvoUnity
{
    namespace IuvoRPG
    {
        namespace IuvoItems
        {
#nullable enable
            public class ConsumableEffect
            {
                public delegate void ConsumableEffectDelegate();
            }

            public class ConsumableConfiguration : ItemConfiguration
            {
                public ItemConfiguration? m_itemConfig;
                public ConsumableEffect.ConsumableEffectDelegate? m_effect;

                public ConsumableConfiguration()
                {
                    m_itemConfig = new ItemConfiguration();
                    m_effect = null;
                }
                public ConsumableConfiguration(ItemConfiguration? itemConfig, ConsumableEffect.ConsumableEffectDelegate? effect)
                {
                    m_itemConfig = itemConfig ?? new ItemConfiguration();
                    m_effect = effect;
                }


                public ConsumableConfiguration WithItemConfiguration(ItemConfiguration itemConfig)
                {
                    m_itemConfig = itemConfig;
                    return this;
                }

                public ConsumableConfiguration WithExtraEffect(ConsumableEffect.ConsumableEffectDelegate toAdd)
                {
                    m_effect += toAdd;
                    return this;
                }

                public override void DebugConfiguration()
                {
                    base.DebugConfiguration();
                }

                public override void DebugConfigurationWithDescription()
                {
                    base.DebugConfigurationWithDescription();
                    if (m_effect != null) { 
                       Debug.Log("Active Consumable Delegate.");
                    }
                    else
                    {
                        Debug.Log("Inactive Consumable Delegate.");
                    }
                }




            }
        }
    }
}