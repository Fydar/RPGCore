using RPGCore.Utility;
using UnityEngine;

namespace RPGCore.Inventories
{
	[CreateAssetMenu (menuName = "RPGCore/Equipment/Database")]
	public class EquipmentInformationDatabase : StaticDatabase<EquipmentInformationDatabase>
	{
		public EquipmentInformationCollection EquipmentInfos;
	}
}

