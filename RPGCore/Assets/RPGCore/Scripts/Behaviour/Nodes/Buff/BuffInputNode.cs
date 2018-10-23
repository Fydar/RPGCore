using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation ("Buff/Input", "Input")]
	public class BuffInputNode : BehaviourNode
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
			ConnectionEntry<RPGCharacter> targetOutput = Target.GetEntry (context);

			targetOutput.Value = null;

			EventEntry applyOutput = Apply[context];
			applyOutput.Invoke ();
		}

		public void SetTarget (IBehaviourContext context, RPGCharacter target)
		{
			ConnectionEntry<RPGCharacter> targetOutput = Target.GetEntry (context);

			targetOutput.Value = target;
		}

		public void SetRPGCoreBuff (IBehaviourContext context, Buff buff)
		{
			ConnectionEntry<int> stackSizeOutput = StackSize[context];
			EventEntry onTickOutput = OnTick[context];

			stackSizeOutput.Value = buff.StackSize.Value;
			buff.StackSize.onChanged += () =>
			{
				stackSizeOutput.Value = buff.StackSize.Value;
			};

			buff.OnTick += onTickOutput.Invoke;
		}
	}
}