using RPGCore.Utility;
using UnityEngine;

namespace RPGCore.Stats
{
	[CreateAssetMenu (menuName = "RPGCore/Stat/Weapon Database")]
	public class WeaponStatInformationDatabase : StaticDatabase<WeaponStatInformationDatabase>
	{
		public WeaponStatInformationCollection WeaponStatInfos;
	}
}
