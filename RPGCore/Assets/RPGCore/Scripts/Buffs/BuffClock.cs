using System;

#if UNITY_EDITOR
#endif

namespace RPGCore
{
	[Serializable]
	public abstract class BuffClock
	{
		public Action OnRemove;
		public Action OnTick;

		public IntegerStack StackSize = new IntegerStack ();

		public abstract float DisplayPercent
		{
			get;
		}

		public abstract void Update (float deltaTime);

		public virtual void RemoveClock ()
		{
			if (OnRemove != null)
				OnRemove ();
		}
	}
}