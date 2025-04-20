using UnityEngine;

namespace IuvoUnity
{
    namespace IuvoRPG
    {
        namespace IuvoItems
        {
            public class Accessory : Equipment
            {
                public enum AccessorySlot
                {
                    VEIL,
                    CROWN,
                    CAPE,
                    COLLAR,
                    LEFT_EARRING,
                    RIGHT_EARRING,
                    LEFT_WRIST,
                    RIGHT_WRIST,
                    LEFT_RING,
                    RIGHT_RING,
                    NULL
                }
                
                public AccessorySlot m_acessorySlot;
                public Stat toAddTo;

                public Accessory(string name, int value, string description, ItemType type,
                    int max_durability, int curr_durability, int statBonus, int weight, AccessorySlot acessorySlot, Stat toAddTo)
                    : base(name, value, description, type, max_durability, curr_durability, statBonus, weight)

                {
                    m_acessorySlot = acessorySlot;
                    this.toAddTo = toAddTo;
                }
            }
        }
    }
}