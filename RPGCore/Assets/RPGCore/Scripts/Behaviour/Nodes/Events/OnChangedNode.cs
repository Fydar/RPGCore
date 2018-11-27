using RPGCore.Behaviour.Connections;
using System;
using UnityEngine;

namespace RPGCore.Behaviour.Events
{
	[NodeInformation ("Events/On Changed")]
	public class OnChangedNode : BehaviourNode
	{
		public InputSocket Field;
		public EventOutput onChanged;

		protected override void OnSetup (IBehaviourContext context)
		{
			object fieldInputObject = Field.GetConnectionObject (context);

			// Debug.Log (fieldInputObject);

			ConnectionEntry fieldInput = (ConnectionEntry)fieldInputObject;
			EventEntry onChangedOutput = onChanged[context];

			Action eventHandler = () =>
			{
				onChangedOutput.Invoke ();
			};

			fieldInput.OnAfterChanged += eventHandler;
		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}

#if UNITY_EDITOR
		public override Vector2 GetDiamentions ()
		{
			return new Vector2 (120, 38);
		}
#endif
	}
}