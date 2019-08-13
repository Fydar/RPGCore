using RPGCore.Behaviour;
using System;
using UnityEngine;

namespace RPGCore
{
	[Serializable]
	public class BuffClockDecaying : BuffClock
	{
		public float Duration = 3.0f;
		public int Ticks = 20;
		public float TimeRemaining;

		private int TicksCompleted;

		public override float DisplayPercent
		{
			get
			{
				return TimeRemaining / Duration;
			}
		}

		public BuffClockDecaying (BuffForNode buffNode, IBehaviourContext buffNodeContext)
		{
			Ticks = buffNode.Ticks[buffNodeContext].Value;

			Duration = buffNode.Duration[buffNodeContext].Value;
			TimeRemaining = Duration;
		}

		public BuffClockDecaying (int ticks, float duration)
		{
			Ticks = ticks;

			Duration = duration;
			TimeRemaining = Duration;
		}

		public override void Update (float deltaTime)
		{
			TimeRemaining -= deltaTime;

			float percentComplete = 1.0f - (TimeRemaining / Duration);

			float tickTime;
			tickTime = percentComplete;

			int TargetTicks = Mathf.RoundToInt (tickTime * Ticks);

			while (TicksCompleted < TargetTicks)
			{
				if (OnTick != null)
					OnTick ();

				TicksCompleted++;
			}

			if (TimeRemaining <= 0.0f)
			{
				if (OnRemove != null)
					OnRemove ();

				return;
			}
		}
	}
}

