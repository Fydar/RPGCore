using System;
using UnityEngine;

namespace RPGCore
{
	[Serializable]
	public class EnchantmantData
	{
		public ShortEventField seed = new ShortEventField ();
		public int EnchantmentID = -1;
		
		public EnchantmantData Duplicate ()
		{
			return new EnchantmantData (EnchantmentID, seed.Value);
		}

		public EnchantmantData (Enchantment id)
		{
			//EnchantmentID = EnchantmentDatabase.Instance.Enchantments.GetKey (ID.Template);
			seed.Value = RandomSeed ();
		}

		public EnchantmantData (Enchantment id, short overrideSeed)
		{
			//EnchantmentID = EnchantmentDatabase.Instance.Enchantments.GetKey (ID.Template);
			seed.Value = overrideSeed;
		}

		public EnchantmantData (EnchantmentTemplate id)
		{
			//EnchantmentID = EnchantmentDatabase.Instance.Enchantments.GetKey (ID);
			seed.Value = RandomSeed ();
		}

		public EnchantmantData (EnchantmentTemplate id, short overrideSeed)
		{
			//EnchantmentID = EnchantmentDatabase.Instance.Enchantments.GetKey (ID);
			seed.Value = overrideSeed;
		}

		public EnchantmantData (int id)
		{
			EnchantmentID = id;
			seed.Value = RandomSeed ();
		}

		public EnchantmantData (int id, short overrideSeed)
		{
			EnchantmentID = id;
			seed.Value = overrideSeed;
		}

		public void Reset ()
		{
			seed.Value = 0;
			EnchantmentID = -1;
		}

		public short RandomSeed ()
		{
			System.Random rand = new System.Random (Time.renderedFrameCount);

			return (short)rand.Next (short.MinValue, short.MaxValue);
		}
	}
}
