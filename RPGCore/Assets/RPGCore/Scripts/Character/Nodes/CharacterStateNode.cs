using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using System;

namespace RPGCore
{
	[NodeInformation ("Character/Read State", "Attribute")]
	public class CharacterStateNode : BehaviourNode
	{
		[CollectionType (typeof (StateCollection<>))]
		public CollectionEntry entry;

		public FloatOutput Value;
		public CharacterInput Target;

		protected override void OnSetup (IBehaviourContext context)
		{
			ConnectionEntry<RPGCharacter> targetInput = Target.GetEntry (context);
			ConnectionEntry<float> valueInput = Value.GetEntry (context);

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

		protected override void OnRemove (IBehaviourContext character)
		{

		}
	}
}