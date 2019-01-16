using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using System;

namespace RPGCore
{
	[NodeInformation ("Events/On Take Damage")]
	public class OnTakeDamageNode : BehaviourNode
	{
		public CharacterInput Target;
		public CharacterOutput Attacker;
		public EventOutput OnHit;
		public IntOutput DamageTaken;

		protected override void OnSetup (IBehaviourContext context)
		{
			ConnectionEntry<RPGCharacter> targetInput = Target[context];
			ConnectionEntry<int> damageTakenOutput = DamageTaken[context];
			EventEntry onHitOutput = OnHit[context];

			bool isActive = false;

			Action eventHandler = () =>
			{
				if (targetInput.Value.States.CurrentHealth.Delta >= 1.0f)
				{
					damageTakenOutput.Value = (int)targetInput.Value.States.CurrentHealth.Delta;
					onHitOutput.Invoke ();
				}
			};

			Action subscriber = () =>
			{
				if (targetInput.Value == null)
				{
					isActive = false;
					return;
				}

				if (!isActive)
				{
					targetInput.Value.States.CurrentHealth.OnValueChanged += eventHandler;
				}

				isActive = true;
			};

			subscriber ();

			targetInput.OnBeforeChanged += () =>
			{
				if (targetInput.Value == null)
					return;

				if (isActive)
					targetInput.Value.States.CurrentHealth.OnValueChanged -= eventHandler;
			};

			targetInput.OnAfterChanged += subscriber;
		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}
	}
}

