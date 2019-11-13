using System;

namespace RPGCore.Inventories
{
	[Serializable]
	public class EquipmentInstanceCollection : EquipmentCollection<EquipmentInstance>
	{
		public void SetupReferences()
		{
			var Equipments = GetEnumerator();
			var info = EquipmentInformationDatabase.Instance.EquipmentInfos.GetEnumerator();

			while (Equipments.MoveNext())
			{
				info.MoveNext();

				Equipments.Current.Info = info.Current;
			}
		}
	}
}

