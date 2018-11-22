using RPGCore.Behaviour;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation ("Item/Effect Tooltip", "Tooltip")]
	public class EffectTooltipNode : BehaviourNode
	{
		[TextArea (3, 5)]
		public string Template = "{0} Effect";
		public float[] Values = new float[1];

		public Dictionary<IBehaviourContext, string> DescriptionMap = new Dictionary<IBehaviourContext, string> ();

		public string Description (IBehaviourContext context)
		{
			return DescriptionMap[context];
		}

		protected override void OnSetup (IBehaviourContext context)
		{
			Action updateHandler = () =>
			{
				object[] valuesObjects = new object[Values.Length];

				for (int i = 0; i < Values.Length; i++)
				{
					valuesObjects[i] = Values[i];
				}

				// Debug.Log ("Description: " + String.Format (Template, valuesObjects));
				DescriptionMap[context] = string.Format (Template, valuesObjects);
			};

			updateHandler ();
		}

		protected override void OnRemove (IBehaviourContext character)
		{

		}

#if UNITY_EDITOR
		public override Vector2 GetDiamentions ()
		{
			return new Vector2 (260, base.GetDiamentions ().y);
		}
#endif
	}
}