using RPGCore.Quests;
using UnityEngine;

namespace RPGCore.UI
{
	public class GeneralCheats : MonoBehaviour
	{
		public RPGCharacter Character;

		public QuestTable AcceptQuests;

		public void AcceptQuest ()
		{
			Character.Quests.Add (new Quest (AcceptQuests.GetQuest ()));
		}

		public void CompleteQuest ()
		{
			if (Character.Quests.Count == 0)
				return;

			Quest completeQuest = Character.Quests[UnityEngine.Random.Range (0, Character.Quests.Count - 1)];

			Character.Quests.Remove (completeQuest);

			completeQuest.template.GrantRewards (Character);
		}

		public void ClearInventory ()
		{
			Character.inventory.Clear ();
		}
	}
}