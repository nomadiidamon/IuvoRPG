using System;
using UnityEngine;


namespace IuvoUnity
{
	namespace IuvoRPG
	{
		[System.Serializable]
		public enum DamageType
		{
			PSYCHE_CONFUSION, PYSCHE_PAIN, PSYCHE_SLEEP, PSYCHE_PARALYSIS, PYSCHE_UNCONSCIOUS,		// Mental supercession

			PHYSICAL_PIERCING, PHYSICAL_SLASHING, PHYSICAL_BLUNT,									// Physical Suffering
			
			MAGIC_FIRE, MAGIC_LAVA, MAGIC_METAL, MAGIC_EARTH, MAGIC_WATER, MAGIC_ICE,				// Magical Implementation
			MAGIC_AIR, MAGIC_ELECTRICITY, NULL
		}

		[System.Serializable]
		[SerializeField]
		public class DamageValue
		{
			[SerializeField] public DamageType damageType;
			[SerializeField] public int pointsToTakeFromHealth;

			public DamageValue(int points, DamageType type = DamageType.PHYSICAL_BLUNT)
			{
				damageType = type;
				pointsToTakeFromHealth = points;
			}

		}
	}
}