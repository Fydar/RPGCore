using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore
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
			EventEntry eventInput = Event[context];
			ConnectionEntry<bool> conditionInput = Condition[context];
			EventEntry trueOutput = True[context];
			EventEntry falseOutput = False[context];

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

		protected override void OnRemove (IBehaviourContext character)
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