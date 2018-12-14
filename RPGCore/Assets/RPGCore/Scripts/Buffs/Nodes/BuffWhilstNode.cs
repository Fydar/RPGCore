using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using RPGCore.Utility;
using System;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation ("Buff/Buff Whilst")]
	public class BuffWhilstNode : BehaviourNode
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
		public BoolInput Whilst;

		[Space]

		public IntInput StackSize;
		public FloatInput TicksPerSecond;

		protected override void OnSetup (IBehaviourContext context)
		{
			ConnectionEntry<RPGCharacter> targetInput = Target[context];
			ConnectionEntry<bool> whilstInput = Whilst[context];
			ConnectionEntry<int> stackSizeInput = StackSize[context];

			bool isActive = false;

			Buff buff = null;
			IntegerStack.Modifier modifier = null;

			Action changeHandler = () =>
			{
				if (targetInput.Value == null)
					return;

				if (whilstInput.Value)
				{
					if (!isActive)
					{
						if (Mode == ApplyMode.Add)
						{
							buff = new Buff (this, context);
							BuffClock buffClock = new BuffClockFixed (this, context);

							modifier = buffClock.StackSize.AddFlatModifier (stackSizeInput.Value);

							buff.AddClock (buffClock);

							targetInput.Value.Buffs.Add (buff);
						}
						else
						{
							buff = targetInput.Value.Buffs.Find (BuffToApply);

							if (buff == null)
							{
								buff = new Buff (this, context);
								targetInput.Value.Buffs.Add (buff);
							}

							BuffClock buffClock = new BuffClockFixed (this, context);

							modifier = buffClock.StackSize.AddFlatModifier (stackSizeInput.Value);

							buff.AddClock (buffClock);
						}
						isActive = true;
					}
				}
				else if (isActive)
				{
					targetInput.Value.Buffs.Remove (buff);
					isActive = false;
				};
			};

			targetInput.OnBeforeChanged += () =>
			{

				if (targetInput.Value == null)
				{
					isActive = false;
					return;
				}

				if (!isActive)
					return;

				Debug.Log ("Ending");
				targetInput.Value.Buffs.Remove (buff);
				isActive = false;
			};

			stackSizeInput.OnAfterChanged += () =>
			{
				if (modifier != null)
					modifier.Value = stackSizeInput.Value;
			};

			targetInput.OnAfterChanged += changeHandler;
			whilstInput.OnAfterChanged += changeHandler;

			changeHandler ();
		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}
	}
}

