using RPGCore.Behaviour;
using System;

namespace RPGCore.Traits
{
	public class StatModifier
	{
		public StatModificationPhase Phase { get; }
		public StatModificationType Type { get; }
		public IReadOnlyEventField<float> CurrentValue { get; }

		public StatModifier (StatModificationPhase phase, StatModificationType type, IReadOnlyEventField<float> currentValue)
		{
			Phase = phase;
			Type = type;
			CurrentValue = currentValue ?? throw new ArgumentNullException (nameof (currentValue));
		}
	}
}
