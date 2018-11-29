using RPGCore.Utility;
using UnityEngine;

namespace RPGCore.Stats
{
	[CreateAssetMenu (menuName = "RPGCore/Weapon Stat/Database")]
	public class WeaponStatInformationDatabase : StaticDatabase<WeaponStatInformationDatabase>
	{
		public WeaponStatInformationCollection WeaponStatInfos;
	}
}