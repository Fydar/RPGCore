using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;

namespace RPGCore
{
	[NodeInformation ("Item/Armour Input", "Input")]
	public class ArmourInputNode : BehaviourNode
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