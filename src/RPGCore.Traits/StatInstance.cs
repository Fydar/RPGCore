using RPGCore.Behaviour;
using System.Collections.Generic;

namespace RPGCore.Traits
{
	public class StatInstance : IReadOnlyEventField<float>, IEventFieldHandler
	{
		public string Name { get => null; set { } }
		public string Identifier { get; set; }

		public StatTemplate Template;

		public HandlerCollection Handlers { get; set; }

		public List<StatModifier> FlatBaseModifiers;

		public float Value
		{
			get => CalculateValue ();
		}

		public StatInstance ()
		{
			Handlers = new HandlerCollection (this);
		}

		public override string ToString ()
		{
			return Identifier;
		}

		private float CalculateValue ()
		{
			float currentValue = 0.0f;
			foreach (var modifier in FlatBaseModifiers)
			{
				currentValue += modifier.CurrentValue.Value;
			}
			return currentValue;
		}

		public void AddModifier (StatModifier modifier)
		{
			Handlers.InvokeBeforeChanged ();
			FlatBaseModifiers.Add (modifier);
			Handlers.InvokeAfterChanged ();

			modifier.CurrentValue.Handlers[this].Add (this);
		}

		public void OnBeforeChanged ()
		{
		}

		public void OnAfterChanged ()
		{
		}

		public void Dispose ()
		{
		}
	}
}
