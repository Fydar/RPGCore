using System;
using System.Collections.Generic;

namespace RPGCore.Stats
{
	[Serializable]
	public class WeaponStatInstanceCollection : WeaponStatCollection<StatInstance>
	{
		public void SetupReferences ()
		{
			IEnumerator<StatInstance> WeaponStats = GetEnumerator ();
			IEnumerator<StatInformation> info = WeaponStatInformationDatabase.Instance.WeaponStatInfos.GetEnumerator ();

			while (WeaponStats.MoveNext ())
			{
				info.MoveNext ();

				if (info.Current == null)
					continue;

				WeaponStats.Current.Info = info.Current;
			}
		}
	}
}
