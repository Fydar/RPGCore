namespace RPGCore
{
	public class WeaponStatCollection<T> : EnumerableCollection<T>
	{
		public T Attack;
		public T AttackSpeed;
		public T CriticalStrikeChance;
		public T CriticalStrikeMultiplier;
	}
}