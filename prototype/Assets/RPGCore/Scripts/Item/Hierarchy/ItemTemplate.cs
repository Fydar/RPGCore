﻿using RPGCore.Audio;
using RPGCore.Behaviour;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore
{
	public abstract class ItemTemplate : ScriptableObject, IBehaviourGraph
	{
		public abstract string BaseName { get; }
		public abstract string Description { get; }
		public abstract Sprite Icon { get; }

		public abstract GameObject RenderPrefab { get; }
		public abstract float RenderScale { get; }

		public abstract int Weight { get; }
		public abstract int StackSize { get; }

		public abstract ItemRarity Rarity { get; }
		public abstract SfxGroup StartDrag { get; }
		public abstract SfxGroup EndDrag { get; }

		public abstract IEnumerable<float[]> PositiveOverrides { get; }
		public abstract IEnumerable<float[]> NegativeOverrides { get; }
		public abstract List<BehaviourNode> AllNodes { get; set; }

		public abstract ItemSurrogate GenerateItem(ItemData data);

		public ItemSurrogate GenerateItem()
		{
			var data = new ItemData();

			var rand = new System.Random(Time.renderedFrameCount);
			data.seed.Value = (short)rand.Next(short.MinValue, short.MaxValue);

			return GenerateItem(data);
		}

		public static ItemTier ItemTierRandom(ItemSurrogate item, ItemTier min, ItemTier max)
		{
			int random = StatGenerator.IntRange(item, 0, (int)min, (int)max);
			return ((ItemTier)random);
		}
	}
}
