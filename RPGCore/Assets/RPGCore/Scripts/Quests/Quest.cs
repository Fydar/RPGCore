using System;

namespace RPGCore.Quests
{
	[Serializable]
	public class Quest
	{
		public QuestTemplate template;

		public Quest (QuestTemplate _template)
		{
			template = _template;


		}
	}
}

