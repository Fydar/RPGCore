using UnityEngine;
using System.Collections.Generic;
using RPGCore.Audio;

namespace RPGCore
{
	public abstract class ItemTemplate : BehaviourGraph
	{
		public abstract GameObject RenderPrefab { get; }
		public abstract float RenderScale { get; }

		public abstract ItemRarity Rarity { get; }

		public abstract SfxGroup StartDrag { get; }
		public abstract SfxGroup EndDrag { get; }

		public Sprite Icon
		{
			get
			{
				return GetItemIcon ();
			}
		}

		public string BaseName
		{
			get
			{
				return GetItemBaseName ();
			}
		}

		public string Description
		{
			get
			{
				return GetItemDescription ();
			}
		}

		public int Weight
		{
			get
			{
				return GetItemWeight ();
			}
		}

		public int StackSize
		{
			get
			{
				return GetItemMaxStack ();
			}
		}

		public IEnumerable<float[]> PositiveOverrides
		{
			get
			{
				return GetItemPositiveOverrides ();
			}
		}

		public IEnumerable<float[]> NegativeOverrides
		{
			get
			{
				return GetItemNegativeOverrides ();
			}
		}

		public abstract ItemSurrogate GenerateItem (ItemData data);

		protected abstract string GetItemBaseName ();
		protected abstract string GetItemDescription ();
		protected abstract int GetItemWeight ();
		protected abstract int GetItemMaxStack ();

		protected abstract IEnumerable<float[]> GetItemPositiveOverrides ();
		protected abstract IEnumerable<float[]> GetItemNegativeOverrides ();

		protected abstract Sprite GetItemIcon ();

		public ItemSurrogate GenerateItem ()
		{
			ItemData data = new ItemData ();

			System.Random rand = new System.Random (Time.renderedFrameCount);
			data.seed.Value = (short)rand.Next (short.MinValue, short.MaxValue);

			return GenerateItem (data);
		}

		public static ItemTier ItemTierRandom (ItemSurrogate Item, ItemTier min, ItemTier max)
		{
			int random = StatGenerator.IntRange (Item, 0, (int)min, (int)max);
			return ((ItemTier)random);
		}
	}
}