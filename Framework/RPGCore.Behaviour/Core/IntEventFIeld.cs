using System;

namespace RPGCore.Behaviour
{
	public interface IIntCalculation
	{
		int Calculate ();
	}

	public class IntEventField
	{
		public event Action OnChanged;
		public event Action<int> OnChangedValue;

		public int internalValue;

		public int Value
		{
			get => internalValue;
			set
			{
				internalValue = value;

				OnChanged?.Invoke ();

				OnChangedValue?.Invoke (internalValue);
			}
		}

		public IntEventField (int defaultValue)
		{
			Value = defaultValue;
		}

		public IntEventField (IIntCalculation calculation)
		{
			var eventField = new IntEventField (calculation.Calculate ());

			//calculation.left.OnChanged += () => { eventField.Value = calculation.Calculate(); };
			//calculation.right.OnChanged += () => { eventField.Value = calculation.Calculate(); };
		}

		public IntEventField (IntAddCalculation calculation)
		{
			internalValue = calculation.Calculate ();

			calculation.left.OnChanged += () => { internalValue = calculation.Calculate (); };
			calculation.right.OnChanged += () => { internalValue = calculation.Calculate (); };
		}

		public static IntEventField operator + (IntEventField left, Action right)
		{
			left.OnChanged += right;
			return left;
		}

		public static IntEventField operator + (IntEventField left, Action<int> right)
		{
			left.OnChangedValue += right;
			return left;
		}

		public static IntEventField operator - (IntEventField left, Action right)
		{
			left.OnChanged -= right;
			return left;
		}

		public static IntEventField operator - (IntEventField left, Action<int> right)
		{
			left.OnChangedValue -= right;
			return left;
		}


		public static IntAddCalculation operator + (IntEventField left, IntEventField right)
		{
			return new IntAddCalculation (left, right);
		}

		public static IntAddCalculation operator + (IntAddCalculation left, IntEventField right)
		{
			return new IntAddCalculation (right, right);
		}

		public static IntAddCalculation operator + (IntEventField left, int right)
		{
			return new IntAddCalculation (left, new IntEventField (right));
		}

		public static IntMinusCalculation operator - (IntEventField left, IntEventField right)
		{
			return new IntMinusCalculation (left, right);
		}
	}

	public struct IntAddCalculation : IIntCalculation
	{
		public IntEventField left;
		public IntEventField right;

		public IntAddCalculation (IntEventField left, IntEventField right)
		{
			this.left = left;
			this.right = right;
		}

		public int Calculate ()
		{
			return left.Value + right.Value;
		}

		/*public static implicit operator int (IntAddCalculation calc)
		{
			return calc.left.Value + calc.right.Value;
		}*/
	}

	public struct IntMinusCalculation : IIntCalculation
	{
		public IntEventField left;
		public IntEventField right;

		public IntMinusCalculation (IntEventField left, IntEventField right)
		{
			this.left = left;
			this.right = right;
		}

		public int Calculate ()
		{
			return left.Value - right.Value;
		}

		/*public static implicit operator int (IntMinusCalculation calc)
		{
			return calc.Calculate ();
		}*/
	}
}