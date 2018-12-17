using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using System;

namespace RPGCore
{
	[NodeInformation ("Character/Read State", "Attribute")]
	public class CharacterStateNode : BehaviourNode
	{
		public StateEntry entry;

		public FloatOutput Value;
		public CharacterInput Target;

		protected override void OnSetup (IBehaviourContext context)
		{
			ConnectionEntry<RPGCharacter> targetInput = Target[context];
			ConnectionEntry<float> valueInput = Value[context];

			Action updateListener = () =>
			{
				valueInput.Value = targetInput.Value.States[entry].Value;
			};

			if (targetInput.Value != null)
			{
				targetInput.Value.States[entry].OnValueChanged += updateListener;

				updateListener ();
			}

			targetInput.OnBeforeChanged += () =>
			{
				if (targetInput.Value == null)
					return;

				targetInput.Value.States[entry].OnValueChanged -= updateListener;
			};

			targetInput.OnAfterChanged += () =>
			{
				if (targetInput.Value == null)
					return;

				targetInput.Value.States[entry].OnValueChanged += updateListener;
				updateListener ();
			};
		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}
	}
}

