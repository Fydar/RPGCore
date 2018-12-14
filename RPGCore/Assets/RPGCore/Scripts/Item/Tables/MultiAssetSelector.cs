#if UNITY_EDITOR
#endif

namespace RPGCore.Tables
{
	public class MultiAssetSelector<T, E> : AssetSelector<T, E>
		where E : GenericTableEntry<T>
	{
		public uint Rolls = 1;

		public T[] SelectMultiple ()
		{
			T[] selected = new T[Rolls];

			for (uint i = 0; i < Rolls; i++)
			{
				selected[i] = Select ();
			}

			return selected;
		}
	}
}

