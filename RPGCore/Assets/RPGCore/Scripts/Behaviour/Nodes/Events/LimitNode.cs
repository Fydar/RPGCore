using RPGCore.Behaviour.Connections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore.Behaviour.Events
{
	[NodeInformation ("Events/Limit", "EventLogic")]
	public class LimitNode : BehaviourNode
	{
		public EventInput Event;
		public IntInput Fires;
		public FloatInput PerSeconds;
		public FloatInput Spacing;

		public EventOutput Limited;

		protected override void OnSetup (IBehaviourContext context)
		{
			ConnectionEntry<int> firesInput = Fires[context];
			ConnectionEntry<float> perSecondsInput = PerSeconds[context];
			ConnectionEntry<float> spacingInput = Spacing[context];
			EventEntry eventInput = Event[context];
			EventEntry limitedtOutput = Limited[context];

			Queue<float> FiringTimes = new Queue<float> ();
			float lastFiringTime = float.MinValue;

			eventInput.OnEventFired += () =>
			{
				if (Time.time < lastFiringTime + spacingInput.Value)
					return;

				if (FiringTimes.Count < firesInput.Value)
				{
					FiringTimes.Enqueue (Time.time);
					lastFiringTime = Time.time;
					limitedtOutput.Invoke ();
				}
				else
				{
					float lastTime = FiringTimes.Peek ();

					if (Time.time > lastTime + perSecondsInput.Value)
					{
						FiringTimes.Dequeue ();
						FiringTimes.Enqueue (Time.time);
						lastFiringTime = Time.time;
						limitedtOutput.Invoke ();
					}
				}
			};
		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}

#if UNITY_EDITOR
		public override Vector2 GetDiamentions ()
		{
			return new Vector2 (160, base.GetDiamentions ().y);
		}
#endif
	}
}