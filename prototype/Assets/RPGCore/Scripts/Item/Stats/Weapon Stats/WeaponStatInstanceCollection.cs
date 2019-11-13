using System;

namespace RPGCore.Stats
{
	[Serializable]
	public class WeaponStatInstanceCollection : WeaponStatCollection<StatInstance>
	{
		public void SetupReferences()
		{
			var WeaponStats = GetEnumerator();
			var info = WeaponStatInformationDatabase.Instance.WeaponStatInfos.GetEnumerator();

			while (WeaponStats.MoveNext())
			{
				info.MoveNext();

				if (info.Current == null)
				{
					continue;
				}

				WeaponStats.Current.Info = info.Current;
			}
		}
	}
}
