using System;

namespace RPGCore.Stats
{
	[Serializable]
	public abstract class AttributeInstance
	{
		public Action OnValueChanged;

		public void InvokeChanged ()
		{
			if (OnValueChanged != null)
			{
				OnValueChanged ();
			}
		}

		public abstract float Value
		{
			get;
			set;
		}
	}
}