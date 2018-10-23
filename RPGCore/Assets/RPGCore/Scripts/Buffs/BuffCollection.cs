using System;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RPGCore
{
	public class BuffCollection : IEnumerable<Buff>
	{
		public event Action<Buff> OnAddBuff;
		public event Action<Buff> OnRemoveBuff;

		private List<Buff> buffs = new List<Buff> ();

		//private RPGCharacter character = null;

		public int Count
		{
			get
			{
				return buffs.Count;
			}
		}

		public Buff this[int index]
		{
			get
			{
				return buffs[index];
			}
		}

		public BuffCollection (RPGCharacter applyBuffs)
		{
			//character = applyBuffs;
		}

		public IEnumerator<Buff> GetEnumerator ()
		{
			return buffs.GetEnumerator ();
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return buffs.GetEnumerator ();
		}

		public void Add (Buff buff)
		{
			buffs.Add (buff);

			Action removeCallback = null;

			removeCallback = () =>
			{
				buffs.Remove (buff);
				buff.OnRemove -= removeCallback;
			};

			buff.OnRemove += removeCallback;

			if (OnAddBuff != null)
				OnAddBuff (buff);
		}

		public void Remove (Buff buff)
		{
			buff.RemoveBuff ();

			if (OnRemoveBuff != null)
				OnRemoveBuff (buff);
		}

		public Buff Find (BuffTemplate template)
		{
			for (int i = buffs.Count - 1; i >= 0; i--)
			{
				Buff buff = buffs[i];

				if (buff.buffTemplate == template)
				{
					return buff;
				}
			}

			return null;
		}

		public void Update (float deltaTime)
		{
			for (int i = buffs.Count - 1; i >= 0; i--)
			{
				Buff buff = buffs[i];

				buff.Update (deltaTime);
			}
		}
	}
}