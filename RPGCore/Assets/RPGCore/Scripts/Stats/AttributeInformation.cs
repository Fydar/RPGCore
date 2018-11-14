using UnityEngine;

namespace RPGCore.Stats
{
	public class AttributeInformation : ScriptableObject
	{
		public enum ModifierType
		{
			Additive,
			Multiplicitive
		}

		public enum Rounding
		{
			Round,
			Floor,
			Ceil
		}

		public enum Accuracy
		{
			Integer,
			OneDecimal,
			TwoDecimals,
			ThreeDecimals,
			FourDecimals,
			Float,
		}

		[Header ("Information")]
		public string Name = "New State";
		[TextArea (3, 5)]
		public string Description = "New Description";

		[Header ("Display")]
		public string renderFormat = "{0.##}";

		[Space]

		public string positiveFormat = "{0.##} increased {name}";
		public string negativeFormat = "{0.##} reduced {name}";

		[Space]

		public string multiplierPositiveFormat = "{0.##}% increased {name}";
		public string multiplierNegativeFormat = "{0.##}% reduced {name}";

		[Header ("Usage")]
		public Accuracy accuracy = Accuracy.Float;
		public Rounding rounding = Rounding.Round;

		public virtual float Filter (float original)
		{
			int multiple = 0;

			switch (accuracy)
			{
				case Accuracy.Integer:
					multiple = 1;
					break;
				case Accuracy.OneDecimal:
					multiple = 10;
					break;
				case Accuracy.TwoDecimals:
					multiple = 100;
					break;
				case Accuracy.ThreeDecimals:
					multiple = 1000;
					break;
				case Accuracy.FourDecimals:
					multiple = 10000;
					break;
				case Accuracy.Float:
					multiple = 0;
					break;
			}

			if (multiple != 0)
			{
				switch (rounding)
				{
					case Rounding.Round:
						return Mathf.Round (original * multiple) / multiple;

					case Rounding.Floor:
						return Mathf.Floor (original * multiple) / multiple;

					case Rounding.Ceil:
						return Mathf.Ceil (original * multiple) / multiple;
				}
			}

			return original;
		}

		public string RenderValue (float value)
		{
			value = Filter (value);

			string render = renderFormat.Replace ("{name}", Name);

			int start = render.IndexOf ('{');
			int end = render.IndexOf ('}');

			string toStringController = render.Substring (start + 1, end - start - 1);

			string valueFormatted = value.ToString (toStringController);

			string prefix = render.Substring (0, start);
			string suffix = render.Substring (end + 1, render.Length - end - 1);

			render = prefix + valueFormatted + suffix;

			return render;
		}

		public string RenderModifier (float value, ModifierType modifierType)
		{
			string format;

			if (modifierType == ModifierType.Additive)
			{
				value = Filter (value);
				format = (value > 0) ? positiveFormat : negativeFormat;
			}
			else
			{
				format = (value > 0) ? multiplierPositiveFormat : multiplierNegativeFormat;
				value = value * 100;
			}
			value = Mathf.Abs (value);

			string render = format.Replace ("{name}", Name);

			int start = render.IndexOf ('{');
			int end = render.IndexOf ('}');

			string toStringController = render.Substring (start + 1, end - start - 1);

			string valueFormatted = value.ToString (toStringController);

			string prefix = render.Substring (0, start);
			string suffix = render.Substring (end + 1, render.Length - end - 1);

			render = prefix + valueFormatted + suffix;

			return render;
		}
	}
}