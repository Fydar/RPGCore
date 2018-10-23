using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif

namespace RPGCore
{
	[System.Serializable]
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