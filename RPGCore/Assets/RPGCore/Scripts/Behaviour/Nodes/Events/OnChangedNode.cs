using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGCore;

namespace RPGCore
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
			EventEntry onChangedOutput = onChanged.GetEntry (context);

			Action eventHandler = () =>
			{
				onChangedOutput.Invoke ();
			};

			fieldInput.OnAfterChanged += eventHandler;
		}

		protected override void OnRemove (IBehaviourContext character)
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