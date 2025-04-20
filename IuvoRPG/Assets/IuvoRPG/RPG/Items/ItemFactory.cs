using IuvoUnity.IuvoRPG.IuvoItems;
using UnityEngine;

namespace IuvoUnity
{
    namespace IuvoRPG
    {
        public static class ItemFactory
        {
            public static Item CreateItem(ItemConfiguration config)
            {
                Item item = new(config.m_name, config.m_value.Value, config.m_description, config.m_itemType.Value);
                // apply configuration
                item.Configure(config);
                return item;
            }


            public static Consumable CreateConsumable(ConsumableConfiguration config)
            {
                Consumable item = new(config.m_name, config.m_value.Value, config.m_description, config.m_itemType.Value);
                // apply configuration
                item.Configure(config);
                return item;
            }

            public static Equipment CreateEquipment(EquipmentConfiguration config)
            {
                Equipment item = new(config.m_name, config.m_value.Value, config.m_description, config.m_itemType.Value, config.m_maxDurability.Value, config.m_currDurability.Value, config.m_statBonus.Value, config.m_weight.Value);
                // apply configuration
                item.Configure(config);
                return item;
            }

            public static Armor CreateArmor(ArmorConfiguration config)
            {
                Armor item = new(config.m_name, config.m_value.Value, config.m_description, config.m_itemType.Value, config.m_maxDurability.Value, config.m_currDurability.Value, config.m_statBonus.Value, config.m_weight.Value, config.m_defense.Value, config.m_armorSlot.Value, config.m_damageType.Value);

                // apply configuration
                item.Configure(config);
                return item;
            }

            //public static Accessory CreateAccessory(AccessoryConfiguration config)
            //{
            //    Accessory item;

            //    // apply configuration

            //    return item;
            //}

            //public static Weapon CreateWeapon(WeaponConfiguration config)
            //{
            //    Weapon item;

            //    // apply configuration

            //    return item;
            //}

        }
    }
}