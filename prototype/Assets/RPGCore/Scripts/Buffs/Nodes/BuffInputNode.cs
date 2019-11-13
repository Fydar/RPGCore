using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation("Buff/Input", "Input", OnlyOne = true)]
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

		protected override void OnSetup(IBehaviourContext context)
		{
			var applyOutput = Apply[context];
			var removeOutput = Remove[context];
			var targetOutput = Target[context];

			ConnectionEntry<int> ticksOutput = Ticks[context];
			//ConnectionEntry<float> totalDurationOutput = TotalDuration[context];
			//ConnectionEntry<float> remainingDurationOutput = RemainingDuration[context]

			targetOutput.OnBeforeChanged += () =>
			{
				if (targetOutput.Value != null)
				{
					removeOutput.Invoke();
				}
			};

			targetOutput.OnAfterChanged += () =>
			{
				if (targetOutput.Value != null)
				{
					applyOutput.Invoke();
				}
			};
		}

		protected override void OnRemove(IBehaviourContext context)
		{
			var removeOutput = Remove[context];
			var targetOutput = Target[context];

			targetOutput.Value = null;

			var applyOutput = Apply[context];
			applyOutput.Invoke();
		}

		public void SetTarget(IBehaviourContext context, Buff target)
		{
			ConnectionEntry<int> stackSizeOutput = StackSize[context];
			var onTickOutput = OnTick[context];

			stackSizeOutput.Value = target.StackSize.Value;
			target.StackSize.onChanged += () =>
			{
				stackSizeOutput.Value = target.StackSize.Value;
			};

			target.OnTick += onTickOutput.Invoke;
		}
	}
}

