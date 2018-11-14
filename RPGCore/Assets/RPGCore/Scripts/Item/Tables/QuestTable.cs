using RPGCore.Tables;
using UnityEngine;

namespace RPGCore.Quests
{
	[CreateAssetMenu (menuName = "RPGCore/Quest Table")]
	public class QuestTable : ScriptableObject
	{
		[System.Serializable]
		public class EnchantmentEntry : GenericTableEntry<QuestTemplate>
		{
			public EnchantmentEntry (QuestTemplate item, float balance)
				: base (item, balance) { }
		}

		[System.Serializable]
		public class EnchantmentRoll : AssetSelector<QuestTemplate, EnchantmentEntry> { }

		public EnchantmentRoll PossibleEnchantments;

		public QuestTemplate GetQuest ()
		{
			return PossibleEnchantments.Select ();
		}
	}
}