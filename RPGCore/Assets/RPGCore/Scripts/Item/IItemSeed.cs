using System.Collections.Generic;

namespace RPGCore
{
	public interface IItemSeed
	{
		ShortEventField Seed
		{
			get;
		}

		IEnumerable<float[]> NegativeOverrides
		{
			get;
		}

		IEnumerable<float[]> PositiveOverrides
		{
			get;
		}
	}
}

