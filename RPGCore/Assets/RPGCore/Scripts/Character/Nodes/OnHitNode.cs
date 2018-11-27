using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using System;

namespace RPGCore
{
	[NodeInformation ("Events/On Hit")]
	public class OnHitNode : BehaviourNode
	{
		public CharacterInput Target;
		public CharacterOutput HitTarget;
		public EventOutput OnHit;
		public IntOutput DamageDelt;

		protected override void OnSetup (IBehaviourContext context)
		{
			ConnectionEntry<RPGCharacter> targetInput = Target[context];
			ConnectionEntry<RPGCharacter> hitTargetOutput = HitTarget[context];
			EventEntry onHitOutput = OnHit[context];

			bool isActive = false;

			Action<RPGCharacter> eventHandler = (RPGCharacter target) =>
			{
				hitTargetOutput.Value = target;
				onHitOutput.Invoke ();
			};

			Action subscriber = () =>
			{
				if (targetInput.Value == null)
				{
					isActive = false;
					return;
				}

				if (!isActive)
					targetInput.Value.OnHit += eventHandler;

				isActive = true;
			};

			subscriber ();

			targetInput.OnBeforeChanged += () =>
			{
				if (targetInput.Value == null)
					return;

				if (isActive)
					targetInput.Value.OnHit -= eventHandler;
			};

			targetInput.OnAfterChanged += subscriber;
		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}
	}
}