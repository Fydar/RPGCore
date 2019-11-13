﻿using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using RPGCore.Utility;
using System;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation("Character/Continuous Effect", "VFX")]
	public class ContinuousEffectNode : BehaviourNode
	{
		[ErrorIfNull]
		public ContinuousEffect Effect;
		public CharacterInput Target;
		public BoolInput Whilst;

		protected override void OnSetup(IBehaviourContext context)
		{
			var targetInput = Target[context];
			var whilstInput = Whilst[context];

			var visualEffect = Instantiate(Effect) as ContinuousEffect;
			visualEffect.gameObject.SetActive(false);

			Action changeHandler = () =>
			{
				if (targetInput.Value == null)
				{
					visualEffect.transform.SetParent(null);
					visualEffect.gameObject.SetActive(false);
					return;
				}

				if (whilstInput.Value)
				{
					visualEffect.transform.SetParent(targetInput.Value.transform);
					visualEffect.gameObject.SetActive(true);
					visualEffect.transform.localPosition = Vector3.zero;
				}
				else
				{
					visualEffect.transform.SetParent(null);
					visualEffect.gameObject.SetActive(false);
				}
			};

			changeHandler();

			targetInput.OnAfterChanged += changeHandler;
			whilstInput.OnAfterChanged += changeHandler;
		}

		protected override void OnRemove(IBehaviourContext context)
		{
		}
	}
}

