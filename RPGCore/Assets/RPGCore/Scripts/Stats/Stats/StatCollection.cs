using RPGCore.Behaviour.Connections;
using RPGCore.Stats;
using System;
using UnityEngine;

namespace RPGCore
{
	public class StatCollection<T> : EnumerableCollection<T>
	{
		[Header ("General")]
		public T Attack;
		public T AttackSpeed;
		public T CriticalStrikeChance;
		public T CriticalStrikeDamage;

		[Space]
		public T MovementSpeed;
		public T Armour;
		public T MagicResistance;

		[Header ("Health")]
		public T MaxHealth;
		public T HealthRegeneration;
		public T HealthRegenerationDelay;

		[Header ("Mana")]
		public T MaxMana;
		public T ManaRegeneration;
		public T ManaRegenerationDelay;
		public T ManaBurnoutDelay;
		public T ManaBurnoutDebt;

		//public WeaponCollection<T> WeaponModifiers;
		//public ElementCollection<T> ElementalResistances;
	}

	public class WeaponCollection<T> : EnumerableCollection<T>
	{
		public T OneHanded;
		public T TwoHanded;
		public T Bow;
		public T Axe;
		public T Dagger;
	}

	public class ElementCollection<T> : EnumerableCollection<T>
	{
		public T Physical;
		public T Fire;
		public T Water;
		public T Nature;
	}
}
