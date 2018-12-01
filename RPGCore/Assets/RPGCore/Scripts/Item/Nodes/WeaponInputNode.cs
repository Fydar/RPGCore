using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;

namespace RPGCore
{
	[NodeInformation ("Item/Weapon Input", "Input", OnlyOne = true)]
	public class WeaponInputNode : BehaviourNode, IInputNode<ItemSurrogate>
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

		public void SetTarget (IBehaviourContext context, ItemSurrogate target)
		{

		}
	}
}
