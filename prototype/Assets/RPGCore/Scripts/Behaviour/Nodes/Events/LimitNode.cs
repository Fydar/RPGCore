using RPGCore.Behaviour.Connections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore.Behaviour.Events
{
	[NodeInformation("Events/Limit", "EventLogic")]
	public class LimitNode : BehaviourNode
	{
		public EventInput Event;
		public IntInput Fires;
		public FloatInput PerSeconds;
		public FloatInput Spacing;

		public EventOutput Limited;

		protected override void OnSetup(IBehaviourContext context)
		{
			ConnectionEntry<int> firesInput = Fires[context];
			var perSecondsInput = PerSeconds[context];
			var spacingInput = Spacing[context];
			var eventInput = Event[context];
			var limitedtOutput = Limited[context];

			var FiringTimes = new Queue<float>();
			float lastFiringTime = float.MinValue;

			eventInput.OnEventFired += () =>
			{
				if (Time.time < lastFiringTime + spacingInput.Value)
				{
					return;
				}

				if (FiringTimes.Count < firesInput.Value)
				{
					FiringTimes.Enqueue(Time.time);
					lastFiringTime = Time.time;
					limitedtOutput.Invoke();
				}
				else
				{
					float lastTime = FiringTimes.Peek();

					if (Time.time > lastTime + perSecondsInput.Value)
					{
						FiringTimes.Dequeue();
						FiringTimes.Enqueue(Time.time);
						lastFiringTime = Time.time;
						limitedtOutput.Invoke();
					}
				}
			};
		}

		protected override void OnRemove(IBehaviourContext context)
		{
		}

#if UNITY_EDITOR
		public override Vector2 GetDiamentions()
		{
			return new Vector2(160, base.GetDiamentions().y);
		}
#endif
	}
}

