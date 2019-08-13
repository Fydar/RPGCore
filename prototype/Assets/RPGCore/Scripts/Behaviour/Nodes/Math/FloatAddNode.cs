﻿using RPGCore.Behaviour.Connections;
using System;
using UnityEngine;

namespace RPGCore.Behaviour.Math
{
	[NodeInformation ("Float/Add")]
	public class FloatAddNode : BehaviourNode
	{
		public FloatInput ValueA;
		public FloatInput ValueB;

		public FloatOutput Output;

		protected override void OnSetup (IBehaviourContext context)
		{
			ConnectionEntry<float> valueAInput = ValueA[context];
			ConnectionEntry<float> valueBInput = ValueB[context];
			ConnectionEntry<float> output = Output[context];

			Action updateHandler = () =>
			{
				output.Value = valueAInput.Value + valueBInput.Value;
			};

			valueAInput.OnAfterChanged += updateHandler;
			valueBInput.OnAfterChanged += updateHandler;

			updateHandler ();
		}

		protected override void OnRemove (IBehaviourContext context)
		{
		}

#if UNITY_EDITOR
		public override Vector2 GetDiamentions ()
		{
			return new Vector2 (140, 54);
		}
#endif
	}
}

