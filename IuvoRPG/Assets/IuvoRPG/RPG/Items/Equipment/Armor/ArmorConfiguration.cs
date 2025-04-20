using UnityEngine;

namespace IuvoUnity
{
    namespace IuvoRPG
    {
        namespace IuvoItems
        {
#nullable enable
            public class ArmorConfiguration : EquipmentConfiguration
            {
                public EquipmentConfiguration? m_equipmentConfig;
                public int? m_defense;
                public ArmorSlot? m_armorSlot;
                public DamageType? m_damageType;
                public DamageValue[]? m_protections;


                public ArmorConfiguration()
                {
                    m_equipmentConfig = new EquipmentConfiguration();
                    m_defense = 10;
                    m_armorSlot = ArmorSlot.NULL;
                    m_damageType = DamageType.NULL;
                    m_protections = new DamageValue[0];             
                }

                public ArmorConfiguration(EquipmentConfiguration? equipmentConfig, int? defense, ArmorSlot? slot, DamageType? damageType, DamageValue[]? protections)
                {
                    m_equipmentConfig = equipmentConfig ?? new EquipmentConfiguration();
                    m_defense = defense ?? 10;
                    m_armorSlot = slot ?? ArmorSlot.NULL;
                    m_damageType = damageType ?? DamageType.NULL;
                    m_protections = protections ?? new DamageValue[0];
                }

                public ArmorConfiguration WithEquipmentConfiguration(EquipmentConfiguration equipmentConfig)
                {
                    m_equipmentConfig = equipmentConfig;
                    return this;
                }
                public ArmorConfiguration WithDefense(int defense)
                {
                    m_defense = defense;
                    return this;
                }
                public ArmorConfiguration WithArmorSlot(ArmorSlot slot)
                {
                    m_armorSlot = slot;
                    return this;

                }
                public ArmorConfiguration WithDamageType(DamageType damageType) { 
                
                    m_damageType = damageType;
                    return this;
                }
                public ArmorConfiguration WithProtections(DamageValue[] protections)
                {

                    m_protections = protections;
                    return this;
                }

                public override void DebugConfiguration()
                {
                    base.DebugConfiguration();
                    Debug.Log("Armor Defense: " + m_defense);
                    Debug.Log("Armor Slot: " + m_armorSlot.ToString());
                    Debug.Log("Armor Damage: " + m_damageType.ToString());
                    Debug.Log("Armor Defense: " + m_defense);
                }

                public override void DebugConfigurationWithDescription()
                {
                    base.DebugConfigurationWithDescription();
                    Debug.Log("Armor Defense: " + m_defense);
                    Debug.Log("Armor Slot: " + m_armorSlot.ToString());
                    Debug.Log("Armor Damage: " + m_damageType.ToString());
                    Debug.Log("Armor Defense: " + m_defense);
                }
            }
        }
    }
}