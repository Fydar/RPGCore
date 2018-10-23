using UnityEngine;

namespace RPGCore
{
	public class StateCollection<T> : EnumerableCollection<T>
	{
		[Header ("General")]
		public T CurrentHealth;
		public T CurrentMana;

		[Header ("Regeneration")]
		public T ManaDelayRemaining;
		public T HealthDelayRemaining;
	}
}