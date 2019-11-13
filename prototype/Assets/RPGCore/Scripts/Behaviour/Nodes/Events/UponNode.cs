using RPGCore.Behaviour.Connections;
using System;
using UnityEngine;

namespace RPGCore.Behaviour.Events
{
	[NodeInformation("Events/Upon", "EventLogic")]
	public class UponNode : BehaviourNode
	{
		public BoolInput Condition;

		public EventOutput True;
		public EventOutput False;

		protected override void OnSetup(IBehaviourContext context)
		{
			var conditionInput = Condition[context];
			var trueOutput = True[context];
			var falseOutput = False[context];

			Action eventHandler = () =>
			{
				if (conditionInput.Value)
				{
					trueOutput.Invoke();
				}
				else
				{
					falseOutput.Invoke();
				}
			};

			conditionInput.OnAfterChanged += eventHandler;
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

