using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation ("Buff/Input", "Input", OnlyOne = true)]
	public class BuffInputNode : BehaviourNode, IInputNode<Buff>
	{
		public CharacterOutput Target;
		public EventOutput Apply;
		public EventOutput Remove;
		public EventOutput OnTick;

		[Space]
		public IntOutput Ticks;
		public IntOutput StackSize;

		//[Space]
		//public FloatOutput RemainingDuration;
		//public FloatOutput TotalDuration;

		protected override void OnSetup (IBehaviourContext context)
		{
			EventEntry applyOutput = Apply[context];
			EventEntry removeOutput = Remove[context];
			ConnectionEntry<RPGCharacter> targetOutput = Target[context];

			ConnectionEntry<int> ticksOutput = Ticks[context];
			//ConnectionEntry<float> totalDurationOutput = TotalDuration[context];
			//ConnectionEntry<float> remainingDurationOutput = RemainingDuration[context]

			targetOutput.OnBeforeChanged += () =>
			{
				if (targetOutput.Value != null)
					removeOutput.Invoke ();
			};

			targetOutput.OnAfterChanged += () =>
			{
				if (targetOutput.Value != null)
					applyOutput.Invoke ();
			};
		}

		protected override void OnRemove (IBehaviourContext context)
		{
			EventEntry removeOutput = Remove[context];
			ConnectionEntry<RPGCharacter> targetOutput = Target[context];

			targetOutput.Value = null;

			EventEntry applyOutput = Apply[context];
			applyOutput.Invoke ();
		}

		public void SetTarget (IBehaviourContext context, Buff target)
		{
			ConnectionEntry<int> stackSizeOutput = StackSize[context];
			EventEntry onTickOutput = OnTick[context];

			stackSizeOutput.Value = target.StackSize.Value;
			target.StackSize.onChanged += () =>
			{
				stackSizeOutput.Value = target.StackSize.Value;
			};

			target.OnTick += onTickOutput.Invoke;
		}
	}
}

