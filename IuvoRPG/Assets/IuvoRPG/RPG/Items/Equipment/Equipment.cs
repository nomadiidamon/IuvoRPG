namespace IuvoUnity
{
    namespace IuvoRPG
    {
        namespace IuvoItems
        {
            public class Equipment : Item
            {
                int m_maxDurability { get; set; }
                int m_currDurability { get; set; }
                int m_statBonus { get; set; }
                int m_weight { get; set; }

                public Equipment(string name, int value, string description, ItemType type,
                    int max_durability, int curr_durability, int statBonus, int weight)
                    : base(name, value, description, type)
                {
                    m_maxDurability = max_durability;
                    m_currDurability = curr_durability;
                    m_statBonus = statBonus;
                    m_weight = weight;
                }
            }
        }
    }
}
