using RPGCore.Behaviour;
using System;
using UnityEngine;

namespace RPGCore
{
	[Serializable]
	public class BuffClockFixed : BuffClock
	{
		public float TicksPerSecond = 3.0f;

		private float tickProgress;

		public override float DisplayPercent
		{
			get
			{
				return 0.0f;
			}
		}

		public BuffClockFixed (float ticksPerSecond)
		{
			TicksPerSecond = ticksPerSecond;
		}

		public BuffClockFixed (BuffWhilstNode buffNode, IBehaviourContext context)
		{
			TicksPerSecond = buffNode.TicksPerSecond[context].Value;
		}

		public BuffClockFixed (BuffGrantNode buffNode, IBehaviourContext context)
		{
			TicksPerSecond = buffNode.TicksPerSecond[context].Value;
		}

		public override void Update (float deltaTime)
		{
			tickProgress += deltaTime;

			int TargetTicks = Mathf.FloorToInt (tickProgress / (1.0f / TicksPerSecond));

			for (int i = 0; i < TargetTicks; ++i)
			{
				if (OnTick != null)
					OnTick ();
			}

			tickProgress -= TargetTicks * (1.0f / TicksPerSecond);
		}
	}
}