using UnityEngine;
using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RPGCore.Tables
{
	[CreateAssetMenu (menuName = "RPGCore/Loottable")]
	public class Loottable : ScriptableObject
	{
		[Serializable]
		public class ItemEntry : GenericTableEntry<ItemGenerator>
		{
			public ItemEntry (ItemGenerator item, float balance)
				: base (item, balance)
			{

			}
		}

		[Serializable]
		public class ItemRoll : MultiAssetSelector<ItemGenerator, ItemEntry>
		{

		}

		public ItemRoll[] TableRolls;
		private List<ItemSurrogate> cachedGeneratedItems;

		public List<ItemSurrogate> Select ()
		{
			if (cachedGeneratedItems == null)
			{
				cachedGeneratedItems = new List<ItemSurrogate> ();
			}
			cachedGeneratedItems.Clear ();

			for (int i = 0; i < TableRolls.Length; i++)
			{
				ItemGenerator itemGenerator = TableRolls[i].Select ();
				if (itemGenerator == null)
					continue;

				if (itemGenerator.RewardTemplate != null)
				{
					ItemSurrogate generatedItem = itemGenerator.Generate ();
					cachedGeneratedItems.Add (generatedItem);
				}
			}

			return cachedGeneratedItems;
		}

		public List<ItemSurrogate> SelectMultiple ()
		{
			if (cachedGeneratedItems == null)
			{
				cachedGeneratedItems = new List<ItemSurrogate> ();
			}
			cachedGeneratedItems.Clear ();

			for (int i = 0; i < TableRolls.Length; i++)
			{
				ItemGenerator[] itemGenerator = TableRolls[i].SelectMultiple ();
				for (int x = 0; x < itemGenerator.Length; x++)
				{
					if (itemGenerator[x].RewardTemplate)
					{
						ItemSurrogate generatedItem = itemGenerator[x].Generate ();
						cachedGeneratedItems.Add (generatedItem);
					}
				}
			}

			return cachedGeneratedItems;
		}

#if UNITY_EDITOR
		[CustomEditor (typeof (Loottable), true)]
		class LootTableDrawer : Editor
		{
			Loottable lootTable;
			public override void OnInspectorGUI ()
			{
				//lootTable = (Loottable)target;

				DrawDefaultInspector ();
			}
		}
#endif
	}
}

