using UnityEngine;

namespace RPGCore.Tables
{
	public class AssetSelector<T, E> : ISerializationCallbackReceiver
		where E : GenericTableEntry<T>
	{
		public E[] Loot;

		[System.NonSerialized]
		protected float[] values;

		public T Select ()
		{
			float rand = Random.Range (0.0f, 1.0f);

			for (int i = 0; i < Loot.Length; i++)
			{
				float balance = values[i];

				if (balance == 0.0f)
					continue;

				if (rand <= balance)
				{
					return Loot[i].Item;
				}
			}
			return default (T);
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize () { }

		void ISerializationCallbackReceiver.OnAfterDeserialize ()
		{
			float total = 0.0f;
			for (int i = 0; i < Loot.Length; i++)
			{
				total += Loot[i].Balance;
			}

			values = new float[Loot.Length];
			float lastValue = 0.0f;

			for (int i = 0; i < Loot.Length; i++)
			{
				lastValue = lastValue + (Loot[i].Balance / total);
				values[i] = lastValue;
			}
		}
	}
}