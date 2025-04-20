using System;
using UnityEngine;

namespace IuvoUnity
{

	namespace IuvoRPG
	{
		[System.Serializable]
        [SerializeField]

        public class StatRequirement
		{
			[SerializeField] int minLevel;
			[SerializeField] bool meetsRequirment = false;

			public StatRequirement(Stat stat, int minLevel)
			{
				this.minLevel = minLevel;

				if (stat != null)
				{

					meetsRequirment = stat._value >= minLevel;
				}
			}
		}
	}
}