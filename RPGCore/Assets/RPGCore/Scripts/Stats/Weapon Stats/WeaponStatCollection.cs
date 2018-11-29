using RPGCore.Behaviour.Connections;
using RPGCore.Stats;
using System;

namespace RPGCore
{
	[Serializable]
	public class StatInstanceWeaponStatCollection : WeaponStatCollection<StatInstance> { }

	[Serializable]
	public class FloatInputWeaponStatCollection : WeaponStatCollection<FloatInput> { }

	public class WeaponStatCollection<T> : EnumerableCollection<T>
	{
		public T Attack;
		public T AttackSpeed;
		public T CriticalStrikeChance;
		public T CriticalStrikeMultiplier;
	}
}
