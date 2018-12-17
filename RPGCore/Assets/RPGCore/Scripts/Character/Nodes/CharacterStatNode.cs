using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using System;

namespace RPGCore
{
	[NodeInformation ("Character/Read Stat", "Attribute")]
	public class CharacterStatNode : BehaviourNode
	{
		public StatEntry entry;

		public FloatOutput Value;
		public CharacterInput Target;

		protected override void OnSetup (IBehaviourContext context)
		{
			ConnectionEntry<RPGCharacter> targetInput = Target[context];
			ConnectionEntry<float> valueInput = Value[context];

			Action updateListener = () =>
			{
				valueInput.Value = targetInput.Value.Stats[entry].Value;
			};

			if (targetInput.Value != null)
			{
				targetInput.Value.Stats[entry].OnValueChanged += updateListener;

				updateListener ();
			}

			targetInput.OnBeforeChanged += () =>
			{
				if (targetInput.Value == null)
					return;

				targetInput.Value.Stats[entry].OnValueChanged -= updateListener;
			};

			targetInput.OnAfterChanged += () =>
			{
				if (targetInput.Value == null)
					return;

				targetInput.Value.Stats[entry].OnValueChanged += updateListener;
				updateListener ();
			};
		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}
	}
}

