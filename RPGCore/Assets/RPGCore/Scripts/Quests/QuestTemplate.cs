using RPGCore.Behaviour;
using UnityEngine;

namespace RPGCore.Quests
{
	[CreateAssetMenu (menuName = "RPGCore/Quest")]
	public class QuestTemplate : BehaviourGraph
	{
		public string Name;
		[TextArea (3, 5)]
		public string Description;

		public ItemGenerator[] Rewards;

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