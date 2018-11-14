using RPGCore.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore
{
	[Serializable]
	public class ShortEventField : EventField<short>
	{
		public ShortEventField () : base () { }
		public ShortEventField (short defaultValue) : base (defaultValue) { }
	}

	[Serializable]
	public class UIntEventField : EventField<uint>
	{
		public UIntEventField () : base () { }
		public UIntEventField (uint defaultValue) : base (defaultValue) { }
	}

	[Serializable]
	public class IntEventField : EventField<int>
	{
		public IntEventField () : base () { }
		public IntEventField (int defaultValue) : base (defaultValue) { }
	}

	[Serializable]
	public class FloatEventField : EventField<float>
	{
		public FloatEventField () : base () { }
		public FloatEventField (float defaultValue) : base (defaultValue) { }
	}

	[Serializable]
	public class BoolEventField : EventField<bool>
	{
		public BoolEventField () : base () { }
		public BoolEventField (bool defaultValue) : base (defaultValue) { }
	}

	[Serializable]
	public class ItemTierEventField : EventField<ItemTier>
	{
		public ItemTierEventField () : base () { }
		public ItemTierEventField (ItemTier defaultValue) : base (defaultValue) { }
	}

	[Serializable]
	public class ItemData
	{
		[Header ("General")]
		public int templateID = 0;
		public ShortEventField seed = new ShortEventField ();
		public IntEventField quantity = new IntEventField (1);
		public BoolEventField damaged = new BoolEventField (false);
		public ItemTierEventField tier = new ItemTierEventField (ItemTier.Superior);

		public DataCollection dataCollection = new DataCollection ();

		[Header ("Leveling")]
		public UIntEventField experiance = new UIntEventField ();
		public UIntEventField level = new UIntEventField ();

		[Header ("Enchantment")]
		public EnchantmantData prefixData;
		public EnchantmantData suffixData;
		public EnchantmantData runeData;
		public List<EnchantmantData> modsData = new List<EnchantmantData> ();

		public void RandomizeSeed ()
		{
			seed.Value = (short)UnityEngine.Random.Range (short.MinValue, short.MaxValue);
		}

		public ItemData Duplicate ()
		{
			ItemData newData = new ItemData ();

			newData.seed.Value = seed.Value;
			newData.quantity.Value = quantity.Value;
			newData.damaged.Value = damaged.Value;
			newData.experiance.Value = experiance.Value;
			newData.level.Value = level.Value;
			newData.tier.Value = tier.Value;

			if (prefixData != null)
				newData.prefixData = prefixData.Duplicate ();

			if (suffixData != null)
				newData.suffixData = suffixData.Duplicate ();

			if (runeData != null)
				newData.runeData = runeData.Duplicate ();

			for (int i = 0; i < modsData.Count; i++)
			{
				newData.modsData.Add (modsData[i].Duplicate ());
			}

			return newData;
		}
	}
}