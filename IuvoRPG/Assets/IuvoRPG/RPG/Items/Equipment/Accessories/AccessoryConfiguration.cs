using UnityEngine;
using static IuvoUnity.IuvoRPG.IuvoItems.Accessory;


namespace IuvoUnity
{
    namespace IuvoRPG
    {
        namespace IuvoItems
        {
#nullable enable
            public class AccessoryConfiguration : EquipmentConfiguration
            {
                public EquipmentConfiguration? m_equipmentConfig;
                public AccessorySlot? m_accssorySlot;
                public Stat m_toModify;

                public AccessoryConfiguration(Stat toBoost)
                {
                    m_equipmentConfig = new EquipmentConfiguration();
                    m_accssorySlot = AccessorySlot.NULL;
                    m_toModify = toBoost;
                }

                public AccessoryConfiguration(EquipmentConfiguration? equipmentConfig, AccessorySlot? accssorySlot, Stat toModify)
                {
                    m_equipmentConfig = equipmentConfig ?? new EquipmentConfiguration();
                    m_accssorySlot = accssorySlot ?? AccessorySlot.NULL;
                    m_toModify = toModify;
                }

                // need to add builder functions

                // need to add debug functions

            }
        }
    }
}