using System;
using System.Collections.Generic;
namespace RPGCore.Stats
{
	[Serializable]
	public class ArmourStatInstanceCollection : ArmourStatCollection<StatInstance>
	{
		public void SetupReferences ()
		{
			IEnumerator<StatInstance> ArmourStats = GetEnumerator ();
			IEnumerator<StatInformation> info = ArmourStatInformationDatabase.Instance.ArmourStatInfos.GetEnumerator ();

			while (ArmourStats.MoveNext ())
			{
				info.MoveNext ();
				if (info.Current == null)
					continue;

				ArmourStats.Current.Info = info.Current;
			}
		}
	}
}
