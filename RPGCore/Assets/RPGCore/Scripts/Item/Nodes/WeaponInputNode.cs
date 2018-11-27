using RPGCore.Behaviour;
using RPGCore.Behaviour.Connections;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation ("Item/Weapon Input", "Input")]
	public class WeaponInputNode : BehaviourNode
	{
		public CharacterOutput HitTarget;
		public EventOutput OnHit;

		protected override void OnSetup (IBehaviourContext context)
		{

		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}

#if UNITY_EDITOR
		public override Vector2 GetDiamentions ()
		{
			return new Vector2 (165, base.GetDiamentions ().y);
		}
#endif
	}
}