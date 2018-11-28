using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using System;

namespace RPGCore
{
	[NodeInformation ("Item/Read Stat", "Attribute")]
	public class ItemStatsNode : BehaviourNode
	{
		[CollectionType (typeof (StatCollection<>))]
		public CollectionEntry entry;

		public FloatOutput Value;
		public ItemInput Target;

		protected override void OnSetup (IBehaviourContext context)
		{
			/*
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
			*/
		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}
	}
}