using System;
using System.Collections.Generic;
using RPGCore.Behaviour.Connections;

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
	[Serializable]
	public class WeaponStatFloatInputCollection : WeaponStatCollection<FloatInput>
	{
	}
}
