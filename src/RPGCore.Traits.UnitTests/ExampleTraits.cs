using System.Collections.Generic;

namespace RPGCore.Traits.UnitTests
{
	public class CharacterTrait<TStat, TState> : TraitCollection<TStat, TState>
		where TStat : IFixedElement, new()
		where TState : IFixedElement, new()
	{
		public TState Health;
		public TStat MaxHealth;

		public TState Mana;
		public TStat MaxMana;
	}

	public class WeaponTraits<TStat, TState> : TraitCollection<TStat, TState>
		where TStat : IFixedElement, new()
		where TState : IFixedElement, new()
	{
		public TStat Damage;
		public TStat Quality;

		public TStat MaxDurability;
		public TState Durability;
	}
}
