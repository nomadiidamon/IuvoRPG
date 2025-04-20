using UnityEngine;

namespace IuvoUnity
{
    namespace IuvoRPG
    {
        namespace IuvoItems
        {
            public enum ItemType { KEY_ITEM, MATERIAL_ITEM, EQUIPMENT_ITEM, CONSUMABLE_ITEM, BASIC_ITEM }

            public class Item : ScriptableObject
            {
                private string m_name { get; set; }
                ItemType m_type { get; set; }
                private int m_value { get; set; }
                private string m_description { get; set; }

#nullable enable 
                ItemConfiguration? m_configuration { get; set; }
#nullable disable


                public Item(string name, int value, string description, ItemType type)
                {
                    m_name = name;
                    m_type = type;
                    m_value = value;
                    m_description = description;
                }

                public virtual void Configure(ItemConfiguration config)
                {
                    if (config == null) { return; }

                    m_configuration = config;
                    m_name = config.m_name ?? "default name";
                    m_type = config.m_itemType ?? ItemType.BASIC_ITEM;
                    m_value = config.m_value ?? 0;
                    m_description = config.m_description ?? "default description";
                }
            }
        }
    }
}

