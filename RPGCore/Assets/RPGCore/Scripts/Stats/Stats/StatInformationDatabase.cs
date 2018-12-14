using RPGCore.Utility;
using UnityEngine;

namespace RPGCore.Stats
{
	[CreateAssetMenu (menuName = "RPGCore/Stat/Character Database")]
	public class StatInformationDatabase : StaticDatabase<StatInformationDatabase>
	{
		public StatInformationCollection StatInfos;
	}
}

