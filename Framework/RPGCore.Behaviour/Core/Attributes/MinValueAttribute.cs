using System;

namespace RPGCore.Behaviour
{
	[AttributeUsage (AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
	public class MinValueAttribute : Attribute
	{
		private readonly long longMin;
		private readonly double doubleMin;

		public MinValueAttribute (int minValue)
		{
			longMin = minValue;
		}

		public MinValueAttribute (uint minValue)
		{
			longMin = minValue;
		}

		public MinValueAttribute (byte minValue)
		{
			longMin = minValue;
		}

		public MinValueAttribute (sbyte minValue)
		{
			longMin = minValue;
		}

		public MinValueAttribute (short minValue)
		{
			longMin = minValue;
		}

		public MinValueAttribute (ushort minValue)
		{
			longMin = minValue;
		}

		public MinValueAttribute (long minValue)
		{
			longMin = minValue;
		}

		public MinValueAttribute (float minValue)
		{
			doubleMin = minValue;
		}

		public MinValueAttribute (double minValue)
		{
			doubleMin = minValue;
		}

		public void Filter (ref int value)
		{
			if (value < longMin)
			{
				value = (int)longMin;
			}
		}

		public void Filter (ref uint value)
		{
			if (value < longMin)
			{
				value = (uint)longMin;
			}
		}

		public void Filter (ref byte value)
		{
			if (value < longMin)
			{
				value = (byte)longMin;
			}
		}

		public void Filter (ref sbyte value)
		{
			if (value < longMin)
			{
				value = (sbyte)longMin;
			}
		}

		public void Filter (ref short value)
		{
			if (value < longMin)
			{
				value = (short)longMin;
			}
		}

		public void Filter (ref ushort value)
		{
			if (value < longMin)
			{
				value = (ushort)longMin;
			}
		}

		public void Filter (ref float value)
		{
			if (value < doubleMin)
			{
				value = (float)doubleMin;
			}
		}

		public void Filter (ref double value)
		{
			if (value < doubleMin)
			{
				value = doubleMin;
			}
		}
	}
}
