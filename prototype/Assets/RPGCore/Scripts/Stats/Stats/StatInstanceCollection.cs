using System;
using System.Collections.Generic;

namespace RPGCore.Stats
{
	[Serializable]
	public class StatInstanceCollection : StatCollection<StatInstance>
	{
		public void SetupReferences ()
		{
			IEnumerator<StatInstance> stats = GetEnumerator ();
			IEnumerator<StatInformation> info = StatInformationDatabase.Instance.StatInfos.GetEnumerator ();

			while (stats.MoveNext ())
			{
				info.MoveNext ();

				if (info.Current == null)
					continue;

				stats.Current.Info = info.Current;
			}
		}
	}
}

