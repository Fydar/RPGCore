using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation ("Events/Upon", "EventLogic")]
	public class UponNode : BehaviourNode
	{
		public BoolInput Condition;

		public EventOutput True;
		public EventOutput False;

		protected override void OnSetup (IBehaviourContext context)
		{
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

			conditionInput.OnAfterChanged += eventHandler;
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