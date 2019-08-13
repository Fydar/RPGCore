using RPGCore.Utility;
using UnityEngine;

namespace RPGCore.Stats
{
	[CreateAssetMenu (menuName = "RPGCore/Stat/Armour Database")]
	public class ArmourStatInformationDatabase : StaticDatabase<ArmourStatInformationDatabase>
	{
		public ArmourStatInformationCollection ArmourStatInfos;
	}
}
