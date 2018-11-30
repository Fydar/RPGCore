using System.Collections.Generic;

namespace RPGCore
{
	public class Enchantment : IItemSeed
	{
		private EnchantmentTemplate template;
		private ItemSurrogate item;
		private EnchantmantData data;

		public ItemSurrogate Item
		{
			get
			{
				return item;
			}
		}

		public EnchantmentTemplate Template
		{
			get
			{
				return template;
			}
		}

		public string Affix
		{
			get
			{
				return template.Affix;
			}
		}

		public Enchantment (EnchantmentTemplate _template)
		{
			template = _template;
		}

		public void Setup (ItemSurrogate _item, EnchantmantData _data)
		{
			item = _item;
			data = _data;

		}

		public ShortEventField Seed
		{
			get
			{
				return data.seed;
			}
		}

		public IEnumerable<float[]> NegativeOverrides
		{
			get
			{
				yield return null;
			}
		}

		public IEnumerable<float[]> PositiveOverrides
		{
			get
			{
				yield return null;
			}
		}

		public void Start ()
		{

		}

		public void Stop ()
		{

		}
	}
}

