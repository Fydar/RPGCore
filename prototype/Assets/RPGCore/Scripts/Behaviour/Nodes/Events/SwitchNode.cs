using RPGCore.Behaviour.Connections;
using System;
using UnityEngine;

namespace RPGCore.Behaviour.Events
{
	[NodeInformation ("Events/Switch", "EventLogic")]
	public class SwitchNode : BehaviourNode
	{
		public EventInput Event;
		public BoolInput Condition;

		public EventOutput True;
		public EventOutput False;

		protected override void OnSetup (IBehaviourContext context)
		{
			var eventInput = Event[context];
			var conditionInput = Condition[context];
			var trueOutput = True[context];
			var falseOutput = False[context];

			Action eventHandler = () =>
			{
				if (conditionInput.Value)
				{
					trueOutput.Invoke ();
				}
				else
				{
					falseOutput.Invoke ();
				}
			};

			eventInput.OnEventFired += eventHandler;
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

