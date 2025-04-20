using UnityEngine;

namespace IuvoUnity
{
    namespace IuvoRPG
    {
        namespace IuvoItems
        {
#nullable enable
            [System.Serializable]
            public class ItemConfiguration
            {
                public string? m_name;
                public ItemType? m_itemType;
                public int? m_value;
                public string? m_description;

                public ItemConfiguration()
                {
                    m_name = "defualt";
                    m_itemType = ItemType.BASIC_ITEM;
                    m_value = 0;
                    m_description = "default";
                }
                public ItemConfiguration(string? name, ItemType? type, int? value, string? description)
                {
                    m_name = name ?? "default";
                    m_itemType = type ?? ItemType.BASIC_ITEM;
                    m_value = value ?? 0;
                    m_description = description ?? "default";
                }
                public ItemConfiguration WithName(string name)
                {
                    m_name = name;
                    return this;
                }
                public ItemConfiguration WithItemType(ItemType type)
                {
                    m_itemType = type;
                    return this;
                }
                public ItemConfiguration WithValue(int value)
                {
                    m_value = value;
                    return this;
                }
                public ItemConfiguration WithDescription(string description)
                {
                    m_description = description;
                    return this;
                }

                public virtual void DebugConfiguration()
                {
                    Debug.Log("Item Name: " + m_name);
                    Debug.Log("Item Type: " + m_itemType.ToString());
                    Debug.Log("Item Value: " + m_value);
                }

                public virtual void DebugConfigurationWithDescription()
                {
                    DebugConfiguration();
                    Debug.Log("Description: " + m_description);
                }



            }
        }
    }
}