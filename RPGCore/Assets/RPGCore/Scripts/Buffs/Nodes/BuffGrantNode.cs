using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using RPGCore.Utility;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation ("Buff/Buff Grant")]
	public class BuffGrantNode : BehaviourNode
	{
		public enum ApplyMode
		{
			Add,
			Stack
		}

		[ErrorIfNull]
		public BuffTemplate BuffToApply;
		public ApplyMode Mode;

		[Space]

		public CharacterInput Target;
		public EventInput Apply;

		[Space]

		public IntInput StackSize;
		public FloatInput TicksPerSecond;

		protected override void OnSetup (IBehaviourContext context)
		{
			EventEntry applyInput = Apply[context];
			ConnectionEntry<RPGCharacter> targetInput = Target[context];
			ConnectionEntry<int> stackSizeInput = StackSize[context];

			applyInput.OnEventFired += () =>
			{
				if (Mode == ApplyMode.Add)
				{
					Buff buff = new Buff (this, context);
					BuffClock buffClock = new BuffClockFixed (this, context);

					buffClock.StackSize.AddFlatModifier (stackSizeInput.Value);

					buff.AddClock (buffClock);

					targetInput.Value.Buffs.Add (buff);
				}
				else if (Mode == ApplyMode.Stack)
				{
					Buff buff = targetInput.Value.Buffs.Find (BuffToApply);

					Debug.Log (BuffToApply.name);

					if (buff == null)
					{
						buff = new Buff (this, context);

						BuffClock buffClock = new BuffClockFixed (this, context);
						buffClock.StackSize.AddFlatModifier (0);

						buff.AddClock (buffClock);

						targetInput.Value.Buffs.Add (buff);
					}

					buff.BaseStackSize.Value += stackSizeInput.Value;
				}
			};

			stackSizeInput.OnAfterChanged += () =>
			{
				return;

				// if (modifier != null)
				//	modifier.Value = stackSizeInput.Value;
			};
		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}
	}
}