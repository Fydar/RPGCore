using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation ("Item/Weapon Input", "Input")]
	public class WeaponInputNode : BehaviourNode
	{
		public FloatInput AttackDamage;
		public FloatInput AttackSpeed;
		public FloatInput CriticalChance;
		public FloatInput CriticalMultiplier;

		public CharacterOutput HitTarget;
		public EventOutput OnHit;

		protected override void OnSetup (IBehaviourContext context)
		{

		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}
	}
}