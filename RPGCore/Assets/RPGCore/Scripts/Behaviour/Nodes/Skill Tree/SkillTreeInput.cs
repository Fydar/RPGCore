using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore
{
	[NodeInformation ("Skill/Input", "Input")]
	public class SkillTreeInput : BehaviourNode
	{
		public BoolOutput Learned;

		protected override void OnSetup (IBehaviourContext context)
		{

		}

		protected override void OnRemove (IBehaviourContext context)
		{

		}
	}
}