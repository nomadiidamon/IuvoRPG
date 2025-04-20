namespace IuvoUnity
{
    namespace IuvoRPG
    {
        namespace IuvoItems
        {   
            public enum ArmorSlot { HEAD, CHEST, ARMS, LEGS, NULL}

            public class Armor : Equipment
            {
                int m_defense { get; set; }
                ArmorSlot armorSlot { get; set; }
                DamageType m_damageType { get; set; }
                public DamageValue[] m_protections;

                public Armor(string name, int value, string description, ItemType type,
                    int max_durability, int curr_durability, int statBonus, int weight,
                    int defense, ArmorSlot slot, DamageType damageType)
                    : base(name, value, description, type, max_durability, curr_durability, statBonus, weight)
                {
                    m_defense = defense;
                    armorSlot = slot;
                    m_damageType = damageType;

                }

            }

        }
    }
}