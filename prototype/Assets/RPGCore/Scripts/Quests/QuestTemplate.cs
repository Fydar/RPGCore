using RPGCore.Behaviour;
using System.Collections.Generic;
using UnityEngine;

namespace RPGCore.Quests
{
	[CreateAssetMenu (menuName = "RPGCore/Quest")]
	public class QuestTemplate : ScriptableObject, IBehaviourGraph
	{
		public string Name;
		[TextArea (3, 5)]
		public string Description;

		public ItemGenerator[] Rewards;

		[UnityEngine.Serialization.FormerlySerializedAs ("Nodes")]
		[SerializeField, HideInInspector] private List<BehaviourNode> nodes;

		public List<BehaviourNode> AllNodes
		{
			get
			{
				return nodes;
			}
			set
			{
				AllNodes = value;
			}
		}

		public void GrantRewards (RPGCharacter character)
		{
			foreach (ItemGenerator generator in Rewards)
			{
				if (generator == null || generator.RewardTemplate == null)
					continue;

				character.inventory.Add (generator.Generate ());
			}
		}
	}
}
