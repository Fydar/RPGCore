using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using RPGCore.Utility;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation ("Buff/Buff For")]
	public class BuffForNode : BehaviourNode
	{
		public enum ApplyMode
		{
			AddNewBuff,
			SeperateClock,
			Refresh,
			RefreshAndAdd,
			Add
		}

		[ErrorIfNull]
		public BuffTemplate BuffToApply;
		public ApplyMode Mode;

		[Space]

		public CharacterInput Target;
		public EventInput Apply;
		public FloatInput Duration;

		[Space]

		public IntInput StackSize;
		public IntInput Ticks;

		protected override void OnSetup (IBehaviourContext context)
		{
			EventEntry applyInput = Apply[context];
			ConnectionEntry<RPGCharacter> targetInput = Target[context];
			ConnectionEntry<float> durationInput = Duration[context];
			ConnectionEntry<int> stackSizeInput = StackSize[context];

			IntegerStack.Modifier modifier = null;

			BuffClockDecaying refreshClock = null;

			applyInput.OnEventFired += () =>
			{
				if (targetInput.Value == null)
					return;

				if (Mode == ApplyMode.AddNewBuff)
				{
					BuffClock buffClock = new BuffClockDecaying (this, context);
					buffClock.StackSize.AddFlatModifier (stackSizeInput.Value);
					modifier = buffClock.StackSize.AddFlatModifier (stackSizeInput.Value);

					Buff buff = new Buff (BuffToApply, targetInput.Value, buffClock);

					targetInput.Value.Buffs.Add (buff);
				}
				else if (Mode == ApplyMode.SeperateClock)
				{
					BuffClock buffClock = new BuffClockDecaying (this, context);
					buffClock.StackSize.AddFlatModifier (stackSizeInput.Value);
					modifier = buffClock.StackSize.AddFlatModifier (stackSizeInput.Value);

					Buff buff = targetInput.Value.Buffs.Find (BuffToApply);

					if (buff == null)
					{
						buff = new Buff (BuffToApply, targetInput.Value, buffClock);
						targetInput.Value.Buffs.Add (buff);
					}
					else
					{
						buff.AddClock (buffClock);
					}
				}
				else if (Mode == ApplyMode.Refresh || Mode == ApplyMode.RefreshAndAdd)
				{
					Buff buff = targetInput.Value.Buffs.Find (BuffToApply);

					if (buff == null)
					{
						buff = new Buff (this, context);
						targetInput.Value.Buffs.Add (buff);
					}

					if (refreshClock == null)
					{
						refreshClock = new BuffClockDecaying (this, context);
						modifier = refreshClock.StackSize.AddFlatModifier (stackSizeInput.Value);

						buff.AddClock (refreshClock);

						refreshClock.OnRemove += () =>
						{
							refreshClock = null;
						};
					}

					refreshClock.TimeRemaining = refreshClock.Duration;

					if (Mode == ApplyMode.RefreshAndAdd)
						refreshClock.StackSize.AddFlatModifier (stackSizeInput.Value);
				}
				else if (Mode == ApplyMode.Add)
				{
					Buff buff = targetInput.Value.Buffs.Find (BuffToApply);

					if (buff == null)
					{
						buff = new Buff (this, context);
						targetInput.Value.Buffs.Add (buff);
					}

					if (refreshClock == null)
					{
						refreshClock = new BuffClockDecaying (this, context);
						modifier = refreshClock.StackSize.AddFlatModifier (stackSizeInput.Value);

						buff.AddClock (refreshClock);

						refreshClock.OnRemove += () =>
						{
							refreshClock = null;
						};
					}

					refreshClock.StackSize.AddFlatModifier (stackSizeInput.Value);
				}
			};

			stackSizeInput.OnAfterChanged += () =>
			{
				if (Mode != ApplyMode.Refresh)
					return;

				if (modifier != null)
					modifier.Value = stackSizeInput.Value;
			};
		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}
	}
}

