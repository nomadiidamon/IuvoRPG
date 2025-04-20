using System.Collections.Generic;
using UnityEngine;

namespace IuvoUnity
{
    namespace IuvoRPG
    {
        namespace IuvoItems
        {
            public class Inventory : ScriptableObject
            {
                List<Item> items;
                int maxCapacity;
                int currentCapacity;
                int totalWeight;

                public Inventory()
                {

                }
            }
        }
    }
}