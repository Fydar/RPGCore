using System;

namespace RPGCore.Stats
{
	[Serializable]
	public class StatInstanceCollection : StatCollection<StatInstance>
	{
		public void SetupReferences()
		{
			var stats = GetEnumerator();
			var info = StatInformationDatabase.Instance.StatInfos.GetEnumerator();

			while (stats.MoveNext())
			{
				info.MoveNext();

				if (info.Current == null)
				{
					continue;
				}

				stats.Current.Info = info.Current;
			}
		}
	}
}

