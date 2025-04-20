using System.Diagnostics.Contracts;
using UnityEngine;

namespace IuvoUnity
{
    namespace IuvoRPG
    {
        namespace IuvoItems
        {
#nullable enable
            public class EquipmentConfiguration : ItemConfiguration
            {
                public ItemConfiguration? m_itemConfig;

                public int? m_maxDurability;
                public int? m_currDurability;
                public int? m_statBonus;
                public int? m_weight;

                public EquipmentConfiguration()
                {
                    m_itemConfig = new ItemConfiguration();
                    m_maxDurability = 50;
                    m_currDurability = 25;
                    m_statBonus = 1;
                    m_weight = 5;
                }
                public EquipmentConfiguration(ItemConfiguration? itemConfig, int? maxDurability, int? currDurability, int? statBonus, int? weight)
                {
                    m_itemConfig = itemConfig ?? new ItemConfiguration();
                    m_maxDurability = maxDurability ?? 100;
                    m_currDurability = currDurability ?? 50;
                    m_statBonus = statBonus ?? 2;
                    m_weight = weight ?? 10;
                }

                public EquipmentConfiguration WithMaxDurability(int newMax)
                {
                    m_maxDurability = newMax;
                    return this;
                }
                public EquipmentConfiguration WithCurrentDurability(int durabilityToLose)
                {
                    m_currDurability -= durabilityToLose;
                    return this;
                }
                public EquipmentConfiguration WithStatBonus(int bonus)
                {
                    m_statBonus = bonus;
                    return this;
                }
                public EquipmentConfiguration WithWeight(int weight)
                {
                    m_weight = weight;
                    return this;
                }

                public override void DebugConfiguration()
                {
                    base.DebugConfiguration();
                    Debug.Log("Equipment Max Durability: " + m_maxDurability);
                    Debug.Log("Equipment Curr. Durability: " + m_currDurability);
                    Debug.Log("Equipment Stat Bonus: " + m_statBonus);
                    Debug.Log("Equipment Weight: " + m_weight);
                }

                public override void DebugConfigurationWithDescription()
                {
                    base.DebugConfigurationWithDescription();
                }

            }
        }
    }
}