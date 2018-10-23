using System;
using System.Collections.Generic;

namespace RPGCore.Inventories
{
	[Serializable]
	public class EquipmentInstanceCollection : EquipmentCollection<EquipmentInstance>
	{
		public void SetupReferences ()
		{
			IEnumerator<EquipmentInstance> Equipments = GetEnumerator ();
			IEnumerator<EquipmentInformation> info = EquipmentInformationDatabase.Instance.EquipmentInfos.GetEnumerator ();

			while (Equipments.MoveNext ())
			{
				info.MoveNext ();

				Equipments.Current.Info = info.Current;
			}
		}
	}
}