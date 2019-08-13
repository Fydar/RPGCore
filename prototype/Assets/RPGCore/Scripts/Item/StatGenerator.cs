using UnityEngine;

namespace RPGCore
{
	public static class StatGenerator
	{
		public static float Value (IItemSeed context, short StatID)
		{
			if (StatID < 0)
			{
				int negativeIDs = 1 + StatID;

				foreach (float[] overrides in context.NegativeOverrides)
				{
					if (overrides == null)
						continue;

					if (overrides.Length > negativeIDs)
					{
						return overrides[negativeIDs];
					}
				}
				return 0.0f;
			}

			foreach (float[] overrides in context.PositiveOverrides)
			{
				if (overrides == null)
					continue;

				if (overrides.Length > StatID)
				{
					return overrides[StatID];
				}
			}

			int seed = (context.Seed.Value << 16) + StatID;

			var rand = new System.Random (seed);

			return (float)rand.NextDouble ();
		}

		public static int IntRange (IItemSeed context, short StatID, int min, int max)
		{
			int value = Mathf.RoundToInt (Mathf.Lerp ((float)min, (float)max, Value (context, StatID)));

			if (value == max + 1)
				return max;
			return value;
		}

		public static float FloatRange (IItemSeed context, short StatID, float min, float max)
		{
			return Mathf.Lerp ((float)min, (float)max, Value (context, StatID));
		}

		private static int HashInts (short a, short b)
		{
			var A = (uint)(a >= 0 ? 2 * a : -2 * a - 1);
			var B = (uint)(b >= 0 ? 2 * b : -2 * b - 1);
			var C = (int)((A >= B ? A * A + A + B : A + B * B) / 2);

			return a < 0 && b < 0 || a >= 0 && b >= 0 ? C : -C - 1;
		}
	}
}

