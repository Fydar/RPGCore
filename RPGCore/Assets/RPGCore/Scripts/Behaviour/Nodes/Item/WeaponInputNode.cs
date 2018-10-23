using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation ("Item/Weapon Input", "Input")]
	public class WeaponInputNode : BehaviourNode
	{
		public CharacterOutput HitTarget;
		public EventOutput OnHit;

		protected override void OnSetup (IBehaviourContext character)
		{

		}

		protected override void OnRemove (IBehaviourContext character)
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