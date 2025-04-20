using UnityEngine;


namespace IuvoUnity
{
    namespace IuvoRPG
    {
        namespace IuvoItems
        {
#nullable enable
            public class WeaponConfiguration
            {
                public EquipmentConfiguration? m_equipmentConfig;
                public int? m_accuracy;
                public int? m_damage;
                public DamageType? m_damageType;
                public bool? m_isTwoHanded;


                public WeaponConfiguration()
                {
                    m_equipmentConfig = new EquipmentConfiguration();
                    m_accuracy = 25;
                    m_damage = 3;
                    m_damageType = DamageType.NULL;
                    m_isTwoHanded = false;
                }

                public WeaponConfiguration(EquipmentConfiguration? equipmentConfig, int? accuracy, int? damage, DamageType? damageType, bool? isTwoHanded)
                {
                    m_equipmentConfig = equipmentConfig ?? new EquipmentConfiguration();
                    m_accuracy = accuracy ?? 5;
                    m_damage = damage ?? 1;
                    m_damageType = damageType ?? DamageType.NULL;
                    m_isTwoHanded = isTwoHanded ?? false;
                }
            }
        }
    }
}