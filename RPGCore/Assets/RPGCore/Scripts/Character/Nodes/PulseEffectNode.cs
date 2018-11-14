using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using RPGCore.Utility;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation ("Character/Pulse Effect", "VFX")]
	public class PulseEffectNode : BehaviourNode
	{
		[ErrorIfNull]
		public PulseEffect Effect;
		public CharacterInput Target;
		public EventInput Activate;

		protected override void OnSetup (IBehaviourContext context)
		{
			ConnectionEntry<RPGCharacter> targetInput = Target.GetEntry (context);
			EventEntry activateInput = Activate.GetEntry (context);

			activateInput.OnEventFired += () =>
			{
				if (targetInput.Value == null)
					return;

				PulseEffect visualEffect = Instantiate (Effect) as PulseEffect;
				visualEffect.gameObject.SetActive (false);

				visualEffect.transform.SetParent (targetInput.Value.transform);
				visualEffect.gameObject.SetActive (true);
				visualEffect.transform.localPosition = Vector3.zero;
			};
		}

		protected override void OnRemove (IBehaviourContext character)
		{

		}
	}
}