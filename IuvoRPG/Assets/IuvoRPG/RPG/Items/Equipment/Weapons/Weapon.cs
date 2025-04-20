using System;

namespace IuvoUnity
{
    namespace IuvoRPG
    {
        namespace IuvoItems
        {
            public class Weapon : Equipment
            {
                int m_accuracy { get; set; }
                int m_damage { get; set; }
                DamageType m_damageType { get; set; }
                bool m_isTwoHanded;


                public Weapon(string name, int value, string description, ItemType type,
                    int max_durability, int curr_durability, int statBonus, int weight,
                    int accuracy, int damage, DamageType damamageType, bool twoHanded = false)
                    : base(name, value, description, type, max_durability, curr_durability, statBonus, weight)
                {
                    m_accuracy = accuracy;
                    m_damage = damage;
                    m_damageType = damamageType;
                    m_isTwoHanded = twoHanded;
                }
            }
        }
    }
}